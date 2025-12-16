using System;
using System.Collections;
using System.Collections.Generic;
using GameCycle;
using SO;
using Static;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class LockedButtonUI : MonoCashed<Button>
    {
        [SerializeField] private UnityEvent _onClick;
        [SerializeField] private bool _withAd;

        public bool isMultiplyButton;
        
        private void OnEnable()
        {
            Cashed1.onClick.AddListener(OnClick);
            
            ProgressUI.OnStartFillProgress += ProgressUIOnOnStartFillProgress;
            ProgressUI.OnFinishFillProgress += ProgressUIOnOnFinishFillProgress;
        }

        private void OnDisable()
        {
            Cashed1.onClick.RemoveListener(OnClick);
            
            ProgressUI.OnStartFillProgress -= ProgressUIOnOnStartFillProgress;
            ProgressUI.OnFinishFillProgress -= ProgressUIOnOnFinishFillProgress;
        }

        private void OnClick()
        {
            if (_withAd)
                StartCoroutine(ShowAd());
            else
                _onClick?.Invoke();
        }

        private IEnumerator ShowAd()
        {
            GameController.ShowAd();
            yield return null;
            
            _onClick?.Invoke();
        }

        private void ProgressUIOnOnFinishFillProgress()
        {
            Cashed1.enabled = true;
        }

        private void ProgressUIOnOnStartFillProgress()
        {
            Cashed1.enabled = false;
            if (isMultiplyButton)
                StartCoroutine(Fill());
        }

        private IEnumerator Fill()
        {
            yield return null;

            if(!GameSettings.Instance.stages.rewardLevels.Contains(LevelManager.Instance.CurrentLevelCount))
                Cashed1.enabled = true;
        }
    }
}
