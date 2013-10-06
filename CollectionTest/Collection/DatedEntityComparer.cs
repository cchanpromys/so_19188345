using System.Collections.Generic;
using CollectionTest.Model;

namespace CollectionTest.Collection
{
    public class DatedEntityComparer : IComparer<IDated>
    {
        public int Compare(IDated x, IDated y)
        {
            if (x.Date == y.Date)
                return 0;
            if (x.Date < y.Date)
                return -1;
            return 1;
        }
    }
}
