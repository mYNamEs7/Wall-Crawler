using System;
using System.Collections;
using System.Collections.Generic;
using SO;
using UI;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private float _rotatedSpeed = 5f;
    
    private Transform _spawnedItem;
    
    private void OnEnable()
    {
        ItemQuestUI.OnShowItem += ItemQuestUIOnOnShowItem;
        QuestItemListUI.OnQuestExit += QuestItemListUIOnOnQuestExit;
    }
    
    private void OnDisable()
    {
        ItemQuestUI.OnShowItem -= ItemQuestUIOnOnShowItem;
        QuestItemListUI.OnQuestExit -= QuestItemListUIOnOnQuestExit;
    }
    
    private void QuestItemListUIOnOnQuestExit()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void ItemQuestUIOnOnShowItem(int id)
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        
        _spawnedItem = Instantiate(GameSettings.Instance.stuffOnLevel[id].itemPrefab, transform);
        _spawnedItem.GetComponent<Rigidbody>().isKinematic = true;
        
        StopAllCoroutines();
        StartCoroutine(RotateItem());
    }

    private IEnumerator RotateItem()
    {
        while (true)
        {
            transform.Rotate(Vector3.up * (_rotatedSpeed * Time.deltaTime));
            yield return null;
        }
    }
}
