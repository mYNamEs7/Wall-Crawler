using System;
using System.Collections;
using System.Collections.Generic;
using EnemySpace;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{
    [SerializeField] private GameObject ignoreObject;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 6 && other.TryGetComponent(out Death death) && other.gameObject != ignoreObject)
        {
            var targetPoint = other.ClosestPoint(transform.position);
            death.TakeDamage((targetPoint - transform.position).normalized, targetPoint);
        }
    }
}
