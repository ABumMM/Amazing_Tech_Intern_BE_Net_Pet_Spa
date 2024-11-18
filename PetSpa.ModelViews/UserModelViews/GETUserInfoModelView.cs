namespace PetSpa.ModelViews.UserModelViews
{
    public class GETUserInfoModelView
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime DayofBirth { get; set; }
        public string? Address { get; set; }
        public decimal? Salary { get; set; }
        public string? BankAccount { get; set; }
        public string? BankAccountName { get; set; }
        public string? Bank { get; set; }
    }
}
