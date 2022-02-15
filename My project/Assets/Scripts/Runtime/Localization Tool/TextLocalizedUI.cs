using UnityEngine;
using TMPro;

namespace RogueLike.Localization
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextLocalizedUI : MonoBehaviour
    {
        TextMeshProUGUI textField;
        public LocalisedString localisedString;
        
        private void OnDisable()
        {
            LocalizationSystem.OnLanguageChanged -= OnValueUpdated;
        }

        private void Start()
        {
            textField = GetComponent<TextMeshProUGUI>();
            textField.text = localisedString.Value;

            LocalizationSystem.OnLanguageChanged += OnValueUpdated;
        }

        private void OnValueUpdated()
        {
            textField.text = localisedString.Value;
        }
    }
}