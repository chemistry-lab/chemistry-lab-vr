using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace CrazyMinnow.SALSA.OneClicks
{
	public class OneClickBase : MonoBehaviour
	{
		/// <summary>
		/// RELEASE NOTES:
		///		2.1.6:
		/// 		~ audio source now defaults to loop = false.
		///		2.1.5:
		/// 		+ support for UMA core controller, requires SALSA core 2.1.0-experimental+
		/// 	2.1.4:
		/// 		+ new regex blendshape search to handle more complex scenarios.
		/// 	2.1.3:
		/// 		! AddBoneComponent now includes duration values.
		///		2.1.2:
		/// 		~ null AudioClip does not assign.
		/// 	2.1.1:
		/// 		+ new string constants supporting: 2018.4+ check for prefab and warn > then unpack or cancel.
		/// 	2.1.0:
		/// 		~ convert from editor code to full engine code and move to Plugins.
		/// 	2.0.1:
		/// 		! fix out-of-range error when no blendshapes are detected.
		///		2.0.0-BETA : Initial release.
		/// ==========================================================================
		/// PURPOSE: This script provides simple, simulated lip-sync input to the
		///		Salsa component from text/string values. For the latest information
		///		visit crazyminnowstudio.com.
		/// ==========================================================================
		/// DISCLAIMER: While every attempt has been made to ensure the safe content
		///		and operation of these files, they are provided as-is, without
		///		warranty or guarantee of any kind. By downloading and using these
		///		files you are accepting any and all risks associated and release
		///		Crazy Minnow Studio, LLC of any and all liability.
		/// ==========================================================================
		/// </summary>

		public const string RESOURCE_CLIP = "Assets/Crazy Minnow Studio/Examples/Audio/Promo-male.mp3";
		public const string PREFAB_ALERT_TITLE = "Prefab Unpack Warning";
		public const string PREFAB_ALERT_MSG = "Your selection is a prefab and must be unpacked to apply this setup. " +
											   "You can create a new prefab once the setup is complete. " +
											   "Do you want to proceed?";


		protected static List<SkinnedMeshRenderer> requiredSmrs = new List<SkinnedMeshRenderer>();
		protected static List<OneClickConfiguration> oneClickConfigurations = new List<OneClickConfiguration>();
		protected static Salsa salsa;
		protected static Emoter emoter;
		protected static UmaUepProxy uepProxy; // only if there is a UMA component requiring it.
//		protected static Eyes eyes;
		protected static GameObject selectedObject;

		protected static float emphasizerTrigger;

		// adjust these salsa settings to taste...
		//	- data analysis settings
		protected static bool autoAdjustAnalysis = true;
		protected static bool autoAdjustMicrophone = false; // only true if you are using micInput
		// advanced dynamics
		protected static float loCutoff = 0.045f;
		protected static float hiCutoff = 0.73f;
		protected static bool useAdvDyn = true;
		protected static float advDynPrimaryBias = 0.40f;
		protected static bool useAdvDynJitter = true;
		protected static float advDynJitterAmount = 0.25f;
		protected static float advDynJitterProb = 0.50f;
		protected static float advDynSecondaryMix = 0.0f;

		// emoter settings...
		protected static bool useRandomEmotes = false;
		protected static bool isChancePerEmote = false;
		protected static int numRandomEmotesPerCycle = 1;
		protected static float randomEmoteMinTimer = 1.0f;
		protected static float randomEmoteMaxTimer = 2.0f;
		protected static float randomChance = 0.5f;
		protected static bool useRandomFrac = false;
		protected static float randomFracBias = 0.5f;
		protected static bool useRandomHoldDuration = false;
		protected static float randomHoldDurationMin = 0.1f;
		protected static float randomHoldDurationMax = 0.5f;

		protected static void NewConfiguration(OneClickConfiguration.ConfigType configType)
		{
			oneClickConfigurations.Add(new OneClickConfiguration(configType));
		}

		protected static void AddSmrSearch(string search)
		{
			oneClickConfigurations[oneClickConfigurations.Count - 1].smrSearches.Add(search);
		}

		/// <summary>
		/// Abstraction of setting up a new expression...
		/// </summary>
		/// <param name="expressionName"></param>
		protected static void NewExpression(string expressionName)
		{
			oneClickConfigurations[oneClickConfigurations.Count - 1].oneClickExpressions.Add(new OneClickExpression(expressionName, new List<OneClickComponent>()));
		}

		/// <summary>
		/// Abstraction of adding a component to a expression...
		/// </summary>
		/// <param name="blendshapeNames">array of string names to attempt.</param>
		/// <param name="componentName">default expression name.</param>
		/// <param name="durOff"></param>
		/// <param name="amount">default 1.0f</param>
		/// <param name="durOn"></param>
		/// <param name="durHold"></param>
		protected static void AddShapeComponent(string[] blendshapeNames,
												float durOn,
												float durHold,
												float durOff,
		                                        string componentName = "",
		                                        float amount = 1.0f,
												bool useRegex = false
			)
		{
			var config = oneClickConfigurations[oneClickConfigurations.Count - 1];
			config.oneClickExpressions[config.oneClickExpressions.Count - 1]
				.components.Add(new OneClickComponent(componentName,
				                                      blendshapeNames,
				                                      amount,
													  durOn,
													  durHold,
													  durOff,
				                                      OneClickComponent.ComponentType.Shape,
													  useRegex));
		}


		protected static void AddUepPoseComponent(string poseName,
												  float durOn,
												  float durHold,
												  float durOff,
												  string componentName = "",
												  float amount = 1.0f
			)
		{
			var config = oneClickConfigurations[oneClickConfigurations.Count - 1];
			config.oneClickExpressions[config.oneClickExpressions.Count - 1]
				.components.Add(new OneClickComponent(componentName,
													  poseName,
				                                      amount,
													  durOn,
													  durHold,
													  durOff,
				                                      OneClickComponent.ComponentType.UMA));
		}



		/// <summary>
		/// Absraction of adding a bone component to an expression...
		/// </summary>
		/// <param name="minTform"></param>
		/// <param name="maxTform"></param>
		/// <param name="durOn"></param>
		/// <param name="durHold"></param>
		/// <param name="durOff"></param>
		/// <param name="componentName">defaults to expression name</param>
		/// <param name="constrainPos">defaults false</param>
		/// <param name="constrainRot">defaults true</param>
		/// <param name="constrainScl">defaults false</param>
		protected static void AddBoneComponent(string boneSearch,
		                                       TformBase maxTform,
											   float durOn,
											   float durHold,
											   float durOff,
		                                       string componentName = "",
		                                       bool constrainPos = false,
		                                       bool constrainRot = true,
		                                       bool constrainScl = false)
		{
			var config = oneClickConfigurations[oneClickConfigurations.Count - 1];
			config.oneClickExpressions[config.oneClickExpressions.Count - 1]
				.components.Add(new OneClickComponent(componentName,
				                                      boneSearch,
				                                      maxTform,
				                                      constrainPos,
				                                      constrainRot,
				                                      constrainScl,
													  durOn,
													  durHold,
													  durOff,
				                                      OneClickComponent.ComponentType.Bone));
		}

		protected static void AddEmoteFlags(bool isRandom, bool isEmph, bool isRepeater, float frac = 1.0f)
		{
			var config = oneClickConfigurations[oneClickConfigurations.Count - 1];
			config.oneClickExpressions[config.oneClickExpressions.Count - 1].SetEmoterBools(isRandom, isEmph, isRepeater, frac);
		}

		protected static void DoOneClickiness(GameObject go, AudioClip clip)
		{
			selectedObject = go;

			// setup QueueProcessor
			var qp = selectedObject.GetComponent<QueueProcessor>(); // get QueueProcessor on current object -- we no longer look in-scene.
			if (qp == null)
				qp = selectedObject.AddComponent<QueueProcessor>();

			// if there is a uepProxy, get reference to it.
			uepProxy = go.GetComponent<UmaUepProxy>();

			foreach (var configuration in oneClickConfigurations)
			{
				// module-specific configuraiton requirements::
				switch (configuration.type)
				{
					#region salsa-specific setup
					case OneClickConfiguration.ConfigType.Salsa:
						salsa = selectedObject.GetComponent<Salsa>();
						if (salsa == null)
							salsa = selectedObject.AddComponent<Salsa>();

						// configure AudioSource for demonstration
						var audSrc = selectedObject.GetComponent<AudioSource>();
						if (audSrc == null)
							audSrc = selectedObject.AddComponent<AudioSource>();
						audSrc.playOnAwake = true;
						audSrc.loop = false;
						if (clip != null)
							audSrc.clip = clip;
						salsa.audioSrc = audSrc;

						salsa.queueProcessor = qp;

						// adjust salsa settings
						//	- data analysis settings
						salsa.autoAdjustAnalysis = autoAdjustAnalysis;
						salsa.autoAdjustMicrophone = autoAdjustMicrophone;
						//	- advanced dynamics
						salsa.loCutoff = loCutoff;
						salsa.hiCutoff = hiCutoff;
						salsa.useAdvDyn = useAdvDyn;
						salsa.advDynPrimaryBias = advDynPrimaryBias;
						salsa.useAdvDynJitter = useAdvDynJitter;
						salsa.advDynJitterAmount = advDynJitterAmount;
						salsa.advDynJitterProb = advDynJitterProb;
						salsa.advDynSecondaryMix = advDynSecondaryMix;

						salsa.emphasizerTrigger = emphasizerTrigger;
						break;
					#endregion

					#region emoter-specific setup
					case OneClickConfiguration.ConfigType.Emoter:
						emoter = selectedObject.GetComponent<Emoter>();
						if (emoter == null)
							emoter = selectedObject.AddComponent<Emoter>();

						salsa.emoter = emoter;
						emoter.queueProcessor = qp;

						emoter.useRandomEmotes = useRandomEmotes;
						emoter.isChancePerEmote = isChancePerEmote;
						emoter.NumRandomEmotesPerCycle = numRandomEmotesPerCycle;
						emoter.randomEmoteMinTimer = randomEmoteMinTimer;
						emoter.randomEmoteMaxTimer = randomEmoteMaxTimer;
						emoter.randomChance = randomChance;
						emoter.useRandomFrac = useRandomFrac;
						emoter.randomFracBias = randomFracBias;
						emoter.useRandomHoldDuration = useRandomHoldDuration;
						emoter.randomHoldDurationMin = randomHoldDurationMin;
						emoter.randomHoldDurationMax = randomHoldDurationMax;

						break;
					#endregion
				}

				#region Skinned Mesh Renderer search for shape component creation.
				if (configuration.smrSearches.Count > 0)
				{
					var allSmrs = selectedObject.GetComponentsInChildren<SkinnedMeshRenderer>();
					// quick test to see if this object has the necessary stuff
					if (allSmrs == null || allSmrs.Length == 0)
					{
						Debug.LogError("This object does not have the required components. No Skinned Mesh Renderers found. Ensure this one-click script was applied to the root of the model prefab in the scene hierarchy.");
						return;
					}

					// Find the necessary SMR's
					foreach (var smr in allSmrs)
					{
						foreach (var smrSearch in configuration.smrSearches)
						{
							if (Regex.IsMatch(smr.name, smrSearch, RegexOptions.IgnoreCase))
								requiredSmrs.Add(smr);
						}
					}

					if (requiredSmrs.Count == 0)
					{
						Debug.LogError("This object does not have the required components. Could not find the referenced SMRs. Ensure the appropriate one-click was used for your model type and generation.");
						return;
					}
				}
				#endregion // Skinned Mesh Renderer search for shape component creation.

				ConfigureModule(configuration);

				ResetForNextConfiguration();
			}
		}

		protected static void Init()
		{
			oneClickConfigurations.Clear();	// clean configurations to prevent additive configurations
			requiredSmrs.Clear();
		}

		protected static void ResetForNextConfiguration()
		{
			requiredSmrs.Clear();
		}

		private static void ConfigureModule(OneClickConfiguration configuration)
		{
			// module-specific initialization::
			switch (configuration.type)
			{
				case OneClickConfiguration.ConfigType.Salsa:
					salsa.visemes.Clear();
					break;
				case OneClickConfiguration.ConfigType.Emoter:
					emoter.emotes.Clear();
					break;
			}

			for (int exp = 0; exp < configuration.oneClickExpressions.Count; exp++)
			{
				Expression expression = new Expression();
				// module-specific expression actions
				switch (configuration.type)
				{
					case OneClickConfiguration.ConfigType.Salsa:
						// create our salsa viseme for each oneClickExpression.
						salsa.visemes.Add(new LipsyncExpression(configuration.oneClickExpressions[exp].name, new InspectorControllerHelperData(), 0f));
						var viseme = salsa.visemes[salsa.visemes.Count - 1];
						viseme.expData.inspFoldout = false;
						expression = viseme.expData;
						break;
					case OneClickConfiguration.ConfigType.Emoter:
						// create our emoter emote for each oneClickExpression.
						emoter.emotes.Add(new EmoteExpression(configuration.oneClickExpressions[exp].name, new InspectorControllerHelperData(), false, true, false, 0f));
						var emote = emoter.emotes[emoter.emotes.Count - 1];
						emote.expData.inspFoldout = false;
						emote.isRandomEmote = configuration.oneClickExpressions[exp].isRandom;
						emote.isLipsyncEmphasisEmote = configuration.oneClickExpressions[exp].isEmphasis;
						emote.isRepeaterEmote = configuration.oneClickExpressions[exp].isRepeater;
						emote.frac = configuration.oneClickExpressions[exp].expressionDynamics;
						expression = emote.expData;
						break;
					case OneClickConfiguration.ConfigType.Eyes:
						// create our eyes eye for each oneClickExpression.


						//expression = eyes.eyes[eyes.eyes.Count - 1].expData;
						break;
					default:
						expression = salsa.visemes[salsa.visemes.Count - 1].expData;
						break;
				}

				var componentCount = 0;
				var currCmpnt = 0;

				for (int j = 0; j < configuration.oneClickExpressions[exp].components.Count; j++)
				{
					var oneClickComponent = configuration.oneClickExpressions[exp].components[j];
					switch (oneClickComponent.type)
					{
						case OneClickComponent.ComponentType.Shape:
							for (int i = 0; i < requiredSmrs.Count; i++)
							{
								int blendshapeIndex = -1;
								// we need to confirm proposed blendshape names are viable...
								foreach (var blendshapeName in oneClickComponent.blendshapeNames)
								{
									if (oneClickComponent.useRegex)
										blendshapeIndex = RegexFindBlendshapeName(requiredSmrs[i], blendshapeName);
									else
										blendshapeIndex = requiredSmrs[i].sharedMesh.GetBlendShapeIndex(blendshapeName);

									if (blendshapeIndex > -1) // we found one!
										break;
								}
								if (blendshapeIndex == -1) // we didn't find our blendshape...
									continue;

								// Create a new component if applicable.
								CreateNewComponent(componentCount, expression);

								currCmpnt = expression.components.Count - 1;
								ApplyCommonSettingsToComponent(configuration, expression, currCmpnt, oneClickComponent, exp);

								expression.controllerVars[currCmpnt].smr = requiredSmrs[i];
								expression.controllerVars[currCmpnt].blendIndex = blendshapeIndex;
								expression.controllerVars[currCmpnt].minShape = 0f;
								expression.controllerVars[currCmpnt].maxShape = oneClickComponent.maxAmount;
								expression.components[currCmpnt].durationOn = oneClickComponent.durOn;
								expression.components[currCmpnt].durationHold = oneClickComponent.durHold;
								expression.components[currCmpnt].durationOff = oneClickComponent.durOff;

								componentCount++;
							}
							break;
						case OneClickComponent.ComponentType.UMA:
							// Create a new component if applicable.
							CreateNewComponent(componentCount, expression);

							currCmpnt = expression.components.Count - 1;
							ApplyCommonSettingsToComponent(configuration, expression, currCmpnt, oneClickComponent, exp);

							expression.components[currCmpnt].controlType = ExpressionComponent.ControlType.UMA;
							expression.controllerVars[currCmpnt].umaUepProxy = uepProxy;
							expression.controllerVars[currCmpnt].blendIndex = uepProxy.GetPoseIndex(oneClickComponent.poseName);
							expression.controllerVars[currCmpnt].minShape = 0f;
							expression.controllerVars[currCmpnt].uepAmount = oneClickComponent.maxAmount;
							expression.components[currCmpnt].durationOn = oneClickComponent.durOn;
							expression.components[currCmpnt].durationHold = oneClickComponent.durHold;
							expression.components[currCmpnt].durationOff = oneClickComponent.durOff;

							componentCount++;
							break;
						case OneClickComponent.ComponentType.Bone:
							// confirm bone is viable...
							var bone = FindBone(oneClickComponent.boneSearch);
							if (bone == null)
							{
								Debug.LogWarning("Could not find bone: " + oneClickComponent.boneSearch);
								continue;
							}

							// Create a new component if applicable.
							CreateNewComponent(componentCount, expression);

							currCmpnt = expression.components.Count - 1;
							ApplyCommonSettingsToComponent(configuration, expression, currCmpnt, oneClickComponent, exp);

							expression.components[currCmpnt].controlType = ExpressionComponent.ControlType.Bone;
							var controlHelper = expression.controllerVars[currCmpnt];
							controlHelper.bone = bone;
							controlHelper.startTform = ConvertBoneToTform(bone);
							controlHelper.endTform = oneClickComponent.max;
							controlHelper.inspIsSetStart = true;
							controlHelper.inspIsSetEnd = true;
							controlHelper.fracRot = oneClickComponent.useRot;
							controlHelper.fracPos = oneClickComponent.usePos;
							controlHelper.fracScl = oneClickComponent.useScl;

							controlHelper.StoreBoneBase();
							controlHelper.StoreStartTform();

							componentCount++;
							break;
					}
				}

				// if no component was created for this expression, remove it.
				if (componentCount == 0)
				{
					Debug.Log("Removed expression " + expression.name + " This may be OK, but may indicate a change in the model generator. If this is a supported model, contact Crazy Minnow Studio via assetsupport@crazyminnow.com.");
					switch (configuration.type)
					{
						case OneClickConfiguration.ConfigType.Salsa:
							salsa.visemes.RemoveAt(salsa.visemes.Count - 1);
							break;
						case OneClickConfiguration.ConfigType.Emoter:
							emoter.emotes.RemoveAt(emoter.emotes.Count - 1);
							break;
					}
				}

				// module-specific wrap-up
				switch (configuration.type)
				{
					case OneClickConfiguration.ConfigType.Salsa:
						salsa.DistributeTriggers(LerpEasings.EasingType.SquaredIn);
						break;
				}
			}
		}

		private static int RegexFindBlendshapeName(SkinnedMeshRenderer smr, string bName)
		{
			var bNames = GetBlendshapeNames(smr);
			for (int i = 0; i < bNames.Length; i++)
			{
				if (Regex.IsMatch(bNames[i], bName, RegexOptions.IgnoreCase))
					return i;
			}

			return -1;
		}

		private static string[] GetBlendshapeNames(SkinnedMeshRenderer smr)
		{
			string[] bNames = new string[smr.sharedMesh.blendShapeCount];
			for (int i = 0; i < smr.sharedMesh.blendShapeCount; i++)
			{
				bNames[i] = smr.sharedMesh.GetBlendShapeName(i);
			}

			return bNames;
		}

		private static TformBase ConvertBoneToTform(Transform bone)
		{
			return new TformBase(new Vector3(bone.localPosition.x, bone.localPosition.y, bone.localPosition.x),
			                     new Quaternion(bone.localRotation.x, bone.localRotation.y, bone.localRotation.z, bone.localRotation.w),
			                     new Vector3(bone.localScale.x, bone.localScale.y, bone.localScale.z));
		}

		private static Transform FindBone(string boneSearch)
		{
			var bones = selectedObject.GetComponentsInChildren<Transform>();
			foreach (var bone in bones)
			{
				if (Regex.IsMatch(bone.name, boneSearch, RegexOptions.IgnoreCase))
					return bone;
			}

			return null;
		}

		private static void ApplyCommonSettingsToComponent(OneClickConfiguration configuration, Expression expression,
		                                                   int currCmpnt, OneClickComponent oneClickComponent, int exp)
		{
			expression.components[currCmpnt].durationOn = .08f;
			expression.components[currCmpnt].durationOff = .06f;
			expression.components[currCmpnt].easing = LerpEasings.EasingType.CubicOut;
			expression.components[currCmpnt].inspFoldout = false;
			var label = String.IsNullOrEmpty(oneClickComponent.componentName)
				            ? configuration.oneClickExpressions[exp].name
				            : oneClickComponent.componentName;
			expression.components[currCmpnt].name = label + " cmpnt " + (currCmpnt);
		}

		private static void CreateNewComponent(int componentCount, Expression expression)
		{
			if (componentCount > expression.components.Count - 1)
			{
				expression.components.Add(new ExpressionComponent());
				expression.controllerVars.Add(new InspectorControllerHelperData());
			}
		}
	}

	public class OneClickConfiguration
	{
		public ConfigType type;
		public List<OneClickExpression> oneClickExpressions = new List<OneClickExpression>();
		public List<string> smrSearches = new List<string>();

		public enum ConfigType
		{
			Salsa,
			Emoter,
			Eyes,
			Head,
			Eyelids
		}

		public OneClickConfiguration(ConfigType type)
		{
			this.type = type;
			smrSearches.Clear();
			oneClickExpressions.Clear();
		}
	}
	public class OneClickExpression
	{
		public string name;
		public bool isRandom = false;
		public bool isEmphasis = false;
		public bool isRepeater = false;
		public float expressionDynamics = 1.0f;
		public List<OneClickComponent> components;

		public OneClickExpression(string name, List<OneClickComponent> components)
		{
			this.name = name;
			this.components = components;
		}

		public void SetEmoterBools(bool isRand, bool isEmph, bool isRep, float frac)
		{
			isRandom = isRand;
			isEmphasis = isEmph;
			isRepeater = isRep;
			expressionDynamics = frac;
		}
	}
	public class OneClickComponent
	{
		public string componentName;
		public string[] blendshapeNames;
		public string poseName;
		public float maxAmount;
		public float durOn;
		public float durHold;
		public float durOff;

		public string boneSearch;
		public TformBase max;
		public bool usePos;
		public bool useRot;
		public bool useScl;
		public ComponentType type;

		public bool useRegex = false;

		public enum ComponentType
		{
			Shape,
			UMA,
			Bone
		}

		/// <summary>
		/// Shape type setup
		/// </summary>
		/// <param name="componentName"></param>
		/// <param name="blendshapeNames"></param>
		/// <param name="maxAmount"></param>
		/// <param name="durOn"></param>
		/// <param name="durHold"></param>
		/// <param name="durOff"></param>
		/// <param name="type"></param>
		/// <param name="useRegex"></param>
		public OneClickComponent(string componentName,
								 string[] blendshapeNames,
								 float maxAmount,
								 float durOn,
								 float durHold,
								 float durOff,
								 ComponentType type,
								 bool useRegex)
		{
			this.componentName = componentName;
			this.blendshapeNames = blendshapeNames;
			this.maxAmount = maxAmount;
			this.durOn = durOn;
			this.durHold = durHold;
			this.durOff = durOff;
			this.type = type;
			this.useRegex = useRegex;
		}

		/// <summary>
		/// UMA proxy type setup
		/// </summary>
		/// <param name="componentName"></param>
		/// <param name="poseName"></param>
		/// <param name="maxAmount"></param>
		/// <param name="durOn"></param>
		/// <param name="durHold"></param>
		/// <param name="durOff"></param>
		/// <param name="type"></param>
		public OneClickComponent(string componentName,
								 string poseName,
								 float maxAmount,
								 float durOn,
								 float durHold,
								 float durOff,
								 ComponentType type)
		{
			this.componentName = componentName;
			this.poseName = poseName;
			this.maxAmount = maxAmount;
			this.durOn = durOn;
			this.durHold = durHold;
			this.durOff = durOff;
			this.type = type;
		}

		/// <summary>
		/// Bone type setup
		/// </summary>
		/// <param name="componentName"></param>
		/// <param name="boneSearch"></param>
		/// <param name="max"></param>
		/// <param name="usePos"></param>
		/// <param name="useRot"></param>
		/// <param name="useScl"></param>
		/// <param name="durOn"></param>
		/// <param name="durHold"></param>
		/// <param name="durOff"></param>
		/// <param name="type"></param>
		public OneClickComponent(string componentName,
								 string boneSearch,
								 TformBase max,
								 bool usePos,
								 bool useRot,
								 bool useScl,
								 float durOn,
								 float durHold,
								 float durOff,
								 ComponentType type)
		{
			this.boneSearch = boneSearch;
			this.componentName = componentName;
			this.max = max;
			this.usePos = usePos;
			this.useRot = useRot;
			this.useScl = useScl;
			this.durOn = durOn;
			this.durHold = durHold;
			this.durOff = durOff;
			this.type = type;
		}
	}
}