using System;
using UnityEngine;

namespace ES3Types
{
	[UnityEngine.Scripting.Preserve]
	[ES3PropertiesAttribute("_offlineAchievementSave")]
	public class ES3UserType_SteamAchievementHelper : ES3ComponentType
	{
		public static ES3Type Instance = null;

		public ES3UserType_SteamAchievementHelper() : base(typeof(SteamAchievementHelper)){ Instance = this; priority = 1;}


		protected override void WriteComponent(object obj, ES3Writer writer)
		{
			var instance = (SteamAchievementHelper)obj;
			
			writer.WritePrivateField("_offlineAchievementSave", instance);
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
			var instance = (SteamAchievementHelper)obj;
			foreach(string propertyName in reader.Properties)
			{
				switch(propertyName)
				{
					
					case "_offlineAchievementSave":
					instance = (SteamAchievementHelper)reader.SetPrivateField("_offlineAchievementSave", reader.Read<SteamAchievementHelper.OfflineAchievementSave>(), instance);
					break;
					default:
						reader.Skip();
						break;
				}
			}
		}
	}


	public class ES3UserType_SteamAchievementHelperArray : ES3ArrayType
	{
		public static ES3Type Instance;

		public ES3UserType_SteamAchievementHelperArray() : base(typeof(SteamAchievementHelper[]), ES3UserType_SteamAchievementHelper.Instance)
		{
			Instance = this;
		}
	}
}