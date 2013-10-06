using CollectionTest.Helper;
using ProtoBuf;

namespace CollectionTest.Model
{
    public interface IDated
    {
        int Date { get; set; }
    }

    [ProtoContract]
    public class Dog : IDated
    {
        [ProtoMember(1)]
        public int Date { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; }
    }

    
}