using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EnemySpace
{
    public class Boss : Death
    {
        public static event Action<Transform> OnStartMove;
        public static event Action OnStopMove;
        
#if UNITY_EDITOR
        [SerializeField] private bool _isNext;
#endif
        [SerializeField] private List<Path> _pathPoints;

        private Animator _animator;
        private static readonly int State = Animator.StringToHash("State");

        protected override void Awake()
        {
            base.Awake();
            
            _animator = GetComponentInChildren<Animator>();
        }

        private void OnEnable()
        {
            OnStartMove += BossOnStartMove;
            OnStopMove += BossOnStopMove;
        }

        private void OnDisable()
        {
            OnStartMove -= BossOnStartMove;
            OnStopMove -= BossOnStopMove;
        }

        private void BossOnStopMove()
        {
            gameObject.layer = 6;
        }

        private void BossOnStartMove(Transform obj)
        {
            gameObject.layer = 2;
        }

        protected override void Move()
        {
            OnStartMove?.Invoke(transform);
            StopAllCoroutines();
            StartCoroutine(WaitForMove());
        }

        private IEnumerator WaitForMove()
        {
            _animator.SetInteger(State, 4);
            yield return new WaitForSeconds(0.6f);
            StartCoroutine(MoveToNextPoint());
        }
        
#if UNITY_EDITOR
        private void Update()
        {
            if (!_isNext) return;
            
            StartCoroutine(MoveToNextPoint());
            _isNext = false;
        }
#endif
        
        private IEnumerator Inspect(bool isLeft)
        {
            var movingLeft = isLeft;
            var startPosition = transform.position;
            var walkDistance = 0.5f;
            var walkSpeed = 1.5f;
            var pauseDuration = 5.4f;
            
            while (true)
            {
                yield return new WaitForSeconds(pauseDuration);
                
                // Поворачиваем бота в другую сторону
                var currentRotation = transform.eulerAngles;
                currentRotation.y = movingLeft ? -90 : 90;
                transform.eulerAngles = currentRotation;
                
                // Устанавливаем направление движения
                float targetX = movingLeft ? startPosition.x - walkDistance : startPosition.x + walkDistance;
                Vector3 targetPosition = new Vector3(targetX, transform.position.y, transform.position.z);

                // Включаем анимацию ходьбы
                _animator.SetInteger("State", 1);

                // Двигаем бота до целевой позиции
                while (Vector3.Distance(transform.position, targetPosition) > 0.01f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, walkSpeed * Time.deltaTime);
                    yield return null;
                }

                // Останавливаем анимацию ходьбы и делаем паузу
                if(_animator.GetInteger(State) != 0)
                    _animator.SetInteger(State, 0);
                
                // Меняем направление движения
                movingLeft = !movingLeft;
            }
        }

        private IEnumerator MoveToNextPoint()
        {
            var currentPointIndex = 0;
            while (currentPointIndex < _pathPoints.First().pathPoints.Count)
            {
                var targetPoint = _pathPoints.First().pathPoints[currentPointIndex].transform;
                
                var rot = _pathPoints.First().pathPoints[currentPointIndex].TargetRotation;
                var currentRotation = transform.eulerAngles;
                currentRotation.y = -rot;
                transform.eulerAngles = currentRotation;

                var speed = _pathPoints.First().pathPoints[currentPointIndex].TargetSpeed;
                var waitTime = _pathPoints.First().pathPoints[currentPointIndex].WaitTime;
                
                var anim = _pathPoints.First().pathPoints[currentPointIndex].AnimationId;
                _animator.SetInteger(State, anim);

                yield return null;
                if (anim == 7)
                {
                    _animator.SetInteger(State, -1);
                    yield return new WaitForSeconds(waitTime);
                    
                }
            
                // Перемещаемся к следующей точке
                while (Vector3.Distance(transform.position, targetPoint.position) > 0.1f)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);
                    yield return null;
                }

                // Ожидаем на точке

                // Переходим к следующей точке, зацикливаем список
                currentPointIndex++;
            }
            var isLeft = _pathPoints.First().isLeft;
            
            _pathPoints.RemoveAt(0);
            
            OnStopMove?.Invoke();
            
            StartCoroutine(Inspect(isLeft));
        }
        
        [Serializable]
        private struct Path
        {
            public bool isLeft;
            public List<PathPoint> pathPoints;
        }
    }
}
