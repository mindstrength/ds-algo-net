using System;

namespace DsAlgo
{
    internal static class Conditions
    {
        internal static T RequireNonNull<T>(T value, string message)
        {
            if (value is not null) 
            {
                return value;
            }
            throw new ArgumentNullException(message);
        }
    }
}