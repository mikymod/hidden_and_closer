using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HNC
{
    public interface IState
    {
        void OnEnable();
        void Enter();
        void Update();
        void Exit();
        void OnDisable();
    }
}
