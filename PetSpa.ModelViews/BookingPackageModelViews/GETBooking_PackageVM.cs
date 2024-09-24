using PetSpa.ModelViews.PackageModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.BookingPackageModelViews
{
    public class GETBooking_PackageVM
    {
        public string BookingId { get; set; } = string.Empty;      
        public string? Description { get; set; }       
        public DateTime? Date { get; set; }            
        public string? Status { get; set; }            
        public List<GETPackageModelView> Packages { get; set; } = new List<GETPackageModelView>();
    }
}
