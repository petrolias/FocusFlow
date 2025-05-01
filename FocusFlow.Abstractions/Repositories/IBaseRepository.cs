using FocusFlow.Abstractions.Models;

namespace FocusFlow.Abstractions.Repositories
{
    /// <summary>
    /// A base repository that implements the save changes
    /// </summary>
    /// <typeparam name="TEntityModel"></typeparam>
    public interface IBaseRepository<TEntityModel>
        where TEntityModel : IEntityModel
    {
        Task<TEntityModel?> GetByIdAsync(Guid id);

        Task<IEnumerable<TEntityModel>> GetAllAsync();

        Task AddAsync(TEntityModel entity, bool saveChanges);

        Task UpdateAsync(TEntityModel entity, bool saveChanges);

        Task DeleteAsync(Guid id, bool saveChanges);
    }
}