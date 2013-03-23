using System;
using System.Collections.Generic;
using Common;


namespace SearcherTests
{
    public static class TestHelper
    {
        
        public static bool ContainsSimilarElement(this ICollection<string> collection, string element)
        {
            foreach (var el in collection)
            {
                if (el.ContainsIgnoreCase(element))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
