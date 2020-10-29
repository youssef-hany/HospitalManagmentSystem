using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models
{
    public class Guest
    {

        //[Key] redundant because name of id matches class 
        //auto-assigned as primary
        public int GuestId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Status { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int IDNumber { get; set; }
        public byte IsChild { get; set; }
        [ForeignKey("Room")]
        public int RoomID { get; set; }
        public int RoomNumber { get; set; }
        public Room Room { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime CreatedOn { get; set; }
        public string MaritalStatus { get; set; }
        public DateTime Checkin { get; set; }
        public DateTime Checkout { get; set; }
    }
}
