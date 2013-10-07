using System;
using System.Collections.Generic;
using System.Linq;
using CollectionTest.Model;
using ProtoBuf;

namespace CollectionTest.Collection
{
    public class SortedIndexedArray<T>: List<T> where T : IDated
    {
        private int[] indexer;

        public SortedIndexedArray()
        {
            indexer = new int[10];
        }

        public void BuildIndex()
        {
            var length = base[Count - 1].Date;
            indexer = new int[length];

            var num = 0;
            var last = base[0].Date;
            
            for (int i = 0; i < length; i++)
            {
                if (i > last)
                {
                    num++;
                    last = base[num].Date;
                }

                indexer[i] = num;
            }
        }

        public T Right(int date)
        {
            if (date >= base[Count - 1].Date)
                return default(T);

            var index = indexer[date];

            if (base[index].Date == date)
                index++;

            return base[index];
        }

        public IEnumerable<T> RangeFromTo(int from, int to)
        {
            var fromIndex = GetIndex(from);

            if (fromIndex < 0)
                return new List<T>();

            var toIndex = GetIndex(to);

            int length;
            if (toIndex < 0)
                length = Count - fromIndex;
            else
            {
                length = toIndex - fromIndex;

                if (base[toIndex].Date == to)
                    length++;
            }

            return this.Skip(fromIndex).Take(length);
        }

        private int GetIndex(int date)
        {
            if (date > indexer.Length - 1)
                return -1;

            return indexer[date];
        }

        public new void Add(T obj)
        {
            base.Add(obj);

            //int index = 0;
            //if(indexer.Count > 0)
            //    index = indexer.Last() + 1;

            //var length = obj.Date + 1 - indexer.Count;

            //var array = new int[length];

            //for (int i = 0; i < length; i++)
            //    array[i] = index;

            //indexer.AddRange(array);
        }

        public new void AddRange(IEnumerable<T> collection)
        {
            throw new NotSupportedException();
        }
    }
}
