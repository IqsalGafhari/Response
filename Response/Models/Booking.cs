using BookingManagementApp.Utilities.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookingManagementApp.Models
{
    [Table("tb_tr_bookings")]
    public class Booking : BaseEntity
    {
        [Required, Column("start_date")]
        public DateTime StartDate { get; set; }
        [Required, Column("status")]
        public StatusLevel Status { get; set; }
        [Required, Column("remarks", TypeName = "nvarchar")]
        public string Remarks { get; set; } 
        [Required, Column("employee_guid")]
        public Guid EmployeeGuid { get; set; }
        [Required, Column("room_guid")]
        public Guid RoomGuid { get; set; }
        public Employee? Employee { get; set; }
        public Room? Room { get; set; }
    }
}
