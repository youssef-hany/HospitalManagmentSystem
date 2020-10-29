using Microsoft.EntityFrameworkCore.ChangeTracking;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;

namespace Server.Models.Repositories
{
    public class GuestDb : IRepository<Guest>
    {
        //private readonly DatabaseContext db = new DatabaseContext();
        DatabaseContext db;
        public GuestDb()
        {
            db = new DatabaseContext();
        }

        public IEnumerable<Guest> GetAll()
        {
            IEnumerable<Guest> Guest = db.Guests.ToList();
            return Guest;   
        }

        public Guest GetById(int id)
        {

            Guest Guest = db.Guests.Find(id);
            return Guest;
            
            
        }
        public Guest GetByRoomNumber(int Number)
        {
            Guest room = db.Guests
                    .Where(b => b.RoomNumber == Number)
                    .FirstOrDefault();

            return room;
        }

        public Guest Add(Guest Guest)
        {
            if(Guest.Checkin == null)
            {
                Guest.Checkin = DateTime.Now;
            }
            EntityEntry guest = db.Guests.Add(Guest);
            if(guest != null)
            {
                return Guest;
            }
            else
            {
                return null;
            }

        }
        public Guest Update(int id, Guest Guest)
        {
            
            Guest dbguest = db.Guests.Find(id);
            if (dbguest != null)
            {
                dbguest.FirstName = Guest.FirstName;
                dbguest.LastName = Guest.LastName;
                dbguest.Status = Guest.Status;
                dbguest.Email = Guest.Email;
                dbguest.Phone = Guest.Phone;
                dbguest.IDNumber = Guest.IDNumber;
                dbguest.IsChild = Guest.IsChild;
                dbguest.MaritalStatus = Guest.MaritalStatus;
                dbguest.Checkin = Guest.Checkin;
                dbguest.Checkout = Guest.Checkout;
                db.Guests.Update(dbguest);
                return dbguest;
            }
            else
            {
                return null;
            }
           
           
         
        }

        public int CalculateCheckout(Guest Guest)
        {
            int Days;
            int pricePerDay = 200;
            int totalPrice;

            //getPricePerDay(RoomNo) fromConstants later
            if (Guest.Checkout == null)
            {
                Guest.Checkout = DateTime.Now;
            }
            if(Guest.Checkin == null)
            {
                Guest.Checkin = DateTime.Now;
            }
            Days = (Guest.Checkout - Guest.Checkin).Days;

            if (Days > 0)
            {
                totalPrice = Days * pricePerDay;
            }
            else
            {
                totalPrice = 0;
            }
            return totalPrice;
        }

        public Guest Remove(int id)
        {
 
            Guest guestToDel = db.Guests.Find(id);
            if (guestToDel != null)
            {
                db.Guests.Remove(guestToDel);
                return guestToDel;

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