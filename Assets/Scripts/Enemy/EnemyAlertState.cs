using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HNC {
    public class EnemyAlertState : IState {
        private EnemyController _enemy;

        public EnemyAlertState(EnemyController enemy) {
            _enemy = enemy;
        }

        public void Enter() {
            _enemy.AlertGO.SetActive(true);
        }

        public void Exit() {
            _enemy.AlertGO.SetActive(false);
        }

        public void Update() {
            _enemy.AlertTimer -= Time.deltaTime;
        }
    }
}
