using System;
using System.Collections.Generic;
using Static;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class MultiplierArrowUI : MonoBehaviour
    {
        [SerializeField] private Text _moneyText;

        private int _currentMoneyAmount;

        private void MoneyMultiplier(int multiplier)
        {
            _currentMoneyAmount = 100 * multiplier;
            _moneyText.text = $"{_currentMoneyAmount}";
        }

        public void GetMoney() => StaticData.MoneyAmount += _currentMoneyAmount - 100;
    }
}
