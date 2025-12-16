using System;
using EnemySpace;
using PlayerSpace;
using UnityEngine;

namespace UI
{
    public class BossHPUI : MarkedUI
    {
        [SerializeField] private int _id;
        
        private int _hp = 3;

        protected override void OnEnable()
        {
            base.OnEnable();
            
            Death.OnEnemyHit += PlayerOnOnTimeSlowDown;
        }

        private void OnDisable()
        {
            _hp = 3;
            Death.OnEnemyHit -= PlayerOnOnTimeSlowDown;
        }

        private void PlayerOnOnTimeSlowDown(Transform enemy)
        {
            if(_id == _hp)
                SetCleared();
            
            _hp--;
        }
    }
}
