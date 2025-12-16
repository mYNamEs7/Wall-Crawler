using System;
using System.Collections;
using System.Threading.Tasks;
using Cameras;
using EnemySpace;
using GameCycle.States;
using PlayerSpace;
using Shop;
using Static;
using UI;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using YG;

namespace GameCycle
{
    [DefaultExecutionOrder(-1)]
    public class GameController : Singleton<GameController>
    {
        public static event Action OnFillProgress;
        
        public static int IsStreakReward;
        
        [SerializeField] private ParticleSystem _hitEffect;
        
        private readonly GameStateMachine _stateMachine;

        public GameController() => _stateMachine = new GameStateMachine(this);
        
        public IState<GameController> CurrentState => _stateMachine.CurrentState;

        protected override void OnAwake()
        {
            Application.targetFrameRate = 60;
        }

        private async void Start()
        {
            if (!Application.isMobilePlatform)
                QualitySettings.SetQualityLevel(1);

            ShopDataLoader.LoadData();
        }

        private void OnEnable()
        {
            Player.OnTimeSlowDown += PlayerOnOnTimeSlowDown;
            Player.OnTimeNormalized += PlayerOnOnTimeNormalized;
            Death.OnEnemyHit += DeathOnOnEnemyDeath;
        }
        
        private void OnDisable()
        {
            Player.OnTimeSlowDown -= PlayerOnOnTimeSlowDown;
            Player.OnTimeNormalized -= PlayerOnOnTimeNormalized;
            Death.OnEnemyHit -= DeathOnOnEnemyDeath;
        }
        
        private void DeathOnOnEnemyDeath(Transform enemy)
        {
            var targetPosition = enemy.position;
            targetPosition.y += 1f;
            
            var effect = Instantiate(_hitEffect, targetPosition, Quaternion.identity);
            effect.Play();
        }

        private void PlayerOnOnTimeNormalized(bool _)
        {
            Time.timeScale = 1f;
        }

        private void PlayerOnOnTimeSlowDown()
        {
            Time.timeScale = 0.45f;
        }

        private void OnDestroy() => _stateMachine.CurrentState?.OnExit();

        private void FixedUpdate() => _stateMachine.Run();

        public void MainMenu()
        {
            FindObjectOfType<ResolutionHandler>().FOVByResolution();
            Time.timeScale = 1f;
            _stateMachine.SetState<MainMenuState>();
        }

        public async void BossFight()
        {
            _stateMachine.SetState<BossFightState>();
            StartCoroutine(WaitForStartGameplay());
        }

        private IEnumerator WaitForStartGameplay()
        {
            yield return new WaitForSeconds(1.5f);
            
            EnterGameplay(true);
        }
        
        public void EnemyDefeat() => _stateMachine.SetState<EnemyDefeatState>();

        public void EnterGameplay(bool isBossFight)
        {
            _stateMachine.InitGameplayState(isBossFight);
            _stateMachine.SetState<EnterGameplayState>();
            StartCoroutine(WaitForGameplay());
        }

        private IEnumerator WaitForGameplay()
        {
            yield return new WaitForSeconds(1.5f);
            
            Gameplay();
        }
        
        public void FindItem() => _stateMachine.SetState<FindItemState>();
        public void ForgotItem() => _stateMachine.SetState<ForgotItemState>();
        private void Gameplay() => _stateMachine.SetState<GameplayState>();

        public void GetReward(int isStreakReward = 0)
        {
            IsStreakReward = isStreakReward;
            _stateMachine.SetState<GetRewardState>();
        }
        public void Loading() => _stateMachine.SetState<LoadingState>();

        public void Lose()
        {
            _stateMachine.SetState<LoseState>();
        }

        public void SaveWinStreak()
        {
            if (StaticData.WinStreak > 1)
                _stateMachine.SetState<SaveWinStreakState>();
            else
                Lose();
        }

        public void ResetWinStreak() => StaticData.WinStreak = 0;

