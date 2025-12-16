using System;
using System.Collections;
using System.Collections.Generic;
using PlayerSpace;
using UnityEngine;

public class BrokenGlass : MonoBehaviour
{
    [SerializeField] private Transform _wholeGlass;
    [SerializeField] private Transform _brokenGlass;
    [SerializeField] private int _playerLayer;
    
    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.layer);
        if (other.gameObject.layer != _playerLayer && other.gameObject.layer != 13) return;
        
        _wholeGlass.gameObject.SetActive(false);
        _brokenGlass.gameObject.SetActive(true);
        var myPosition = transform.position;

        var otherPosition = other.transform.position;

        var collisionDirection = otherPosition - myPosition;

        var collisionDirectionNormalized = -collisionDirection.normalized;
        
        foreach (Transform glass in _brokenGlass)
        {
            glass.GetComponent<Rigidbody>().AddForce(collisionDirectionNormalized * 10f);
        }
    }
}
