using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;


namespace WeddingPlanner.Models
{
    public class User
    {
        [Key]
        public int UserId {get;set;}
        [Required(ErrorMessage="Please enter your first name.")]
        [Display(Name="First Name:")]
        public string FirstName {get;set;}
        [Required(ErrorMessage="Please enter your last name.")]
        [Display(Name="Last Name:")]
        public string LastName {get;set;}
        [Required(ErrorMessage="Please enter your email address.")]
        [EmailAddress(ErrorMessage="Please enter a valid email address.")]
        [Display(Name="Email Address:")]
        public string Email {get;set;}
        [Required(ErrorMessage="You must create a password to register.")]
        [DataType(DataType.Password)]
        [MinLength(8, ErrorMessage="Your password must be at least 8 characters.")]
        [Display(Name="Password:")]
        public string Password {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        [NotMapped]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name="Confirm Password:")]
        public string ConfirmPassword {get;set;}
        public List<Wedding> WeddingsPlanned {get;set;}
        public List<Invite> WeddingsAttending {get;set;}
    }
}