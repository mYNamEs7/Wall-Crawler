using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnemySpace;
using PlayerSpace.Web;
using Static;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Serialization;

namespace PlayerSpace
{
    [RequireComponent(typeof(RigBuilder))]
    public class PlayerAnimator : Singleton<PlayerAnimator>
    {
        public static event Action<Vector3, Vector3, Death> OnStartAttack;
        public static event Action<Vector3, Vector3> OnPlayerDead;
        
        [SerializeField] private int _enemyLayer;
        [SerializeField] private int _obstacleLayer;
        [SerializeField] private int _enemyObstacleLayer = 9;
        [SerializeField] private List<Rig> _allRigs;
        [SerializeField] private bool _isValidate;
        
        private Player _player;
        private Animator _animator;
        private int _lastAnimHash = IsBottom;
        private bool _isSimilar;
        public StartAnim[] LastAnim { get; private set; } = new[] { StartAnim.Bottom, StartAnim.Bottom };
        
        private static readonly int IsFly = Animator.StringToHash("IsFly");
        private static readonly int IsTop = Animator.StringToHash("IsTop");
        private static readonly int IsLeft = Animator.StringToHash("IsLeft");
        private static readonly int IsMirror = Animator.StringToHash("IsMirror");
        private static readonly int IsBottom = Animator.StringToHash("IsBottom");
        private static readonly int IsAttackSuccess = Animator.StringToHash("IsAttackSuccess");
        private static readonly int Stand = Animator.StringToHash("Stand");

        public bool IsFlyState => _animator.GetCurrentAnimatorStateInfo(0).IsName("IsFly") || _animator.GetNextAnimatorStateInfo(0).IsName("IsFly");

        private void OnEnable()
        {
            _player = FindObjectOfType<Player>();
            _animator = GetComponent<Animator>();
            
            Player.OnStartFly += PlayerOnOnStartFly;
            Player.OnFinishFly += PlayerOnOnFinishFly;
            _player.OnPlayerGrounded += PlayerOnOnPlayerGrounded;
            WebAim.OnWebVisibleChanged += WebAimOnOnWebVisibleChanged;
        }

        private void OnDisable()
        {
            Player.OnStartFly -= PlayerOnOnStartFly;
            Player.OnFinishFly -= PlayerOnOnFinishFly;
            WebAim.OnWebVisibleChanged -= WebAimOnOnWebVisibleChanged;
        }

        private void OnValidate()
        {
#if UNITY_EDITOR
            if (!_isValidate) return;

            GameObject.Find("Rig 1").GetComponentInChildren<MultiAimConstraint>().data.constrainedObject = GameObject.Find("Left arm").transform;
            GameObject.Find("Rig 2").GetComponentInChildren<MultiAimConstraint>().data.constrainedObject = GameObject.Find("Spine").transform;
            GameObject.Find("Mirror Rig 1").GetComponentInChildren<MultiAimConstraint>().data.constrainedObject = GameObject.Find("Right arm").transform;
            GameObject.Find("Mirror Rig 2").GetComponentInChildren<MultiAimConstraint>().data.constrainedObject = GameObject.Find("Spine").transform;
            
            _allRigs.Clear();
            _allRigs = FindObjectsOfType<Rig>().ToList();

            var rigBuilder = GetComponent<RigBuilder>();
            rigBuilder.layers.Clear();
            rigBuilder.layers.Add(new RigLayer(_allRigs.First(rig => rig.name == "Rig 2"), true));
            rigBuilder.layers.Add(new RigLayer(_allRigs.First(rig => rig.name == "Mirror Rig 2"), false));
            rigBuilder.layers.Add(new RigLayer(_allRigs.First(rig => rig.name == "Rig 1"), true));
            rigBuilder.layers.Add(new RigLayer(_allRigs.First(rig => rig.name == "Mirror Rig 1"), false));
#endif
        }

        private void WebAimOnOnWebVisibleChanged(bool isEnable)
        {
            _allRigs.ForEach(rig => rig.weight = isEnable ? 1f : 0f);
        }

        public void SetStandAnim(int id)
        {
            _animator.SetInteger(Stand, id);
        }

        public void SetAnim(StartAnim startAnim)
        {
            _animator.SetBool(IsFly, false);
            _animator.SetBool(IsTop, false);
            _animator.SetBool(IsLeft, false);
            _animator.SetBool(IsBottom, false);
            _animator.SetBool(IsMirror, false);
            
            switch (startAnim)
            {
                case StartAnim.Left:
                    _animator.SetBool(IsFly, true);
                    _animator.SetBool(IsLeft, true);

                    StartCoroutine(StopFly());
                    break;
                case StartAnim.Top:
                    _animator.SetBool(IsFly, true);
                    _animator.SetBool(IsTop, true);

                    StartCoroutine(StopFly());
                    break;
                case StartAnim.Right:
                    _animator.SetBool(IsFly, true);
                    _animator.SetBool(IsLeft, true);
                    _animator.SetBool(IsMirror, true);

                    StartCoroutine(StopFly());
                    break;
                case StartAnim.Bottom:
                    _animator.SetBool(IsFly, true);
                    _animator.SetBool(IsBottom, true);

                    StartCoroutine(StopFly());
                    break;
                case StartAnim.None:
                default:
                    break;
            }
        }

