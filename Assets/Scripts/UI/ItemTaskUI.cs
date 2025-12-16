using Static;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ItemTaskUI : MonoBehaviour
    {
        [SerializeField] private int _id;
        [SerializeField] private Text _taskText;
        [SerializeField] private Image _progressImage;
        [SerializeField] private Text _progressText;
        [SerializeField] private Transform _claimButton;
        [SerializeField] private Transform _adsButton;
        [SerializeField] private Transform _claimedButton;

        private int _count;

        public void SetText(string text) => _taskText.text = text;
        public string GetText() => _taskText.text;

        public void SetCount(int currentProgress, int count)
        {
            _count = count;
            var clampedProgress = 0f;
            
            if (StaticData.GetSkippedTaskIndex(_id) == 1)
            {
                clampedProgress = count;
                _progressImage.fillAmount = 1f;
                _progressText.text = $"{_count}/{_count}";
            }
            else
            {
                clampedProgress = Mathf.Clamp(currentProgress, 0, count);
                _progressImage.fillAmount = clampedProgress / count;
                _progressText.text = $"{clampedProgress}/{count}";
            }
            
            if (!Mathf.Approximately(clampedProgress, count)) return;
            
            _claimButton.gameObject.SetActive(true);
            _adsButton.gameObject.SetActive(false);

            if (StaticData.GetClaimedTaskIndex(_id) == 1)
                ClaimRewardVisual();
        }

        public void SkipTask()
        {
            StaticData.SetSkippedTaskIndex(_id, 1);
            
            _progressImage.fillAmount = 1f;
            _progressText.text = $"{_count}/{_count}";
            
            _claimButton.gameObject.SetActive(true);
            _adsButton.gameObject.SetActive(false);
        }

        private void ClaimRewardVisual()
        {
            _claimButton.gameObject.SetActive(false);
            _adsButton.gameObject.SetActive(false);
            _claimedButton.gameObject.SetActive(true);
        }

        public void ClaimReward()
        {
            StaticData.SetClaimedTaskIndex(_id, 1);
            ClaimRewardVisual();
            
            FindObjectOfType<DailyRewardGridUI>().UpdateProgress();
        }
    }
}
