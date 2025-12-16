using System.Linq;
using SO;
using Static;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class LockedItemUI : MonoCashed<Image>
    {
        [SerializeField] private Transform[] _children;
        
        private void OnEnable()
        {
            ShopLoader.OnCharacterSkinChanged += ShopLoaderOnOnCharacterSkinChanged;
            ShopLoader.OnRopeSkinChanged += ShopLoaderOnOnRopeSkinChanged;

            Cashed1.enabled = false;
            foreach (var child in _children)
            {
                child.gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            ShopLoader.OnCharacterSkinChanged -= ShopLoaderOnOnCharacterSkinChanged;
            ShopLoader.OnRopeSkinChanged -= ShopLoaderOnOnRopeSkinChanged;
        }

        public void ShopLoaderOnOnRopeSkinChanged()
        {
            var isLocked = GameSettings.Instance.GetRewardsByType(ItemType.Rope)
                    .FirstOrDefault(reward => reward.id == StaticData.PreviewSelectedRopeId).level >=
                LevelManager.Instance.CurrentLevelCount &&
                StaticData.PreviewSelectedRopeId != StaticData.SelectedRopeId;
            
            Cashed1.enabled = isLocked;
            foreach (var child in _children)
            {
                child.gameObject.SetActive(Cashed1.enabled);
            }
        }

        public void ShopLoaderOnOnCharacterSkinChanged()
        {
            var isLocked = GameSettings.Instance.GetRewardsByType(ItemType.Character)
                               .FirstOrDefault(reward => reward.id == StaticData.PreviewSelectedCharacterId).level >=
                           LevelManager.Instance.CurrentLevelCount &&
                           StaticData.PreviewSelectedCharacterId != StaticData.SelectedCharacterId;
            
            Cashed1.enabled = isLocked;
            foreach (var child in _children)
            {
                child.gameObject.SetActive(Cashed1.enabled);
            }
        }
    }
}
