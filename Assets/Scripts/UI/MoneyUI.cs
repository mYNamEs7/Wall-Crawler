using System;
using System.Collections;
using GameCycle;
using Shop;
using Static;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MoneyUI : MonoBehaviour
    {
        [SerializeField] private Animator _moneyAnim;
        [SerializeField] private Text _moneyText;

        private Camera _camera;
        
        private void OnEnable()
        {
            _camera = Camera.main;
            
            _moneyText.text = $"{StaticData.MoneyAmount}";
            ProgressUI.OnWinMenuClosed += InstanceOnOnWinStateExit;
            ItemQuestUI.OnGetPrize += ItemQuestUIOnOnGetPrize;
            StreakRewardUI.OnMoneyAdded += StreakRewardUIOnOnMoneyAdded;
            DailyRewardUI.OnAddMoney += RaceFinishUIOnOnAddMoney;
            RaceFinishUI.OnAddMoney += RaceFinishUIOnOnAddMoney;
            ShopLoader.OnMoneyChanged += ShopLoaderOnOnMoneyChanged;
        }
        
        private void OnDisable()
        {
            ProgressUI.OnWinMenuClosed -= InstanceOnOnWinStateExit;
            ItemQuestUI.OnGetPrize -= ItemQuestUIOnOnGetPrize;
            StreakRewardUI.OnMoneyAdded -= StreakRewardUIOnOnMoneyAdded;
            DailyRewardUI.OnAddMoney -= RaceFinishUIOnOnAddMoney;
            RaceFinishUI.OnAddMoney -= RaceFinishUIOnOnAddMoney;
            ShopLoader.OnMoneyChanged -= ShopLoaderOnOnMoneyChanged;
        }

        private void ShopLoaderOnOnMoneyChanged()
        {
            _moneyText.text = $"{StaticData.MoneyAmount}";
        }
        
        private void RaceFinishUIOnOnAddMoney(int amount)
        {
            _moneyAnim.gameObject.SetActive(true);

            var money = StaticData.MoneyAmount;
            StartCoroutine(IncrementValueOverTime(money, money + amount, 0.67f));
        }

        private void StreakRewardUIOnOnMoneyAdded(int amount, Vector3 pos)
        {
            var startPos = _moneyAnim.transform.position;
            
            var screenPoint = _camera.WorldToScreenPoint(pos);
            _moneyAnim.transform.position = screenPoint;
            
            _moneyAnim.gameObject.SetActive(true);

            var money = StaticData.MoneyAmount;
            StartCoroutine(IncrementValueOverTime(money, money + amount, 0.67f));
            StartCoroutine(ChangePosition(_moneyAnim.transform.position, startPos, 0.67f));
        }

        private void ItemQuestUIOnOnGetPrize()
        {
            _moneyAnim.gameObject.SetActive(true);

            var money = StaticData.MoneyAmount;
            StartCoroutine(IncrementValueOverTime(money, money + 1000, 0.67f));
        }

        private void InstanceOnOnWinStateExit()
        {
            _moneyAnim.gameObject.SetActive(true);

            var money = StaticData.MoneyAmount;
            StartCoroutine(IncrementValueOverTime(money, money + 100, 0.67f));
        }
        
        private IEnumerator IncrementValueOverTime(int start, int end, float duration)
        {
            var elapsed = 0f;
            
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                
                var currentValue = (int)Mathf.Lerp(start, end, elapsed / duration);
                _moneyText.text = $"{currentValue}";
                
                yield return null;
            }
            
            _moneyText.text = $"{end}";
            StaticData.MoneyAmount = end;
            
            _moneyAnim.gameObject.SetActive(false);
        }
        
        private IEnumerator ChangePosition(Vector3 start, Vector3 end, float duration)
        {
            var elapsed = 0f;
            
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                
                var currentValue = Vector3.Lerp(start, end, elapsed / duration);
                _moneyAnim.transform.position = currentValue;
                
                yield return null;
            }
            
            _moneyAnim.transform.position = end;
        }
    }
}
