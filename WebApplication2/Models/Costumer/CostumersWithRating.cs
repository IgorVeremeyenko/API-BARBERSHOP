namespace WebApplication2.Models.Costumer
{
    public class CostumersWithRating
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int RatingValue { get; set; }
        public int Total { get; set; }
        public int Cost { get; set; }
        public int SuccessCount { get; set; }
        public DateTime Date { get; set; }
        public int UserId { get; set; }
        public List<Appointment> apps { get; set; }
    }
}
