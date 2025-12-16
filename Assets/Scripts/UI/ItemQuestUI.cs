using System;
using System.Collections.Generic;
using System.Linq;
using SO;
using Static;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ItemQuestUI : MonoBehaviour
    {
        public static event Action OnGetPrize;
        public static event Action<int> OnShowItem;

        [SerializeField] private int _rewardId;
        [SerializeField] private List<Elements> _elements = new(3);
        
        [Header("Buttons")] 
        [SerializeField] private Transform btnCleared;
        [SerializeField] private Transform btnClearedAds;
        [SerializeField] private Transform btnUnlock;
        [SerializeField] private Transform btnClaimed;
        [SerializeField] private Transform btnLocked;

        private int _id;
        
        public void Init(int id)
        {
            _id = id;
            
            var endId = 3 * (id + 1);
            for (var i = endId - 3; i < endId; i++)
            {
                if (i == 0) OnShowItem?.Invoke(i);
                
                var items = GameSettings.Instance.stuffOnLevel;
                var targetItem = items[i];
                var targetElement = _elements[i - 3 * id];
                
                targetElement.image.sprite = targetItem.image;
                
                var firstItemLevel = items[endId - 3].level;
                var currentLevel = LevelManager.Instance.CurrentLevelCount;
                
                targetElement.button.onClick.RemoveAllListeners();
                var i1 = i;
                if (currentLevel > firstItemLevel)
                    targetElement.button.onClick.AddListener(() => OnShowItem?.Invoke(i1));
                
                if (StaticData.ClaimedItems.Contains($"{id}"))
                {
                    targetElement.locked.gameObject.SetActive(false);
                    targetElement.unlocked.gameObject.SetActive(true);
                    
                    targetElement.missed.gameObject.SetActive(false);
                    targetElement.cleared.gameObject.SetActive(true);

                    continue;
                }
                
                targetElement.locked.gameObject.SetActive(currentLevel <= firstItemLevel);
                targetElement.unlocked.gameObject.SetActive(currentLevel > firstItemLevel);

                var itemMissed = StaticData.Stuff.GetStuffByIndex(targetItem.stuffIndex) == 0;
                var canGetPrize = currentLevel > items[endId - 1].level;
                targetElement.missed.gameObject.SetActive(itemMissed && canGetPrize);
                targetElement.cleared.gameObject.SetActive(!itemMissed && canGetPrize);
            }

            if (StaticData.ClaimedItems.Contains($"{id}"))
            {
                btnClaimed.gameObject.SetActive(true);
                
                btnCleared.gameObject.SetActive(false);
                btnClearedAds.gameObject.SetActive(false);
                
                return;
            }

            btnCleared.gameObject.SetActive(_elements.All(element => element.cleared.gameObject.activeInHierarchy));
            
            btnClearedAds.gameObject.SetActive(_elements.Count(element => element.missed.gameObject.activeInHierarchy) > 0);
            btnClearedAds.GetComponent<RewardedButton>().SetRewardId(_rewardId + id);

            if (btnCleared.gameObject.activeInHierarchy || btnClearedAds.gameObject.activeInHierarchy) return;
            
            btnUnlock.gameObject.SetActive(_elements.All(element => element.unlocked.gameObject.activeInHierarchy));
            btnClaimed.gameObject.SetActive(StaticData.ClaimedItems.Contains($"{id}"));
            btnLocked.gameObject.SetActive(_elements.All(element => element.locked.gameObject.activeInHierarchy));
        }

        public void GetPrize()
        {
            OnGetPrize?.Invoke();
            
            var targetText = $"{_id}";
            print(targetText);
            if (!StaticData.ClaimedItems.Contains(targetText))
                StaticData.ClaimedItems += targetText;
            
            Init(_id);
        }
        
        [Serializable]
        private struct Elements
        {
            public Image image;
            public Transform locked;
            public Transform unlocked;
            public Transform missed;
            public Transform cleared;
            public Button button;
        }
    }
}
