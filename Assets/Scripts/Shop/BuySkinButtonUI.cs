using System;
using Static;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class BuySkinButtonUI : MonoBehaviour
    {
        [SerializeField] private Text _priceText;

        private int _price;
        private int _id;
        private ShopLoader _shop;
        private bool _isCharacter = true;

        // public static bool IsCharacter => IsCharacter;

        private void Awake()
        {
            _shop = FindObjectOfType<ShopLoader>();
            
            GetComponentInChildren<Button>().onClick.AddListener(() =>
            {
                var result = _isCharacter ? _shop.BuyCharacterById(_id, _price) : _shop.BuyRopeById(_id, _price);
                if(result)
                    gameObject.SetActive(false);
            });
        }

        public void Init(int price, int id)
        {
            _id = id;
            _price = price;
            _priceText.text = $"{_price}";
        }

        public void SetItemType(bool isCharacter)
        {
            _isCharacter = isCharacter;
            gameObject.SetActive(gameObject.activeInHierarchy && !gameObject.activeInHierarchy);
        }
    }
}
