using System;
using System.Collections.Generic;
using System.Text;

namespace Features.CustomLinq
{
    public static class MyLinqExt
    {
        public static int CustomCount<T>(this IEnumerable<T> sequence)
        {
            int count = 0;
            foreach (var item in sequence)
            {
                count += 1;
            }
            return count;
        }

    }
}
