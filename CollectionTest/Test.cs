using System;
using System.Diagnostics;
using CollectionTest.Collection;
using CollectionTest.Helper;
using CollectionTest.Model;
using NUnit.Framework;
using ProtoBuf;

namespace CollectionTest
{
    [TestFixture]
    public class Test
    {
        private CrazyPerson1 p1;
        private CrazyPerson2 p2;
        private CrazyPerson3 p3;
        private ProtobufStore store;

        [TestFixtureSetUp]
        public void SetUp()
        {
            Assert.Fail(@"The test will write to 'C:\SO_Question_Protobuf\'. Please remove after testing.");

            //p1 = new CrazyPerson1 {Id = "cp1", Dogs = new DatedC5SortedArray<Dog>()};
            //p2 = new CrazyPerson2 {Id = "cp2", Dogs = new DatedSortedSet<Dog>()};
            //p3 = new CrazyPerson3 {Id = "cp3", Dogs = new SortedIndexedArray<Dog>()};

            //for (int i = 0; i < 500000; i++)
            //{
            //    p1.Dogs.Add(new Dog {Date = i + 3000, Name = i.ToString()});
            //    p2.Dogs.Add(new Dog {Date = i + 3000, Name = i.ToString()});
            //    p3.Dogs.Add(new Dog {Date = i + 3000, Name = i.ToString()});
            //}

            //store = new ProtobufStore();
            //store.Store(p1);
            //store.Store(p2);
            //store.Store(p3);
        }

        [Test]
        public void Run()
        {
            //Compare loading
            var sw = Stopwatch.StartNew();
            var loaded1 = store.Get<CrazyPerson1>(p1.Id);
            sw.Stop();

            Console.WriteLine("C5 sorted array deserialize took " + sw.ElapsedMilliseconds);

            sw = Stopwatch.StartNew();
            var loaded2 = store.Get<CrazyPerson2>(p2.Id);
            sw.Stop();

            Console.WriteLine("Sorted set deserialize took " + sw.ElapsedMilliseconds);

            sw = Stopwatch.StartNew();
            var loaded3 = store.Get<CrazyPerson3>(p3.Id);
            sw.Stop();

            Console.WriteLine("SortedIndexedArray deserialize took " + sw.ElapsedMilliseconds);
            Console.WriteLine();

            //Compare right()
            sw = Stopwatch.StartNew();
            for (int i = 0; i < 50000; i++)
            {
                loaded1.Dogs.Right(i);
            }
            sw.Stop();

            Console.WriteLine("C5 sorted array .Right() took " + sw.ElapsedMilliseconds);

            sw = Stopwatch.StartNew();
            for (int i = 0; i < 500; i++)
            {
                loaded2.Dogs.Right(i);
            }
            sw.Stop();

            Console.WriteLine("Sorted set .Right() took " + sw.ElapsedMilliseconds*100 + " <-- this is very slow");

            sw = Stopwatch.StartNew();
            for (int i = 0; i < 50000; i++)
            {
                loaded3.Dogs.Right(i);
            }
            sw.Stop();

            Console.WriteLine("SortedIndexedArray .Right() took " + sw.ElapsedMilliseconds);
            Console.WriteLine();

            //Compare RangeFromTo()
            sw = Stopwatch.StartNew();
            for (int i = 0; i < 50000; i++)
            {
                loaded1.Dogs.RangeFromTo(i, i + 10000);
            }
            sw.Stop();

            Console.WriteLine("C5 sorted array .RangeFromTo() took " + sw.ElapsedMilliseconds);

            sw = Stopwatch.StartNew();
            for (int i = 0; i < 50000; i++)
            {
                loaded1.Dogs.RangeFromTo(i, i + 10000);
            }
            sw.Stop();

            Console.WriteLine("Sorted set .RangeFromTo() took " + sw.ElapsedMilliseconds);

            sw = Stopwatch.StartNew();
            for (int i = 0; i < 50000; i++)
            {
                loaded3.Dogs.RangeFromTo(i, i + 10000);
            }
            sw.Stop();

            Console.WriteLine("SortedIndexedArray .RangeFromTo() took " + sw.ElapsedMilliseconds);
        }
    }

    [ProtoContract]
    public class CrazyPerson1 : IEntity
    {
        [ProtoMember(1)]
        public string Id { get; set; }

        [ProtoMember(2)]
        public DatedC5SortedArray<Dog> Dogs { get; set; }
    }

    [ProtoContract]
    public class CrazyPerson2 : IEntity
    {
        [ProtoMember(1)]
        public string Id { get; set; }

        [ProtoMember(2)]
        public DatedSortedSet<Dog> Dogs { get; set; }
    }

    [ProtoContract]
    public class CrazyPerson3 : IEntity
    {
        [ProtoMember(1)]
        public string Id { get; set; }

        [ProtoMember(2)]
        public SortedIndexedArray<Dog> Dogs { get; set; }

        [ProtoAfterDeserialization]
        public void Init()
        {
            Dogs.BuildIndex();
        }
    }
}
