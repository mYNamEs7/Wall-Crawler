using System.Linq;
using SO;
using Static;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class ShopItemNameUI : MonoCashed<Text>
    {
        private void OnEnable()
        {
            ShopLoader.OnCharacterSkinChanged += ShopLoaderOnOnCharacterSkinChanged;
            ShopLoader.OnRopeSkinChanged += ShopLoaderOnOnRopeSkinChanged;

            ShopLoaderOnOnCharacterSkinChanged();
        }

        private void OnDisable()
        {
            ShopLoader.OnCharacterSkinChanged -= ShopLoaderOnOnCharacterSkinChanged;
            ShopLoader.OnRopeSkinChanged -= ShopLoaderOnOnRopeSkinChanged;
        }

        public void ShopLoaderOnOnRopeSkinChanged()
        {
            var targetRewardDetails = GameSettings.Instance.GetRewardsByType(ItemType.Rope)
                .FirstOrDefault(reward => reward.id == StaticData.PreviewSelectedRopeId).details;

            Cashed1.text = GameSettings.Instance.CurrentLanguageIndex == 0
                ? targetRewardDetails.ruName
                : targetRewardDetails.enName;
        }

        public void ShopLoaderOnOnCharacterSkinChanged()
        {
            var targetRewardDetails = GameSettings.Instance.GetRewardsByType(ItemType.Character)
                .FirstOrDefault(reward => reward.id == StaticData.PreviewSelectedCharacterId).details;

            Cashed1.text = GameSettings.Instance.CurrentLanguageIndex == 0
                ? targetRewardDetails.ruName
                : targetRewardDetails.enName;
        }
    }
}
