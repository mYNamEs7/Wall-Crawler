using System;
using Static;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StateTextUI : MonoCashed<Text>
    {
        private void OnEnable()
        {
            Cashed1.text = $"{StaticData.WinStreak - 1}";
        }
    }
}
