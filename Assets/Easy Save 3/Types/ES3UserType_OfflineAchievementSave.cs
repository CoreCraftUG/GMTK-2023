using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("_score1000Achievement", "_score2500Achievement", "_score10000Achievement", "_score20000Achievement", "_score100000Achievement", "_multiply2", "_multiply3", "_multiply4", "_multiply5", "_multiply10", "_level5", "_level10", "_level15", "_level20", "_redRow", "_greenRow", "_blueRow", "_yellowRow", "_purpleRow", "_2Rows", "_3Rows", "_5Rows", "_bronzeCenter", "_silverCenter", "_goldCenter", "_successCenter", "_matchTree", "_tutorialCleared", "_decoSlots", "_clearNeighbourRow", "_fillGrids", "_rimExplosion")]
	public class ES3UserType_OfflineAchievementSave : ES3Type
	{
		public static ES3Type Instance = null;

		public ES3UserType_OfflineAchievementSave() : base(typeof(SteamAchievementHelper.OfflineAchievementSave)){ Instance = this; priority = 1;}


		public override void Write(object obj, ES3Writer writer)
		{
			var instance = (SteamAchievementHelper.OfflineAchievementSave)obj;
			
			writer.WriteProperty("_score1000Achievement", instance.Score1000Achievement, ES3Type_bool.Instance);
			writer.WriteProperty("_score2500Achievement", instance.Score2500Achievement, ES3Type_bool.Instance);
			writer.WriteProperty("_score10000Achievement", instance.Score10000Achievement, ES3Type_bool.Instance);
			writer.WriteProperty("_score20000Achievement", instance.Score20000Achievement, ES3Type_bool.Instance);
			writer.WriteProperty("_score100000Achievement", instance.Score100000Achievement, ES3Type_bool.Instance);
			writer.WriteProperty("_multiply2", instance.Multiply2, ES3Type_bool.Instance);
			writer.WriteProperty("_multiply3", instance.Multiply3, ES3Type_bool.Instance);
			writer.WriteProperty("_multiply4", instance.Multiply4, ES3Type_bool.Instance);
			writer.WriteProperty("_multiply5", instance.Multiply5, ES3Type_bool.Instance);
			writer.WriteProperty("_multiply10", instance.Multiply10, ES3Type_bool.Instance);
			writer.WriteProperty("_level5", instance.Level5, ES3Type_bool.Instance);
			writer.WriteProperty("_level10", instance.Level10, ES3Type_bool.Instance);
			writer.WriteProperty("_level15", instance.Level15, ES3Type_bool.Instance);
			writer.WriteProperty("_level20", instance.Level20, ES3Type_bool.Instance);
			writer.WriteProperty("_redRow", instance.RedRow, ES3Type_bool.Instance);
			writer.WriteProperty("_greenRow", instance.GreenRow, ES3Type_bool.Instance);
			writer.WriteProperty("_blueRow", instance.BlueRow, ES3Type_bool.Instance);
			writer.WriteProperty("_yellowRow", instance.YellowRow, ES3Type_bool.Instance);
			writer.WriteProperty("_purpleRow", instance.PurpleRow, ES3Type_bool.Instance);
			writer.WriteProperty("_2Rows", instance.Rows2, ES3Type_bool.Instance);
			writer.WriteProperty("_3Rows", instance.Rows3, ES3Type_bool.Instance);
			writer.WriteProperty("_5Rows", instance.Rows5, ES3Type_bool.Instance);
			writer.WriteProperty("_bronzeCenter", instance.BronzeCenter, ES3Type_bool.Instance);
			writer.WriteProperty("_silverCenter", instance.SilverCenter, ES3Type_bool.Instance);
			writer.WriteProperty("_goldCenter", instance.GoldCenter, ES3Type_bool.Instance);
			writer.WriteProperty("_successCenter", instance.SuccessCenter, ES3Type_bool.Instance);
			writer.WriteProperty("_matchTree", instance.MatchTree, ES3Type_bool.Instance);
			writer.WriteProperty("_tutorialCleared", instance.TutorialCleared, ES3Type_bool.Instance);
			writer.WriteProperty("_decoSlots", instance.DecoSlots, ES3Type_bool.Instance);
			writer.WriteProperty("_clearNeighbourRow", instance.ClearNeighbourRow, ES3Type_bool.Instance);
			writer.WriteProperty("_fillGrids", instance.FillGrids, ES3Type_bool.Instance);
			writer.WriteProperty("_rimExplosion", instance.RimExplosion, ES3Type_bool.Instance);
		}

		public override object Read<T>(ES3Reader reader)
		{
			var instance = new SteamAchievementHelper.OfflineAchievementSave();
			string propertyName;
			while((propertyName = reader.ReadPropertyName()) != null)
			{
				switch(propertyName)
				{
					
					case "_score1000Achievement":
						instance.Score1000Achievement = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "_score2500Achievement":
						instance.Score2500Achievement = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "_score10000Achievement":
						instance.Score10000Achievement = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "_score20000Achievement":
						instance.Score20000Achievement = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "_score100000Achievement":
						instance.Score100000Achievement = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "_multiply2":
						instance.Multiply2 = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "_multiply3":
						instance.Multiply3 = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "_multiply4":
						instance.Multiply4 = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "_multiply5":
						instance.Multiply5 = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "_multiply10":
						instance.Multiply10 = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "_level5":
						instance.Level5 = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "_level10":
						instance.Level10 = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "_level15":
						instance.Level15 = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "_level20":
						instance.Level20 = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "_redRow":
						instance.RedRow = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "_greenRow":
						instance.GreenRow = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "_blueRow":
						instance.BlueRow = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "_yellowRow":
						instance.YellowRow = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "_purpleRow":
						instance.PurpleRow = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "_2Rows":
						instance.Rows2 = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "_3Rows":
						instance.Rows3 = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "_5Rows":
						instance.Rows5 = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "_bronzeCenter":
						instance.BronzeCenter = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "_silverCenter":
						instance.SilverCenter = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "_goldCenter":
						instance.GoldCenter = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "_successCenter":
						instance.SuccessCenter = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "_matchTree":
						instance.MatchTree = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "_tutorialCleared":
						instance.TutorialCleared = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "_decoSlots":
						instance.DecoSlots = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "_clearNeighbourRow":
						instance.ClearNeighbourRow = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "_fillGrids":
						instance.FillGrids = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "_rimExplosion":
						instance.RimExplosion = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					default:
						reader.Skip();
						break;
				}
			}
			return instance;
		}
	}


	public class ES3UserType_OfflineAchievementSaveArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_OfflineAchievementSaveArray() : base(typeof(SteamAchievementHelper.OfflineAchievementSave[]), ES3UserType_OfflineAchievementSave.Instance)
		{
			Instance = this;
		}
	}
}