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
        
        }

        public void Exit() {
        
        }

        public void Update() {
            _enemy.AlertTimer -= Time.deltaTime;
        }
    }
}
