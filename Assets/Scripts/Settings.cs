using UnityEngine;

namespace Assets.Scripts
{
	public static class Settings
	{
		private static int? _highScore = null;
		public static int HighScore
		{
			get
			{
				if(_highScore == null)
				{
					_highScore= PlayerPrefs.GetInt(nameof(HighScore), 0);
				}
				return _highScore.Value;
			}
			set
			{
				if (value > HighScore)
				{
					PlayerPrefs.SetInt(nameof(HighScore), value);
					PlayerPrefs.Save();
					_highScore = value;
				}
			}
		}
	}
}
