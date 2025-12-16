using System;
using System.Collections.Generic;
using System.Linq;
using Shop;
using UnityEngine;
using UnityEngine.Serialization;
using YG;

namespace SO
{
    [CreateAssetMenu(fileName = "Settings", menuName = "GameSettings/Settings")]
    public class GameSettings : ScriptableObject
    {
        #region Instance

        private static GameSettings _instance;
    
        public static GameSettings Instance
        {
            get
            {
                if (_instance != null) return _instance;
                
                _instance = Resources.Load<GameSettings>("game settings/Settings");
                if (_instance == null)
                {
                    Debug.LogError("Settings not found!");
                }
                return _instance;
            }
        }

        #endregion

        public int CurrentLanguageIndex => YandexGame.EnvironmentData.language == "ru" ? 0 : 1;
        
        public Layers layers;
        [SerializeField] public Stages stages;
        public List<StuffOnLevel> stuffOnLevel;
        public List<Reward> rewards;
        
        public List<Reward> GetRewardsByType(ItemType itemType) => rewards.Where(reward => reward.type == itemType).ToList();
        
        [Serializable]
        public struct Stages
        {
            public List<Stage> stages;
            public List<int> rewardLevels;
            public List<int> bossLevels;
            public List<int> finalLevels;
        }
        
        [Serializable]
        public struct Stage
        {
            public int startLevel;
            public int endLevel;
        }
        
        [Serializable]
        public struct StuffOnLevel
        {
            public int level;
            public int stuffIndex;
            public Sprite image;
            public Transform itemPrefab;
        }
        
        [Serializable]
        public struct Reward
        {
            public int id;
            public int level;
            public Sprite icon;
            public ItemType type;
            public ItemRare rare;
            public RewardDetails details;
        }
        
        [Serializable]
        public struct RewardDetails
        {
            public string enText;
            public string ruText;

            public string enName;
            public string ruName;
        }
        
        [Serializable]
        public struct Layers
        {
            public LayerMask aimLayer;
            public LayerMask buttonLayer;
            public LayerMask withoutEnemyLayer;
            public LayerMask stuffLayer;
            public LayerMask enemyLayer;
            public LayerMask cutObstacleLayer;
            public LayerMask laserMask;
            public LayerMask playerMask;
            public LayerMask obstacleMask;
        }
    }
}
