using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameCycle;
using GameCycle.States;
using PlayerSpace;
using Static;
using UnityEngine;

namespace EnemySpace
{
    public class Death : MonoCashed<Rigidbody>
    {
        private event Action OnGameplayEnter;
        public static event Action OnEnemyDeath;
        public static event Action<Transform> OnEnemyHit;
        
        [SerializeField] protected float _force = 1f;
        [SerializeField] private float _stopFallTime = 1f;
        [SerializeField] private int _health = 1;
        [SerializeField] private float _velocityToStop = 0.5f;
        [SerializeField] private EnemyFace _enemyFace;
        [SerializeField] private bool _isKinematic = true;
        [SerializeField] private Transform _enemyFireAttack;
        [SerializeField] private Transform _enemyAlert;
        
        private Animator _animator;
        private List<Rigidbody> _rbList;
        private List<Collider> _colliderList;
        private static readonly int State = Animator.StringToHash("State");

        private LineRenderer _line;
        private Obstacle _range;
        private Laser _laser;

        protected override void Awake()
        {
            base.Awake();
            
            _line = GetComponent<LineRenderer>();
            _range = GetComponentInChildren<Obstacle>();
            _laser = GetComponentInChildren<Laser>();
            
            OnAwake();
        }

        protected virtual void OnAwake()
        {
            _animator = GetComponentInChildren<Animator>();

            OnGameplayEnter += DeathOnGameplayEnter;
            if (gameObject.layer == 6 && !TryGetComponent<Boss>(out _))
            {
                GameController.Instance.OnGameplayStateEnter += InstanceOnOnGameplayStateEnter;
                // OnGameplayEnter += DeathOnGameplayEnter;
            }
            
            if (!_isKinematic) return;
            _rbList = new List<Rigidbody>(GetComponentsInChildren<Rigidbody>()) { GetComponent<Rigidbody>() };
            _rbList.ForEach(rb => rb.isKinematic = true);

            if (gameObject.layer == 7 || TryGetComponent<Boss>(out _))
            {
                _colliderList = new List<Collider>(GetComponentsInChildren<Collider>());
                _colliderList.ForEach(rb => rb.isTrigger = true);
            }

            if (gameObject.layer == 6 && !TryGetComponent<Boss>(out _))
                Cashed1.isKinematic = false;

            var enemyRange = GetComponentInChildren<Obstacle>();
            if (enemyRange)
                enemyRange.OnEnemyAttack += OnEnemyAttack;
            // _colliders.ForEach(_collider => _collider.excludeLayers = 7);
        }

        private void OnDestroy()
        {
            OnGameplayEnter -= DeathOnGameplayEnter;
        }

        private void DeathOnGameplayEnter()
        {
            transform.parent = null;
        }

        private void InstanceOnOnGameplayStateEnter()
        {
            OnGameplayEnter?.Invoke();
        }

        private void OnEnemyAttack()
        {
            if (!_enemyAlert && !_enemyFireAttack) return;
            
            if (_enemyAlert)
            {
                _enemyAlert.gameObject.SetActive(true);
                
                _animator.SetInteger(State, 3);
                StartCoroutine(EnemyAttack());
            }
            else if (_enemyFireAttack)
            {
                _enemyFireAttack.gameObject.SetActive(true);
                StartCoroutine(EnemyAttack());
            }
            
            GetComponent<LineRenderer>().enabled = false;
        }

        private IEnumerator EnemyAttack()
        {
            yield return new WaitForSeconds(0.5f);
            
            _animator.SetInteger(State, 2);
            transform.eulerAngles = Vector3.up * 180f;
            transform.SetParent(LevelManager.Instance.transform.GetChild(0).transform);
            Destroy(this);
        }
        
        public virtual void TakeDamage(Vector3 hitDirection, Vector3 targetPoint)
        {
            if (_health <= 0 || GameController.Instance.CurrentState.GetType() != typeof(GameplayState)) return;
            
            _health--;
            if (!TryGetComponent<Player>(out _) && GetType() != typeof(Obstacle))
            {
                OnEnemyHit?.Invoke(transform);
                if (_health <= 0 && TryGetComponent(out Collider col)) col.enabled = false;
            }

            if (_health > 0)
            {
                Move();
                return;
            }

            _rbList.ForEach(rb => rb.isKinematic = false);
            _colliderList?.ForEach(col => col.isTrigger = false);
            
            targetPoint.z = 0;
            hitDirection.z = 0;
            if (hitDirection.y < 0)
                hitDirection.y = 0;
            if (targetPoint.y < 0)
                targetPoint.y = 0;
            _animator.enabled = false;

            var body = _rbList.OrderBy(rb => Vector3.Distance(rb.position, targetPoint)).First();
            
            body.AddForceAtPosition(hitDirection * _force, targetPoint, ForceMode.Impulse);
            
            OnDeath();
            
            StartCoroutine(WaitForEnemyDeath());
            
            if (!TryGetComponent<Player>(out _) && GetType() != typeof(Obstacle))
            {
                StaticData.KilledEnemies++;
                OnEnemyDeath?.Invoke();
                _enemyFace?.Die();
            }
        }

        private IEnumerator DisableBodies()
        {
            for (var i = 0; i < _rbList.Count; i++)
            {
                if (i + 1 % 3 == 0)
                    yield return null;
                
                _rbList[i].isKinematic = false;
            }
        }

        protected virtual void Move() { }

        protected IEnumerator WaitForEnemyDeath()
        {
            if (_stopFallTime == 0) yield break;
            
            yield return new WaitForSeconds(_stopFallTime);
            
            yield return new WaitUntil(() => Cashed1.velocity.magnitude < _velocityToStop);
            
            OnTriggered();
        }

        protected virtual void OnTriggered()
        {
            
        }

        protected virtual void OnDeath()
        {
            StartCoroutine(ChangeLayer());
            
            if (_line)
                _line.enabled = false;
            
            if (_range)
                Destroy(_range.gameObject);

            if(_laser)
                Destroy(_laser.gameObject);
        }

        private IEnumerator ChangeLayer()
        {
            yield return null;
            
            gameObject.layer = 2;
            enabled = false;
        }

        protected virtual void OnCollisionEnter(Collision other)
        {
            if (gameObject.layer == 6)
            {
                if (other.gameObject.layer == 6)
                {
                    var targetPoint = other.collider.ClosestPoint(transform.position);
                    TakeDamage((targetPoint - transform.position).normalized, targetPoint);
                }
            }
        }
    }
}
