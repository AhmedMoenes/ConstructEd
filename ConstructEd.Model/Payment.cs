namespace ConstructEd.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int CourseId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string TransactionId { get; set; }

        // Navigation Properties
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Course Course { get; set; }
    }
}
