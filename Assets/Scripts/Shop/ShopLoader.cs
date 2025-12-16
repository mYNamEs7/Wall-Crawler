using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SO;
using Static;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Shop
{
    public class ShopLoader : MonoBehaviour
    {
        public static event Action OnMoneyChanged;
        public static event Action OnCharacterSkinChanged;
        public static event Action OnRopeSkinChanged;
        
        [Header("Panels")] 
        [SerializeField] private Transform _characterPanel;
        [SerializeField] private Transform _ropePanel;
        
        [Header("Prefabs")]
        [SerializeField] private Transform _shopItemPrefab;

        [Header("Other")] 
        [SerializeField] private BuySkinButtonUI _buyButton;

        [SerializeField] private UnityEvent _onDisable;

        private List<ShopColorItem> _characterItems = new();
        private List<ShopColorItem> _ropeItems = new();

        private static ShopItems _shopItems => ShopDataLoader.ShopItems;

        private void Start()
        {
            StartCoroutine(LoadData());
        }

        private void OnEnable()
        {
            print("SHOP");
            _characterItems = new List<ShopColorItem>();
            _ropeItems = new List<ShopColorItem>();
            
            StartCoroutine(DisplayShopItems());
            
            ShopColorItem.OnSelectUnlocked += ShopColorItemOnOnSelectUnlocked;
            ShopColorItem.OnSelectLocked += ShopColorItemOnOnSelectLocked;
            ShopColorItem.OnSelectSelectable += ShopColorItemOnOnSelectSelectable;
        }
        
        private void OnDisable()
        {
            // _characterItems = new List<ShopColorItem>();
            // _ropeItems = new List<ShopColorItem>();
            
            foreach (Transform child in _characterPanel)
            {
                Destroy(child.gameObject);
            }
            
            foreach (Transform child in _ropePanel)
            {
                Destroy(child.gameObject);
            }
            
            _onDisable?.Invoke();
            
            ShopColorItem.OnSelectUnlocked -= ShopColorItemOnOnSelectUnlocked;
            ShopColorItem.OnSelectLocked -= ShopColorItemOnOnSelectLocked;
            ShopColorItem.OnSelectSelectable -= ShopColorItemOnOnSelectSelectable;
        }

        private IEnumerator LoadData()
        {
            yield return new WaitForSeconds(0.5f);
            
            gameObject.SetActive(false);
        }

        private void ShopColorItemOnOnSelectSelectable()
        {
            _buyButton.gameObject.SetActive(false);
        }

        private void ShopColorItemOnOnSelectLocked()
        {
            _buyButton.gameObject.SetActive(false);
        }

        private void ShopColorItemOnOnSelectUnlocked(int price, int id)
        {
            _buyButton.gameObject.SetActive(true);
            _buyButton.Init(price, id);
        }

        private IEnumerator DisplayShopItems()
        {
            yield return new WaitUntil(() => _shopItems != null);

            SpawnItems(ItemType.Character, _characterPanel, _shopItems.characters, _shopItemPrefab,
                _characterItems, StaticData.SelectedCharacterId);
            StaticData.PreviewSelectedCharacterId = StaticData.SelectedCharacterId;
            
            SpawnItems(ItemType.Rope, _ropePanel, _shopItems.ropes, _shopItemPrefab,
                _ropeItems, StaticData.SelectedRopeId);
            StaticData.PreviewSelectedRopeId = StaticData.SelectedRopeId;
        }

        private void SpawnItems(ItemType itemType, Transform itemPanel,
            IReadOnlyList<ShopItemBase> items, Transform itemPrefab,
            IList<ShopColorItem> itemList, int selectedIndex)
        {
            var targetItems = GameSettings.Instance.GetRewardsByType(itemType);
                
            for (var i = 0; i < items.Count; i++)
            {
                var currentItem = items[i];

                if (i >= targetItems.Count) break;
                
                var spawnedItem = Instantiate(itemPrefab, itemPanel).GetComponentInChildren<ShopColorItem>();
                spawnedItem.Init(itemType, i, currentItem.isBought, this);
                itemList.Add(spawnedItem);
            }

            itemList[selectedIndex].SelectColor();
        }

        private static int BuyItem(int id, IReadOnlyList<ShopColorItem> shopItemList, IEnumerable<ShopItemBase> items,
            int price)
        {
            if (StaticData.MoneyAmount < price) return -1;

            StaticData.MoneyAmount -= price;
            OnMoneyChanged?.Invoke();
            
            // var colorItems = shopItemList.Where(color => !color.IsBought).ToList();
            var colorToBuy = shopItemList[id];
            
            return BuyItemById(colorToBuy, items);
        }

        private static int BuyItemById(ShopColorItem colorToBuy, IEnumerable<ShopItemBase> items)
        {
            var boughtColorId = colorToBuy.BuyColor();
            ShopDataLoader.SetItemBoughtById(items, boughtColorId);

            return colorToBuy.Id;
        }
        
        private static void SelectItemById(List<ShopColorItem> shopItemList, int id)
        {
            var itemToSelect = shopItemList.First(item => item.Id == id);
            
            foreach (var shopColorItem in shopItemList)
            {
                shopColorItem.DeselectColor();
            }
            
            itemToSelect.SelectColor();
        }
        
        public void SelectCharacterById(int id, bool isBought)
        {
            SelectItemById(_characterItems, id);
            StaticData.PreviewSelectedCharacterId = id;
            if (isBought)
                StaticData.SelectedCharacterId = id;

            OnCharacterSkinChanged?.Invoke();
        }
        
        public void SelectRopeById(int id, bool isBought)
        {
            SelectItemById(_ropeItems, id);
            StaticData.PreviewSelectedRopeId = id;
            if (isBought)
                StaticData.SelectedRopeId = id;

            OnRopeSkinChanged?.Invoke();
        }

        public int GetRandomNotBoughtRopeId()
        {
            var ropes = _ropeItems.Where(rope => !rope.IsBought).ToList();
            return _ropeItems.IndexOf(ropes[Random.Range(0, ropes.Count)]);
        }
        
        public int GetRandomNotBoughtCharacterId()
        {
            var ropes = _characterItems.Where(rope => !rope.IsBought).ToList();
            return _characterItems.IndexOf(ropes[Random.Range(0, ropes.Count)]);
        }
        
        public bool BuyCharacterById(int id, int price)
        {
            var colorId = BuyItem(id, _characterItems, _shopItems.characters, price);
            
            if(colorId != -1)
                SelectCharacterById(colorId, true);

            return colorId != -1;
        }
        
        public bool BuyRopeById(int id, int price)
        {
            var colorId = BuyItem(id, _ropeItems, _shopItems.ropes, price);
            
            if(colorId != -1)
                SelectRopeById(colorId, true);
            
            return colorId != -1;
        }
        
        public static void GetReward(GameSettings.Reward targetReward)
        {
            if (targetReward.id == 0) return;
            
            print("REW");
            // var targetReward =
            //     GameSettings.Instance.rewards.FirstOrDefault(reward =>
            //         reward.level == LevelManager.Instance.CurrentLevelCount);

            if (targetReward.type == ItemType.Character)
            {
                var id = targetReward.id;
                
                ShopDataLoader.SetItemBoughtById(_shopItems.characters, id);
                
                StaticData.PreviewSelectedCharacterId = id;
                StaticData.SelectedCharacterId = id;
                OnCharacterSkinChanged?.Invoke();
            }
            else
            {
                var id = targetReward.id;
                
                ShopDataLoader.SetItemBoughtById(_shopItems.ropes, id);
                
                StaticData.PreviewSelectedRopeId = id;
                StaticData.SelectedRopeId = id;
                OnCharacterSkinChanged?.Invoke();
            }
        }
    }
}