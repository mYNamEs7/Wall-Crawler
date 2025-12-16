using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cameras;
using EnemySpace;
using PlayerSpace.Web;
using Shop;
using Static;
using UI;
using UnityEngine;

namespace PlayerSpace
{
    public enum StartAnim
    {
        None,
        Left,
        Top,
        Right,
        Bottom
    }
    
    public class PlayerSpawner : MonoBehaviour
    {
        [SerializeField] private List<Player> _players;
        [SerializeField] private List<Material> _ropes;
        [SerializeField] private StartAnim _startAnim;
        [SerializeField] public bool _isPreview;
        [SerializeField] public bool _isReward;
        [SerializeField] public bool _isWebPreview;

        public bool _isWallAnim => _startAnim != StartAnim.Bottom && _startAnim != StartAnim.None;

        private void OnEnable()
        {
            foreach (var player in FindObjectsOfType<Player>())
            {
                Destroy(player.gameObject);
            }
            
            if (_isReward)
            {
                RewardUI.OnCharacterReward += RewardUIOnOnCharacterReward;
                return;
            }

            StartCoroutine(Spawn1());
        }

        private IEnumerator Spawn1()
        {
            yield return null;
            
            ShopLoader.OnCharacterSkinChanged += ShopLoaderOnOnCharacterSkinChanged;
            ShopLoader.OnRopeSkinChanged += ShopLoaderOnOnRopeSkinChanged;

            SpawnCharacter();
        }

        private void OnDisable()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            
            ShopLoader.OnCharacterSkinChanged -= ShopLoaderOnOnCharacterSkinChanged;
            ShopLoader.OnRopeSkinChanged -= ShopLoaderOnOnRopeSkinChanged;
            if (_isReward)
                RewardUI.OnCharacterReward -= RewardUIOnOnCharacterReward;
        }

        private void RewardUIOnOnCharacterReward(int id)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            if (id == 0) return;

            var targetPlayer = _players[id];
            var player = Instantiate(targetPlayer, transform);
            if (_isPreview && !_isWebPreview)
                player.GetComponentInChildren<PlayerAnimator>().SetStandAnim(id);

            if (_isPreview) return;
            
            FindObjectOfType<CameraController>().SetPlayer(player.Armature);
            FindObjectOfType<LevelController>().SetPlayer(player);
        }

        private void SpawnCharacter()
        {
            print("1");
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            print("1");
            var targetPlayer =
                _players[_isPreview ? StaticData.PreviewSelectedCharacterId : StaticData.SelectedCharacterId];
            print(targetPlayer.name);
            var player = Instantiate(targetPlayer, transform);
            print("1");
            player.GetComponentInChildren<PlayerAnimator>().SetAnim(_startAnim);
            
            if (_isPreview && !_isWebPreview)
                player.GetComponentInChildren<PlayerAnimator>().SetStandAnim(_isPreview ? StaticData.PreviewSelectedCharacterId : StaticData.SelectedCharacterId);
            print("1");
            ShopLoaderOnOnRopeSkinChanged();
            print("2!");
            if (_isPreview) return;
            
            FindObjectOfType<CameraController>().SetPlayer(player.Armature);
            FindObjectOfType<LevelController>().SetPlayer(player);
        }

        private void ShopLoaderOnOnRopeSkinChanged()
        {
            GetComponentInChildren<WebFire>().GetComponent<LineRenderer>().material =
                _ropes[_isPreview ? StaticData.PreviewSelectedRopeId : StaticData.SelectedRopeId];
        }

        private void ShopLoaderOnOnCharacterSkinChanged() => SpawnCharacter();
    }
}
