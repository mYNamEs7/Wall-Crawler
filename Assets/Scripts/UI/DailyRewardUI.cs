using System;
using GameCycle;
using Shop;
using Static;
using UnityEngine;

namespace UI
{
    public class DailyRewardUI : MonoBehaviour
    {
        public static event Action<int> OnAddMoney;
        public static event Action<int, ItemType> InitRewardMenu;
        
        [SerializeField] private int _id;
        [SerializeField] private Transform _defaultReward;
        [SerializeField] private Transform _claimReward;
        [SerializeField] private Transform _clearedReward;
        [SerializeField] private DailyRewardUI1 _dailyRewardUI1;

        private void OnEnable()
        {
            switch (_id)
            {
                case 0:
                    DailyRewardGridUI.OnFirstRewardClaimed += DailyRewardGridUIOnOnFirstRewardClaimed;
                    break;
                case 1:
                    DailyRewardGridUI.OnSecondRewardClaimed += DailyRewardGridUIOnOnFirstRewardClaimed;
                    break;
                case 2:
                    DailyRewardGridUI.OnThirdRewardClaimed += DailyRewardGridUIOnOnFirstRewardClaimed;
                    break;
            }
        }

        private void OnDisable()
        {
            switch (_id)
            {
                case 0:
                    DailyRewardGridUI.OnFirstRewardClaimed -= DailyRewardGridUIOnOnFirstRewardClaimed;
                    break;
                case 1:
                    DailyRewardGridUI.OnSecondRewardClaimed -= DailyRewardGridUIOnOnFirstRewardClaimed;
                    break;
                case 2:
                    DailyRewardGridUI.OnThirdRewardClaimed -= DailyRewardGridUIOnOnFirstRewardClaimed;
                    break;
            }
        }

        private void DailyRewardGridUIOnOnFirstRewardClaimed()
        {
            _defaultReward.gameObject.SetActive(false);
            _claimReward.gameObject.SetActive(true);
            _clearedReward.gameObject.SetActive(false);

            if (StaticData.GetClaimedRewardIndex(_id) != 1) return;
            
            _defaultReward.gameObject.SetActive(false);
            _claimReward.gameObject.SetActive(false);
            _clearedReward.gameObject.SetActive(true);
        }

        public void ClaimReward()
        {
            StaticData.SetClaimedRewardIndex(_id, 1);
            
            _defaultReward.gameObject.SetActive(false);
            _claimReward.gameObject.SetActive(false);
            _clearedReward.gameObject.SetActive(true);

            if (_id == 2)
            {
                GameController.Instance.GetReward(3);
                InitRewardMenu?.Invoke(FindObjectOfType<ShopLoader>(true).GetRandomNotBoughtCharacterId(),
                    ItemType.Character);
            }
            else
            {
                var moneyAmount = _id == 0 ? 100 : 200;
                _dailyRewardUI1.Init(moneyAmount, () => OnAddMoney?.Invoke(moneyAmount));
            }
        }
    }
}
