using System;
using System.Collections.Generic;
using System.Linq;
using CollectionTest.Model;

namespace CollectionTest.Collection
{
    public class SortedIndexedArray<T>: List<T> where T : IDated
    {
        private int[] indexer;

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

        public void AddToIndex()
        {
            if (indexer == null || indexer.Count() <= 1)
            {
                BuildIndex();
                return;
            }

            var lastIndex = indexer[indexer.Length - 1];
            var index = indexer.Length;
            if (Count - 1 == indexer[indexer.Length - 1]) //indexer is not dirty
                return;

            var newIndexer = new int[base[Count - 1].Date];
            Array.Copy(indexer, newIndexer, indexer.Length);

            while(index < base[Count - 1].Date)
            {
                for (int j = 0; j < base[lastIndex + 1].Date - base[lastIndex].Date; j++)
                {
                    newIndexer[index] = lastIndex + 1;
                    index++;
                }

                lastIndex++;
            }

            indexer = newIndexer;
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

            if(indexer != null)
                AddToIndex();
        }

        public new void AddRange(IEnumerable<T> collection)
        {
            throw new NotSupportedException();
        }
    }
}
