namespace Dreamsim.Publishing
{
    public abstract class MediationBase
    {
        protected string _placement;
        protected string _adSource;
        protected int _retryAttempt;

        protected MediationBase(string key) { }
    }
}