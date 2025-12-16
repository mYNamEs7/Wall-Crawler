using System;
using EnemySpace;
using UnityEngine;

namespace PlayerSpace
{
    public class PlayerDetector : MonoCashed<Collider>
    {
        public event Action<Collider> OnTriggered;
        public event Action<Collider> OnWallTriggered;
        public event Action<Collider> OnWallTriggeredStay;

        private void OnEnable()
        {
            Cashed1.isTrigger = false;
            var capsule = (CapsuleCollider)Cashed1;
            capsule.radius -= 0.05f;
            capsule.height -= 0.1f;
            
            Player.OnStartFly += PlayerOnOnStartFly;
            Player.OnFinishFly += PlayerOnOnFinishFly;
            PlayerAnimator.OnPlayerDead += PlayerAnimatorOnOnPlayerDead;
        }
        
        private void OnDisable()
        {
            Player.OnStartFly -= PlayerOnOnStartFly;
            Player.OnFinishFly -= PlayerOnOnFinishFly;
            PlayerAnimator.OnPlayerDead -= PlayerAnimatorOnOnPlayerDead;
        }

        private void PlayerOnOnFinishFly(Vector3 arg1, Vector3 arg2, Vector3 arg3, int arg4, Death arg5)
        {
            if (Cashed1.GetType() == typeof(SphereCollider))
                Cashed1.enabled = true;
        }

        private void PlayerOnOnStartFly(Vector3 arg1, Vector3 arg2)
        {
            if (Cashed1.GetType() == typeof(SphereCollider))
                Cashed1.enabled = false;
        }
        
        private void PlayerAnimatorOnOnPlayerDead(Vector3 arg1, Vector3 arg2)
        {
            gameObject.layer = 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.layer == 0)
                OnWallTriggered?.Invoke(other);
            
            if (other.gameObject.layer != 8 
                // && (other.gameObject.layer != _enemyLayer || !_isPlayerFly) 
                &&
                other.gameObject.layer != 9 && other.gameObject.layer != 11)
                // && other.gameObject.layer != 12)
                return;
            
            OnTriggered?.Invoke(other);
        }

        private void OnTriggerStay(Collider other)
        {
            if(other.gameObject.layer == 0)
                OnWallTriggeredStay?.Invoke(other);
        }
    }
}
