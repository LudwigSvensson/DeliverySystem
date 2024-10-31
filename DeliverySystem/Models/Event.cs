using System.ComponentModel.DataAnnotations;

namespace DriverInfo.Models
{
    public class Event
    {
        [Key]
        public int EventID { get; set; }
        [Display(Name = "Date of Event")]
        public DateTime NoteDate { get; set; }
        [Display(Name = "Description")]
        public string NoteDescription { get; set; }
        [Display(Name ="Earned")]
        [DisplayFormat(DataFormatString = "{0:N2} kr")]
        public decimal AmountIn { get; set; }

        private decimal _amountOut;
        [Display(Name = "Spent")]
        [DisplayFormat(DataFormatString = "{0:N2} kr")]
        public decimal AmountOut
        {
            get => _amountOut;
            set => _amountOut = value > 0 ? -value : value;
        }

        //Relationships
        public string ResponsibleEmployeeid { get; set; }
        public Employee ResponsibleEmployee { get; set; }
        
        public int DriverID { get; set; }
        public Driver Driver { get; set; }
    }
}
