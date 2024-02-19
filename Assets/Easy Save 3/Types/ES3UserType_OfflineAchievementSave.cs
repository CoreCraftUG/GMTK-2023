using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("UserSteamID", "Score1000Achievement", "Score2500Achievement", "Score10000Achievement", "Score20000Achievement", "Score100000Achievement", "Multiply2", "Multiply3", "Multiply4", "Multiply5", "Multiply10", "Level5", "Level10", "Level15", "Level20", "RedRow", "GreenRow", "BlueRow", "YellowRow", "PurpleRow", "Rows2", "Rows3", "Rows4", "BronzeCenter", "SilverCenter", "GoldCenter", "SuccessCenter", "MatchTree", "TutorialCleared", "DecoSlots", "ClearNeighbourRow", "FillGrids", "RimExplosion")]
	public class ES3UserType_OfflineAchievementSave : ES3Type
	{
		public static ES3Type Instance = null;

		public ES3UserType_OfflineAchievementSave() : base(typeof(SteamAchievementHelper.OfflineAchievementSave)){ Instance = this; priority = 1;}


		public override void Write(object obj, ES3Writer writer)
		{
			var instance = (SteamAchievementHelper.OfflineAchievementSave)obj;
			
			writer.WriteProperty("UserSteamID", instance.UserSteamID, ES3Type_ulong.Instance);
			writer.WriteProperty("Score1000Achievement", instance.Score1000Achievement, ES3Type_bool.Instance);
			writer.WriteProperty("Score2500Achievement", instance.Score2500Achievement, ES3Type_bool.Instance);
			writer.WriteProperty("Score10000Achievement", instance.Score10000Achievement, ES3Type_bool.Instance);
			writer.WriteProperty("Score20000Achievement", instance.Score20000Achievement, ES3Type_bool.Instance);
			writer.WriteProperty("Score100000Achievement", instance.Score100000Achievement, ES3Type_bool.Instance);
			writer.WriteProperty("Multiply2", instance.Multiply2, ES3Type_bool.Instance);
			writer.WriteProperty("Multiply3", instance.Multiply3, ES3Type_bool.Instance);
			writer.WriteProperty("Multiply4", instance.Multiply4, ES3Type_bool.Instance);
			writer.WriteProperty("Multiply5", instance.Multiply5, ES3Type_bool.Instance);
			writer.WriteProperty("Multiply10", instance.Multiply10, ES3Type_bool.Instance);
			writer.WriteProperty("Level5", instance.Level5, ES3Type_bool.Instance);
			writer.WriteProperty("Level10", instance.Level10, ES3Type_bool.Instance);
			writer.WriteProperty("Level15", instance.Level15, ES3Type_bool.Instance);
			writer.WriteProperty("Level20", instance.Level20, ES3Type_bool.Instance);
			writer.WriteProperty("RedRow", instance.RedRow, ES3Type_bool.Instance);
			writer.WriteProperty("GreenRow", instance.GreenRow, ES3Type_bool.Instance);
			writer.WriteProperty("BlueRow", instance.BlueRow, ES3Type_bool.Instance);
			writer.WriteProperty("YellowRow", instance.YellowRow, ES3Type_bool.Instance);
			writer.WriteProperty("PurpleRow", instance.PurpleRow, ES3Type_bool.Instance);
			writer.WriteProperty("Rows2", instance.Rows2, ES3Type_bool.Instance);
			writer.WriteProperty("Rows3", instance.Rows3, ES3Type_bool.Instance);
			writer.WriteProperty("Rows4", instance.Rows4, ES3Type_bool.Instance);
			writer.WriteProperty("BronzeCenter", instance.BronzeCenter, ES3Type_bool.Instance);
			writer.WriteProperty("SilverCenter", instance.SilverCenter, ES3Type_bool.Instance);
			writer.WriteProperty("GoldCenter", instance.GoldCenter, ES3Type_bool.Instance);
			writer.WriteProperty("SuccessCenter", instance.SuccessCenter, ES3Type_bool.Instance);
			writer.WriteProperty("MatchTree", instance.MatchTree, ES3Type_bool.Instance);
			writer.WriteProperty("TutorialCleared", instance.TutorialCleared, ES3Type_bool.Instance);
			writer.WriteProperty("DecoSlots", instance.DecoSlots, ES3Type_bool.Instance);
			writer.WriteProperty("ClearNeighbourRow", instance.ClearNeighbourRow, ES3Type_bool.Instance);
			writer.WriteProperty("FillGrids", instance.FillGrids, ES3Type_bool.Instance);
			writer.WriteProperty("RimExplosion", instance.RimExplosion, ES3Type_bool.Instance);
		}

		public override object Read<T>(ES3Reader reader)
		{
			var instance = new SteamAchievementHelper.OfflineAchievementSave();
			string propertyName;
			while((propertyName = reader.ReadPropertyName()) != null)
			{
				switch(propertyName)
				{
					
					case "UserSteamID":
						instance.UserSteamID = reader.Read<System.UInt64>(ES3Type_ulong.Instance);
						break;
					case "Score1000Achievement":
						instance.Score1000Achievement = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "Score2500Achievement":
						instance.Score2500Achievement = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "Score10000Achievement":
						instance.Score10000Achievement = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "Score20000Achievement":
						instance.Score20000Achievement = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "Score100000Achievement":
						instance.Score100000Achievement = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "Multiply2":
						instance.Multiply2 = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "Multiply3":
						instance.Multiply3 = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "Multiply4":
						instance.Multiply4 = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "Multiply5":
						instance.Multiply5 = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "Multiply10":
						instance.Multiply10 = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "Level5":
						instance.Level5 = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "Level10":
						instance.Level10 = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "Level15":
						instance.Level15 = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "Level20":
						instance.Level20 = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "RedRow":
						instance.RedRow = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "GreenRow":
						instance.GreenRow = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "BlueRow":
						instance.BlueRow = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "YellowRow":
						instance.YellowRow = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "PurpleRow":
						instance.PurpleRow = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "Rows2":
						instance.Rows2 = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "Rows3":
						instance.Rows3 = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "Rows4":
						instance.Rows4 = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "BronzeCenter":
						instance.BronzeCenter = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "SilverCenter":
						instance.SilverCenter = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "GoldCenter":
						instance.GoldCenter = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "SuccessCenter":
						instance.SuccessCenter = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "MatchTree":
						instance.MatchTree = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "TutorialCleared":
						instance.TutorialCleared = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "DecoSlots":
						instance.DecoSlots = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "ClearNeighbourRow":
						instance.ClearNeighbourRow = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "FillGrids":
						instance.FillGrids = reader.Read<System.Boolean>(ES3Type_bool.Instance);
						break;
					case "RimExplosion":
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