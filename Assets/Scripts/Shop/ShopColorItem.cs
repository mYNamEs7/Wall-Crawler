using System;
using System.Linq;
using SO;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Shop
{
    public enum ItemType
    {
        Character,
        Rope
    }
    
    public enum ItemRare
    {
        Rare,
        Epic,
        Legend
    }
    
    public class ShopColorItem : MonoCashed<Image>
    {
        public static event Action<int, int> OnSelectUnlocked;
        public static event Action OnSelectLocked;
        public static event Action OnSelectSelectable;
        
        [SerializeField] private Transform _rare;
        [SerializeField] private Text _priceText;
        [SerializeField] private Image _itemImage;
        [SerializeField] private Animation _selectedAnimation;
        
        [Header("Lock States")]
        [SerializeField] private Transform _locked;
        [SerializeField] private Transform _unlocked;
        [SerializeField] private Transform _selectable;
        [SerializeField] private Transform _selected;

        private ShopLoader _shopLoader;
        private ItemType _itemType;
        private int _id;
        private int _price;

        public bool IsBought { get; private set; }
        public int Id => _id;

        protected override void Awake()
        {
            base.Awake();

            GetComponent<Button>().onClick.AddListener(() =>
            {
                // if (!IsBought) return;
                
                switch (_itemType)
                {
                    case ItemType.Character:
                        _shopLoader.SelectCharacterById(Id, IsBought);
                        break;
                    case ItemType.Rope:
                        _shopLoader.SelectRopeById(Id, IsBought);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                var isLocked = LevelManager.Instance.CurrentLevelCount <=
                               GameSettings.Instance.GetRewardsByType(_itemType)[_id]
                                   .level;
                
                switch (isLocked)
                {
                    case true when !IsBought:
                        OnSelectLocked?.Invoke();
                        break;
                    case false when !IsBought:
                        OnSelectUnlocked?.Invoke(_price, _id);
                        break;
                    default:
                        OnSelectSelectable?.Invoke();
                        break;
                }
            });
        }

        public void Init(ItemType itemType, int id, int isBought, ShopLoader shopLoader)
        {
            _id = id;
            _itemType = itemType;
            _shopLoader = shopLoader;
            IsBought = isBought == 1;
            
            var targetReward = GameSettings.Instance.GetRewardsByType(itemType)[_id];
            
            SetRareAndPrice(targetReward.rare);
            _itemImage.sprite = targetReward.icon;
            
            var isLocked = LevelManager.Instance.CurrentLevelCount <= targetReward.level;
            _locked.gameObject.SetActive(isLocked && !IsBought);
            _unlocked.gameObject.SetActive(!isLocked && !IsBought);
            
            _selectable.gameObject.SetActive(IsBought);
        }

        private void SetRareAndPrice(ItemRare itemRare)
        {
            foreach (Transform rare in _rare)
            {
                rare.gameObject.SetActive(rare.GetSiblingIndex() == (int)itemRare);
            }

            _price = 1000 * ((int)itemRare + 1);
            _priceText.text = $"{_price}";
        }

        public int BuyColor()
        {
            _locked.gameObject.SetActive(false);
            _unlocked.gameObject.SetActive(false);
            _selectable.gameObject.SetActive(true);
            
            IsBought = true;
            SelectColor();
            
            return Id;
        }

        public ShopColorItem SelectColor()
        {
            _selectedAnimation.Play();
            
            if (!IsBought) return null;
            
            _selected.gameObject.SetActive(true);
            return this;
        }

        public void DeselectColor()
        {
            _selectedAnimation.Stop();
            _selectedAnimation.transform.localScale = Vector3.one;
            
            _selected.gameObject.SetActive(false);
        }
    }
}
