using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private Transform _bomb;
    [SerializeField] private Transform _explosive;
    
    private void OnEnable()
    {
        GetComponent<Obstacle>().OnEnemyHit += OnEnemyHit;
    }

    private void OnEnemyHit()
    {
        _bomb.gameObject.SetActive(false);
        _explosive.gameObject.SetActive(true);
    }
}
