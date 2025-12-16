using System;
using System.Threading.Tasks;
using GameCycle;
using Static;
using UnityEngine;
using YG;
using Random = UnityEngine.Random;

namespace UI
{
    public class UIController : MonoBehaviour
    {
        [Header("Canvases")] 
        [SerializeField] private Canvas _loadingUI;
        [SerializeField] private Canvas _mainMenuUI;
        [SerializeField] private Canvas _bossFightUI;
        [SerializeField] private Canvas _bossFightGameUI;
        [SerializeField] private Canvas _gameUI;
        [SerializeField] private Transform _winStreakUI;
        [SerializeField] private Transform _saveWinStreakUI;
        [SerializeField] private Canvas _winUI;
        [SerializeField] private Canvas _getRewardUI;
        [SerializeField] private Canvas _loseUI;
        [SerializeField] private Canvas _moneyUI;

        private static GameController _gameController => GameController.Instance;
        private bool _isBossFight;
        private Transform _rewardUI;

        private void Start()
        {
            _gameController.MainMenu();
        }

        private void OnEnable()
        {
            _gameController.OnLoadingStateEnter += GameControllerOnOnLoadingStateEnter;
            _gameController.OnLoadingStateExit += GameControllerOnOnLoadingStateExit;
            
            _gameController.OnMainMenuStateEnter += GameControllerOnOnMainMenuStateEnter;
            _gameController.OnMainMenuStateExit += GameControllerOnOnMainMenuStateExit;
            
            _gameController.OnBossFightStateEnter += GameControllerOnOnBossFightStateEnter;
            _gameController.OnBossFightStateExit += GameControllerOnOnBossFightStateExit;
            
            _gameController.OnEnterGameplayStateEnter += GameControllerOnOnEnterGameplayStateEnter;
            _gameController.OnEnterGameplayStateExit += GameControllerOnOnEnterGameplayStateExit;
            
            _gameController.OnGameplayStateEnter += GameControllerOnOnGameplayStateEnter;
            _gameController.OnGameplayStateExit += GameControllerOnOnGameplayStateExit;
            
            _gameController.OnWinStreakStateEnter += GameControllerOnOnWinStreakStateEnter;
            _gameController.OnWinStreakStateExit += GameControllerOnOnWinStreakStateExit;
            
            _gameController.OnSaveWinStreakStateEnter += GameControllerOnOnSaveWinStreakStateEnter;
            _gameController.OnSaveWinStreakStateExit += GameControllerOnOnSaveWinStreakStateExit;
            
            _gameController.OnWinStateEnter += GameControllerOnOnWinStateEnter;
            _gameController.OnWinStateExit += GameControllerOnOnWinStateExit;
            
            _gameController.OnGetRewardStateEnter += GameControllerOnOnGetRewardStateEnter;
            _gameController.OnGetRewardStateExit += GameControllerOnOnGetRewardStateExit;
            
            _gameController.OnLoseStateEnter += GameControllerOnOnLoseStateEnter;
            _gameController.OnLoseStateExit += GameControllerOnOnLoseStateExit;
        }

        private void GameControllerOnOnEnterGameplayStateExit()
        {
            if(_isBossFight)
                _bossFightUI.gameObject.SetActive(false);
        }

        private void GameControllerOnOnSaveWinStreakStateExit()
        {
            _saveWinStreakUI.gameObject.SetActive(false);
        }

        private void GameControllerOnOnSaveWinStreakStateEnter()
        {
            _saveWinStreakUI.gameObject.SetActive(true);
        }

        private void GameControllerOnOnGetRewardStateExit()
        {
            _rewardUI.gameObject.SetActive(false);
        }

        private void GameControllerOnOnGetRewardStateEnter()
        {
            _rewardUI = _getRewardUI.transform.GetChild(GameController.IsStreakReward);
            _rewardUI.gameObject.SetActive(true);
            
            GameController.IsStreakReward = 0;
        }

        private void GameControllerOnOnWinStreakStateExit()
        {
            _winStreakUI.gameObject.SetActive(false);
        }

        private void GameControllerOnOnWinStreakStateEnter()
        {
            _winStreakUI.gameObject.SetActive(true);
        }

        private void GameControllerOnOnLoseStateExit()
        {
            _loseUI.gameObject.SetActive(false);
        }

        private void GameControllerOnOnLoseStateEnter()
        {
            _loseUI.gameObject.SetActive(true);
        }

        private void GameControllerOnOnWinStateExit()
        {
            _winUI.gameObject.SetActive(false);
        }

        private void GameControllerOnOnWinStateEnter()
        {
            _winUI.gameObject.SetActive(true);
        }

        public void StopGameplay()
        {
            var rand = Random.Range(0, 2);

            if (rand == 0) _gameController.WinStreak();
            else _gameController.SaveWinStreak();
        }

        private void GameControllerOnOnGameplayStateExit()
        {
            if (_isBossFight) _bossFightGameUI.gameObject.SetActive(false);
            else _gameUI.gameObject.SetActive(false);
            
            _moneyUI.gameObject.SetActive(true);
        }

        private void GameControllerOnOnGameplayStateEnter()
        {
            if (_isBossFight) _bossFightGameUI.gameObject.SetActive(true);
            else _gameUI.gameObject.SetActive(true);
        }

        private void GameControllerOnOnEnterGameplayStateEnter(bool isBossFight)
        {
            _isBossFight = isBossFight;
            
            if(isBossFight)
                _bossFightUI.gameObject.SetActive(true);
            
            _moneyUI.gameObject.SetActive(false);
        }

        private void GameControllerOnOnBossFightStateExit()
        {
            
        }

        private void GameControllerOnOnBossFightStateEnter()
        {
            
        }

        private void GameControllerOnOnMainMenuStateExit()
        {
            _mainMenuUI.gameObject.SetActive(false);
        }

        private void GameControllerOnOnLoadingStateExit()
        {
            _loadingUI.gameObject.SetActive(false);
        }

        private void GameControllerOnOnMainMenuStateEnter()
        {
            _mainMenuUI.gameObject.SetActive(true);
        }

        private void GameControllerOnOnLoadingStateEnter()
        {
            _loadingUI.gameObject.SetActive(true);
        }
    }
}
