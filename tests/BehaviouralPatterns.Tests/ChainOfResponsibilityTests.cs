using System;
using ChainOfResponsibility;
using ChainOfResponsibility.Exceptions;
using NUnit.Framework;

namespace BehaviouralPatterns.Tests
{
    public class ChainOfResponsibilityTests
    {
        [SetUp]
        public void SetUp()
        {
            _doughRolled = false;
            _doughMade = false;
            _toppingsAdded = false;
            _baked = false;
            _delivered = false;
            _cockroachAdded = false;
            _qualityChecked = false;
            _customerNotified = false;
        }

        [Test]
        public void PizzaCanBeDelivered()
        {
            var pizzaMaker = new PizzaMaker();
            pizzaMaker.AddStep(MakeDough);
            pizzaMaker.AddStep(RollDough);
            pizzaMaker.AddStep(AddToppings);
            pizzaMaker.AddStep(Bake);
            pizzaMaker.AddStep(CheckQuality);
            pizzaMaker.AddStep(Deliver);
            pizzaMaker.Make();

            AssertPizzaIsMade();
        }

        [Test]
        public void IfQualityIsComprised_ThenCustomerIsNotified()
        {
            var pizzaMaker = new PizzaMaker();
            pizzaMaker.AddStep(MakeDough);
            pizzaMaker.AddStep(RollDough);
            pizzaMaker.AddStep(AddToppings);
            pizzaMaker.AddStep(AddCockroach);
            pizzaMaker.AddStep(Bake);
            pizzaMaker.AddStep(CheckQuality);
            pizzaMaker.AddStep(Deliver);
            pizzaMaker.Make();

            Assert.That(_customerNotified, Is.True);
        }

        private void AssertPizzaIsMade()
        {
            Assert.That(_doughMade, Is.True);
            Assert.That(_doughRolled, Is.True);
            Assert.That(_toppingsAdded, Is.True);
            Assert.That(_baked, Is.True);
            Assert.That(_qualityChecked, Is.True);
            Assert.That(_delivered, Is.True);
        }

        private void NotifyPolicy(Action step)
        {
            try
            {
                step();
            }
            catch (QualityException)
            {
                _customerNotified = true;
            }
        }

        private void AddCockroach(Action next)
        {
            _cockroachAdded = true;
            next();
        }

        private void Deliver(Action next)
        {
            _delivered = true;
            next();
        }

        private void CheckQuality(Action next)
        {
            if (_cockroachAdded)
                throw new QualityException("Ew. Cockroach encountered.");

            _qualityChecked = true;
            next();
        }

        private void Bake(Action next)
        {
            _baked = true;
            next();
        }

        private void AddToppings(Action next)
        {
            _toppingsAdded = true;
            next();
        }

        public void MakeDough(Action next)
        {
            _doughMade = true;
            next();
        }

        public void RollDough(Action next)
        {
            _doughRolled = true;
            next();
        }

        private bool _doughRolled;
        private bool _doughMade;
        private bool _toppingsAdded;
        private bool _baked;
        private bool _delivered;
        private bool _cockroachAdded;
        private bool _qualityChecked;
        private bool _customerNotified;
    }
}