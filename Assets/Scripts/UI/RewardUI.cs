using System;
using System.Linq;
using Cameras;
using GameCycle;
using Shop;
using SO;
using Static;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using YG;

namespace UI
{
    public class RewardUI : MonoBehaviour
    {
        public static event Action<int> OnCharacterReward;
        
        [SerializeField] private Image _image;
        [SerializeField] private Text _text;
        [SerializeField] private Image _charImage;
        [SerializeField] private Transform _playerSpawner;
        [SerializeField] private bool _isTwiceReward;
        [SerializeField] private Transform _ropeBG;
        [SerializeField] private Transform _characterBG;

        [SerializeField] private UnityEvent _afterReward;
        [SerializeField] private UnityEvent _default;

        private bool _isWinStreak;
        private GameSettings.Reward _targetReward;
        private GameSettings.Reward _targetCharacterReward;

        private void OnEnable()
        {
            _default?.Invoke();
            FindObjectOfType<ResolutionHandler>().ResetFOV();
            
            _playerSpawner.gameObject.SetActive(true);
            
            StreakRewardUI.InitRewardMenu += StreakRewardUIOnInitRewardMenu;
            DailyRewardUI.InitRewardMenu += StreakRewardUIOnInitRewardMenu;
            StreakRewardUI.InitTwiceRewardMenu += StreakRewardUIOnInitTwiceRewardMenu;

            _targetReward =
                GameSettings.Instance.rewards.FirstOrDefault(reward =>
                    reward.level == LevelManager.Instance.CurrentLevelCount);

            SetImage(_targetReward);

            string enText;
            string ruText;
            if (_isTwiceReward)
            {
                enText = _targetReward.details.enText;
                ruText = _targetReward.details.ruText;
            }
            else
            {
                enText = _targetReward.details.enName;
                ruText = _targetReward.details.ruName;
            }

            _text.text = GameSettings.Instance.CurrentLanguageIndex == 0 ? ruText : enText;
        }

        private void OnDisable()
        {
            FindObjectOfType<ResolutionHandler>().FOVByResolution();
            
            _playerSpawner.gameObject.SetActive(false);
            
            _isWinStreak = false;
            StreakRewardUI.InitRewardMenu -= StreakRewardUIOnInitRewardMenu;
            DailyRewardUI.InitRewardMenu -= StreakRewardUIOnInitRewardMenu;
            StreakRewardUI.InitTwiceRewardMenu -= StreakRewardUIOnInitTwiceRewardMenu;
            
            _default?.Invoke();
        }

        private void SetImage(GameSettings.Reward reward)
        {
            if (reward.type == ItemType.Rope)
            {
                if (!_isTwiceReward)
                {
                    _ropeBG.gameObject.SetActive(true);
                    _characterBG.gameObject.SetActive(false);
                }
                _image.sprite = reward.icon;
            }
            else
            {
                OnCharacterReward?.Invoke(reward.id);
                if (!_isTwiceReward)
                {
                    _ropeBG.gameObject.SetActive(false);
                    _characterBG.gameObject.SetActive(true);
                }
            }
        }

        private void StreakRewardUIOnInitTwiceRewardMenu(int characterId, ItemType characterType, int ropeId, ItemType ropeType)
        {
            _isWinStreak = true;
            
            _targetCharacterReward =
                GameSettings.Instance.rewards.FirstOrDefault(reward =>
                    reward.type == characterType && reward.id == characterId);
            
            SetImage(_targetCharacterReward);
            
            _targetReward =
                GameSettings.Instance.rewards.FirstOrDefault(reward =>
                    reward.type == ropeType && reward.id == ropeId);
            
            SetImage(_targetReward);
            if (!_text.TryGetComponent<LanguageYG>(out var languageYg)) return;
            
            if (_isTwiceReward)
            {
                languageYg.en = _targetReward.details.enText;
                languageYg.ru = _targetReward.details.ruText;
            }
            else
            {
                languageYg.en = _targetReward.details.enName;
                languageYg.ru = _targetReward.details.ruName;
            }
            _text.text = languageYg.languages[GameSettings.Instance.CurrentLanguageIndex];
        }

        private void StreakRewardUIOnInitRewardMenu(int id, ItemType type)
        {
            _isWinStreak = true;
            
            _targetReward =
                GameSettings.Instance.rewards.FirstOrDefault(reward =>
                    reward.type == type && reward.id == id);
            
            SetImage(_targetReward);
            if (!_text.TryGetComponent<LanguageYG>(out var languageYg)) return;
            
            if (_isTwiceReward)
            {
                languageYg.en = _targetReward.details.enText;
                languageYg.ru = _targetReward.details.ruText;
            }
            else
            {
                languageYg.en = _targetReward.details.enName;
                languageYg.ru = _targetReward.details.ruName;
            }
            _text.text = languageYg.languages[GameSettings.Instance.CurrentLanguageIndex];
        }

        public void GetReward()
        {
            ShopLoader.GetReward(_targetReward);
            ShopLoader.GetReward(_targetCharacterReward);
            
            _afterReward?.Invoke();
        }

        public void Continue()
        {
            if(_isWinStreak)
                GameController.Instance.WinStreak(false);
            else
                GameController.Instance.Win(false);
        }

        public void SimpleGetReward()
        {
            ShopLoader.GetReward(_targetReward);
            ShopLoader.GetReward(_targetCharacterReward);
        }
    }
}
