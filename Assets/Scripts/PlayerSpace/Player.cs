using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using EnemySpace;
using PlayerSpace.Web;
using UnityEngine;

namespace PlayerSpace
{
    public class Player : MonoCashed<Rigidbody>
    {
        public static event Action OnHitObstacle;
        public static event Action<Vector3, Vector3> OnStartFly;
        public static event Action<Vector3, Vector3, Vector3, int, Death> OnFinishFly;
        public event Action OnPlayerGrounded;
        public static event Action OnTimeSlowDown; 
        public static event Action<bool> OnTimeNormalized; 

        [SerializeField] private float _groundCheckDistance = 0.1f;
        [SerializeField] private LayerMask _groundLayer;
        [SerializeField] private int _enemyLayer;
        [SerializeField] private int _obstacleLayer;
        [SerializeField] private int _enemyObstacleLayer = 9;
        [SerializeField] private Transform _centerPoint;
        [SerializeField] private Transform _armature;
        [SerializeField] private bool _isValidate;

        private Death _playerDeath;
        private bool _isPlayerFly;
        private PlayerDetector _playerDetector;
        private PlayerAnimator _playerAnimator;
        private Collider _collider;
        private Vector3 _lastPosition;
        private Vector3 _targetPosition;

        public Vector3 CenterPoint => _centerPoint.position;
        public Transform Armature => _armature;

        // private List<Rigidbody> _rbList;
        // private List<Collider> _colliders;
        // private List<Joint> _joints;

        // protected override void Awake()
        // {
        //     base.Awake();
        //     
        //     _rbList = new List<Rigidbody>(GetComponentsInChildren<Rigidbody>()) { GetComponent<Rigidbody>() };
        //     _rbList.ForEach(rb => rb.isKinematic = true);
        //     _colliders = new List<Collider>(GetComponentsInChildren<Collider>()) { GetComponent<Collider>() };
        //     _joints = new List<Joint>(GetComponentsInChildren<Joint>());
        //     
        //     _colliders.ForEach(Destroy);
        //     _joints.ForEach(Destroy);
        //     _rbList.ForEach(Destroy);
        // }

        protected override void Awake()
        {
            base.Awake();

            _playerDeath = GetComponent<Death>();
            _playerDetector = GetComponentInChildren<PlayerDetector>();
            _collider = GetComponent<Collider>();
            _playerAnimator = GetComponentInChildren<PlayerAnimator>();
            
            _lastPosition = transform.position;
        }

        private void OnEnable()
        {
            WebAim.OnPlayerFly += WebAimOnOnPlayerFly;
            PlayerAnimator.OnStartAttack += PlayerAnimatorOnOnStartAttack;
            PlayerAnimator.OnPlayerDead += PlayerAnimatorOnOnPlayerDead;
            Obstacle.OnPlayerDead += ObstacleOnOnPlayerDead;
            _playerDetector.OnTriggered += PlayerDetectorOnOnTriggered;
            _playerDetector.OnWallTriggered += PlayerDetectorOnOnWallTriggered;
            _playerDetector.OnWallTriggeredStay += PlayerDetectorOnOnWallTriggeredStay;
        }

        private void OnDisable()
        {
            WebAim.OnPlayerFly -= WebAimOnOnPlayerFly;
            PlayerAnimator.OnStartAttack -= PlayerAnimatorOnOnStartAttack;
            PlayerAnimator.OnPlayerDead -= PlayerAnimatorOnOnPlayerDead;
            Obstacle.OnPlayerDead -= ObstacleOnOnPlayerDead;
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (!_isValidate) return;
            
            _centerPoint = GameObject.Find("Center").transform;
            _armature = GameObject.Find("Center").transform;
#endif
        }
        
        private void PlayerDetectorOnOnWallTriggeredStay(Collider other)
        {
            if (other.bounds.Contains(_targetPosition) && _targetPosition != _lastPosition && WebAim.HitLayer != 6)
            {
                print("МЫ ВНУТРИ!");
                var targetPoint = other.ClosestPoint(transform.position);
                OnFinishFly?.Invoke(Vector3.zero, targetPoint, (targetPoint - transform.position).normalized, other.gameObject.layer, other.gameObject.GetComponent<Death>());
                _isPlayerFly = false;
                transform.position = _lastPosition;
                _targetPosition = _lastPosition;
                _playerAnimator.SetAnim(_playerAnimator.LastAnim[0]);
            }
        }
        
        private void PlayerDetectorOnOnWallTriggered(Collider other)
        {
            _lastPosition = transform.position;
        }
        
        private void PlayerDetectorOnOnTriggered(Collider other)
        {
            print(other.gameObject.name);
            if (other.gameObject.layer == 11)
            {
                other.transform.parent.GetComponentInChildren<Obstacle>().Fall();
                Destroy(other.gameObject);
                return;
            }

            if (other.gameObject.layer == _enemyObstacleLayer)
                OnHitObstacle?.Invoke();
            
            var targetPoint = other.ClosestPoint(transform.position);
            OnFinishFly?.Invoke(Vector3.zero, targetPoint, (targetPoint - transform.position).normalized, other.gameObject.layer, other.gameObject.GetComponent<Death>());
            _isPlayerFly = false;
        }

