using System;
using Services.User;

namespace Services.LivesRefill
{
    public class LivesRefillService : ILivesRefillService, IDisposable
    {
        private readonly ITimerService _timerService;
        private readonly IUserService _userService;

        private DateTime _lastRefillTime;

        public LivesRefillService(
            IUserService userService,
            ITimerService timerService)
        {
            _userService = userService;
            _timerService = timerService;

            StartRefillLives();
        }

        public string GetTimeUntilNextRefill()
        {
            var timeSpan = TimeSpan.FromSeconds(_timerService.TimeLeft);
            return $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
        }

        public void Dispose()
        {
            GlobalGameEvents.LivesAmountChanged -= OnLivesAmountChanged;
        }

        private void StartRefillLives()
        {
            RefillLivesInternal();

            GlobalGameEvents.LivesAmountChanged += OnLivesAmountChanged;
        }

        private void RefillLivesInternal()
        {
            if (IsFullLives())
                return;

            if (_userService.LastLivesRefillTime.Year == 1)
            {
                StartRefillTimer();
                return;
            }

            var secondsHasPassed = (int)(DateTime.Now - _lastRefillTime).TotalSeconds;
            var numberOfMissingLives = Constants.MaxLives - _userService.LivesAmount;

            var amountLivesToRefill = secondsHasPassed / Constants.LivesRefillTime;
            var remainingTime = secondsHasPassed % Constants.LivesRefillTime;

            if (amountLivesToRefill > numberOfMissingLives)
                amountLivesToRefill = numberOfMissingLives;

            _userService.AddLives(amountLivesToRefill);

            if (!IsFullLives())
                StartRefillTimer(remainingTime);
        }

        private void StartRefillTimer(float timeLeft = Constants.LivesRefillTime)
        {
            if (IsFullLives() || _timerService.IsTimerOn)
                return;

            _userService.SetLastLivesRefillTime(DateTime.Now);
            _timerService.Start(timeLeft);
            _timerService.OnComplete += OnTimerServiceComplete;
        }

        private void StopRefillTimer()
        {
            _timerService.OnComplete -= OnTimerServiceComplete;
            _timerService.Stop();
        }

        private void OnTimerServiceComplete()
        {
            _timerService.OnComplete -= OnTimerServiceComplete;
            _userService.AddLives(amount: 1);
        }

        private void OnLivesAmountChanged(int amount)
        {
            if (IsFullLives() && _timerService.IsTimerOn)
                StopRefillTimer();

            if (!IsFullLives() && !_timerService.IsTimerOn)
                StartRefillTimer();
        }

        private bool IsFullLives()
            => _userService.LivesAmount == Constants.MaxLives;
    }
}