namespace HNC {
    public abstract class EnemyState : IState {
        protected EnemyController _enemy;
        private readonly EnemyFSMState _state;

        public EnemyState(EnemyController enemy, EnemyFSMState state) {
            _enemy = enemy;
            _state = state;
        }

        public virtual void Enter() => _enemy.CurrentState = _state;

        public abstract void Exit();
        public abstract void Update();
    }
}