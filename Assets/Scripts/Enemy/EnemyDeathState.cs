using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HNC
{
    public class EnemyDeathState : IState
    {
        private EnemyController _enemy;

        public EnemyDeathState(EnemyController enemy)
        {
            _enemy = enemy;
        }

        public void Enter()
        {
            // _enemy.debugTest.text = "Death State";
        }

        public void Exit()
        {

        }

        public void Update()
        {

        }
    }
}
