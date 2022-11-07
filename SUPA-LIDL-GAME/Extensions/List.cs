using System;
using System.Collections.Generic;

namespace SupaLidlGame.Extensions
{
    public static class ListExtensions
    {
        private static Random _random = new Random();

        public static T PickRandomElement<T>(this List<T> list)
        {
            if (list.Count < 1)
                return default(T);

            return list[_random.Next(0, list.Count)];
        }
    }
}
