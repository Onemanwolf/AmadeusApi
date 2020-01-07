using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ReservationApi.Models;
using ReservationApi.Services;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;

namespace ReservationApi.Controllers
{


    //Session 1

    //Uses the BookService class to perform CRUD operations.
    //Contains action methods to support GET, POST, PUT, and DELETE HTTP requests.
    //Calls CreatedAtRoute in the Create action method to return an HTTP 201 response.Status 
    //code 201 is the standard response for an HTTP POST method that creates a new resource on 
    //the server.CreatedAtRoute also adds a Location header to the response. The Location header 
    //specifies the URI of the newly created book.





    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReservationController : ControllerBase
    {

        private readonly ReservationService _reservationService;
        private readonly IConfiguration _config;
        

        public ReservationController(ReservationService bookService, IConfiguration config)
        {
            _reservationService = bookService;
            _config = config;
            
        }
        [Authorize]  //Session 3 Identity Server OpenID Connect OAuth Bearer Token
        [HttpGet]
        public ActionResult<List<Reservation>> Get() {
            var c = _config["ConnectionString"];
           var reservations = _reservationService.Get();
            //Session 2\
            Log.Information($"Config Secret: {c}");
            Log.Information($"In My Reservation the controller:: {reservations} {DateTime.UtcNow}!");

            return reservations;
        } 


        [HttpGet("{id:length(24)}", Name = "GetReservation")]
        public ActionResult<Reservation> Get(string id)
        {
            var reservation = _reservationService.Get(id);

            if (reservation == null)
            {
                return NotFound();
            }

            return reservation;
        }

        [HttpPost]
        public ActionResult<Reservation> Create(Reservation reservation)
        {
            _reservationService.Create(reservation);

            return CreatedAtRoute("GetReservation", new { id = reservation.Id.ToString() }, reservation);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Reservation reservationIn)
        {
            var reservation = _reservationService.Get(id);

            if (reservation == null)
            {
                return NotFound();
            }

            _reservationService.Update(id, reservationIn);

            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var reservation = _reservationService.Get(id);

            if (reservation == null)
            {
                return NotFound();
            }

            _reservationService.Remove(reservation.Id);

            return NoContent();
        }

    }
}