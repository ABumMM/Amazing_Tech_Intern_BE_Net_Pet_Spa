using PetSpa.ModelViews.PackageModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PetSpa.ModelViews.BookingModelViews
{
    public class GETBookingVM
    {
        public string Id { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime Date { get; set; }
        public string? Status { get; set; }
        public required  string? ApplicationUserId { get; set; }
        //public string OrdersId { get; set; } = string.Empty;
        public string? CreatedBy { get; set; }
        //không cho hiển thị nếu null
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? LastUpdatedBy { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DateTimeOffset? LastUpdatedTime { get; set; }
        
    }
}
