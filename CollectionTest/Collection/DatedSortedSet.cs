using System.Collections.Generic;
using System.Linq;
using CollectionTest.Model;

namespace CollectionTest.Collection
{
    public class DatedSortedSet<T> : SortedSet<T> where T : IDated
    {
        public DatedSortedSet()
            : base(new DatedEntityComparer() as IComparer<T>)
        {
        }

        public T Right(int date)
        {
            return this.SkipWhile(x => x.Date <= date).FirstOrDefault();
        }

        public IEnumerable<T> RangeFromTo(int from, int to)
        {
            return this.SkipWhile(x => x.Date < from).TakeWhile(x => x.Date <= to);
        }
    }
}