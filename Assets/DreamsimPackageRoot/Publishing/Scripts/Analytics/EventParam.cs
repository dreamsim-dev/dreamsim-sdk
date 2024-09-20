using System;

namespace Dreamsim.Publishing
{
    public class EventParam
    {
        public readonly string Key;
        public readonly object Value;
        public readonly Type ValueType;

        private EventParam(string key)
        {
            Key = key;
        }

        public EventParam(string key, long value) : this(key)
        {
            Value = value;
            ValueType = typeof(long);
        }

        public EventParam(string key, double value) : this(key)
        {
            Value = value;
            ValueType = typeof(float);
        }

        public EventParam(string key, string value) : this(key)
        {
            Value = value;
            ValueType = typeof(string);
        }
    }
}