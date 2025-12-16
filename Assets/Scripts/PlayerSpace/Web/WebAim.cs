using System;
using System.Threading.Tasks;
using EnemySpace;
using GameCycle;
using SO;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace PlayerSpace.Web
{
    public class WebAim : MonoCashed<LineRenderer>
    {
        public static event Action<bool> OnWebVisibleChanged;
        public static event Action<bool> OnMaxDistance;
        public static event Action<Vector3, Vector3, Vector3, int, Death, Transform, RaycastHit> OnPlayerFly;
        public static event Action<Transform> OnGetStuff;
        public event Action<int> OnHitLayerChanged;
        public event Action<int> OnCutObstacle;
        public static event Action<Vector3, Vector3, RaycastHit> OnDrawWeb;
        
        [SerializeField] private Transform _leftArm;
        [SerializeField] private float _maxDistance;
        [SerializeField] private bool _isValidate;
        
        private static int _stuffLayer => GameSettings.Instance.layers.stuffLayer;
        private static LayerMask _groundLayer => GameSettings.Instance.layers.aimLayer;
        private static LayerMask _buttonLayer => GameSettings.Instance.layers.buttonLayer;
        private static LayerMask WithoutWithoutEnemyLayer => GameSettings.Instance.layers.withoutEnemyLayer;

        private Vector3 _armLocalPosition;
        private Camera _camera;
        private bool _isGameplay;
        private Vector3 _targetPoint;
        private Vector3 _drawingTargetPoint;
        private Vector3 _hitNormal;
        private int _hitLayer;
        private Vector3 _hitDirection;
        private Death _targetDeath;
        private bool _isBossMoved;
        private Transform _parent;
        private Vector3 _firstPointPosition;
        private RaycastHit _raycastHit;
        private Player _player;
        private WebFire _webFire;
        
        public static int HitLayer { get; private set; }

        public Vector3 LastPoint => Cashed1.GetPosition(1);

        public bool IsCutObstacle => Physics.Raycast(Cashed1.GetPosition(0), DistantPoint, out CutHit, 100f,
            GameSettings.Instance.layers.cutObstacleLayer.value);

        public RaycastHit CutHit;
        public Vector3 DistantPoint { get; private set; }

        public static bool CanAim { get; set; }

        private bool IsCanAim => CanAim && _isGameplay && !_isBossMoved;

        private void OnEnable()
        {
            _player = FindObjectOfType<Player>();
            _webFire = FindObjectOfType<WebFire>();
            
            CanAim = true;
            _camera = Camera.main;

            Cashed1.enabled = false;
            
            GameController.Instance.OnGameplayStateEnter += InstanceOnOnGameplayStateEnter;
            GameController.Instance.OnGameplayStateExit += InstanceOnOnGameplayStateExit;
            WebFire.OnWebDrawn += WebFireOnOnWebDrawn;
            TouchPanelUI.OnFingerUp += TouchPanelUIOnOnFingerUp;
            Boss.OnStartMove += BossOnOnStartMove;
            Boss.OnStopMove += BossOnOnStopMove;
            PlayerAnimator.OnPlayerDead += PlayerAnimatorOnOnPlayerDead;
        }
        
        private void OnDisable()
        {
            WebFire.OnWebDrawn -= WebFireOnOnWebDrawn;
            TouchPanelUI.OnFingerUp -= TouchPanelUIOnOnFingerUp;
            Boss.OnStartMove -= BossOnOnStartMove;
            Boss.OnStopMove -= BossOnOnStopMove;
            PlayerAnimator.OnPlayerDead -= PlayerAnimatorOnOnPlayerDead;
        }
        
        private void PlayerAnimatorOnOnPlayerDead(Vector3 arg1, Vector3 arg2)
        {
            CanAim = false;
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (!_isValidate) return;

            _leftArm = GameObject.Find("Chest").transform;
#endif
        }

        private void BossOnOnStopMove() => _isBossMoved = false;

        private void BossOnOnStartMove(Transform boss) => _isBossMoved = true;
        
        private void TouchPanelUIOnOnFingerUp()
        {
            if (!IsCanAim) return;

            Cashed1.enabled = false;
            OnWebVisibleChanged?.Invoke(false);
            
            OnDrawWeb?.Invoke(_leftArm.position, _drawingTargetPoint, _raycastHit);

            CanAim = false;
        }

        private void WebFireOnOnWebDrawn()
        {
            if (Vector3.Distance(Cashed1.GetPosition(0), Cashed1.GetPosition(1)) < _maxDistance && _hitLayer != 13)
            {
                if (_hitLayer == 10)
                    OnGetStuff?.Invoke(_player.transform);
                else
                    OnPlayerFly?.Invoke(_targetPoint, _hitNormal, _hitDirection, _hitLayer, _targetDeath, _parent, _raycastHit);
            }
            else
            {
                _webFire.FastWebErase();
                CanAim = true;
            }
        }

        private void InstanceOnOnGameplayStateExit()
        {
            _isGameplay = false;
        }

        private void InstanceOnOnGameplayStateEnter()
        {
            _isGameplay = true;
        }

        private void FixedUpdate()
        {
            if (!IsCanAim) return;

            if (TouchPanelUI.IsPressed)
                DrawAiming();
        }

        private void DrawAiming()
        {
            if (!Cashed1.enabled)
            {
                Cashed1.enabled = true;
                OnWebVisibleChanged?.Invoke(true);
            }
            
            UpdateStartPosition();
            UpdateFinishPosition();
        }

        private void UpdateStartPosition()
        {
            _firstPointPosition = _leftArm.position;

            if (_firstPointPosition != Cashed1.GetPosition(0))
                Cashed1.SetPosition(0, _firstPointPosition);
        }

        private void UpdateFinishPosition()
        {
            var viewportPoint = new Vector3(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height, 10f);
            var targetPosition = _camera.ViewportToWorldPoint(viewportPoint);
            targetPosition.z = 0f;

            var firstPoint = Cashed1.GetPosition(0);
            var direction = (targetPosition - firstPoint).normalized;
            direction.z = 0f;

            // firstPoint -= direction * 0.5f;

            DistantPoint = targetPosition + direction * 20f;
            // Cashed1.SetPosition(1, DistantPoint);

            var hitLayer = -1;
            if (Physics.Raycast(firstPoint, DistantPoint, out var hit, 100f, _buttonLayer))
            {
                hitLayer = hit.transform.gameObject.layer;
            }

            if (!Physics.Raycast(firstPoint, DistantPoint, out hit, 100f, WithoutWithoutEnemyLayer)) return;
            
            targetPosition = hit.point;
            targetPosition.z = 0f;

            _drawingTargetPoint = targetPosition;
            
            if (!Physics.Raycast(firstPoint, DistantPoint, out hit, 100f, _groundLayer)) return;

            _targetDeath = hit.transform.TryGetComponent<Death>(out var enemy) ? enemy : null;

            if (hit.transform.gameObject.layer != 10)
                _parent = hit.transform;
            
            targetPosition = hit.point;
            targetPosition.z = 0f;
            
            Cashed1.SetPosition(1, targetPosition);

            _targetPoint = targetPosition;
            _hitNormal = hit.normal;
            _hitDirection = (targetPosition - firstPoint).normalized;

            _raycastHit = hit;
            
            // if (targetLayer == _hitLayer) return;
            
            _hitLayer = hit.transform.gameObject.layer;
            if (hitLayer != -1)
                _hitLayer = hitLayer;
            // OnHitLayerChanged?.Invoke(_hitLayer);

            HitLayer = Physics.Raycast(firstPoint, DistantPoint, out hit, 100f,
                GameSettings.Instance.layers.cutObstacleLayer)
                ? hit.transform.gameObject.layer
                : _hitLayer;
            OnHitLayerChanged?.Invoke(HitLayer);

            OnMaxDistance?.Invoke(Vector3.Distance(Cashed1.GetPosition(0), Cashed1.GetPosition(1)) >= _maxDistance || _hitLayer == 13);
        }
    }
}
