﻿using System.ComponentModel.DataAnnotations;

namespace DriverInfo.Models
{
    public class Event
    {
        [Key]
        public int EventID { get; set; }

        public DateTime NoteDate { get; set; }
        public string NoteDescription { get; set; }
        public decimal AmountIn { get; set; }
        private decimal _amountOut;
        public decimal AmountOut
        {
            get => _amountOut;
            set => _amountOut = value > 0 ? -value : value;
        }
        public string ResponsibleEmployee { get; set; }

        //Relationships
        public int DriverID { get; set; }
        public Driver Driver { get; set; }
    }
}