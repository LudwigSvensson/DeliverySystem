using System.ComponentModel.DataAnnotations;

namespace DriverInfo.Models
{
    public class Driver
    {
        [Key]
        public int DriverID { get; set; }

        [Required(ErrorMessage = "Name of the driver is mandatory")]
        [Display(Name = "Driver Name")]
        public string DriverName { get; set; }

        [Required(ErrorMessage = "Registration number is mandatory")]
        [Display(Name = "Registration Number")]
        public string CarReg { get; set; }

        // Referens till den ansvariga anställda
        public string ResponsibleEmployeeId { get; set; }
        public Employee ResponsibleEmployee { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Total amount spent")]
        public decimal TotalAmountSpent
        {
            get
            {
                return Events?.Sum(e => e.AmountOut) ?? 0;
            }
        }
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Total amount earned")]
        public decimal TotalAmountEarned
        {
            get
            {
                return Events?.Sum(e => e.AmountIn) ?? 0;
            }
        }
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Total amount both income and expenses")]
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
