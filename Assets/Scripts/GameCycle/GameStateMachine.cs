using GameCycle.States;

namespace GameCycle
{
    public class GameStateMachine : StateMachine<GameController>
    {
        private readonly MainMenuState _mainMenuState;
        private readonly BossFightState _bossFightState;
        private readonly EnemyDefeatState _enemyDefeatState;
        private readonly EnterGameplayState _enterGameplayState;
        private readonly FindItemState _findItemState;
        private readonly ForgotItemState _forgotItemState;
        private readonly GameplayState _gameplayState;
        private readonly GetRewardState _getRewardState;
        private readonly LoadingState _loadingState;
        private readonly LoseState _loseState;
        private readonly SaveWinStreakState _saveWinStreakState;
        private readonly WinState _winState;
        private readonly WinStreakState _winStreakState;

        public GameStateMachine(GameController context)
        {
            _mainMenuState = new();
            _bossFightState = new();
            _enemyDefeatState = new();
            _enterGameplayState = new();
            _findItemState = new();
            _forgotItemState = new();
            _gameplayState = new();
            _getRewardState = new();
            _loadingState = new();
            _loseState = new();
            _saveWinStreakState = new();
            _winState = new();
            _winStreakState = new();

            AddStates(_mainMenuState, _bossFightState, _enemyDefeatState, _enterGameplayState, _findItemState,
                _forgotItemState, _gameplayState, _getRewardState, _loadingState, _loseState, _saveWinStreakState,
                _winState, _winStreakState);

            TransitionsEnabled = false;
        }

        public void InitGameplayState(bool isBossFight) => _enterGameplayState.Init(isBossFight);

        #region Events

        public event System.Action OnMainMenuStateEnter { add => _mainMenuState.OnEnter += value; remove => _mainMenuState.OnEnter -= value; }
        public event System.Action OnMainMenuStateRun { add => _mainMenuState.OnRun += value; remove => _mainMenuState.OnRun -= value; }
        public event System.Action OnMainMenuStateExit { add => _mainMenuState.OnExit += value; remove => _mainMenuState.OnExit -= value; }
        
        public event System.Action OnBossFightStateEnter { add => _bossFightState.OnEnter += value; remove => _bossFightState.OnEnter -= value; }
        public event System.Action OnBossFightStateRun { add => _bossFightState.OnRun += value; remove => _bossFightState.OnRun -= value; }
        public event System.Action OnBossFightStateExit { add => _bossFightState.OnExit += value; remove => _bossFightState.OnExit -= value; }
        
        public event System.Action OnEnemyDefeatStateEnter { add => _enemyDefeatState.OnEnter += value; remove => _enemyDefeatState.OnEnter -= value; }
        public event System.Action OnEnemyDefeatStateRun { add => _enemyDefeatState.OnRun += value; remove => _enemyDefeatState.OnRun -= value; }
        public event System.Action OnEnemyDefeatStateExit { add => _enemyDefeatState.OnExit += value; remove => _enemyDefeatState.OnExit -= value; }
        
        public event System.Action<bool> OnEnterGameplayStateEnter { add => _enterGameplayState.OnEnter += value; remove => _enterGameplayState.OnEnter -= value; }
        public event System.Action OnEnterGameplayStateRun { add => _enterGameplayState.OnRun += value; remove => _enterGameplayState.OnRun -= value; }
        public event System.Action OnEnterGameplayStateExit { add => _enterGameplayState.OnExit += value; remove => _enterGameplayState.OnExit -= value; }
        
        public event System.Action OnFindItemStateEnter { add => _findItemState.OnEnter += value; remove => _findItemState.OnEnter -= value; }
        public event System.Action OnFindItemStateRun { add => _findItemState.OnRun += value; remove => _findItemState.OnRun -= value; }
        public event System.Action OnFindItemStateExit { add => _findItemState.OnExit += value; remove => _findItemState.OnExit -= value; }
        
        public event System.Action OnForgotItemStateEnter { add => _forgotItemState.OnEnter += value; remove => _forgotItemState.OnEnter -= value; }
        public event System.Action OnForgotItemStateRun { add => _forgotItemState.OnRun += value; remove => _forgotItemState.OnRun -= value; }
        public event System.Action OnForgotItemStateExit { add => _forgotItemState.OnExit += value; remove => _forgotItemState.OnExit -= value; }
        
        public event System.Action OnGameplayStateEnter { add => _gameplayState.OnEnter += value; remove => _gameplayState.OnEnter -= value; }
        public event System.Action OnGameplayStateRun { add => _gameplayState.OnRun += value; remove => _gameplayState.OnRun -= value; }
        public event System.Action OnGameplayStateExit { add => _gameplayState.OnExit += value; remove => _gameplayState.OnExit -= value; }
        
        public event System.Action OnGetRewardStateEnter { add => _getRewardState.OnEnter += value; remove => _getRewardState.OnEnter -= value; }
        public event System.Action OnGetRewardStateRun { add => _getRewardState.OnRun += value; remove => _getRewardState.OnRun -= value; }
        public event System.Action OnGetRewardStateExit { add => _getRewardState.OnExit += value; remove => _getRewardState.OnExit -= value; }
        
        public event System.Action OnLoadingStateEnter { add => _loadingState.OnEnter += value; remove => _loadingState.OnEnter -= value; }
        public event System.Action OnLoadingStateRun { add => _loadingState.OnRun += value; remove => _loadingState.OnRun -= value; }
        public event System.Action OnLoadingStateExit { add => _loadingState.OnExit += value; remove => _loadingState.OnExit -= value; }
        
        public event System.Action OnLoseStateEnter { add => _loseState.OnEnter += value; remove => _loseState.OnEnter -= value; }
        public event System.Action OnLoseStateRun { add => _loseState.OnRun += value; remove => _loseState.OnRun -= value; }
        public event System.Action OnLoseStateExit { add => _loseState.OnExit += value; remove => _loseState.OnExit -= value; }
        
        public event System.Action OnSaveWinStreakStateEnter { add => _saveWinStreakState.OnEnter += value; remove => _saveWinStreakState.OnEnter -= value; }
        public event System.Action OnSaveWinStreakStateRun { add => _saveWinStreakState.OnRun += value; remove => _saveWinStreakState.OnRun -= value; }
        public event System.Action OnSaveWinStreakStateExit { add => _saveWinStreakState.OnExit += value; remove => _saveWinStreakState.OnExit -= value; }
        
        public event System.Action OnWinStateEnter { add => _winState.OnEnter += value; remove => _winState.OnEnter -= value; }
        public event System.Action OnWinStateRun { add => _winState.OnRun += value; remove => _winState.OnRun -= value; }
        public event System.Action OnWinStateExit { add => _winState.OnExit += value; remove => _winState.OnExit -= value; }
        
        public event System.Action OnWinStreakStateEnter { add => _winStreakState.OnEnter += value; remove => _winStreakState.OnEnter -= value; }
        public event System.Action OnWinStreakStateRun { add => _winStreakState.OnRun += value; remove => _winStreakState.OnRun -= value; }
        public event System.Action OnWinStreakStateExit { add => _winStreakState.OnExit += value; remove => _winStreakState.OnExit -= value; }

        #endregion
    }
}