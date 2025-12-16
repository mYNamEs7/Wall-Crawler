using System;
using System.Collections;
using System.Threading.Tasks;
using EnemySpace;
using GameCycle;
using PlayerSpace;
using UnityEngine;

namespace Cameras
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private Vector3 _offset;
        [SerializeField] private float _speed;

        private Vector3 _startPosition;
        private Transform _player;
        private Camera _camera;
        
        private float _upBorder;
        private float _rightBorder;
        private float _leftBorder;
        private float _bottomBorder;
        private bool _isGameplay;
        private Transform _playerTransform;
        private Vector3 _lastPosition;
        private Coroutine _coroutine;
        
        private void OnEnable()
        {
            _startPosition = transform.position;
            _camera = GetComponent<Camera>();
            GameController.Instance.OnMainMenuStateEnter += InstanceOnOnMainMenuStateEnter;
            GameController.Instance.OnEnterGameplayStateEnter += InstanceOnOnEnterGameplayStateEnter;
            Player.OnTimeSlowDown += PlayerOnOnTimeSlowDown;
            Player.OnTimeNormalized += PlayerOnOnTimeNormalized;
            Boss.OnStartMove += BossOnOnStartMove;
            Boss.OnStopMove += BossOnOnStopMove;
        }
        
        private void OnDisable()
        {
            Player.OnTimeSlowDown -= PlayerOnOnTimeSlowDown;
            Player.OnTimeNormalized -= PlayerOnOnTimeNormalized;
            Boss.OnStartMove -= BossOnOnStartMove;
            Boss.OnStopMove -= BossOnOnStopMove;
        }

        private void BossOnOnStopMove()
        {
            _player = _playerTransform;
        }

        private void BossOnOnStartMove(Transform boss)
        {
            _player = boss;
        }

        private void InstanceOnOnMainMenuStateEnter()
        {
            _isGameplay = false;
            
            var targetPos = FindObjectOfType<LevelController>().CameraPosition;
            targetPos.z = -13f;
            transform.position = targetPos;
        }

        private void InstanceOnOnEnterGameplayStateEnter(bool obj)
        {
            StartCoroutine(MoveToPosition(_player.position + _offset, 1f));
        }
        
        private IEnumerator MoveToPosition(Vector3 target, float duration)
        {
            target.x = Mathf.Clamp(target.x, _leftBorder, _rightBorder);
            target.y = Mathf.Clamp(target.y, _bottomBorder, _upBorder);
            
            var startPosition = transform.position; // Запоминаем начальную позицию
            var elapsedTime = 0f; // Время, прошедшее с начала перемещения

            while (elapsedTime < duration)
            {
                transform.position = Vector3.Lerp(startPosition, target, elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null; // Ожидаем до следующего кадра
            }

            // Устанавливаем объект в целевую позицию в конце перемещения
            transform.position = target;

            _isGameplay = true;
        }

        public void SetPlayer(Transform player) => _playerTransform = _player = player;
        
        public void SetBorders(float up, float right, float left, float bottom)
        {
            _upBorder = up - 0.4655f;
            _rightBorder = right - 0.4655f;
            _leftBorder = left + 0.4655f;
            _bottomBorder = bottom + 0.4655f;
        }

        private void PlayerOnOnTimeNormalized(bool _)
        {
            StartCoroutine(SmoothChangeFOV(ResolutionHandler.ScaledFOV, ResolutionHandler.StartFOV, 0.25f));
        }
        
        private void PlayerOnOnTimeSlowDown()
        {
            _coroutine ??= StartCoroutine(SmoothChangeFOV(ResolutionHandler.StartFOV, ResolutionHandler.ScaledFOV, 0.25f));
        }

        private IEnumerator SmoothChangeFOV(float start, float end, float duration)
        {
            var elapsed = 0f;
            
            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                
                var currentValue = (int)Mathf.Lerp(start, end, elapsed / duration);
                _camera.fieldOfView = currentValue;

                yield return null;
            }
            
            _camera.fieldOfView = end;
            _coroutine = null;
        }
        
        public bool IsPlayerVisible()
        {
            var viewPos = _camera.WorldToViewportPoint(_player.position);
            var isVisible = viewPos.x is > 0 and < 1 && viewPos.y is > 0 and < 1 && viewPos.z > 0;

            return isVisible;
        }

        private void LateUpdate()
        {
            if (_isGameplay && _player)
                UpdatePosition(_player.position + _offset);
        }

        private void UpdatePosition(Vector3 position)
        {
            if (_lastPosition == position) return;
            
            _lastPosition = position;
            
            position.x = Mathf.Clamp(position.x, _leftBorder, _rightBorder);
            position.y = Mathf.Clamp(position.y, _bottomBorder, _upBorder);

            transform.position = Vector3.Lerp(transform.position, position, _speed * Time.deltaTime);
        }
    }
}
