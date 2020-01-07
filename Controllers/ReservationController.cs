using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.Extensions.Configuration;
using ReservationApi.Models;
using ReservationApi.Services;
using Serilog;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReservationApi.Controllers
{


    //Session 1

    //Uses the BookService class to perform CRUD operations.
    //Contains action methods to support GET, POST, PUT, and DELETE HTTP requests.
    //Calls CreatedAtRoute in the Create action method to return an HTTP 201 response.Status 
    //code 201 is the standard response for an HTTP POST method that creates a new resource on 
    //the server.CreatedAtRoute also adds a Location header to the response. The Location header 
    //specifies the URI of the newly created book.




    [Produces("application/json")]
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


        /// <summary>
        /// Gets all Reservation.
        /// </summary>
        [Authorize]  //Session 3 Identity Server OpenID Connect OAuth Bearer Token
        [HttpGet]
        public async Task<ActionResult<List<Reservation>>> Get() {
            
           var reservations =  await _reservationService.Get().ConfigureAwait(true);
           
            //Session 2\
            Log.Information($"Created Reservation the controller:: {reservations} {DateTime.UtcNow}!");

            return reservations;
        }


        /// <summary>
        /// Gets a specific Reservation.
        /// </summary>
        /// <param name="id"></param> 
        [HttpGet("{id:length(24)}", Name = "GetReservation")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Reservation>> Get(string id)
        {
            var reservation = await _reservationService.Get(id).ConfigureAwait(true);

            if (reservation == null)
            {
                return NotFound();
            }

            return reservation;
        }

        /// <summary>
        /// Creates a Reservation.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Reservation 
        ///     {
        ///        
        ///       "Name": "string",
        ///       "Price": 0,
        ///       "RoomId": "string",
        ///       "FromDate": "string",
        ///       "ToDate": "string"
        ///     }
        ///
        /// </remarks>
        /// <param name="reservation"></param>
        /// <returns>A newly created Reservation</returns>
        /// <response code="201">Returns the newly created Reservation </response>
        /// <response code="400">If the Reservation is null</response>            
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<Reservation> Create(Reservation reservation)
        {
            _reservationService.Create(reservation);

            return CreatedAtRoute("GetReservation", new { id = reservation.Id.ToString() }, reservation);
        }

        
        /// <summary>
        /// Updates a specific Reservation.
        /// </summary>
        /// <param name="id"></param> 
        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Reservation reservationIn)
        {
            var reservation = _reservationService.Get(id).Result;

            if (reservation == null)
            {
                return NotFound();
            }

            _reservationService.Update(id, reservationIn);

            return NoContent();
        }



        /// <summary>
        /// Deletes a specific Reservation.
        /// </summary>
        /// <param name="id"></param> 
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var reservation = _reservationService.Get(id).Result;

            if (reservation == null)
            {
                return NotFound();
            }

            _reservationService.Remove(reservation.Id);

            return NoContent();
        }

    }
}