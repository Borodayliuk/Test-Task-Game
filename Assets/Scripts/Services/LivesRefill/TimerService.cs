using System;
using UnityEngine;
using Zenject;

namespace Services.LivesRefill
{
    public class TimerService : ITimerService, ITickable
    {
        public Action OnComplete { get; set; }
        public bool IsTimerOn => !_isCompleted && _isStarted;

        public float TimeLeft { get; private set; }
        private bool _isCompleted;
        private bool _isStarted;

        public void Start(float timeLeft)
        {
            TimeLeft = timeLeft;
            _isStarted = true;
            _isCompleted = false;
        }

        public void Stop()
        {
            _isCompleted = true;
            TimeLeft = 0f;
        }

        public void Tick()
        {
            if (_isCompleted || !_isStarted)
                return;

            var time = Time.unscaledDeltaTime;

            if (TimeLeft <= time)
            {
                Complete();
                return;
            }

            TimeLeft -= time;
        }

        private void Complete()
        {
            _isCompleted = true;
            TimeLeft = 0f;
            OnComplete?.Invoke();
        }
    }
}