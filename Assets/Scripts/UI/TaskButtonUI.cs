using System;
using Static;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TaskButtonUI : MonoCashed<Button>
    {
        [SerializeField] private Transform _lock;
        private void OnEnable()
        {
            if (LevelManager.Instance.CurrentLevelCount >= 6)
            {
                Cashed1.enabled = true;
                _lock.gameObject.SetActive(false);
            }
            else
                Cashed1.enabled = false;

            if (LevelManager.Instance.CurrentLevelCount != 6) return;
            
            StaticData.KilledEnemies = 0;
            StaticData.CompletedLevels = 0;
            StaticData.OnWall = 0;
            StaticData.FoundedItems = 0;
            StaticData.AdWatchedCount = 0;
            StaticData.RaceCompletedLevels = 0;
        }
    }
}
