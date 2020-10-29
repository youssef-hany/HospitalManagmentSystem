using System;
using System.Collections.Generic;
using Server.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq;

namespace Server.Controllers
{
    //api/guests
    [Route("api/[controller]")]
    [ApiController]
    public class GuestsController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            GuestDb GuestDb = new GuestDb();
            IEnumerable<Guest> Guests = GuestDb.GetAll();
           
            return Ok(Guests);
        }

        //Get api/guest/5
        [HttpGet("{id}")]
        public ActionResult<Guest> GetById(int id)
        {
            JObject Response = new JObject();
            GuestDb GuestDb = new GuestDb();
            Guest Guest = GuestDb.GetById(id);
            if(Guest != null)
            {
                return Ok(Guest);

            }
            Response.Add("error", $"[S] Could not find room with id #{id}");
            return NotFound(Response);

        }

        [HttpPut("modify/{id}")]
        public JObject UpdateRegistry(int id, [FromBody] Guest Guest)
        {
            JObject Response = new JObject();

            try
            {
                GuestDb GuestDb = new GuestDb();
                RoomDb RoomDb = new RoomDb();
                Guest guest = GuestDb.Update(id, Guest);
                if (guest != null)
                {
                    Room room = RoomDb.GetByRoomNumber(Guest.RoomNumber);
                    room.Status = Guest.Status;
                    if (Guest.Status == "CheckedIn")
                        room.IsOccupied = 1;
                    Room dbRoom = RoomDb.Update(Guest.RoomNumber, room);
                    if(RoomDb.Save() && GuestDb.Save())
                    {

                        Response.Add("success", $"[S] Successfully updated registry for {guest.FirstName} with id {guest.GuestId}");
                        return Response;
                    }
                    else
                    {
                        Response.Add("error", $"[S:E59] Failed to updated registry for {guest.FirstName} with id {guest.GuestId}");
                        return Response;
                    }

                }
                else
                {
                    Response.Add("error", $"[S:E59] Failed to updated registry for {guest.FirstName} with id {guest.GuestId}");
                    return Response;
                }
            }
            catch (Exception ex)
            {

                Response.Add("error", $"[S:E59] Failed to updated registry for {Guest.FirstName}. check server!");
                Console.WriteLine(ex);
                return Response;
            }
           
        }
        //api/guest/reservation
        //api/guest/checkin
        [HttpPost("{type}/{1}")]

        public ActionResult<JObject> NewEntry(Guest Guest, string Type, int Confirmed)
        {
            JObject Response = new JObject();
            GuestDb GuestDb = new GuestDb();
            RoomDb RoomDb = new RoomDb();
            try
            {
                if(Confirmed == null)
                {
                    Room room = RoomDb.GetByRoomNumber(Guest.RoomNumber);
                    if (room != null)
                    {
                        Guest.RoomID = room.RoomID;
                        if (Type == "reservation")
                        {
                            Guest.Status = "Reserved";
                            room.IsOccupied = 0;
                            room.Status = "Reserved";
                        }
                        else
                        {

                            if (Type == "checkin")
                            {
                                Guest.Status = "Checked_In";
                                room.IsOccupied = 1;
                                room.Status = "Checked_In";
                            }
                        }
                        List<Guest> roomGuests = room.Guests.ToList();
                        foreach (var g in roomGuests)
                        {
                            if (g.IDNumber == Guest.IDNumber)
                            {
                                Response.Add("error", $"[S] Cannot add guest with same ID number {g.IDNumber}");
                                return Ok(Response);
                            }
                        }
                        roomGuests.Add(Guest);
                        room.Guests = roomGuests;
                        Room dbRoom = RoomDb.Update(Guest.RoomNumber, room);
                        RoomDb.Save();
                        Guest guest = GuestDb.Add(Guest);
                        if (guest != null && GuestDb.Save())
                        {

                            Response.Add("success", "[S] Successfully Added new reservation");
                            return Ok(Response);
                        }
                        else
                        {
                            Response.Add("error", "[S] Could not add to database");
                            return Ok(Response);

                        }
                    }
                    else
                    {                   
                        Response.Add("error", $"[S] Room with number {Guest.RoomNumber} not found");
                        return Ok(Response);
                    }
                }
                else
                {
                    Guest guest = GuestDb.Add(Guest);
                    if (guest != null && GuestDb.Save())
                    {

                        Response.Add("success", "[S] Successfully Added new reservation");
                        return Ok(Response);
                    }
                    else
                    {
                        Response.Add("error", "[S] Could not add to database");
                        return Ok(Response);

                    }
                }
                
            }
            catch (Exception ex)
            {
                Response.Add("error", "[S] Could not add to database");
                Console.WriteLine(ex);
                return Ok(Response);
            }
            

        }

        //[HttpPost]
        //public IEnumerable<Guest> Checkin()
        //{
        //    GuestDb GuestDb = new GuestDb();
        //    IEnumerable<Guest> Guests = GuestDb.GetAll();

        //    return Guests;
        //}

        //[HttpPost]
        //public IEnumerable<Guest> Checkout()
        //{
        //    GuestDb GuestDb = new GuestDb();
        //    IEnumerable<Guest> Guests = GuestDb.GetAll();

        //    return Guests;
        //}
    }
}
