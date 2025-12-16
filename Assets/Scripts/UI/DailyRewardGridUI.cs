using System;
using System.Collections;
using Static;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DailyRewardGridUI : MonoBehaviour
    {
        public static event Action OnFirstRewardClaimed;
        public static event Action OnSecondRewardClaimed;
        public static event Action OnThirdRewardClaimed;
        
        [SerializeField] private Image _progressImage;

        private void OnEnable()
        {
            UpdateProgress();
        }

        public void UpdateProgress()
        {
            _progressImage.fillAmount = 0f;
            
            for (var i = 0; i < 5; i++)
            {
                if (StaticData.GetClaimedTaskIndex(i) == 1)
                    IncProgress();
            }

            StartCoroutine(ClaimRewards());
        }

        private IEnumerator ClaimRewards()
        {
            yield return new WaitForSeconds(0.1f);
            
            if(_progressImage.fillAmount >= 0.2f)
                OnFirstRewardClaimed?.Invoke();
            
            if(_progressImage.fillAmount >= 0.5f)
                OnSecondRewardClaimed?.Invoke();
            
            if(_progressImage.fillAmount >= 1f)
                OnThirdRewardClaimed?.Invoke();
        }

        private void IncProgress()
        {
            _progressImage.fillAmount += 0.2f;
        }
    }
}
