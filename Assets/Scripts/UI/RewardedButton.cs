using System;
using Static;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using YG;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class RewardedButton : MonoCashed<Button>
    {
        [SerializeField] private int RewardId;
        [SerializeField] private UnityEvent _onReward;

        private void OnEnable()
        {
            Cashed1.onClick.AddListener(RaceAgainClicked);
            YandexGame.RewardVideoEvent += Reward;
        }

        private void OnDisable()
        {
            Cashed1.onClick.RemoveListener(RaceAgainClicked);
            YandexGame.RewardVideoEvent -= Reward;
        }

        private void RaceAgainClicked() => YandexGame.RewVideoShow(RewardId);
        private void Reward(int id)
        {
            if (id != RewardId) return;
            
            _onReward?.Invoke();
            StaticData.AdWatchedCount++;
            YandexGame.timerShowAd = 0;
        }

        public void SetRewardId(int id) => RewardId = id;
    }
}