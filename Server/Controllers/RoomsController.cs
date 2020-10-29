using System;
using System.Collections.Generic;
using Server.Models.Repositories;
using Microsoft.AspNetCore.Mvc;
using Server.Models;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Server.Controllers
{
    //api/guests
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        [HttpGet]
        public ActionResult Get()
        {
            RoomDb RoomDb = new RoomDb();
            IEnumerable<Room> Rooms = RoomDb.GetAll();
           
            return Ok(Rooms);
        }

        //Get api/rooms/5
        [HttpGet("{id}")]
        public ActionResult<Room> GetById(int id)
        {
            JObject Response = new JObject();
            RoomDb RoomDb = new RoomDb();
            Room Room = RoomDb.GetById(id);
            if(Room != null)
            {
                return Ok(Room);

            }
            Response.Add("error", $"[S] Could not find room with id #{id}");
            return NotFound(Response);

        }
        [HttpGet("number/{number}")]
        public ActionResult<Room> GetByNumber(int number)
        {
            JObject Response = new JObject();
            RoomDb RoomDb = new RoomDb();
            Room Room = RoomDb.GetByRoomNumber(number);
            if (Room != null)
            {
                return Ok(Room);

            }
            Response.Add("error", $"[S] Could not find room #{number}");
            return NotFound(Response);

        }

        [HttpPut("modify/number/{number}")]
        public JObject UpdateRoomByNumber(int number, [FromBody] Room Room)
        {
            JObject Response = new JObject();

            try
            {
        
                RoomDb RoomDb = new RoomDb();
                Room room = RoomDb.Update(number, Room);
                if (room != null && RoomDb.Save())
                {
                    Response.Add("success", $"[S] Successfully updated registry for room with id {room.RoomID}. Number #{room.Number} to #{number}");
                    return Response;
                }
                else
                {
                    Response.Add("error", $"[S:E59] Room #{Room.Number} not found!");
                    return Response;
                }
              
                
            }
            catch (Exception ex)
            {

                Response.Add("error", $"[S:E59] Failed to updated room #{Room.Number}. check server!");
                Console.WriteLine(ex);
                return Response;
            }
           
        }

        [HttpPut("modify/id/{id}")]
        public JObject UpdateRoomById(int id, [FromBody] Room Room)
        {
            JObject Response = new JObject();

            try
            {

                RoomDb RoomDb = new RoomDb();
                Room OldRoom = RoomDb.GetById(id);
                Room room = RoomDb.UpdateById(id, Room);
                if (room != null && RoomDb.Save())
                {
                    Response.Add("success", $"[S] Successfully updated registry for room with id {room.RoomID}. Number #{OldRoom.Number} to #{room.Number}");
                    return Response;
                }
                else
                {
                    Response.Add("error", $"[S:E59] Room #{Room.Number} not found!");
                    return Response;
                }


            }
            catch (Exception ex)
            {

                Response.Add("error", $"[S:E59] Failed to updated room #{Room.Number}. check server!");
                Console.WriteLine(ex);
                return Response;
            }

        }


        [HttpPost("add")]

        public ActionResult<JObject> NewRoom(Room Room)
        {
            JObject Response = new JObject();
            RoomDb RoomDb = new RoomDb();
            try
            {
                Room roomToCheck = RoomDb.GetByRoomNumber(Room.Number);
                if(roomToCheck == null)
                {
                    Room room = RoomDb.Add(Room);
                    if (room != null && RoomDb.Save())
                    {

                        Response.Add("success", $"[S] Successfully Added room #{room.Number}");
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
                    Response.Add("error", $"[S] Room with number {Room.Number} already exists!");
                    return Ok(Response);
                }
               
            }
            catch (Exception ex)
            {
                Response.Add("error", "[S] Could not add to database");
                Console.WriteLine(ex);
                return Ok(Response);
            }
            

        }

        [HttpDelete("{id}")]
        public ActionResult<JObject> Delete(int id)
        {
            JObject Response = new JObject();
            RoomDb RoomDb = new RoomDb();
            Room roomToDel = RoomDb.Remove(id);
            if (roomToDel != null)
            {
                Response.Add("success", $"[S] Deleted room #{roomToDel.Number} with id #{roomToDel.RoomID} !");
                return Ok(Response); ;

            }
            Response.Add("error", $"[S] Failed to remove room {roomToDel.Number}!, check server!");
            return Ok(Response);

        }
    }
}
