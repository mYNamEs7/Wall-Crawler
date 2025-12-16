using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DailyRewardUI1 : MonoBehaviour
{
    [SerializeField] private Text _moneyText;
    [SerializeField] private Button _claimButton;

    private void OnEnable()
    {
        _claimButton.gameObject.SetActive(true);
    }

    public void Init(int moneyAmount, Action onClick)
    {
        gameObject.SetActive(true);
        _moneyText.text = $"{moneyAmount}";
        _claimButton.onClick.AddListener(() => StartCoroutine(OnClaimMoney(onClick)));
    }

    private IEnumerator OnClaimMoney(Action onClick)
    {
        onClick();
        _claimButton.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.8f);
        
        gameObject.SetActive(false);
    }
}
