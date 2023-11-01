using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Assets.SimpleLocalization.Scripts
{
	/// <summary>
	/// Localize text component.
	/// </summary>
    //[RequireComponent(typeof(Text))]
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
            if(GetComponent<Text>() != null) 
            {
                GetComponent<Text>().text = LocalizationManager.Localize(LocalizationKey);
            }
            else
                GetComponent<TextMeshProUGUI>().text = LocalizationManager.Localize(LocalizationKey);
        }
    }
}