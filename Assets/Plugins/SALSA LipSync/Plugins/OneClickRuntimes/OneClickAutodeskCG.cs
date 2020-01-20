using System.Collections.Generic;
using UnityEngine;

namespace CrazyMinnow.SALSA.OneClicks
{
	public class OneClickAutodeskCG : OneClickBase
	{
		/// <summary>
		/// RELEASE NOTES:
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
		public static void Setup(GameObject gameObject, AudioClip clip, int lodLevel)
		{
			////////////////////////////////////////////////////////////////////////////////////////////////////////////
			//	SETUP Requirements:
			//		use NewViseme("viseme name") to start a new viseme.
			//		use AddShapeComponent to add blendshape configurations, passing:
			//			- string array of shape names to look for.
			//			- optional string name prefix for the component.
			//			- optional blend amount (default = 1.0f).

			string bodySearch = "";
			string teethSearch = "";
			switch (lodLevel)
			{
				case 0:
					bodySearch = "^H_DDS_CrowdRes$";
					teethSearch = "^c_teethdown$";
					break;
				case 1:
					bodySearch = "^H_DDS_LowRes$";
					teethSearch = "^l_teethdown$";
					break;
				case 2:
					bodySearch = "^H_DDS_MidRes$";
					teethSearch = "^m_teethdown$";
					break;
				case 3:
					bodySearch = "^H_DDS_HighRes$";
					teethSearch = "^h_teethdown$";
					break;
			}

			Init();

			#region SALSA-Configuration

			NewConfiguration(OneClickConfiguration.ConfigType.Salsa);
			{
				////////////////////////////////////////////////////////
				// SMR regex searches (enable/disable/add as required).
				AddSmrSearch(bodySearch);
				AddSmrSearch(teethSearch);


				////////////////////////////////////////////////////////
				// Adjust SALSA settings to taste...
				// - data analysis settings
				autoAdjustAnalysis = true;
				autoAdjustMicrophone = false;
				// - advanced dynamics settings
				loCutoff = 0.03f;
				hiCutoff = 0.75f;
				useAdvDyn = true;
				advDynPrimaryBias = 0.5f;
				useAdvDynJitter = true;
				advDynJitterAmount = 0.1f;
				advDynJitterProb = 0.25f;
				advDynSecondaryMix = 0f;
				emphasizerTrigger = 0.2f;


				////////////////////////////////////////////////////////
				// Viseme setup...


				NewExpression("w");
				AddShapeComponent(new[] {"c_teeth.t_UW_U_c", "l_teeth.t_UW_U_l", "m_teeth.t_UW_U_m", "h_teeth.t_UW_U_h"}, 0.08f, 0f, 0.06f, "t_UW_U", 1f);
				AddShapeComponent(new[] {"c_expressions.UW_U_c", "l_expressions.UW_U_l", "m_expressions.UW_U_m", "h_expressions.UW_U_h"}, 0.08f, 0f, 0.06f, "UW_U", 1f);


				NewExpression("t");
				AddShapeComponent(new[] {"c_teeth.t_TD_I_c", "l_teeth.t_TD_I_l", "m_teeth.t_TD_I_m", "h_teeth.t_TD_I_h"}, 0.08f, 0f, 0.06f, "t_TD_I", 1f);
				AddShapeComponent(new[] {"c_expressions.TD_I_c", "l_expressions.TD_I_l", "m_expressions.TD_I_m", "h_expressions.TD_I_h"}, 0.08f, 0f, 0.06f, "TD_I", 1f);
				AddShapeComponent(new[] {"c_expressions.UW_U_c", "l_expressions.UW_U_l", "m_expressions.UW_U_m", "h_expressions.UW_U_h"}, 0.08f, 0f, 0.06f, "UW_U", 0.602f);


				NewExpression("f");
				AddShapeComponent(new[] {"c_teeth.t_FV_c","l_teeth.t_FV_l","m_teeth.t_FV_m","h_teeth.t_FV_h"}, 0.08f, 0f, 0.06f, "t_FV", 1f);
				AddShapeComponent(new[] {"c_expressions.FV_c","l_expressions.FV_l","m_expressions.FV_m","h_expressions.FV_h"}, 0.08f, 0f, 0.06f, "FV", 1f);


				NewExpression("th");
				AddShapeComponent(new[] {"c_teeth.t_KG_c","l_teeth.t_KG_l","m_teeth.t_KG_m","h_teeth.t_KG_h"}, 0.08f, 0f, 0.06f, "t_KG", 1f);
				AddShapeComponent(new[] {"c_expressions.TD_I_c", "l_expressions.TD_I_l", "m_expressions.TD_I_m", "h_expressions.TD_I_h"}, 0.08f, 0f, 0.06f, "TD_I", 1f);
				AddShapeComponent(new[] {"c_expressions.UW_U_c", "l_expressions.UW_U_l", "m_expressions.UW_U_m", "h_expressions.UW_U_h"}, 0.08f, 0f, 0.06f, "UW_U", 0.721f);


				NewExpression("ow");
				AddShapeComponent(new[] {"c_teeth.t_AO_a_c","l_teeth.t_AO_a_l","m_teeth.t_AO_a_m","h_teeth.t_AO_a_h"}, 0.08f, 0f, 0.06f, "t_AO_a", 0.735f);
				AddShapeComponent(new[] {"c_expressions.AO_a_c","l_expressions.AO_a_l","m_expressions.AO_a_m","h_expressions.AO_a_h"}, 0.08f, 0f, 0.06f, "AO_a", 0.246f);
				AddShapeComponent(new[] {"c_expressions.UH_OO_c","l_expressions.UH_OO_l","m_expressions.UH_OO_m","h_expressions.UH_OO_h"}, 0.08f, 0f, 0.06f, "UH_OO", 0.986f);


				NewExpression("ee");
				AddShapeComponent(new[] {"c_teeth.t_H_EST_c","l_teeth.t_H_EST_l","m_teeth.t_H_EST_m","h_teeth.t_H_EST_h"}, 0.08f, 0f, 0.06f, "t_H_EST", 0.588f);
				AddShapeComponent(new[] {"c_expressions.S_c","l_expressions.S_l","m_expressions.S_m","h_expressions.S_h"}, 0.08f, 0f, 0.06f, "S", 0.665f);
				AddShapeComponent(new[] {"c_expressions.H_EST_c","l_expressions.H_EST_l","m_expressions.H_EST_m","h_expressions.H_EST_h"}, 0.08f, 0f, 0.06f, "H_EST", 0.804f);


				NewExpression("oo");
				AddShapeComponent(new[] {"c_teeth.t_AO_a_c","l_teeth.t_AO_a_l","m_teeth.t_AO_a_m","h_teeth.t_AO_a_h"}, 0.08f, 0f, 0.06f, "t_AO_a", 1f);
				AddShapeComponent(new[] {"c_expressions.AO_a_c","l_expressions.AO_a_l","m_expressions.AO_a_m","h_expressions.AO_a_h"}, 0.08f, 0f, 0.06f, "AO_a", 1f);
			}

			#endregion // SALSA-configuration


			#region EmoteR-Configuration

			NewConfiguration(OneClickConfiguration.ConfigType.Emoter);
			{
				////////////////////////////////////////////////////////
				// SMR regex searches (enable/disable/add as required).
				AddSmrSearch(bodySearch);

				useRandomEmotes = true;
				isChancePerEmote = true;
				numRandomEmotesPerCycle = 0;
				randomEmoteMinTimer = 1f;
				randomEmoteMaxTimer = 2f;
				randomChance = 0.5f;
				useRandomFrac = false;
				randomFracBias = 0.5f;
				useRandomHoldDuration = false;
				randomHoldDurationMin = 0.1f;
				randomHoldDurationMax = 0.5f;


				NewExpression("exasper");
				AddEmoteFlags(false, true, false, 1f);
				AddShapeComponent(new[] {"c_expressions.Lblow_c","l_expressions.Lblow_l","m_expressions.Lblow_m","h_expressions.Lblow_h"}, 0.25f, 0.1f, 0.2f, "Lblow", 0.483f);
				AddShapeComponent(new[] {"c_expressions.Rblow_c","l_expressions.Rblow_l","m_expressions.Rblow_m","h_expressions.Rblow_h"}, 0.25f, 0.1f, 0.2f, "Rblow", 0.441f);
				AddShapeComponent(new[] {"c_expressions.LbrowUp_c","l_expressions.LbrowUp_l","m_expressions.LbrowUp_m","h_expressions.LbrowUp_h"}, 0.25f, 0.1f, 0.2f, "LbrowUp", 0.714f);
				AddShapeComponent(new[] {"c_expressions.RbrowUp_c","l_expressions.RbrowUp_l","m_expressions.RbrowUp_m","h_expressions.RbrowUp_h"}, 0.25f, 0.1f, 0.2f, "RbrowUp", 1f);


				NewExpression("soften");
				AddEmoteFlags(false, true, false, 1f);
				AddShapeComponent(new[] {"c_expressions.RsmileClose_c","l_expressions.RsmileClose_l","m_expressions.RsmileClose_m","h_expressions.RsmileClose_h"}, 0.25f, 0.1f, 0.2f, "RsmileClose", 0.804f);
				AddShapeComponent(new[] {"c_expressions.LsmileClose_c","l_expressions.LsmileClose_l","m_expressions.LsmileClose_m","h_expressions.LsmileClose_h"}, 0.25f, 0.1f, 0.2f, "LsmileClose", 0.707f);
				AddShapeComponent(new[] {"c_expressions.RlowLid_c","l_expressions.RlowLid_l","m_expressions.RlowLid_m","h_expressions.RlowLid_h"}, 0.25f, 0.1f, 0.2f, "RlowLid", 0.232f);
				AddShapeComponent(new[] {"c_expressions.LlowLid_c","l_expressions.LlowLid_l","m_expressions.LlowLid_m","h_expressions.LlowLid_h"}, 0.25f, 0.1f, 0.2f, "LlowLid", 0.197f);
				AddShapeComponent(new[] {"c_expressions.LbrowUp_c","l_expressions.LbrowUp_l","m_expressions.LbrowUp_m","h_expressions.LbrowUp_h"}, 0.25f, 0.1f, 0.2f, "LbrowUp", 1f);
				AddShapeComponent(new[] {"c_expressions.RbrowUp_c","l_expressions.RbrowUp_l","m_expressions.RbrowUp_m","h_expressions.RbrowUp_h"}, 0.25f, 0.1f, 0.2f, "RbrowUp", 1f);


				NewExpression("browsUp");
				AddEmoteFlags(false, true, false, 1f);
				AddShapeComponent(new[] {"c_expressions.RbrowUp_c","l_expressions.RbrowUp_l","m_expressions.RbrowUp_m","h_expressions.RbrowUp_h"}, 0.25f, 0.1f, 0.2f, "RbrowUp", 1f);
				AddShapeComponent(new[] {"c_expressions.LbrowUp_c","l_expressions.LbrowUp_l","m_expressions.LbrowUp_m","h_expressions.LbrowUp_h"}, 0.25f, 0.1f, 0.2f, "LbrowUp", 1f);
				AddShapeComponent(new[] {"c_expressions.RRbrowUp_c","l_expressions.RRbrowUp_l","m_expressions.RRbrowUp_m","h_expressions.RRbrowUp_h"}, 0.25f, 0.1f, 0.2f, "RRbrowUp", 1f);
				AddShapeComponent(new[] {"c_expressions.LLbrowUp_c","l_expressions.LLbrowUp_l","m_expressions.LLbrowUp_m","h_expressions.LLbrowUp_h"}, 0.25f, 0.1f, 0.2f, "LLbrowUp", 0.532f);


				NewExpression("browUp");
				AddEmoteFlags(false, true, false, 1f);
				AddShapeComponent(new[] {"c_expressions.RbrowUp_c","l_expressions.RbrowUp_l","m_expressions.RbrowUp_m","h_expressions.RbrowUp_h"}, 0.25f, 0.1f, 0.2f, "RbrowUp", 1f);
				AddShapeComponent(new[] {"c_expressions.RRbrowUp_c","l_expressions.RRbrowUp_l","m_expressions.RRbrowUp_m","h_expressions.RRbrowUp_h"}, 0.25f, 0.1f, 0.2f, "RRbrowUp", 0.971f);
				AddShapeComponent(new[] {"c_expressions.LbrowUp_c","l_expressions.LbrowUp_l","m_expressions.LbrowUp_m","h_expressions.LbrowUp_h"}, 0.25f, 0.1f, 0.2f, "LbrowUp", 0.539f);


				NewExpression("squint");
				AddEmoteFlags(false, true, false, 1f);
				AddShapeComponent(new[] {"c_expressions.Rsquint_c","l_expressions.Rsquint_l","m_expressions.Rsquint_m","h_expressions.Rsquint_h"}, 0.2f, 0.05f, 0.1f, "Rsquint", 0.204f);
				AddShapeComponent(new[] {"c_expressions.Lsquint_c","l_expressions.Lsquint_l","m_expressions.Lsquint_m","h_expressions.Lsquint_h"}, 0.2f, 0.05f, 0.1f, "Lsquint", 0.134f);
				AddShapeComponent(new[] {"c_expressions.RRbrowDown_c","l_expressions.RRbrowDown_l","m_expressions.RRbrowDown_m","h_expressions.RRbrowDown_h"}, 0.2f, 0.05f, 0.1f, "RRbrowDown", 0.51f);
				AddShapeComponent(new[] {"c_expressions.LLbrowDown_c","l_expressions.LLbrowDown_l","m_expressions.LLbrowDown_m","h_expressions.LLbrowDown_h"}, 0.2f, 0.05f, 0.1f, "LLbrowDown", 0.785f);


				NewExpression("focus");
				AddEmoteFlags(false, true, false, 0.517f);
				AddShapeComponent(new[] {"c_expressions.RlowLid_c","l_expressions.RlowLid_l","m_expressions.RlowLid_m","h_expressions.RlowLid_h"}, 0.2f, 0.1f, 0.1f, "RlowLid", 1f);
				AddShapeComponent(new[] {"c_expressions.LlowLid_c","l_expressions.LlowLid_l","m_expressions.LlowLid_m","h_expressions.LlowLid_h"}, 0.2f, 0.1f, 0.1f, "LlowLid", 1f);


				NewExpression("flare");
				AddEmoteFlags(false, true, false, 1f);
				AddShapeComponent(new[] {"c_expressions.Rnostril_c","l_expressions.Rnostril_l","m_expressions.Rnostril_m","h_expressions.Rnostril_h"}, 0.2f, 0.05f, 0.1f, "Rnostril", 1f);
				AddShapeComponent(new[] {"c_expressions.Lnostril_c","l_expressions.Lnostril_l","m_expressions.Lnostril_m","h_expressions.Lnostril_h"}, 0.2f, 0.05f, 0.1f, "Lnostril", 1f);


				NewExpression("scrunch");
				AddEmoteFlags(false, true, false, 1f);
				AddShapeComponent(new[] {"c_expressions.Rdisgust_c","l_expressions.Rdisgust_l","m_expressions.Rdisgust_m","h_expressions.Rdisgust_h"}, 0.2f, 0.1f, 0.2f, "h_expressions.Rdisgust_h", 0.703f);
				AddShapeComponent(new[] {"c_expressions.Ldisgust_c","l_expressions.Ldisgust_l","m_expressions.Ldisgust_m","h_expressions.Ldisgust_h"}, 0.2f, 0.1f, 0.2f, "h_expressions.Ldisgust_h", 0.667f);
			}

			#endregion // EmoteR-configuration

			DoOneClickiness(gameObject, clip);
		}
	}
}