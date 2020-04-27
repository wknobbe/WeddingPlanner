using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace WeddingPlanner.Models
{
    public class Wedding
    {
        [Key]
        public int WeddingId {get;set;}
        [Required(ErrorMessage="Please enter a wedder.")]
        [Display(Name="Wedder One:")]
        public string WedderOne {get;set;}
        [Required(ErrorMessage="Please enter a wedder.")]
        [Display(Name="Wedder Two:")]
        public string WedderTwo {get;set;}
        [Required]
        [Display(Name="Date:")]
        [DataType(DataType.Date)]
        [Future]
        public DateTime Date {get;set;}
        [Required(ErrorMessage="Please enter an address.")]
        [Display(Name="Wedding Address:")]
        public string Address {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        public int UserId {get;set;}
        public User Planner {get;set;}
        public List<Invite> Guests {get;set;}
        
    }
    public class FutureAttribute : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                if (value == null)
                {
                    return new ValidationResult("Please enter a date for the wedding.");
                }
                else
                {
                    DateTime compare;
                    if (value is DateTime)
                    {
                        compare = (DateTime)value;
                        if (compare < DateTime.Now)
                        {
                            return new ValidationResult("Please enter a future date.");
                        }
                        else
                        {
                            return ValidationResult.Success;
                        }
                    }
                    else
                    {
                        return new ValidationResult("Please enter a valid date.");
                    }
                }
            }
        }
}