using Assets.Scripts.Utils;
using TMPro;
using UnityEngine;

namespace Assets.Scrtips.Utils
{
	[RequireComponent(typeof(TextMeshProUGUI))]
	public class BuildDisplayer : MonoBehaviour
	{
		private TextMeshProUGUI _buildText;

		private void Awake()
		{
			_buildText = GetComponent<TextMeshProUGUI>();
			var resourceLoader = Resources.LoadAsync<BuildNumberRepository>("BuildNumberRepository");
			resourceLoader.completed += ResourceLoader_completed;
		}

		private void ResourceLoader_completed(AsyncOperation obj)
		{
			var buildNumberRepository = ((ResourceRequest)obj).asset as BuildNumberRepository;
			if (buildNumberRepository != null)
			{
				var version = $"{Application.version}.{buildNumberRepository.BuildNumber}";
				var text = _buildText.text;
				if(!string.IsNullOrEmpty(text))
				{
					_buildText.text = text.Replace("#version#", version);
				}
				else
				{
					_buildText.SetText($"Build: v{version}");
				}
			}
			else
			{
				Debug.LogError($"Expected {nameof(BuildNumberRepository)} but found nothing in assets/resources/BuildNumberRepository.asset");
			}
		}
	}
}