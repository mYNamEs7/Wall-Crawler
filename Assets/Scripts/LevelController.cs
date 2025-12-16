using System;
using System.Collections;
using System.Collections.Generic;
using Cameras;
using EnemySpace;
using GameCycle;
using PlayerSpace;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [Header("Enemy")] 
    [SerializeField] private int _enemyCount;
    
    [Header("Borders")]
    [SerializeField] private Transform _upBorder;
    [SerializeField] private Transform _rightBorder;
    [SerializeField] private Transform _leftBorder;
    [SerializeField] private Transform _bottomBorder;

    [Header("Camera")] 
    [SerializeField] private Transform _cameraPosition;

    [SerializeField] private float _pcFov = 45;
    [SerializeField] private float _baseFov = 60;
    [SerializeField] private float _maxFov = 75;

    private Rigidbody _playerBody;
    private CameraController _camera;

    public int EnemyCount => _enemyCount;

    public Vector3 CameraPosition => _cameraPosition ? _cameraPosition.position : Vector3.zero;

    private void Awake()
    {
        ResolutionHandler.PC_FOV = _pcFov;
        ResolutionHandler.Base_FOV = _baseFov;
        ResolutionHandler.Max_FOV = _maxFov;
        
        if(LevelManager.Instance.CurrentLevelCount > 80)
            Destroy(FindObjectOfType<Stuff>()?.gameObject);
        
        FindObjectOfType<CameraController>().SetBorders(_upBorder.position.y, _rightBorder.position.x,
            _leftBorder.position.x, _bottomBorder.position.y);
        _camera = FindObjectOfType<CameraController>();
    }

    private void OnEnable()
    {
        Death.OnEnemyDeath += PlayerOnOnTimeSlowDown;
        PlayerAnimator.OnPlayerDead += PlayerAnimatorOnOnPlayerDead;
    }

    private void OnDisable()
    {
        Death.OnEnemyDeath -= PlayerOnOnTimeSlowDown;
        PlayerAnimator.OnPlayerDead -= PlayerAnimatorOnOnPlayerDead;
    }

    public void SetPlayer(Player player) => _playerBody = player.GetComponent<Rigidbody>();
    
    private void PlayerAnimatorOnOnPlayerDead(Vector3 arg1, Vector3 arg2)
    {
        StartCoroutine(WaitForLose());
    }

    private IEnumerator WaitForLose()
    {
        yield return new WaitForSeconds(1f);
        // yield return new WaitUntil(() => _playerBody.velocity.magnitude < 5f || !_camera.IsPlayerVisible());

        if (_enemyCount > 0)
            GameController.Instance.SaveWinStreak();
    }

    private void PlayerOnOnTimeSlowDown()
    {
        StartCoroutine(WaitForEnemyRemove());
    }

    private IEnumerator WaitForEnemyRemove()
    {
        _enemyCount--;
        
        yield return new WaitForSeconds(1f);
        
        if(_enemyCount <= 0)
            GameController.Instance.WinStreak();
    }
}
