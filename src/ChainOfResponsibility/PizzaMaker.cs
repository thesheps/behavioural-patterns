using System;
using System.Collections.Generic;

namespace ChainOfResponsibility
{
    public class PizzaMaker
    {
        public void AddStep(Action<Action> step)
        {
            _steps.Add(step);
        }

        public void Make(int currentStep = 0)
        {
            if (currentStep == _steps.Count)
                return;

            var step = _steps[currentStep];
            step(() => Make(++currentStep));
        }

        private readonly List<Action<Action>> _steps = new List<Action<Action>>();
    }
}