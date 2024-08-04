using System;
using System.Collections.Generic;

namespace Devenant
{
    public class StateMachine
    {
        private class Transition
        {
            public readonly IState to;
            public readonly Func<bool> condition;

            public Transition(IState to, Func<bool> condition)
            {
                this.to = to;
                this.condition = condition;
            }
        }

        public IState currentState { get { return _currentState; } private set { _currentState = value; } }
        private IState _currentState;

        private Dictionary<Type, List<Transition>> transitions = new Dictionary<Type, List<Transition>>();

        private List<Transition> currentTransitions = new List<Transition>();
        private List<Transition> anyTransitions = new List<Transition>();

        public void Tick()
        {
            Transition transition = GetTransition();

            if (transition != null)
            {
                SetState(transition.to);
            }

            currentState?.Tick();
        }

        public void SetState(IState state)
        {
            if (state == currentState)
            {
                return;
            }

            currentState?.Exit();
            currentState = state;

            transitions.TryGetValue(currentState.GetType(), out currentTransitions);

            if (currentTransitions == null)
            {
                currentTransitions = new List<Transition>();
            }

            currentState.Enter();
        }

        public void AddTransition(IState from, IState to, Func<bool> predicate)
        {
            if (this.transitions.TryGetValue(from.GetType(), out List<Transition> transitions) == false)
            {
                transitions = new List<Transition>();

                this.transitions[from.GetType()] = transitions;
            }

            transitions.Add(new Transition(to, predicate));
        }

        public void AddAnyTransition(IState state, Func<bool> predicate)
        {
            anyTransitions.Add(new Transition(state, predicate));
        }

        private Transition GetTransition()
        {
            foreach (Transition transition in anyTransitions)
            {
                if (transition.condition())
                {
                    return transition;
                }
            }

            foreach (Transition transition in currentTransitions)
            {
                if (transition.condition())
                {
                    return transition;
                }
            }

            return null;
        }
    }
}