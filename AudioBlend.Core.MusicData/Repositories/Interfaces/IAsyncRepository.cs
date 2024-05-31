using AudioBlend.Core.Shared.Results;

namespace AudioBlend.Core.Repositories.Interfaces
{
    public interface IAsyncRepository<T> where T : class
    {
        Task<Result<T>> GetByIdAsync(Guid id);
        Task<Result<T>> AddAsync(T entity);
        Task<Result<T>> UpdateAsync(T entity);
        Task<Result<T>> DeleteAsync(Guid id);
        Task<Result<IReadOnlyList<T>>> GetAll();

    }
}
