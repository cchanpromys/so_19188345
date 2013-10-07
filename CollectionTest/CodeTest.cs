using System.Linq;
using CollectionTest.Collection;
using CollectionTest.Model;
using FluentAssertions;
using NUnit.Framework;

namespace CollectionTest
{
    [TestFixture]
    public class CodeTest
    {
        [Test]
        public void C5TestRight()
        {
            //Arrange
            var array = new DatedC5SortedArray<Dog>
                {
                    new Dog {Date = 3},
                    new Dog {Date = 7},
                    new Dog {Date = 9},
                    new Dog {Date = 12}
                };

            //Assert
            Assert.That(array.Right(3).Date, Is.EqualTo(7));
            Assert.That(array.Right(4).Date, Is.EqualTo(7));
            Assert.That(array.Right(12), Is.Null);
        }

        [Test]
        public void SortedSetTestRight()
        {
            //Arrange
            var array = new DatedSortedSet<Dog>
                {
                    new Dog {Date = 3},
                    new Dog {Date = 7},
                    new Dog {Date = 9},
                    new Dog {Date = 12}
                };

            //Assert
            Assert.That(array.Right(3).Date, Is.EqualTo(7));
            Assert.That(array.Right(4).Date, Is.EqualTo(7));
            Assert.That(array.Right(12), Is.Null);
        }

        [Test]
        public void SortedIndexArrayRight()
        {
            //Arrange
            var array = new SortedIndexedArray<Dog>
                {
                    new Dog {Date = 3},
                    new Dog {Date = 7},
                    new Dog {Date = 9},
                    new Dog {Date = 12}
                };

            array.BuildIndex();

            //Assert
            Assert.That(array.Right(3).Date, Is.EqualTo(7));
            Assert.That(array.Right(4).Date, Is.EqualTo(7));
            Assert.That(array.Right(12), Is.Null);
        }

        [Test]
        public void C5TestRangeFromTo()
        {
            //Arrange
            var array = new DatedC5SortedArray<Dog>
                {
                    new Dog {Date = 3},
                    new Dog {Date = 7},
                    new Dog {Date = 9},
                    new Dog {Date = 12}
                };

            //Assert
            array.RangeFromTo(2, 9).Select(x => x.Date).Should().Equal(new[] { 3, 7, 9 });
            array.RangeFromTo(2, 8).Select(x => x.Date).Should().Equal(new[] { 3, 7 });
            array.RangeFromTo(7, 13).Select(x => x.Date).Should().Equal(new[] { 7, 9, 12 });
            array.RangeFromTo(13, 14).Count.Should().Be(0);
        }

        [Test]
        public void SortedSetTestRangeFromTo()
        {
            //Arrange
            var array = new DatedSortedSet<Dog>
                {
                    new Dog {Date = 3},
                    new Dog {Date = 7},
                    new Dog {Date = 9},
                    new Dog {Date = 12}
                };

            //Assert
            array.RangeFromTo(2, 9).Select(x => x.Date).Should().Equal(new[] { 3, 7, 9 });
            array.RangeFromTo(2, 8).Select(x => x.Date).Should().Equal(new[] { 3, 7 });
            array.RangeFromTo(7, 13).Select(x => x.Date).Should().Equal(new[] { 7, 9, 12 });
            array.RangeFromTo(13, 14).Count().Should().Be(0);
        }

        [Test]
        public void SortedIndexedArrayRangeFromTo()
        {
            //Arrange
            var array = new SortedIndexedArray<Dog>
                {
                    new Dog {Date = 3},
                    new Dog {Date = 7},
                    new Dog {Date = 9},
                    new Dog {Date = 12}
                };

            array.BuildIndex();

            //Assert
            array.RangeFromTo(2, 9).Select(x => x.Date).Should().Equal(new[] { 3, 7, 9 });
            array.RangeFromTo(2, 8).Select(x => x.Date).Should().Equal(new[] { 3, 7 });
            array.RangeFromTo(7, 13).Select(x => x.Date).Should().Equal(new[] { 7, 9, 12 });
            array.RangeFromTo(13, 14).Count().Should().Be(0);
        }
    }
}
