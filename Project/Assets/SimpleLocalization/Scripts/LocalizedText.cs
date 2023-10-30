using UnityEngine;
using TMPro;

namespace Assets.SimpleLocalization.Scripts
{
	/// <summary>
	/// Localize text component.
	/// </summary>
    //[RequireComponent(typeof(text))]
    public class LocalizedText : MonoBehaviour
    {
        public string LocalizationKey;

        public void Start()
        {
            Localize();
            LocalizationManager.OnLocalizationChanged += Localize;
        }

        public void OnDestroy()
        {
            LocalizationManager.OnLocalizationChanged -= Localize;
        }

        private void Localize()
        {
            GetComponent<TextMeshPro>().text = LocalizationManager.Localize(LocalizationKey);
        }
    }
}