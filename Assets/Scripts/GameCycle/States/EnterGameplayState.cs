namespace GameCycle.States
{
    public class EnterGameplayState : IState<GameController>
    {
        public GameController Initializer => null;

        private bool _isBossFight;

        public void Init(bool isBossFight)
        {
            _isBossFight = isBossFight;
        }
    
        public event System.Action<bool> OnEnter;
        public event System.Action OnRun;
        public event System.Action OnExit;
    
        void IState<GameController>.OnEnter() => OnEnter?.Invoke(_isBossFight);
        void IState<GameController>.OnRun() => OnRun?.Invoke();
        void IState<GameController>.OnExit() => OnExit?.Invoke();
    }
}
