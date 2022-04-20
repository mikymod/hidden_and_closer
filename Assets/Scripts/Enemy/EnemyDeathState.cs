namespace HNC {
    public class EnemyDeathState : EnemyState {
        public EnemyDeathState(EnemyController enemy, EnemyFSMState state) : base(enemy, state) { }

        public override void Enter() {
            base.Enter();
            _enemy.NavMeshAgent.speed = 0;
            _enemy.NavMeshAgent.isStopped = true;
            _enemy.Animator.SetTrigger("Hitted");
            _enemy.BodyCollider.enabled = false;
            _enemy.DetectionSystem.enabled = false;
        }

        public override void Exit() {

        }

        public override void Update() {

        }
    }
}
