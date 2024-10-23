namespace Dreamsim.Publishing
{
    public abstract class MediationBase
    {
        protected readonly string _appKey;
        protected string _placement;
        protected string _adSource;
        protected int _retryAttempt;

        protected MediationBase(string appKey)
        {
            _appKey = appKey;
        }
    }
}