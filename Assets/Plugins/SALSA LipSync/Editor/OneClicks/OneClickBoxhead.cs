using UnityEditor;
using UnityEngine;

namespace CrazyMinnow.SALSA.OneClicks
{
	/// <summary>
	/// The boxHead one-click script is included as an example template
	/// of one-click implementation for SALSA and/or EmoteR.
	/// </summary>
	public class OneClickBoxhead : MonoBehaviour
	{
		[MenuItem("GameObject/Crazy Minnow Studio/SALSA LipSync/One-Clicks/boxHead.v2-SALSA demo")]
		public static void BoxheadSetup()
		{
			var selectedObject = Selection.activeGameObject;
			var salsa = selectedObject.GetComponent<Salsa>();
			var smr = selectedObject.GetComponent<SkinnedMeshRenderer>();
			var audSrc = selectedObject.GetComponent<AudioSource>();
			var qp = FindObjectOfType<QueueProcessor>();

			// quick test to see if this object has the necessary stuff
			if (smr == null || smr.sharedMesh == null || smr.sharedMesh.blendShapeCount == 0
			    || smr.sharedMesh.GetBlendShapeIndex("saySml") == -1
			    || smr.sharedMesh.GetBlendShapeIndex("sayMed") == -1
			    || smr.sharedMesh.GetBlendShapeIndex("sayLrg") == -1)
			{
				Debug.Log("This object does not have the required components.");
				return;
			}

			if (salsa == null)
				salsa = selectedObject.AddComponent<Salsa>();

			if (audSrc == null)
				audSrc = selectedObject.AddComponent<AudioSource>();

			// setup a new queue processor if we didn't find one. recommend using one
			// queue processor per character (especially advanced characters with many
			// components.
			if (qp == null)
				qp = salsa.gameObject.AddComponent<QueueProcessor>();

			// configure AudioSource for demonstration. NOTE: if the clip is not found, it won't be setup.
			audSrc.playOnAwake = true;
			audSrc.loop = true;
			audSrc.clip = AssetDatabase.LoadAssetAtPath<AudioClip>("Assets/Crazy Minnow Studio/Examples/Audio/Promo-male.mp3");

			salsa.audioSrc = audSrc;
			salsa.queueProcessor = qp;


			// adjust salsa settings
			//	- data analysis settings
			salsa.autoAdjustAnalysis = true;
			salsa.autoAdjustMicrophone = false;

			//	- advanced dynamics
			salsa.loCutoff = 0.045f;
			salsa.hiCutoff = 0.80f;
			salsa.useAdvDyn = true;
			salsa.advDynPrimaryBias = 0.32f;
			salsa.useAdvDynJitter = true;
			salsa.advDynJitterAmount = 0.25f;
			salsa.advDynJitterProb = 0.27f;
			salsa.advDynSecondaryMix = 0.0f;


			// setup visemes
			salsa.visemes.Clear(); // start fresh

			// setup viseme 1 -- saySmall
			salsa.visemes.Add(new LipsyncExpression("saySmall", new InspectorControllerHelperData(), 0f));
			var saySmallViseme = salsa.visemes[0].expData;
			saySmallViseme.components[0].name = "saySmall component";
			saySmallViseme.controllerVars[0].smr = smr;
			saySmallViseme.controllerVars[0].blendIndex = smr.sharedMesh.GetBlendShapeIndex("saySml");
			saySmallViseme.controllerVars[0].minShape = 0f;
			saySmallViseme.controllerVars[0].maxShape = 1f;

			// setup viseme 2 -- sayMedium
			salsa.visemes.Add(new LipsyncExpression("sayMedium", new InspectorControllerHelperData(), 0f));
			var sayMediumViseme = salsa.visemes[1].expData;
			sayMediumViseme.components[0].name = "sayMedium component";
			sayMediumViseme.controllerVars[0].smr = smr;
			sayMediumViseme.controllerVars[0].blendIndex = smr.sharedMesh.GetBlendShapeIndex("sayMed");
			sayMediumViseme.controllerVars[0].minShape = 0f;
			sayMediumViseme.controllerVars[0].maxShape = 1f;
			// to add extra components to a viseme,
			// add new .controllerVars and .components list elements
			// then adjust settings...
			// sayMediumViseme.controllerVars.Add(new InspectorControllerHelperData());
			// sayMediumViseme.components.Add(new ExpressionComponent());
			// sayMediumViseme.components[1].durationOn = .08f;
			// sayMediumViseme.components[1].durationOff = .06f;
			// sayMediumViseme.components[1].easing = LerpEasings.EasingType.CubicOut;
			// sayMediumViseme.controllerVars[1].smr = smr;
			// sayMediumViseme.controllerVars[1].blendIndex = smr.sharedMesh.GetBlendShapeIndex("sayLrg");
			// sayMediumViseme.controllerVars[1].minShape = 0f;
			// sayMediumViseme.controllerVars[1].maxShape = 1f;
			// etc...

			// setup viseme 3 -- sayLarge
			salsa.visemes.Add(new LipsyncExpression("sayLarge", new InspectorControllerHelperData(), 0f));
			var sayLargeViseme = salsa.visemes[2].expData;
			sayLargeViseme.components[0].name = "sayLarge component";
			sayLargeViseme.controllerVars[0].smr = smr;
			sayLargeViseme.controllerVars[0].blendIndex = smr.sharedMesh.GetBlendShapeIndex("sayLrg");
			sayLargeViseme.controllerVars[0].minShape = 0f;
			sayLargeViseme.controllerVars[0].maxShape = 1f;

			salsa.DistributeTriggers(LerpEasings.EasingType.SquaredIn);

		}

//		[MenuItem("GameObject/Crazy Minnow Studio/SALSA LipSync/One-Clicks/boxHead.v2-EmoteR demo")]
		public static void BoxheadEmoterSetup()
		{
			var selectedObject = Selection.activeGameObject;
			var emoter = selectedObject.GetComponent<Emoter>();
			var smr = selectedObject.GetComponent<SkinnedMeshRenderer>();
			var qp = FindObjectOfType<QueueProcessor>();

			// quick test to see if this object has the necessary stuff
			if (smr == null || smr.sharedMesh == null || smr.sharedMesh.blendShapeCount == 0)
			{
				Debug.Log("This object does not have the required components.");
				return;
			}

			if (emoter == null)
				emoter = selectedObject.AddComponent<Emoter>();

			// setup a new queue processor if we didn't find one. recommend using one
			// queue processor per character (especially advanced characters with many
			// components.
			if (qp == null)
				qp = emoter.gameObject.AddComponent<QueueProcessor>();

			emoter.queueProcessor = qp;

			emoter.emotes.Clear();

			emoter.emotes.Add(new EmoteExpression("lookUp", new InspectorControllerHelperData(), true, false, false, 0f));
			emoter.emotes[0].isRandomEmote = true;
			var emote = emoter.emotes[0].expData;
			emote.components[0].name = "lookUp component";
			emote.components[0].durationOn = .5f;
			emote.components[0].durationHold = .2f;
			emote.components[0].durationOff = .3f;
			emote.controllerVars[0].smr = smr;
			emote.controllerVars[0].blendIndex = smr.sharedMesh.GetBlendShapeIndex("lookUp");
			emote.controllerVars[0].minShape = 0f;
			emote.controllerVars[0].maxShape = 1f;

			emoter.emotes.Add(new EmoteExpression("lookDown", new InspectorControllerHelperData(), true, false, false, 0f));
			emoter.emotes[1].isRandomEmote = true;
			emote = emoter.emotes[1].expData;
			emote.components[0].name = "lookDown component";
			emote.components[0].durationOn = .5f;
			emote.components[0].durationHold = .2f;
			emote.components[0].durationOff = .3f;
			emote.controllerVars[0].smr = smr;
			emote.controllerVars[0].blendIndex = smr.sharedMesh.GetBlendShapeIndex("lookDown");
			emote.controllerVars[0].minShape = 0f;
			emote.controllerVars[0].maxShape = 1f;
			// emote.controllerVars.Add(new InspectorControllerHelperData());
			// emote.components.Add(new ExpressionComponent());
			// emote.components[1].name = "sayLarge component";
			// emote.components[1].durationOn = .5f;
			// emote.components[1].durationHold = .3f;
			// emote.components[1].durationOff = .3f;
			// emote.components[1].easing = LerpEasings.EasingType.CubicOut;
			// emote.controllerVars[1].smr = smr;
			// emote.controllerVars[1].blendIndex = smr.sharedMesh.GetBlendShapeIndex("sayLrg");
			// emote.controllerVars[1].minShape = 0f;
			// emote.controllerVars[1].maxShape = 1f;

			emoter.emotes.Add(new EmoteExpression("lookLeft", new InspectorControllerHelperData(), true, false, false, 0f));
			emoter.emotes[2].isRandomEmote = true;
			emote = emoter.emotes[2].expData;
			emote.components[0].name = "lookLeft component";
			emote.components[0].durationOn = .5f;
			emote.components[0].durationHold = .2f;
			emote.components[0].durationOff = .3f;
			emote.controllerVars[0].smr = smr;
			emote.controllerVars[0].blendIndex = smr.sharedMesh.GetBlendShapeIndex("lookLeft");
			emote.controllerVars[0].minShape = 0f;
			emote.controllerVars[0].maxShape = 1f;

			emoter.emotes.Add(new EmoteExpression("lookRight", new InspectorControllerHelperData(), true, false, false, 0f));
			emoter.emotes[3].isRandomEmote = true;
			emote = emoter.emotes[3].expData;
			emote.components[0].name = "lookRight component";
			emote.components[0].durationOn = .5f;
			emote.components[0].durationHold = .2f;
			emote.components[0].durationOff = .3f;
			emote.controllerVars[0].smr = smr;
			emote.controllerVars[0].blendIndex = smr.sharedMesh.GetBlendShapeIndex("lookRight");
			emote.controllerVars[0].minShape = 0f;
			emote.controllerVars[0].maxShape = 1f;


			emoter.useRandomEmotes = true;
			emoter.randomChance = 1f;
			emoter.isChancePerEmote = true;
			emoter.NumRandomEmotesPerCycle = 2;
			emoter.randomEmoteMinTimer = 1.5f;
			emoter.randomEmoteMaxTimer = 3f;
			emoter.useRandomFrac = true;
			emoter.useRandomHoldDuration = true;
			emoter.randomHoldDurationMin = .15f;
			emoter.randomHoldDurationMax = .53f;
			emoter.randomFracBias = .25f;
		}
	}
}
