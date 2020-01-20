using UnityEngine;
using System.Text.RegularExpressions;

namespace CrazyMinnow.SALSA.OneClicks
{
	public class OneClickAutodeskCgEyes : MonoBehaviour
	{
		public static void Setup(GameObject go, int lod)
		{
			string head = "head";
			string eyeL = "lefteye";
			string eyeR = "righteye";
			string body = "body";
			string blinkL = "^.*reyeclose.*$";
			string blinkR = "^.*leyeclose.*$";

			if (go)
			{
				Eyes eyes = go.GetComponent<Eyes>();
				if (eyes == null)
				{
					eyes = go.AddComponent<Eyes>();
				}
				else
				{
					DestroyImmediate(eyes);
					eyes = go.AddComponent<Eyes>();
				}
				QueueProcessor qp = go.GetComponent<QueueProcessor>();
				if (qp == null) qp = go.AddComponent<QueueProcessor>();

				// System Properties
                eyes.characterRoot = go.transform;
                eyes.queueProcessor = qp;

                // Heads - Bone_Rotation
                eyes.BuildHeadTemplate(Eyes.HeadTemplates.Bone_Rotation_XY);
                eyes.heads[0].expData.controllerVars[0].bone = FindTransform(eyes.characterRoot, head);
                eyes.headTargetOffset.y = 0.052f;
				eyes.FixAllTransformAxes(ref eyes.heads, false);
				eyes.FixAllTransformAxes(ref eyes.heads, true);

                // Eyes - Bone_Rotation
                eyes.BuildEyeTemplate(Eyes.EyeTemplates.Bone_Rotation);
                eyes.eyes[0].expData.controllerVars[0].bone = FindTransform(eyes.characterRoot, eyeL);
                eyes.eyes[1].expData.controllerVars[0].bone = FindTransform(eyes.characterRoot, eyeR);
				eyes.FixAllTransformAxes(ref eyes.eyes, false);
				eyes.FixAllTransformAxes(ref eyes.eyes, true);

                // Eyelids - Bone_Rotation
                eyes.BuildEyelidTemplate(Eyes.EyelidTemplates.BlendShapes); // includes left/right eyelid
                eyes.SetEyelidShapeSelection(Eyes.EyelidSelection.Upper);
                float blinkMax = 1f;
                switch (lod)
                {
	                case 0: // Crowd
		                body = "^h_dds_.*crowd.*$";
		                break;
		            case 1: // low
			            body = "^h_dds_.*low.*$";
			            break;
		            case 2: // mid
			            body = "^h_dds_.*mid.*$";
			            break;
		            case 3: // high
			            body = "^h_dds_.*high.*$";
			            break;
                }
                // Left eyelid
                eyes.eyelids[0].referenceIdx = 0;
                eyes.eyelids[0].expData.controllerVars[0].smr = FindTransform(eyes.characterRoot, body).GetComponent<SkinnedMeshRenderer>();
				eyes.eyelids[0].expData.controllerVars[0].blendIndex = FindBlendIdx(eyes.eyelids[0].expData.controllerVars[0].smr, blinkL);
                eyes.eyelids[0].expData.controllerVars[0].maxShape = blinkMax;
                // Right eyelid
                eyes.eyelids[1].referenceIdx = 1;
                eyes.eyelids[1].expData.controllerVars[0].smr = eyes.eyelids[0].expData.controllerVars[0].smr;
                eyes.eyelids[1].expData.controllerVars[0].blendIndex = FindBlendIdx(eyes.eyelids[1].expData.controllerVars[0].smr, blinkR);
                eyes.eyelids[1].expData.controllerVars[0].maxShape = blinkMax;

                // Initialize the Eyes module
                eyes.Initialize();
			}
		}

		public static Transform FindTransform(Transform charRoot, string regexSearch)
		{
			Regex regex = new Regex(regexSearch, RegexOptions.IgnoreCase);
			Transform[] bones = charRoot.GetComponentsInChildren<Transform>();
			foreach (Transform bone in bones)
				if (regex.IsMatch(bone.name))
					return bone;
			return null;
		}

		public static int FindBlendIdx(SkinnedMeshRenderer smr, string regexSearch)
		{
			Regex regex = new Regex(regexSearch, RegexOptions.IgnoreCase);
			for (int i = 0; i < smr.sharedMesh.blendShapeCount; i++)
				if (regex.IsMatch(smr.sharedMesh.GetBlendShapeName(i)))
					return i;
			return -1;
		}
	}
}