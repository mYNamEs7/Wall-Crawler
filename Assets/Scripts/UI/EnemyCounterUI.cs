using System;
using System.Collections.Generic;
using System.Linq;
using EnemySpace;
using PlayerSpace;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class EnemyCounterUI : MonoBehaviour
    {
        [FormerlySerializedAs("_enemyImage")] [SerializeField] private MarkedUI marked;

        private List<MarkedUI> _enemyImages = new();
        
        private void OnEnable()
        {
            var enemyCount = FindObjectOfType<LevelController>().EnemyCount;
            for (var i = 0; i < enemyCount; i++)
            {
                var enemyImage = Instantiate(marked, transform);
                enemyImage.gameObject.SetActive(true);
                _enemyImages.Add(enemyImage);
            }
            
            Death.OnEnemyDeath += PlayerOnOnTimeSlowDown;
        }

        private void OnDisable()
        {
            _enemyImages.ForEach(image => Destroy(image.gameObject));
            _enemyImages = new List<MarkedUI>();
            
            Death.OnEnemyDeath -= PlayerOnOnTimeSlowDown;
        }

        private void PlayerOnOnTimeSlowDown()
        {
            var targetEnemyImage = _enemyImages.First(image => !image.IsCleared);
            
            targetEnemyImage.SetCleared();
        }
    }
}
