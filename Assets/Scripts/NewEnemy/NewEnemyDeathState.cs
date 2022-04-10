namespace HNC {
    public class NewEnemyDeathState : NewEnemyState {
        public NewEnemyDeathState(NewEnemyController enemy, EnemyFSMState state) : base(enemy, state) { }

        public override void Enter() {
            base.Enter();
            _enemy.Animator.SetTrigger("Hitted");
            _enemy.Collider.enabled = false;
            _enemy.DetectionSystem.enabled = false;
        }

        public override void Exit() {

        }

        public override void Update() {

        }
    }
}
