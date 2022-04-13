using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace HNC
{
    public class NewChangeStateEvent
    {
        public static UnityAction<GameObject, EnemyFSMState> OnChangeState;
    }
}
