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
        
        /*
        public void SetEffectSprite(Sprite sprite)
        {
            _effectImage.sprite = sprite;
        }
        */

        public void SetEffectAnimation(EffectAnimation effectAnimation)
        {
            switch (effectAnimation)
            {
                case EffectAnimation.None:
                    _effectImage.GetComponent<Animator>().Play("None");
                    break;
                case EffectAnimation.SlowLeftButtonClick:
                    _effectImage.GetComponent<Animator>().Play("Slow Left Button Click");
                    break;
                case EffectAnimation.FastLeftButtonClickLooped:
                    _effectImage.GetComponent<Animator>().Play("Fast Left Button Click Looped");
                    break;
            }
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

    public enum EffectAnimation
    {
        None,
        SlowLeftButtonClick,
        FastLeftButtonClickLooped
    }
}