        private IEnumerator StopFly()
        {
            yield return new WaitForSeconds(0.1f);
            
            _animator.SetBool(IsFly, false);
        }

        private void PlayerOnOnPlayerGrounded()
        {
            _animator.SetBool(IsAttackSuccess, false);
            _animator.SetBool(IsFly, false);
            _animator.SetBool(IsTop, false);
            _animator.SetBool(IsLeft, false);
            _animator.SetBool(IsBottom, false);
            _animator.SetBool(IsMirror, false);
        }

        public static void InvokePlayerDeath(Vector3 targetPoint, Vector3 hitDirection) => OnPlayerDead?.Invoke(hitDirection, targetPoint);
        public void InvokePlayerDeath(Vector3 targetPoint) => OnPlayerDead?.Invoke((targetPoint - transform.position).normalized, targetPoint);

        private void PlayerOnOnFinishFly(Vector3 hitNormal, Vector3 targetPoint, Vector3 hitDirection, int hitLayer, Death death)
        {
            if (!_animator.GetBool(IsFly)) return;
            
            _animator.SetBool(IsFly, false);
            
            if (_isSimilar)
            {
                _animator.SetBool(IsTop, false);
                _animator.SetBool(IsLeft, false);
                _animator.SetBool(IsBottom, false);
                _animator.SetBool(IsMirror, false);
                
                _animator.SetBool(_lastAnimHash, true);
                if (_lastAnimHash == IsMirror)
                    _animator.SetBool(IsLeft, true);

                _isSimilar = false;
            }

            if (hitLayer == _obstacleLayer)
            {
                OnPlayerDead?.Invoke(hitDirection, targetPoint);
                return;
            }

            if (hitLayer != _enemyLayer && hitLayer != _enemyObstacleLayer)
            {
                StartCoroutine(AllowAim());
                return;
            }
            
            _animator.SetBool(IsAttackSuccess, true);
            OnStartAttack?.Invoke(hitDirection, targetPoint, death);
        }

        private static IEnumerator AllowAim()
        {
            yield return new WaitForSeconds(0.1f);
            
            WebAim.CanAim = true;
        }

        private void PlayerOnOnStartFly(Vector3 hitNormal, Vector3 targetPoint)
        {
            _animator.SetBool(IsTop, false);
            _animator.SetBool(IsLeft, false);
            _animator.SetBool(IsMirror, false);
            _animator.SetBool(IsBottom, false);
            
            var targetAnim = DetermineCubeSide(hitNormal);
            var startAnim = targetAnim;
            
            Rotate(targetPoint, targetAnim);

            if (targetAnim == _lastAnimHash)
                _isSimilar = true;

            if (targetAnim == -1 || targetAnim == _lastAnimHash)
            {
                print("111111111");
                targetAnim = targetPoint.x > transform.position.x ? IsMirror : IsLeft;
            }
            else
                StaticData.OnWall++;
            
            _animator.SetBool(IsFly, true);
            _animator.SetBool(targetAnim, true);
            
            if (targetAnim == IsMirror)
                _animator.SetBool(IsLeft, true);
            
            _lastAnimHash = startAnim;
        }

        private void Rotate(Vector3 targetPoint, int targetAnim)
        {
            if (targetAnim == IsLeft || targetAnim == IsMirror)
            {
                Flip(180f);
                return;
            }
            
            if (targetPoint.x > transform.position.x)
            {
                Flip(180f);
                _animator.SetBool(IsMirror, false);
            }
            else if (targetPoint.x < transform.position.x)
            {
                Flip(0f);
                _animator.SetBool(IsMirror, true);
            }
        }

        private void Flip(float value)
        {
            var scale = transform.eulerAngles;
            scale.y = value;
            transform.eulerAngles = scale;
            
            // _rigBuilder.layers.ForEach(layer => layer.active = !layer.active);
        }
        
        private int DetermineCubeSide(Vector3 hitNormal)
        {
            LastAnim[0] = LastAnim[1];
            
            if (hitNormal == Vector3.up)
            {
                LastAnim[1] = StartAnim.Bottom;
                print("IsBottom");
                return IsBottom;
            }

            if (hitNormal == Vector3.down)
            {
                LastAnim[1] = StartAnim.Top;
                print("IsTop");
                return IsTop;
            }

            if (hitNormal == Vector3.left)
            {
                LastAnim[1] = StartAnim.Right;
                print("IsRight");
                return IsMirror;
            }

            if (hitNormal == Vector3.right)
            {
                LastAnim[1] = StartAnim.Left;
                print("IsLeft");
                return IsLeft;
            }

            return -1;
        }
    }
}
