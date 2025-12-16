using System;
using System.Collections;
using System.Collections.Generic;
using Static;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class CheckTaskUI : MonoBehaviour
{
    private Text _text;
    private int _count;
    
    private void OnEnable()
    {
        _text = GetComponentInChildren<Text>();
        TaskGridUIOnOnTaskComplete();
    }

    public void TaskGridUIOnOnTaskComplete()
    {
        if (LevelManager.Instance.CurrentLevelCount < 6)
        {
            gameObject.SetActive(false);
            return;
        }
        
        _count = 0;
        
        for (var i = 0; i < 5; i++)
        {
            if (StaticData.GetTask(i) >= StaticData.GetTaskCount(i) && StaticData.GetClaimedTaskIndex(i) == 0)
                _count++;
        }

        if (_count > 0)
            _text.text = $"{_count}";
        else
            gameObject.SetActive(false);
    }
}
