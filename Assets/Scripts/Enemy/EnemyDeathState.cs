namespace HNC {
    public class EnemyDeathState : EnemyState {
        public EnemyDeathState(EnemyController enemy, EnemyFSMState state) : base(enemy, state) { }

        public override void Enter() {
            base.Enter();
            _enemy.AnimatorComponent.SetTrigger(_enemy.AnimDeathHash);
            _enemy.Collider.enabled = false;
            _enemy.Detector.SetActive(false);
            _enemy.DetectedAudioGO.SetActive(false);
            _enemy.DetectedVideoGO.SetActive(false);
        }

        public override void Exit() {

        }

        public override void Update() {

        }
    }
}
