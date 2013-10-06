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
        private ProtobufStore store;

        [TestFixtureSetUp]
        public void SetUp()
        {
            Assert.Fail(@"The test will write to 'C:\SO_Question_Protobuf\'. Please remove after testing.");

            p1 = new CrazyPerson1 { Id = "cp1", Dogs = new DatedC5SortedArray<Dog>() };
            p2 = new CrazyPerson2 { Id = "cp2", Dogs = new DatedSortedSet<Dog>() };

            for (int i = 0; i < 500000; i++)
            {
                p1.Dogs.Add(new Dog { Date = i, Name = i.ToString() });
                p2.Dogs.Add(new Dog { Date = i, Name = i.ToString() });
            }

            store = new ProtobufStore();
            store.Store(p1);
            store.Store(p2);
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

            //Compare right()
            sw = Stopwatch.StartNew();
            for (int i = 0; i < 5000; i++)
            {
                loaded1.Dogs.Right(i);
            }
            sw.Stop();

            Console.WriteLine("C5 sorted array .Right() took " + sw.ElapsedMilliseconds);

            sw = Stopwatch.StartNew();
            for (int i = 0; i < 5000; i++)
            {
                loaded2.Dogs.Right(i);
            }
            sw.Stop();

            Console.WriteLine("Sorted set .Right() took " + sw.ElapsedMilliseconds);

            //Compare RangeFromTo()
            sw = Stopwatch.StartNew();
            for (int i = 0; i < 50000; i++)
            {
                loaded1.Dogs.RangeFromTo(i, i + 10);
            }
            sw.Stop();

            Console.WriteLine("C5 sorted array .RangeFromTo() took " + sw.ElapsedMilliseconds);

            sw = Stopwatch.StartNew();
            for (int i = 0; i < 50000; i++)
            {
                loaded1.Dogs.RangeFromTo(i, i + 10);
            }
            sw.Stop();

            Console.WriteLine("Sorted set .RangeFromTo() took " + sw.ElapsedMilliseconds);

            
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
}
