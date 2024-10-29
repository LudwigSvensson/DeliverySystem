using System.ComponentModel.DataAnnotations;

namespace DriverInfo.Models
{
    public class Driver
    {
        [Key]
        public int DriverID { get; set; }

        [Required(ErrorMessage = "Name of the driver is mandatory")]
        public string DriverName { get; set; }

        [Required(ErrorMessage = "Registration number is mandatory")]
        public string CarReg { get; set; }

        // Referens till den ansvariga anställda
        public string ResponsibleEmployeeId { get; set; }
        public Employee ResponsibleEmployee { get; set; }

        public decimal TotalAmountSpent
        {
            get
            {
                return Events?.Sum(e => e.AmountOut) ?? 0;
            }
        }

        public decimal TotalAmountEarned
        {
            get
            {
                return Events?.Sum(e => e.AmountIn) ?? 0;
            }
        }

        public decimal TotalAmountAllEvents
        {
            get
            {
                return Events?.Sum(e => e.AmountOut + e.AmountIn) ?? 0;
            }
        }

        //Relationships
        public List<Event> Events { get; set; }
    }
}
