using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ConstructEd.Models
{
    public class PaymentCourse
    {
        [Key, Column(Order = 0)]
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }

        [Key, Column(Order = 1)] 
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}