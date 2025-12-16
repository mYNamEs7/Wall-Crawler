using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EnemySpace;
using PlayerSpace;
using SO;
using UnityEngine;

public class Obstacle : Death
{
    public static event Action<Vector3, Vector3, Vector3, int, Death> OnPlayerDead;
    public event Action OnEnemyAttack;
    public event Action OnEnemyHit;
    
    private enum Type
    {
        Enemy,
        Player
    }

    [SerializeField] private Type _type;
    [SerializeField] private int _enemyLayer = 6;
    [SerializeField] private int _playerLayer = 7;
    [SerializeField] private bool _isPlayAduio;
    [SerializeField] private bool _isEnemyRange;
    [SerializeField] private bool _isBox;

    private List<Rigidbody> _obstacles;

    private void Start()
    {
        _obstacles = FindObjectsOfType<Obstacle>().Select(obstacle => obstacle.GetComponent<Rigidbody>()).ToList();
    }

    public override void TakeDamage(Vector3 hitDirection, Vector3 targetPoint)
    {
        foreach (var obstacle in _obstacles.Where(obstacle => obstacle))
        {
            obstacle.isKinematic = false;
            
            if (Vector3.Distance(transform.position, obstacle.position) < 1.5f)
            {
                obstacle.AddForceAtPosition(hitDirection * _force, targetPoint, ForceMode.Impulse);
                obstacle.gameObject.layer = 15;
                if (obstacle.TryGetComponent(out Obstacle obs)) obs.enabled = false;
            }
        }

        Cashed1.AddForceAtPosition(hitDirection * _force, targetPoint, ForceMode.Impulse);
        gameObject.layer = 15;
        enabled = false;
    }

    protected override void OnTriggered()
    {
        base.OnTriggered();
        Cashed1.isKinematic = true;
    }

    public static void InvokePlayerDeath(Vector3 normal, Vector3 targetPoint, Vector3 direction, int hitLayer, Death death) =>
        OnPlayerDead?.Invoke(normal, targetPoint, direction, hitLayer, death);

    public void Fall()
    {
        Cashed1.isKinematic = false;
        Cashed1.AddForce(Vector3.down * _force, ForceMode.Force);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (_type)
        {
            case Type.Enemy when other.gameObject.layer != _enemyLayer || (_isBox && Cashed1.velocity.magnitude <= 0.1f):
                return;
            case Type.Enemy:
            {
                OnEnemyHit?.Invoke();
                
                var targetPoint = other.ClosestPoint(transform.position);
                other.GetComponent<Death>().TakeDamage((targetPoint - transform.position).normalized, targetPoint);
                break;
            }
            case Type.Player when other.gameObject.layer != _playerLayer || (_isBox && Cashed1.velocity.magnitude > 0.1f):
                return;
            case Type.Player:
            {
                OnEnemyAttack?.Invoke();
                
                var targetPoint = other.ClosestPoint(transform.position);
                // other.GetComponent<Death>().TakeDamage((targetPoint - transform.position).normalized, targetPoint);
                var dir = _isEnemyRange
                    ? -(targetPoint - transform.position).normalized
                    : (targetPoint - transform.position).normalized;
                OnPlayerDead?.Invoke(Vector3.zero, targetPoint, dir,
                    other.gameObject.layer, other.gameObject.GetComponent<Death>());
                PlayerAnimator.InvokePlayerDeath(targetPoint, dir);
                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
