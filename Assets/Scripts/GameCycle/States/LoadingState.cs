namespace GameCycle.States
{
    public class LoadingState : IState<GameController>
    {
        public GameController Initializer => null;
    
        public event System.Action OnEnter;
        public event System.Action OnRun;
        public event System.Action OnExit;
    
        void IState<GameController>.OnEnter() => OnEnter?.Invoke();
        void IState<GameController>.OnRun() => OnRun?.Invoke();
        void IState<GameController>.OnExit() => OnExit?.Invoke();
    }
}
