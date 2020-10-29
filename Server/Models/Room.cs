using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Server.Models
{
    public class Room
    {
        //[Key] redundant because name of id matches class 
        //auto-assigned as primary
        public int RoomID { get; set; }
        public int Number { get; set; }
        public byte BedType { get; set; }
        public string Type { get; set; }
        public byte IsOccupied { get; set; }
        public string Status { get; set; }
        //[ConcurrencyCheck] //check if the input is the same in db
        public uint Price { get; set; }
        public IEnumerable<Guest> Guests{ get; set; }

        //[DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public DateTime LastChange { get; set; }
        public string LastChanger { get; set; }
    }
}
