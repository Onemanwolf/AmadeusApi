using ReservationApi.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationApi.Services
{
    public class ReservationService : IReservationService
    {



        //In the code below, an IBookstoreDatabaseSettings instance is retrieved from DI via constructor 
        //injection. This technique provides access to the appsettings.json configuration values that were added 
        //in the Add a configuration model section.
       
            private readonly IMongoCollection<Reservation> _reservation;

            public ReservationService(IReservationDataSettings settings)
            {
                var client = new MongoClient(settings.ConnectionString);
                var database = client.GetDatabase(settings.DatabaseName);

                _reservation = database.GetCollection<Reservation>(settings.ReservationCollectionName);
            }

            public List<Reservation> Get() =>
               _reservation.Find(reservation => true).ToList();

            public Reservation Get(string id) =>
                _reservation.Find<Reservation>(book => book.Id == id).FirstOrDefault();

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

