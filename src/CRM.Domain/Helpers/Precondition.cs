using System;

namespace CRM.Domain.Helpers
{
    public class Precondition
    {
        public static void Requires(bool condition)
        {
            if (!condition)
            {
                throw new ArgumentOutOfRangeException(nameof(condition),"Precondition is not matched");
            }

        }
    }
}