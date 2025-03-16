using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace ConstructEd.Models
{
    [PrimaryKey(nameof(PaymentId), nameof(CourseId))] 
    public class PaymentCourse
    {
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }

        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}