using System;
using System.Collections.Generic;
using System.Linq;
using SO;
using Static;
using UnityEngine;

namespace UI
{
    public class RetryButtonUI : MonoBehaviour
    {
        private static IEnumerable<GameSettings.StuffOnLevel> _stuffLevels => GameSettings.Instance.stuffOnLevel;

        private void OnEnable()
        {
            var currentLevelIndex = LevelManager.Instance.CurrentLevelCount;
            var stuffLevel = _stuffLevels.FirstOrDefault(stuffLevel => stuffLevel.level == currentLevelIndex);

            if (stuffLevel.level <= 0)
            {
                gameObject.SetActive(false);
                print("first!");
            }
            else if (StaticData.Stuff.GetStuffByIndex(stuffLevel.stuffIndex) == 0)
            {
                gameObject.SetActive(true);
                print("second!");
            }
            else
            {
                gameObject.SetActive(false);
                print("else");
            }
        }

        private void OnDisable()
        {
            gameObject.SetActive(true);
        }
    }
}
