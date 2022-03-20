using UnityEngine;

namespace HNC {

    public class PlayerIdle : IState {
        public void OnEnable() => Debug.Log("Player Idle Enable");
        public void Enter() => Debug.Log("Player Idle Enter");
        void IState.Update() => Debug.Log("Player Idle Update");
        public void Exit() => Debug.Log("Player Idle Exit");
        public void OnDisable() => Debug.Log("Player Idle Disable");
    }
}
