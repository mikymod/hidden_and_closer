using System;
using System.Collections.Generic;
using UnityEngine;

namespace HNC {
    public class StateMachine {
        private IState _current;

        private readonly Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type, List<Transition>>();
        private List<Transition> _currentTransitions = new List<Transition>();
        private readonly List<Transition> _anyTransitions = new List<Transition>();

        public void SetInitialState(IState state) {
            _current = state;

            _transitions.TryGetValue(_current.GetType(), out _currentTransitions);
            if (_currentTransitions == null) {
                _currentTransitions = new List<Transition>(0);
            }

            _current.Enter();
        }

        public void SetState(IState state) {
            if (state == _current) {
                return;
            }

            _current.Exit();
            _current = state;
            //Debug.Log($"Current transition {state}");

            _transitions.TryGetValue(_current.GetType(), out _currentTransitions);
            if (_currentTransitions == null) {
                _currentTransitions = new List<Transition>(0);
            }

            _current.Enter();
        }

        public void AddTransition(IState from, IState to, Func<bool> predicate) {
            if (_transitions.TryGetValue(from.GetType(), out List<Transition> transitions) == false) {
                transitions = new List<Transition>();
                _transitions[from.GetType()] = transitions;
            }

            transitions.Add(new Transition(to, predicate));
        }

        public void AddAnyTransition(IState to, Func<bool> predicate) => _anyTransitions.Add(new Transition(to, predicate));

        public void Update() {
            if (_current == null) {
                Debug.LogWarning("No current state on State Machine");
                return;
            }
            Transition transition = GetActiveTransition();
            if (transition != null) {
                SetState(transition.To);
            }

            _current.Update();
        }

        private Transition GetActiveTransition() {
            // AnyTransitions have priority
            foreach (Transition transition in _anyTransitions) {
                if (transition.Condition()) {
                    return transition;
                }
            }

            foreach (Transition transition in _currentTransitions) {
                if (transition.Condition()) {
                    return transition;
                }
            }

            return null;
        }
    }
}
