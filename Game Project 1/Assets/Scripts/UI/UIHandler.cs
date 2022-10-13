using System;
using System.Collections.Generic;
using System.Globalization;
using Managers.Points;
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
        [SerializeField] private List<TMP_Text> _pointsTexts;

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

        private void Update()
        {
            UpdatePointsText();
        }

        public void UpdatePointsText()
        {
            for (int i = 0; i < _pointsTexts.Count; i++)
            {
                _pointsTexts[i].text = PointsManager.GetPoints(i).ToString();
            }
        }
    }
}
