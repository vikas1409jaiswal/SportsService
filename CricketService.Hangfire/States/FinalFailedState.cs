using Hangfire.States;

namespace CricketService.Hangfire.States
{
    public class FinalFailedState : FailedState, IState
    {
        public FinalFailedState(Exception exception)
            : base(exception)
        {
        }

        public new bool IsFinal => true;
    }
}