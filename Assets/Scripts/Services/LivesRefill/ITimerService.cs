using System;

namespace Services.LivesRefill
{
    public interface ITimerService
    {
        Action OnComplete { get; set; }
        bool IsTimerOn { get; }
        float TimeLeft { get; }
        void Start(float timeLeft);
        void Stop();
    }
}