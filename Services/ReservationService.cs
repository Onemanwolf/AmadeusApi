﻿using MongoDB.Driver;
using ReservationApi.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationApi.Services
{
    public class ReservationService : IReservationService
    {

        //Session 1

        //In the code below, an IBookstoreDatabaseSettings instance is retrieved from DI via constructor 
        //injection. This technique provides access to the appsettings.json configuration values that were added 
        //in the Add a configuration model section.

        private readonly IMongoCollection<Reservation> _reservation;

        public ReservationService(IMongoCollection<Reservation> collection)
        {
            _reservation = collection;
        }

        public async Task<List<Reservation>> Get() =>
          await _reservation.Find(reservation => true).ToListAsync().ConfigureAwait(true);

        public async Task<Reservation> Get(string id) =>
         await   _reservation.Find<Reservation>(book => book.Id == id).FirstOrDefaultAsync().ConfigureAwait(true);

        public Reservation Create(Reservation reservation)
        {
            _reservation.InsertOne(reservation);
            return reservation;
        }

        public void Update(string id, Reservation reservationIn) =>
            _reservation.ReplaceOne(book => book.Id == id, reservationIn);

        public void Remove(Reservation reservationIn) =>
            _reservation.DeleteOne(book => book.Id == reservationIn.Id);

        public void Remove(string id) =>
            _reservation.DeleteOne(reservation => reservation.Id == id);
    }
}

