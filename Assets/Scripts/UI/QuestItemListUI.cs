using System;
using UnityEngine;

namespace UI
{
    public class QuestItemListUI : MonoBehaviour
    {
        public static event Action OnQuestExit;
        
        [SerializeField] private Transform _content;
        [SerializeField] private ItemQuestUI _itemPrefab;

        private void OnEnable()
        {
            SpawnItems();
        }

        private void OnDisable()
        {
            OnQuestExit?.Invoke();
            
            foreach (Transform child in _content)
            {
                Destroy(child.gameObject);
            }
        }

        private void SpawnItems()
        {
            for (var i = 0; i < 6; i++)
            {
                var itemQuest = Instantiate(_itemPrefab, _content);
                itemQuest.Init(i);
            }
        }
    }
}
