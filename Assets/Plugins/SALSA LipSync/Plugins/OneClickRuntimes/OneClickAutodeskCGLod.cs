using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace CrazyMinnow.SALSA.OneClicks
{
	public class OneClickAutodeskCGLod : MonoBehaviour
	{
		public Salsa salsa;
		public Emoter emoter;
		public Eyes eyes;

		private List<string> lodSmrSearches =
			new List<string>(4)
			{
				"^((c_[^(dds)])|(h_dds_crowd)).*$", // lod0
				"^((l_[^(dds)])|(h_dds_low)).*$",	// lod1
				"^((m_[^(dds)])|(h_dds_mid)).*$",	// lod2
				"^((h_[^(dds)])|(h_dds_high)).*$"	// lod3
			};
		private List<List<SkinnedMeshRenderer>> lodSmrs = new List<List<SkinnedMeshRenderer>>();
		private List<List<SkinnedMeshRenderer>> keySmrs = new List<List<SkinnedMeshRenderer>>();
		[SerializeField] private int activeLOD = -1;

		public int ActiveLOD
		{
			get { return activeLOD; }
			set { activeLOD = Mathf.Clamp(value, 0, GetMaxLod()); }
		}

		private void Awake()
		{
			OneClickConfig();
		}

		public void OneClickConfig(AudioClip clip = null)
		{
			Init();

			if (activeLOD < 0)
				SetLOD(GetMaxLod());	// LOD not specified, start with highest available.
			else
				if (!SetLOD(activeLOD))	// start with specified LOD.
					SetLOD(GetMaxLod());	// can't find an LOD at specified or higher, start with highest available.

			SetReferences(activeLOD, clip);
		}

		/// <summary>
		/// Find and deactivate all LODs associated with the configured SMR object searches.
		/// </summary>
		public void Init()
		{
			lodSmrs.Clear(); // ensure the list of lod SMRs is clean.

			// get smrs for each lod level
			for (int i = 0; i < lodSmrSearches.Count; i++)
			{
				lodSmrs.Add(new List<SkinnedMeshRenderer>());
				keySmrs.Add(new List<SkinnedMeshRenderer>());
				keySmrs[i].Add(new SkinnedMeshRenderer());
				keySmrs[i].Add(new SkinnedMeshRenderer());
				foreach (Transform child in transform)
				{
					if (Regex.IsMatch(child.name, lodSmrSearches[i], RegexOptions.IgnoreCase))
					{
						var possibleSmr = child.GetComponent<SkinnedMeshRenderer>();
						if (possibleSmr != null)
						{
							lodSmrs[i].Add(child.GetComponent<SkinnedMeshRenderer>());

							// store key smr components for switching Salsa/Emoter
							if (child.name.ToLower().Contains("teethdown"))
								keySmrs[i][1] = possibleSmr;
							if (child.name.ToLower().Contains("h_dds_"))
								keySmrs[i][0] = possibleSmr;
						}
					}
				}
			}

			DeactivateAllLODs(); // fresh slate
		}

		/// <summary>
		/// Ensure all Suite references are linked. Requires an LOD to be active.
		/// </summary>
		/// <param name="lodLevel"></param>
		/// <param name="clip"></param>
		private void SetReferences(int lodLevel, AudioClip clip = null)
		{
			salsa = GetComponent<Salsa>();
			emoter = GetComponent<Emoter>();
			eyes = GetComponent<Eyes>();

			if (salsa == null || emoter == null)
			{
				OneClickAutodeskCG.Setup(gameObject, clip, lodLevel);
				salsa = GetComponent<Salsa>();
				emoter = GetComponent<Emoter>();
			}

			if (eyes == null)
			{
				OneClickAutodeskCgEyes.Setup(gameObject, lodLevel);
				eyes = GetComponent<Eyes>();
			}
		}

		private bool ActivateLOD(int lodToActivate)
		{
			if (lodToActivate >= 0 && lodToActivate < lodSmrs.Count && lodSmrs[lodToActivate].Count > 0)
			{
				foreach (var smr in lodSmrs[lodToActivate])
					smr.gameObject.SetActive(true);
				activeLOD = lodToActivate; // update the current LOD pointer.
				UpdateSalsaSuite(activeLOD);
				return true;
			}

			return false;
		}

		private void DeactivateLOD(int lodToDeactivate)
		{
			if (lodToDeactivate >= 0 && lodToDeactivate < lodSmrs.Count)
			{
				if (lodToDeactivate > -1) // LOD has previously been set.
					foreach (var smr in lodSmrs[lodToDeactivate])
						smr.gameObject.SetActive(false);
				else // probably first time running, ensure all LODs are off.
					DeactivateAllLODs();
			}
		}

		private void DeactivateAllLODs()
		{
			foreach (var lod in lodSmrs)
				foreach (var smr in lod)
					smr.gameObject.SetActive(false);
		}

		/// <summary>
		/// Deactivate current LOD and activate new LOD. If specified LOD does not exist, try next.
		/// </summary>
		/// <param name="lodLevel"></param>
		public bool SetLOD(int lodLevel)
		{
			DeactivateLOD(ActiveLOD);
			while (lodLevel >= 0 && lodLevel < lodSmrs.Count && !ActivateLOD(lodLevel))
			{
				lodLevel++;
				if (lodLevel > lodSmrs.Count - 1)
					return false;
			}
			return true;
		}


		/// <summary>
		/// Find the highest LOD available.
		/// </summary>
		/// <returns></returns>
		public int GetMaxLod()
		{
			var maxLod = -1;
			for (int i = 0; i < lodSmrSearches.Count; i++)
				maxLod = lodSmrs[i].Count > 0 ? i : maxLod;

			return maxLod;
		}


		/// <summary>
		/// Helper used for UI buttons (etc.) which can't receive a return.
		/// </summary>
		public void Increase()
		{
			IncreaseLOD();
		}
		/// <summary>
		/// Helper used for UI buttons (etc.) which can't receive a return.
		/// </summary>
		public void Decrease()
		{
			DecreaseLOD();
		}
		/// <summary>
		/// Increase the LOD to the next available LOD.
		/// </summary>
		/// <returns>True=success; False=failure (no higher LOD found)</returns>
		public bool IncreaseLOD()
		{
			for (int i = activeLOD + 1; i < lodSmrs.Count; i++)
				if (lodSmrs[i].Count > 0) // is a viable LOD
				{
					SetLOD(i);
					return true;
				}

			return false;
		}

		private void UpdateSalsaSuite(int lodLevel)
		{
			if (salsa)
			{
				// flip Salsa smrs to new lod.
				foreach (var viseme in salsa.visemes)
				{
					foreach (var controllerVar in viseme.expData.controllerVars)
					{
						if (controllerVar.smr.name.ToLower().Contains("h_dds_"))
							controllerVar.smr = keySmrs[lodLevel][0]; // body smr
						else
							controllerVar.smr = keySmrs[lodLevel][1]; // teeth smr
					}
				}

				salsa.UpdateExpressionControllers();
			}

			if (emoter)
			{
				// flip Emoter smrs to new lod
				foreach (var emote in emoter.emotes)
				{
					foreach (var controllerVar in emote.expData.controllerVars)
						controllerVar.smr = keySmrs[lodLevel][0]; // body smr
				}

				emoter.UpdateExpressionControllers();
			}

			if (eyes)
			{
				// flip Eyes (blink) smrs to new lod
				foreach (var eyelid in eyes.eyelids)
				{
					foreach (var controllerVar in eyelid.expData.controllerVars)
						controllerVar.smr = keySmrs[lodLevel][0]; // body smr
				}

				eyes.UpdateRuntimeExpressionControllers(ref eyes.eyelids);
			}
		}

		/// <summary>
		/// Decrease the LOD to the next available LOD.
		/// </summary>
		/// <returns>True=success; False=failure (no lower LOD found)</returns>
		public bool DecreaseLOD()
		{
			for (int i = activeLOD - 1; i >= 0; i--)
				if (lodSmrs[i].Count > 0) // is a viable LOD
				{
					SetLOD(i);
					return true;
				}

			return false;
		}
	}
}