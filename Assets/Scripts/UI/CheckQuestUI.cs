using System.Collections;
using System.Collections.Generic;
using Static;
using UnityEngine;
using UnityEngine.UI;

public class CheckQuestUI : MonoBehaviour
{
    private Text _text;
    
    private void OnEnable()
    {
        _text = GetComponentInChildren<Text>();
        TaskGridUIOnOnTaskComplete();
    }

    public void TaskGridUIOnOnTaskComplete()
    {
        var count = 0;
        
        for (var j = 1; j < 7; j++)
        {
            var clearedItemsCount = 0;
            
            for (var i = 3 * j - 3; i < 3 * j; i++)
            {
                var itemMissed = StaticData.Stuff.GetStuffByIndex(i + 1) == 0;
                if (!itemMissed) clearedItemsCount++;
            }

            if (clearedItemsCount >= 3 && !StaticData.ClaimedItems.Contains($"{j - 1}"))
                count++;
        }
        
        if (count > 0)
            _text.text = $"{count}";
        else
            gameObject.SetActive(false);
    }
}
