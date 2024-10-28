namespace Dreamsim.Publishing
{
    public abstract class MediationBase
    {
        protected readonly string _key;
        protected readonly string _adUnitId;
        protected string _placement;
        protected string _adSource;
        protected int _retryAttempt;

        protected MediationBase(string key)
        {
            _key = key;
        }
        
        protected MediationBase(string key, string adUnitId)
        {
            _key = key;
            _adUnitId = adUnitId;
        }
    }
}