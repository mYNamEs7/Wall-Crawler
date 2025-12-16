using System;
using Static;
using UnityEngine;

namespace UI
{
    public class RaceFinishUI : MonoBehaviour
    {
        public static event Action<int> OnAddMoney;
        
        private void OnEnable()
        {
            RaceInfoUI.OnRaceComplete += RaceInfoUIOnOnRaceComplete;
        }

        private void OnDisable()
        {
            RaceInfoUI.OnRaceComplete -= RaceInfoUIOnOnRaceComplete;
        }

        private void RaceInfoUIOnOnRaceComplete()
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }

        public void AddMoney()
        {
            StaticData.IsRace = 0;
            StaticData.RaceCompletedLevels = 0;
            OnAddMoney?.Invoke(500);
        }
    }
}
