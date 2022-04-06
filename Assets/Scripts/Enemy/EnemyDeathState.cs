namespace HNC {
    public class EnemyDeathState : EnemyState {
        public EnemyDeathState(EnemyController enemy, EnemyFSMState state) : base(enemy, state) { }

        public override void Enter()
        {
            base.Enter();
            _enemy.AnimatorComponent.SetTrigger(_enemy.AnimDeathHash);
        }

        public override void Exit() {

        }

        public override void Update() {

        }
    }
}
