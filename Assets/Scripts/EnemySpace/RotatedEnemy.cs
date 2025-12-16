using System;
using System.Collections;
using UnityEngine;

namespace EnemySpace
{
    public class RotatedEnemy : Death
    {
        [SerializeField] private bool _isLeft;

        [Header("Add Time")] 
        [SerializeField] private bool isAddTime;
        [SerializeField] private bool _addTimeWhenIsLeft;
        [SerializeField] private float _addTime = 3f;

        private bool _canRotate = true;
        
        private void OnEnable()
        {
            StartCoroutine(Inspect(_isLeft));
        }

        protected override void OnDeath()
        {
            base.OnDeath();

            _canRotate = false;
        }

        private IEnumerator Inspect(bool isLeft)
        {
            var movingLeft = isLeft;
            var pauseDuration = 5.4f;
            
            while (_canRotate)
            {
                yield return new WaitForSeconds(pauseDuration);
                
                // Поворачиваем бота в другую сторону
                var currentRotation = transform.eulerAngles;
                currentRotation.y = movingLeft ? -90 : 90;
                transform.eulerAngles = currentRotation;

                if (isAddTime)
                {
                    if(movingLeft == _addTimeWhenIsLeft)
                        yield return new WaitForSeconds(_addTime);
                }
                
                movingLeft = !movingLeft;
            }
        }
    }
}
