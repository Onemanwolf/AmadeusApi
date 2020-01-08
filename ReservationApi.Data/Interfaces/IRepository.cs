using ReservationApi.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ReservationApi.Data.Intefaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task DeleteAsync(T entity);
        Task DeleteAsync(string id);
        Task<T> GetAsync(string id);
        Task<List<T>> GetAsync();
        Task<T> InsertAsync(T entity);
        Task UpdateAsync(T entity);
    }
}