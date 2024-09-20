using PetSpa.Contract.Repositories.Entity;

namespace PetSpa.ModelViews.ModelViews
{
    public class BookingResponseModel
    {
        public string? Id { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public string? CustomerId { get; set; }
        public string? EmployeesId { get; set; }

        public string? OrdersId { get; set; }

        public ICollection<string>? PackageIds { get; set; }
        //public ICollection<Packages>? Package { get; set; }
    }
}