        public void Win(bool isFillProgress)
        {
            _stateMachine.SetState<WinState>();

            if (!isFillProgress) return;
            
            OnFillProgress?.Invoke();
        }

        public static void ShowAd()
        {
            if (!YandexGame.nowAdsShow &&
                YandexGame.timerShowAd >= YandexGame.Instance.infoYG.fullscreenAdInterval)
                YandexGame.FullscreenShow();
        }

        public void WinStreak(bool isAddStreak = true)
        {
            if (isAddStreak)
                if (LevelManager.Instance.CurrentLevelCount >= 10)
                    StaticData.WinStreak++;
            
            if (StaticData.WinStreak > 1)
                _stateMachine.SetState<WinStreakState>();
            else
                Win(true);
        } 
        
        private void OnApplicationFocus(bool hasFocus)
        {
            Time.timeScale = !hasFocus ? 0 : 1;
        }

        private void OnApplicationPause(bool isPaused)
        {
            Time.timeScale = isPaused ? 0 : 1;
        }

        #region Events

        public event Action OnMainMenuStateEnter { add => _stateMachine.OnMainMenuStateEnter += value; remove => _stateMachine.OnMainMenuStateEnter -= value; }
        public event Action OnMainMenuStateRun { add => _stateMachine.OnMainMenuStateRun += value; remove => _stateMachine.OnMainMenuStateRun -= value; }
        public event Action OnMainMenuStateExit { add => _stateMachine.OnMainMenuStateExit += value; remove => _stateMachine.OnMainMenuStateExit -= value; }
        
        public event Action OnBossFightStateEnter { add => _stateMachine.OnBossFightStateEnter += value; remove => _stateMachine.OnBossFightStateEnter -= value; }
        public event Action OnBossFightStateRun { add => _stateMachine.OnBossFightStateRun += value; remove => _stateMachine.OnBossFightStateRun -= value; }
        public event Action OnBossFightStateExit { add => _stateMachine.OnBossFightStateExit += value; remove => _stateMachine.OnBossFightStateExit -= value; }
        
        public event Action OnEnemyDefeatStateEnter { add => _stateMachine.OnEnemyDefeatStateEnter += value; remove => _stateMachine.OnEnemyDefeatStateEnter -= value; }
        public event Action OnEnemyDefeatStateRun { add => _stateMachine.OnEnemyDefeatStateRun += value; remove => _stateMachine.OnEnemyDefeatStateRun -= value; }
        public event Action OnEnemyDefeatStateExit { add => _stateMachine.OnEnemyDefeatStateExit += value; remove => _stateMachine.OnEnemyDefeatStateExit -= value; }
        
        public event Action<bool> OnEnterGameplayStateEnter { add => _stateMachine.OnEnterGameplayStateEnter += value; remove => _stateMachine.OnEnterGameplayStateEnter -= value; }
        public event Action OnEnterGameplayStateRun { add => _stateMachine.OnEnterGameplayStateRun += value; remove => _stateMachine.OnEnterGameplayStateRun -= value; }
        public event Action OnEnterGameplayStateExit { add => _stateMachine.OnEnterGameplayStateExit += value; remove => _stateMachine.OnEnterGameplayStateExit -= value; }
        
        public event Action OnFindItemStateEnter { add => _stateMachine.OnFindItemStateEnter += value; remove => _stateMachine.OnFindItemStateEnter -= value; }
        public event Action OnFindItemStateRun { add => _stateMachine.OnFindItemStateRun += value; remove => _stateMachine.OnFindItemStateRun -= value; }
        public event Action OnFindItemStateExit { add => _stateMachine.OnFindItemStateExit += value; remove => _stateMachine.OnFindItemStateExit -= value; }
        
