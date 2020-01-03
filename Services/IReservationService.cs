using ReservationApi.Models;
using System.Collections.Generic;

namespace ReservationApi.Services
{
    public interface IReservationService
    {
        Reservation Create(Reservation reservation);
        List<Reservation> Get();
        Reservation Get(string id);
        void Remove(Reservation reservationIn);
        void Remove(string id);
        void Update(string id, Reservation reservationIn);
    }
}