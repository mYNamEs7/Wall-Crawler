using System;
using GameCycle;
using Static;
using UnityEngine;

namespace UI
{
    public class RaceStartUI : MonoBehaviour
    {
        private void OnEnable()
        {
            GameController.Instance.OnMainMenuStateEnter += InstanceOnOnMainMenuStateEnter;
            
            // if(StaticData.RaceCompletedLevels <= 5 || StaticData.IsRace == 1)
            transform.GetChild(0).gameObject.SetActive(false);
        }

        private void InstanceOnOnMainMenuStateEnter()
        {
            if (StaticData.RaceCompletedLevels > 5 && StaticData.IsRace == 0)
                transform.GetChild(0).gameObject.SetActive(true);
        }

        public void SetRace() => StaticData.IsRace = 1;
    }
}