        public event Action OnForgotItemStateEnter { add => _stateMachine.OnForgotItemStateEnter += value; remove => _stateMachine.OnForgotItemStateEnter -= value; }
        public event Action OnForgotItemStateRun { add => _stateMachine.OnForgotItemStateRun += value; remove => _stateMachine.OnForgotItemStateRun -= value; }
        public event Action OnForgotItemStateExit { add => _stateMachine.OnForgotItemStateExit += value; remove => _stateMachine.OnForgotItemStateExit -= value; }
        
        public event Action OnGameplayStateEnter { add => _stateMachine.OnGameplayStateEnter += value; remove => _stateMachine.OnGameplayStateEnter -= value; }
        public event Action OnGameplayStateRun { add => _stateMachine.OnGameplayStateRun += value; remove => _stateMachine.OnGameplayStateRun -= value; }
        public event Action OnGameplayStateExit { add => _stateMachine.OnGameplayStateExit += value; remove => _stateMachine.OnGameplayStateExit -= value; }
        
        public event Action OnGetRewardStateEnter { add => _stateMachine.OnGetRewardStateEnter += value; remove => _stateMachine.OnGetRewardStateEnter -= value; }
        public event Action OnGetRewardStateRun { add => _stateMachine.OnGetRewardStateRun += value; remove => _stateMachine.OnGetRewardStateRun -= value; }
        public event Action OnGetRewardStateExit { add => _stateMachine.OnGetRewardStateExit += value; remove => _stateMachine.OnGetRewardStateExit -= value; }
        
        public event Action OnLoadingStateEnter { add => _stateMachine.OnLoadingStateEnter += value; remove => _stateMachine.OnLoadingStateEnter -= value; }
        public event Action OnLoadingStateRun { add => _stateMachine.OnLoadingStateRun += value; remove => _stateMachine.OnLoadingStateRun -= value; }
        public event Action OnLoadingStateExit { add => _stateMachine.OnLoadingStateExit += value; remove => _stateMachine.OnLoadingStateExit -= value; }
        
        public event Action OnLoseStateEnter { add => _stateMachine.OnLoseStateEnter += value; remove => _stateMachine.OnLoseStateEnter -= value; }
        public event Action OnLoseStateRun { add => _stateMachine.OnLoseStateRun += value; remove => _stateMachine.OnLoseStateRun -= value; }
        public event Action OnLoseStateExit { add => _stateMachine.OnLoseStateExit += value; remove => _stateMachine.OnLoseStateExit -= value; }
        
        public event Action OnSaveWinStreakStateEnter { add => _stateMachine.OnSaveWinStreakStateEnter += value; remove => _stateMachine.OnSaveWinStreakStateEnter -= value; }
        public event Action OnSaveWinStreakStateRun { add => _stateMachine.OnSaveWinStreakStateRun += value; remove => _stateMachine.OnSaveWinStreakStateRun -= value; }
        public event Action OnSaveWinStreakStateExit { add => _stateMachine.OnSaveWinStreakStateExit += value; remove => _stateMachine.OnSaveWinStreakStateExit -= value; }
        
        public event Action OnWinStateEnter { add => _stateMachine.OnWinStateEnter += value; remove => _stateMachine.OnWinStateEnter -= value; }
        public event Action OnWinStateRun { add => _stateMachine.OnWinStateRun += value; remove => _stateMachine.OnWinStateRun -= value; }
        public event Action OnWinStateExit { add => _stateMachine.OnWinStateExit += value; remove => _stateMachine.OnWinStateExit -= value; }
        
        public event Action OnWinStreakStateEnter { add => _stateMachine.OnWinStreakStateEnter += value; remove => _stateMachine.OnWinStreakStateEnter -= value; }
        public event Action OnWinStreakStateRun { add => _stateMachine.OnWinStreakStateRun += value; remove => _stateMachine.OnWinStreakStateRun -= value; }
        public event Action OnWinStreakStateExit { add => _stateMachine.OnWinStreakStateExit += value; remove => _stateMachine.OnWinStreakStateExit -= value; }

        #endregion
    }
}