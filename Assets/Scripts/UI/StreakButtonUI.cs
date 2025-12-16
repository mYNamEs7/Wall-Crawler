using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace UI
{
    public class StreakButtonUI : MonoCashed<Button>
    {
        private void OnEnable()
        {
            Cashed1.onClick.AddListener(OnClick);
            
            StreakRewardUI.StartGetReward += ProgressUIOnOnStartFillProgress;
        }

        private void OnDisable()
        {
            Cashed1.onClick.RemoveListener(OnClick);
            
            StreakRewardUI.StartGetReward -= ProgressUIOnOnStartFillProgress;
        }

        private void OnClick() => Cashed1.onClick?.Invoke();

        private void ProgressUIOnOnStartFillProgress(int delay)
        {
            StartCoroutine(DisableButton(delay));
        }

        private IEnumerator DisableButton(float delay)
        {
            Cashed1.enabled = false;
            
            yield return new WaitForSeconds(delay);
            
            Cashed1.enabled = true;
        }
    }
}
