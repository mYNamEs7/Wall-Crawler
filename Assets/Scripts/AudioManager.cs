using System;
using System.Collections;
using System.Collections.Generic;
using EnemySpace;
using PlayerSpace;
using PlayerSpace.Web;
using UnityEngine;

public class AudioManager : MonoCashed<AudioSource>
{
    [SerializeField] private AudioClip _webClip;
    [SerializeField] private AudioClip _flyClip;
    [SerializeField] private AudioClip _enemyDeathClip;
    [SerializeField] private AudioClip _playerDeathClip;
    [SerializeField] private AudioClip _bossDamagedClip;
    [SerializeField] private AudioClip _hitClip;
    
    private void OnEnable()
    {
        WebAim.OnDrawWeb += WebAimOnOnDrawWeb;
        WebAim.OnPlayerFly += WebAimOnOnPlayerFly;
        Death.OnEnemyDeath += DeathOnOnEnemyDeath;
        PlayerAnimator.OnPlayerDead += PlayerAnimatorOnOnPlayerDead;
        Boss.OnStartMove += BossOnOnStartMove;
        Player.OnHitObstacle += ObstacleOnOnHitObstacle;
    }

    private void OnDisable()
    {
        WebAim.OnDrawWeb -= WebAimOnOnDrawWeb;
        WebAim.OnPlayerFly -= WebAimOnOnPlayerFly;
        Death.OnEnemyDeath -= DeathOnOnEnemyDeath;
        PlayerAnimator.OnPlayerDead -= PlayerAnimatorOnOnPlayerDead;
        Boss.OnStartMove -= BossOnOnStartMove;
        Player.OnHitObstacle -= ObstacleOnOnHitObstacle;
    }

    private void ObstacleOnOnHitObstacle()
    {
        Cashed1.PlayOneShot(_hitClip);
    }

    private void BossOnOnStartMove(Transform obj)
    {
        Cashed1.PlayOneShot(_bossDamagedClip);
    }

    private void PlayerAnimatorOnOnPlayerDead(Vector3 arg1, Vector3 arg2)
    {
        Cashed1.PlayOneShot(_playerDeathClip);
    }

    private void DeathOnOnEnemyDeath()
    {
        Cashed1.PlayOneShot(_enemyDeathClip);
    }

    private void WebAimOnOnPlayerFly(Vector3 arg1, Vector3 arg2, Vector3 arg3, int arg4, Death arg5, Transform arg6, RaycastHit hit)
    {
        Cashed1.PlayOneShot(_flyClip);
    }

    private void WebAimOnOnDrawWeb(Vector3 arg1, Vector3 arg2, RaycastHit hit)
    {
        Cashed1.PlayOneShot(_webClip);
    }
}
