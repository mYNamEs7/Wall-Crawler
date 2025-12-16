using System;
using GameCycle;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayButtonUI : MonoCashed<Button>
    {
        private bool _isBossFight;

        private static GameController _gameController => GameController.Instance;

        private void OnEnable()
        {
            _isBossFight = false;
            
            Cashed1.onClick.AddListener(StartGameplay);
            StageProgressUI.OnBossFight += StageProgressUIOnOnBossFight;
        }

        private void StageProgressUIOnOnBossFight()
        {
            _isBossFight = true;
        }

        private void OnDisable()
        {
            Cashed1.onClick.RemoveListener(StartGameplay);
            StageProgressUI.OnBossFight -= StageProgressUIOnOnBossFight;
        }

        private void StartGameplay()
        {
            MyAnalytics.OnLevelStart(LevelManager.Instance.CurrentLevelCount);
            _gameController.EnterGameplay(_isBossFight);
        }
    }
}