        private void ObstacleOnOnPlayerDead(Vector3 normal, Vector3 targetPoint, Vector3 direction, int hitLayer, Death death)
        {
            OnFinishFly?.Invoke(normal, targetPoint, direction, hitLayer, death);
            _isPlayerFly = false;
            PlayerAnimator.InvokePlayerDeath(targetPoint, direction);
        }

        private void PlayerAnimatorOnOnPlayerDead(Vector3 hitDirection, Vector3 hitPoint)
        {
            _playerDeath.TakeDamage(-hitDirection, hitPoint);
        }
        
        private void PlayerAnimatorOnOnStartAttack(Vector3 hitDirection, Vector3 targetPoint, Death death)
        {
            if (!Mathf.Approximately(Time.timeScale, 1f)) return;
            
            transform.position += Vector3.up * 0.4f;
            
            StartCoroutine(WaitForGrounded());
            death.TakeDamage(hitDirection, targetPoint);
            
            OnTimeSlowDown?.Invoke();
            
            StartCoroutine(WaitForAttackFinished(death.GetType() == typeof(Obstacle)));
        }

        private static IEnumerator WaitForAttackFinished(bool isObstacle)
        {
            yield return new WaitForSeconds(1f);

            OnTimeNormalized?.Invoke(isObstacle);

            yield return new WaitForSeconds(0.1f);
            WebAim.CanAim = true;
        }

        private IEnumerator WaitForGrounded()
        {
            while (!IsGrounded())
            {
                transform.position += Vector3.down * (5f * Time.deltaTime);
                yield return new WaitForFixedUpdate();
            }
            
            OnPlayerGrounded?.Invoke();
        }
        
        private bool IsGrounded()
        {
            var bottom = _collider.bounds.center - new Vector3(0, _collider.bounds.extents.y, 0);
            
            return Physics.Raycast(bottom, Vector3.down, out _, _groundCheckDistance, _groundLayer);
        }

        private void WebAimOnOnPlayerFly(Vector3 targetPoint, Vector3 hitNormal, Vector3 hitDirection, int hitLayer, Death death, Transform _parent, RaycastHit hit)
        {
            StartCoroutine(PlayerFly(targetPoint, hitNormal, hitDirection, hitLayer, death, _parent, hit));
        }

        private IEnumerator PlayerFly(Vector3 targetPoint, Vector3 hitNormal, Vector3 hitDirection, int hitLayer, Death death, Transform _parent, RaycastHit hit)
        {
            _targetPosition = targetPoint - hitDirection;
            
            transform.SetParent(!death ? _parent : null);
            
            var startHit = hit.transform.position;
            
            var deltaPos = hit.transform.position - startHit;
            var hitPoint = hit.point;
            var hitPos = hitPoint + deltaPos;
            
            OnStartFly?.Invoke(hitNormal, hitPos);
            _isPlayerFly = true;

            var posZ = transform.position.z;
            hitDirection.z = transform.position.z;

            yield return null;
            
            // var minDistance = hitNormal == Vector3.down ? 1f : 1f;
            while (Vector3.Distance(transform.position, hitPos) > 0.4f)
            {
                if (!_isPlayerFly) yield break;

                if (hit.transform.position - startHit != deltaPos)
                {
                    deltaPos = hit.transform.position - startHit;
                    hitPos = hitPoint + deltaPos;
                }
                
                transform.position = Vector3.Lerp(transform.position, hitPos, 5 * Time.deltaTime);
                yield return null;
            }

            var targetPos = hitPos;
            targetPos.z = posZ;
            
            transform.position = targetPos - hitDirection * 0.2f;
            // transform.position = targetPoint;
            
            OnFinishFly?.Invoke(hitNormal, hitPos, hitDirection, hitLayer, death);
            
            if (hitLayer == _enemyObstacleLayer)
                OnHitObstacle?.Invoke();
            
            _isPlayerFly = false;
        }

        // private void OnTriggerEnter(Collider other)
        // {
        //     if (other.gameObject.layer != _obstacleLayer 
        //         // && (other.gameObject.layer != _enemyLayer || !_isPlayerFly) 
        //         &&
        //         other.gameObject.layer != _enemyObstacleLayer && other.gameObject.layer != 11)
        //         // && other.gameObject.layer != 12)
        //         return;
        //
        //     if (other.gameObject.layer == 11)
        //     {
        //         other.transform.parent.GetComponentInChildren<Obstacle>().Fall();
        //         Destroy(other.gameObject);
        //         return;
        //     }
        //
        //     if (other.gameObject.layer == _enemyObstacleLayer)
        //         OnHitObstacle?.Invoke();
        //     
        //     var targetPoint = other.ClosestPoint(transform.position);
        //     OnFinishFly?.Invoke(Vector3.zero, targetPoint, (targetPoint - transform.position).normalized, other.gameObject.layer, other.gameObject.GetComponent<Death>());
        //     _isPlayerFly = false;
        // }
    }
}
