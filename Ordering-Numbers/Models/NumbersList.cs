using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace Ordering_Numbers.Models
{
    public class NumbersList
    {
        [Key]
        public Guid Id { get; set; }
        public List<Number> Numbers { get; set; } 
        public string OrderType { get; set; }

        public long OrderingTimeMilliseconds { get; set; }

        public NumbersList()
        {
            Numbers = new List<Number>();
            OrderType = "Unsorted";
            Id = Guid.NewGuid();
        }

        public NumbersList(List<Number> nums)
        {
            Numbers = nums;
            OrderType = "Unsorted";
            Id = Guid.NewGuid();
        }

        public void SortAscending()
        {
            var timer = Stopwatch.StartNew();

            Numbers.Sort((a, b) => a.Value.CompareTo(b.Value));

            timer.Stop();


            OrderType = "Ascending";
            OrderingTimeMilliseconds = timer.ElapsedMilliseconds;

        }

        public void SortDescending()
        {
            var timer = Stopwatch.StartNew();

            Numbers.Sort((a, b) => b.Value.CompareTo(a.Value));

            timer.Stop();


            OrderType = "Descending";
            OrderingTimeMilliseconds = timer.ElapsedMilliseconds;
        }
    }
}
