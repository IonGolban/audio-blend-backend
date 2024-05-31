using AudioBlend.Core.Repositories.Interfaces;
using AudioBlend.Core.Shared.Results;
using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Query.ExpressionTranslators.Internal;

namespace AudioBlend.Core.MusicData.Repositories.Implementations
{
    public class BaseRepository<T>(DbContext context) : IAsyncRepository<T>
        where T : class
    {
        protected readonly DbContext Context = context;

        public async Task<Result<T>> AddAsync(T entity)
        {
            await Context.Set<T>().AddAsync(entity);
            await Context.SaveChangesAsync();
            return Result<T>.Success(entity);
        }

        public async Task<Result<T>> DeleteAsync(Guid id)
        {
            var result = await GetByIdAsync(id);
            if (!result.IsSuccess)
            {
                return Result<T>.Failure($"Entity with Id {id} not found");
            }
            Context.Set<T>().Remove(result.Value);
            await Context.SaveChangesAsync();
            return Result<T>.Success(result.Value);
        }

        public async Task<Result<IReadOnlyList<T>>> GetAll()
        {
            var items = await Context.Set<T>().ToListAsync();
            return Result<IReadOnlyList<T>>.Success(items);
        }

        public virtual async Task<Result<T>> GetByIdAsync(Guid id)
        {
            var result = await Context.Set<T>().FindAsync(id);
            return result == null ? Result<T>.Failure($"Entity with Id {id} not found") : Result<T>.Success(result);
        }
        public async Task<Result<T>> UpdateAsync(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            await Context.SaveChangesAsync();
            return Result<T>.Success(entity);
        }
    }
}
