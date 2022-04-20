using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HNC
{
    public class ChangeStateEvent
    {
        public static UnityAction<GameObject, EnemyFSMState> OnChangeState;
    }
}
