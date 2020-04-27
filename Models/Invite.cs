using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;


namespace WeddingPlanner.Models
{
    public class Invite
    {
        [Key]
        public int InviteId {get;set;}
        public int UserId {get;set;}
        public int WeddingId {get;set;}
        public User User {get;set;}
        public Wedding Wedding {get;set;}
    }
}