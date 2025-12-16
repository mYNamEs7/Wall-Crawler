using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SO;
using Static;
using UnityEngine;

namespace UI
{
    public class StageProgressUI : MonoBehaviour
    {
        public static event Action OnBossFight;
        
        [Header("Grid")] 
        [SerializeField] private Transform _grid;
        
        [Header("Prefabs")]
        [SerializeField] private MarkedUI _levelPrefab;
        [SerializeField] private MarkedUI _rewardPrefab;
        [SerializeField] private MarkedUI _bossPrefab;

        private static IEnumerable<GameSettings.Stage> _stages => GameSettings.Instance.stages.stages;
        private static List<int> _rewardLevels => GameSettings.Instance.stages.rewardLevels;
        private static List<int> _bossLevels => GameSettings.Instance.stages.bossLevels;
        private static List<int> _finalLevels => GameSettings.Instance.stages.finalLevels;

        private void OnEnable()
        {
            var currentLevel = LevelManager.Instance.CurrentLevelCount;
            var currentStage = _stages.First(stage => stage.startLevel <= LevelManager.Instance.CurrentLevelIndex + 1 && stage.endLevel >= LevelManager.Instance.CurrentLevelIndex + 1);

            for (var i = currentStage.startLevel; i <= currentStage.endLevel; i++)
            {
                MarkedUI levelStage;
                if (_rewardLevels.Contains(i))
                    levelStage = Instantiate(_rewardPrefab, _grid);
                else if (_finalLevels.Contains(i))
                    levelStage = Instantiate(_bossPrefab, _grid);
                else
                    levelStage = Instantiate(_levelPrefab, _grid);

                if(levelStage.TryGetComponent<LevelStageUI>(out var level))
                    level.SetLevel(i + (80 * ((currentLevel - 1) / 80) - 5 * ((currentLevel - 1) / 80)));
                
                if(i == LevelManager.Instance.CurrentLevelIndex + 1)
                    levelStage.SetCleared();
            }

            var nextRewardLevel = _rewardLevels.FirstOrDefault(level => level >= LevelManager.Instance.CurrentLevelIndex + 1);
            var prevRewardLevel = _rewardLevels.LastOrDefault(level => level < nextRewardLevel);
            StaticData.RewardProgressStep = 100 / (nextRewardLevel - prevRewardLevel);
            StaticData.ProgressLevel = LevelManager.Instance.CurrentLevelCount - prevRewardLevel;

            if (_bossLevels.Contains(LevelManager.Instance.CurrentLevelIndex + 1))
                StartCoroutine(InvokeBossLevel());
        }

        private static IEnumerator InvokeBossLevel()
        {
            yield return new WaitForSeconds(0.1f);
            
            OnBossFight?.Invoke();
        }

        private void OnDisable()
        {
            foreach (Transform child in _grid)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
