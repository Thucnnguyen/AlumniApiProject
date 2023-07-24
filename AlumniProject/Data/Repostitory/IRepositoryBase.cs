using AlumniProject.Dto;
using System.Linq.Expressions;

namespace AlumniProject.Data.Repostitory
{
    public interface IRepositoryBase<T>
    {
        Task<PagingResultDTO<T>> GetAllByConditionAsync(int pageNo, int pageSize, params Expression<Func<T, bool>>[] filters);
        Task<IEnumerable<T>> GetAllByConditionAsync(params Expression<Func<T, bool>>[] filters);
        Task<int> CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteByIdAsync(int id);
        Task<T> GetByIdAsync( params Expression<Func<T, bool>>[] filters);
        Task<T> FindOneByCondition(params Expression<Func<T, bool>>[] filters);
        Task<int> CountByCondition(params Expression<Func<T, bool>>[] filters);
        Task CreateRangeAsync(List<T> entities);
    }
}
