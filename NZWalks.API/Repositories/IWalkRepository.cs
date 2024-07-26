using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories;

public interface IWalkRepository
{
    Task<Walk> CreateWalkAsync(Walk walk);
    Task<ICollection<Walk>> GetAllAsync(bool isAscending = true, string? sortBy = null,
        string? filterQuery = null, string? filterOn = null, int pageNumber = 1, int pageSize = 15);
    Task<Walk?> GetByIdAsync(Guid id);
    Task<Walk?> UpdateWalkAsync(Guid id, Walk walk);
    Task<Walk?> DeleteWalkAsync(Guid id);
}