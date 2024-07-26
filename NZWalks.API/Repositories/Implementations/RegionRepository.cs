using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories.Implementations;

public class RegionRepository : IRegionRepository
{
    private readonly NZWalksDbContext _dbContext;

    public RegionRepository(NZWalksDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<ICollection<Region>> GetAllAsync()
    {
        return await _dbContext.Regions.ToListAsync();
    }

    public async Task<Region?> GetByIdAsync(Guid id)
    {
        var region = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

        return region;
    }

    public async Task<Region> CreateRegionAsync(Region region)
    {
        await _dbContext.Regions.AddAsync(region);
        await _dbContext.SaveChangesAsync();

        return region;
    }

    public async Task<Region?> UpdateRegionAsync(Guid id, Region region)
    {
        var existingRegion = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

        if (existingRegion == null)
            return null;
        
        existingRegion.Code = region.Code;
        existingRegion.Name = region.Name;
        existingRegion.RegionImageUrl = region.RegionImageUrl;
        
        await _dbContext.SaveChangesAsync();

        return existingRegion;
    }

    public async Task<Region?> DeleteRegion(Guid id)
    {
        var existingRegion = await _dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

        if (existingRegion == null)
            return null;

        _dbContext.Regions.Remove(existingRegion);
        await _dbContext.SaveChangesAsync();

        return existingRegion;
    }
}