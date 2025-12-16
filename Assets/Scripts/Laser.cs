using System;
using System.Collections;
using System.Collections.Generic;
using Cameras;
using EnemySpace;
using PlayerSpace;
using SO;
using UnityEngine;

public class Laser : MonoCashed<LineRenderer>
{
    [SerializeField] private Transform _start;
    [SerializeField] private Transform _impact;

    [SerializeField] private Transform _startLinePoint;
    [SerializeField] private Transform _endLinePoint;

    private PlayerAnimator _playerAnimator;

    private void Start()
    {
        _playerAnimator = FindObjectOfType<PlayerAnimator>();
    }

    private void FixedUpdate()
    {
        SetLinePoints();
        
        var firstPoint = Cashed1.GetPosition(0);

        var dir = Cashed1.GetPosition(1) - Cashed1.GetPosition(0);

        if (Physics.Raycast(firstPoint, dir, out var hit, 100f, GameSettings.Instance.layers.laserMask))
        {
            var targetPosition = hit.point;
            targetPosition.z = 0f;
            
            Cashed1.SetPosition(1, targetPosition);

            if (_start && _impact)
            {
                _start.position = Cashed1.GetPosition(0);
                _impact.position = Cashed1.GetPosition(1);
            }
        }
        
        firstPoint = Cashed1.GetPosition(0);
        dir = Cashed1.GetPosition(1) - Cashed1.GetPosition(0);
        
        if (Physics.Raycast(firstPoint, dir, out hit, dir.magnitude,
                GameSettings.Instance.layers.playerMask | GameSettings.Instance.layers.enemyLayer)) 
        {
            if (hit.transform.TryGetComponent<Boss>(out _) || (hit.transform.gameObject.layer == 6 && hit.transform.root.gameObject == transform.root.gameObject)) return;
            
            if ((GameSettings.Instance.layers.enemyLayer & (1 << hit.transform.gameObject.layer)) != 0 &&
                hit.transform.TryGetComponent<Death>(out var death))
                death.TakeDamage(Cashed1.GetPosition(1) - hit.point, hit.point);
            else
                Obstacle.InvokePlayerDeath(hit.normal, hit.point, (Cashed1.GetPosition(1) - hit.point).normalized,
                    gameObject.layer, null);
            
            // _playerAnimator.InvokePlayerDeath(hit.point);
        }
    }

    private void SetLinePoints()
    {
        if (!_startLinePoint || !_endLinePoint || Cashed1.GetPosition(0) == _startLinePoint.position) return;
        
        Cashed1.SetPosition(0, _startLinePoint.position);
        Cashed1.SetPosition(1, _endLinePoint.position);
    }

    public void Deactivate()
    {
        Cashed1.enabled = false;
        enabled = false;
    }
}
