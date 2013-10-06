using System;
using System.Collections.Generic;
using C5;
using CollectionTest.Model;

namespace CollectionTest.Collection
{
    public class DatedC5SortedArray<T> : SortedArray<T> where T : IDated
    {
        public DatedC5SortedArray()
            : base(new DatedEntityComparer() as IComparer<T>)
        {
        }

            public T Right(int date)
            {
                if (Count == 0)
                    return default(T);

                var last = this[Count - 1].Date;

                if (date >= last)
                    return default(T);

                var obj = CreateObj(date);
                return Successor(obj);
            }

            public IDirectedCollectionValue<T> RangeFromTo(int from, int to)
            {
                var top = Right(to);

                if (top == null)
                    return RangeFrom(CreateObj(from));

                return RangeFromTo(CreateObj(from), top);
            }

        private static T CreateObj(int date)
        {
            var input = Activator.CreateInstance<T>();
            input.Date = date;
            return input;
        }
    }
}
