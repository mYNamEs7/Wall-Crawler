using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelStageUI : MarkedUI
    {
        [SerializeField] protected Transform _unmarkImage;
        [SerializeField] private List<Text> _levelTexts;

        public void SetLevel(int level) => _levelTexts.ForEach(text => text.text = $"{level}");

        public override void SetCleared()
        {
            base.SetCleared();
            _unmarkImage.gameObject.SetActive(_isInvert);
        }

        protected override void ResetCleared()
        {
            base.ResetCleared();
            _unmarkImage.gameObject.SetActive(!_isInvert);
        }
    }
}
