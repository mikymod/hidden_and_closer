namespace HNC
{
    public abstract class NewEnemyState : IState
    {
        protected NewEnemyController _enemy;
        private readonly EnemyFSMState _state;

        public NewEnemyState(NewEnemyController enemy, EnemyFSMState state)
        {
            _enemy = enemy;
            _state = state;
        }

        public virtual void Enter()
        {
            _enemy.CurrentState = _state;
            _enemy.UI.OnZombieChangeState?.Invoke(_enemy.CurrentState);
            NewChangeStateEvent.OnChangeState?.Invoke(_enemy.gameObject, _enemy.CurrentState);
            //_enemy.UI.OnZombieChangeState?.Invoke(_state);
        }

        public abstract void Exit();
        public abstract void Update();
    }
}