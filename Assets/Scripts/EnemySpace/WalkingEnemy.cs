using System;
using System.Collections;
using UnityEngine;

namespace EnemySpace
{
    public class WalkingEnemy : Death
    {
        [SerializeField] private bool _isLeft;
        [SerializeField] private bool withAddTime;
        [SerializeField] float pauseDuration = 5.4f;
        
        private Animator _animator;
        private static readonly int State = Animator.StringToHash("State");
        private Laser _laser;
        private bool _canMove = true;

        private bool isAdditionalTime;

        private void OnEnable()
        {
            Death.OnEnemyDeath += DeathOnOnEnemyDeath;
            
            _animator = GetComponentInChildren<Animator>();
            StartCoroutine(Inspect(_isLeft));
            _laser = GetComponentInChildren<Laser>();
            _laser.gameObject.SetActive(false);
        }

        private void OnDisable()
        {
            Death.OnEnemyDeath -= DeathOnOnEnemyDeath;
        }

        private void DeathOnOnEnemyDeath()
        {
            isAdditionalTime = true;
        }

        protected override void OnDeath()
        {
            base.OnDeath();

            _canMove = false;
        }

        private IEnumerator Inspect(bool isLeft)
        {
            var movingLeft = isLeft;
            var startPosition = transform.position;
            const float walkDistance = 0.5f;
            const float walkSpeed = 1.5f;
            
            while (_canMove)
            {
                yield return new WaitForSeconds(pauseDuration - 0.5f);

                if (isAdditionalTime && withAddTime)
                {
                    yield return new WaitForSeconds(pauseDuration - 0.5f);
                    isAdditionalTime = false;
                }
                
                // _laser?.gameObject.SetActive(!movingLeft && _animator.GetInteger(State) == 0);
                
                // Поворачиваем бота в другую сторону
                var currentRotation = transform.eulerAngles;
                currentRotation.y = movingLeft ? -90 : 90;
                transform.eulerAngles = currentRotation;
                
                // Устанавливаем направление движения
                var targetX = movingLeft ? startPosition.x - walkDistance : startPosition.x + walkDistance;
                var targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);

                // Включаем анимацию ходьбы
                _animator.SetInteger(State, 1);
                
                _laser?.gameObject.SetActive(!movingLeft && _animator.GetInteger(State) == 0);

                // Двигаем бота до целевой позиции
                while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, walkSpeed * Time.deltaTime);
                    yield return null;
                }

                // Останавливаем анимацию ходьбы и делаем паузу
                if(_animator.GetInteger(State) != 0)
                    _animator.SetInteger(State, 0);

                yield return new WaitForSeconds(0.5f);
                _laser?.gameObject.SetActive(!movingLeft && _animator.GetInteger(State) == 0);
                
                // Меняем направление движения
                movingLeft = !movingLeft;
            }
        }
    }
}
