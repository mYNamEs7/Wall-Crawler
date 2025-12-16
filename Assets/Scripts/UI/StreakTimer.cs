using System;
using System.Globalization;
using SO;
using Static;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace UI
{
    public class StreakTimer : MonoCashed<Text>
    {
        private DateTime lastActionTime;
        private readonly TimeSpan interval = TimeSpan.FromHours(8);

        protected override void Awake()
        {
            base.Awake();
            
            LoadLastActionTime();

            if (IsTimeToAct())
            {
                PerformAction();
            }
        }

        private void OnEnable()
        {
            ArrowUI.OnResetTimer += PerformAction;
            
            LoadLastActionTime();

            if (IsTimeToAct())
            {
                PerformAction();
            }
        
            StartCoroutine(UpdateTimer());
        }

        private void OnDisable()
        {
            ArrowUI.OnResetTimer -= PerformAction;
        }

        private void LoadLastActionTime()
        {
            if (PlayerPrefs.HasKey("LastActionTime"))
            {
                var lastActionTimeString = PlayerPrefs.GetString("LastActionTime");
                lastActionTime = DateTime.Parse(lastActionTimeString);
            }
            else
            {
                lastActionTime = DateTime.Now;
                PerformAction();
                SaveLastActionTime();
            }
        }

        private void SaveLastActionTime()
        {
            PlayerPrefs.SetString("LastActionTime", lastActionTime.ToString());
            PlayerPrefs.Save();
        }

        private bool IsTimeToAct()
        {
            return DateTime.Now - lastActionTime >= interval;
        }

        private void PerformAction()
        {
            StaticData.WinStreak = 2;
            var rand1 = Random.Range(0, 2);
            StaticData.IsWebStreakReward = rand1;
            var rand = Random.Range(0, 3);
            StaticData.FirstRewardState = rand;
            
            lastActionTime = DateTime.Now;
            SaveLastActionTime();
        }

        private System.Collections.IEnumerator UpdateTimer()
        {
            while (true)
            {
                var timeUntilNextAction = (lastActionTime + interval) - DateTime.Now;
                if (timeUntilNextAction <= TimeSpan.Zero)
                {
                    PerformAction();
                    timeUntilNextAction = interval;
                }
                
                var h = GameSettings.Instance.CurrentLanguageIndex == 0 ? "ч" : "h";
                var m = GameSettings.Instance.CurrentLanguageIndex == 0 ? "м" : "m";
                Cashed1.text = $"{(int)timeUntilNextAction.TotalHours:D2}{h}:{timeUntilNextAction.Minutes:D2}{m}";
                yield return new WaitForSeconds(1);
            }
        }
    }
}
