using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameCycle;
using SO;
using Static;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ProgressUI : MonoBehaviour
    {
        public static event Action OnStartFillProgress;
        public static event Action OnFinishFillProgress;
        public static event Action OnWinMenuClosed;
        
        [SerializeField] private float _delay;
        [SerializeField] private Image _fillImage;
        [SerializeField] private Text _percentText;
        
        private void OnEnable()
        {
            if (LevelManager.Instance.CurrentLevelCount > 80)
            {
                transform.parent.gameObject.SetActive(false);
                return;
            }
            
            SetProgress(StaticData.RewardProgress);
            
            GameController.OnFillProgress += GameControllerOnOnFillProgress;
            OnStartFillProgress += MyOnStartFillProgress;
        }

        private void OnDisable()
        {
            GameController.OnFillProgress -= GameControllerOnOnFillProgress;
            OnStartFillProgress -= MyOnStartFillProgress;
        }
        
        private void MyOnStartFillProgress() => StartCoroutine(WaitForSetProgress());

        private static void GameControllerOnOnFillProgress() => OnStartFillProgress?.Invoke();

        private IEnumerator WaitForSetProgress()
        {
            yield return new WaitForSeconds(_delay);
            
            if (StaticData.RewardProgress < StaticData.ProgressLevel * StaticData.RewardProgressStep)
            {
                if(GameSettings.Instance.stages.rewardLevels.Contains(LevelManager.Instance.CurrentLevelCount))
                    SetRewardProgress(StaticData.RewardProgress, 100 - StaticData.RewardProgress, 0.7f);
                else
                {
                    var step = StaticData.RewardProgress + StaticData.RewardProgressStep >= 100 ? 75 : 100;
                    SetRewardProgress(StaticData.RewardProgress, StaticData.RewardProgressStep, 0.7f);
                }
            }
                
            else
                OnFinishFillProgress?.Invoke();
        }

        private void SetRewardProgress(int startValue, int progressStep, float duration)
        {
            StartCoroutine(SetProgress(startValue, progressStep, duration));
        }

        private IEnumerator SetProgress(int startValue, int progressStep, float duration)
        {
            var targetValue = startValue + progressStep;
            if (100 - targetValue == 1) targetValue = 100;
            
            StaticData.RewardProgress = targetValue > 100 ? 100 : targetValue;
            
            var timer = 0f;
        
            while (timer < duration)
            {
                var progress = timer / duration;

                var currentProgress = Mathf.Lerp(startValue, targetValue, progress);

                if (currentProgress > 100) break;
            
                SetProgress(currentProgress);
            
                timer += Time.deltaTime;

                yield return null;
            }

            var progressPercent = targetValue > 100 ? 100 : targetValue;
            SetProgress(progressPercent);

            StaticData.RewardProgress = progressPercent;

            if (progressPercent >= 100 &&
                GameSettings.Instance.stages.rewardLevels.Contains(LevelManager.Instance.CurrentLevelCount))
                StartCoroutine(nameof(SwitchToRewardMenu));
            else
                OnFinishFillProgress?.Invoke();
        }

        private IEnumerator SwitchToRewardMenu()
        {
            yield return new WaitForSeconds(1f);
            
            OnFinishFillProgress?.Invoke();
            GameController.Instance.GetReward();
        }

        public void ResetRewardProgress()
        {
            if (StaticData.RewardProgress >= 100)
                StaticData.RewardProgress = 0;
            
            OnWinMenuClosed?.Invoke();
        }

        private void SetProgress(float progress)
        {
            var targetProgress = Mathf.RoundToInt(progress);
            SetProgressImage(targetProgress);
            SetProgressText(targetProgress);
        }
        
        private void SetProgressImage(float progress) => _fillImage.fillAmount = progress * 0.01f;
        private void SetProgressText(float progress) => _percentText.text = $"{progress}%";
    }
}
