namespace HNC {
    public class NewEnemyDeathState : NewEnemyState {
        public NewEnemyDeathState(NewEnemyController enemy, EnemyFSMState state) : base(enemy, state) { }

        public override void Enter() {
            base.Enter();
            //_enemy.AnimatorComponent.SetTrigger(_enemy.AnimDeathHash);
            //_enemy.Collider.enabled = false;
            _enemy.DetectionSystem.enabled = false;
        }

        public override void Exit() {

        }

        public override void Update() {

        }
    }
}
