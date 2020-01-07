﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ReservationApi.Models;
using ReservationApi.Services;
using Serilog;
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




    [Produces("application/json", "application/xml")]
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status406NotAcceptable)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        /// Get all Reservations.
        /// </summary>
        /// <response code="200">Returns when the Reservation is found </response>
        /// <response code="400">If the Reservation is null</response>
        /// <response code="404">If the Reservation is Not Found</response>
        [HttpGet]
        //[Authorize]  //Session 3 Identity Server OpenID Connect OAuth Bearer Token
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<List<Reservation>>> GetAsync()
        {
            var reservations = await _reservationService.GetAsync();

            Log.Information($"In My Reservation the controller:: {reservations} {DateTime.UtcNow}!");

            return reservations;
        }


        /// <summary>
        /// Get a specific Reservation.
        /// </summary>
        /// <param name="id"></param> 
        /// <response code="200">Returns when the Reservation is found </response>
        /// <response code="400">If the Reservation is null</response>
        /// <response code="404">If the Reservation is Not Found</response>
        [HttpGet("{id}", Name = "GetReservation")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Reservation>> GetAsync(string id)
        {
            var reservation = await _reservationService.GetAsync(id);

            if (reservation == null)
            {
                Log.Information($"Reservation for Id:{id} Not Found");
                return NotFound();
            }

            return Ok(reservation);
        }

        /// <summary>
        /// Create a Reservation.
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
        [Consumes("application/json")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<Reservation>> CreateAsync(Reservation reservation)
        {
            if (reservation == null)
                return BadRequest("No Reservation supplied");

            //if id is passed in with request, empty it so it can be generated by data layer
            if (!string.IsNullOrEmpty(reservation.Id))
                reservation.Id = string.Empty;

            var newReservation = await _reservationService.CreateAsync(reservation);

            return CreatedAtRoute("GetReservation", new { id = reservation.Id }, newReservation);
        }

        /// <summary>
        /// Update a specific reservation
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reservationIn"></param>
        /// <response code="204">Returns when the Reservation is Succesfully Updated </response>
        /// <response code="404">If the Reservation is Not Found</response>
        [Consumes("application/json")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAsync(string id, Reservation reservationIn)
        {
            var reservation = await _reservationService.GetAsync(id);

            if (reservation == null)
            {
                //status code 404
                return NotFound();
            }

            await _reservationService.UpdateAsync(id, reservationIn);

            return NoContent();
        }

        /// <summary>
        /// Delete a specific Reservation.
        /// </summary>
        /// <param name="id"></param> 
        /// <response code="204">Returns when the Reservation is Succesfully Deleted </response>
        /// <response code="404">If the Reservation is Not Found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var reservation = await _reservationService.GetAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            await _reservationService.RemoveAsync(reservation.Id);

            return NoContent();
        }
    }
}