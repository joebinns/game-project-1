using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIHandler : MonoBehaviour
    {
        [SerializeField] private Canvas _effectCanvas;
        [SerializeField] private Image _effectImage;
        [SerializeField] private TMP_Text _effectText;

        public void ActivateEffectCanvas()
        {
            _effectCanvas.enabled = true;
        }
        
        public void DeactivateEffectCanvas()
        {
            _effectCanvas.enabled = false;
        }
        
        public void SetEffectSprite(Sprite sprite)
        {
            _effectImage.sprite = sprite;
        }

        public void SetEffectText(string text)
        {
            _effectText.text = text;
        }
    }
}
