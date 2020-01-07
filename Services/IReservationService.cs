using ReservationApi.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReservationApi.Services
{


    //Session 1
    public interface IReservationService
    {
        Reservation Create(Reservation reservation);
        Task<List<Reservation>> Get();
        Task<Reservation> Get(string id);
        void Remove(Reservation reservationIn);
        void Remove(string id);
        void Update(string id, Reservation reservationIn);
    }
}