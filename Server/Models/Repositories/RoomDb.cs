using Microsoft.EntityFrameworkCore.ChangeTracking;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace Server.Models.Repositories
{
    public class RoomDb : IRepository<Room>
    {
        private readonly DatabaseContext db = new DatabaseContext();


        public IEnumerable<Room> GetAll()
        {
            IEnumerable<Room> Room = db.Rooms.ToList();
            return Room;   
        }

        public Room GetById(int id)
        {
            Room room = db.Rooms.Find(id);
            return room;     
        }
        public Room GetByRoomNumber(int Number)
        {
            Room room = db.Rooms
                    .Where(b => b.Number == Number)
                    .FirstOrDefault();
           
            return room;
        }
        
        public Room Add(Room Room)
        {
            EntityEntry room = db.Rooms.Add(Room);
            if(room != null)
            {
                return Room;

            }
            else
            {
                return null;
            }


        }
        public Room Update(int Number, Room Room)
        {
            Room dbRoom = GetByRoomNumber(Number);
            Room room = db.Rooms.Find(dbRoom.RoomID);
            if (dbRoom != null)
            {
                room.Number = Room.Number;
                room.Type = Room.Type;
                room.BedType = Room.BedType;
                room.IsOccupied = Room.IsOccupied;
                room.Status = Room.Status;
                room.Price = Room.Price;
                room.Number = Room.Number;
                room.LastChange = Room.LastChange;
                room.LastChanger = Room.LastChanger;      
                db.Rooms.Update(room);
                return room;

            }
            else
            {
                return null;
            }
        }
        public Room UpdateById(int id, Room Room)
        {
            Room room = db.Rooms.Find(id);
            if (room != null)
            {
                room.Number = Room.Number;
                room.Type = Room.Type;
                room.IsOccupied = Room.IsOccupied;
                room.Status = Room.Status;
                room.Price = Room.Price;
                room.Number = Room.Number;
                room.LastChange = Room.LastChange;
                room.LastChanger = Room.LastChanger;
                db.Rooms.Update(room);
                return room;
            }
            else
            {
                return null;
            }
        }

        //public bool CheckRoomIfEmpty(Room Room)
        //{

        //}

        public Room Remove(int id)
        {
            Room roomToDel = db.Rooms.Find(id);
            if (roomToDel != null)
            {
                db.Rooms.Remove(roomToDel);
                return roomToDel;
            }
            else
            {
                return null;
            }
        }

        public bool Save()
        {
            int s = db.SaveChanges();
            if (s > 0)
            {
                return true;
            }
            return false;
        }

    }
}