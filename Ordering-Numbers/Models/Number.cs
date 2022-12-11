using System.ComponentModel.DataAnnotations;

namespace Ordering_Numbers.Models
{
    public class Number
    {
        [Key]
        public Guid Id { get; set; }
        public int Value { get; set; }
        public Number()
        {
            Value = new int();
            Id = Guid.NewGuid();
        }

        public Number( int value)
        {
            Value = value;
            Id = Guid.NewGuid();
        }
    }
}
