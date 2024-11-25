using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Dreamsim.Publishing
{
    public class ConsentFlowBase
    {
        public string AdvertisingId { get; protected set;}
        public bool TrackingEnabled { get; protected set;}

        public virtual UniTask ProcessAsync() { throw new NotImplementedException(); }
        public virtual UniTask ProcessAsync(List<string> testDeviceHashedIds) { throw new NotImplementedException(); }
    }
}