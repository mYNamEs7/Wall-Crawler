using UnityEngine;

namespace EnemySpace
{
    public class PathPoint : MonoBehaviour
    {
        [SerializeField] private int _animationId;
        [SerializeField] private int _targetRotation;
        [SerializeField] private int _targetSpeed;
        [SerializeField] private float _waitTime;

        public int AnimationId => _animationId;
        public int TargetRotation => _targetRotation;
        public int TargetSpeed => _targetSpeed;
        public float WaitTime => _waitTime;
    }
}
