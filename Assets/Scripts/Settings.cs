using UnityEngine;

namespace Assets.Scripts
{
	public static class Settings
	{
		public static int HighScore
		{
			get
			{
				return PlayerPrefs.GetInt(nameof(HighScore), 0);
			}
			set
			{
				if(value>HighScore)
				{
					PlayerPrefs.SetInt(nameof(HighScore), value);
					PlayerPrefs.Save();
				}
			}
		}
	}
}
