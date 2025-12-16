using System;
using System.Collections;
using System.Collections.Generic;
using SO;
using UnityEngine;

public class HitEffect : MonoBehaviour
{
    [SerializeField] private GameObject _enText;
    [SerializeField] private GameObject _ruText;

    private void OnEnable()
    {
        _enText.SetActive(GameSettings.Instance.CurrentLanguageIndex != 0);
        _ruText.SetActive(GameSettings.Instance.CurrentLanguageIndex == 0);
        
        Destroy(gameObject, 1f);
    }
}
