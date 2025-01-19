using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDBContext dBContext;

        public SQLWalkRepository(NZWalksDBContext dBContext)
        {
            this.dBContext = dBContext;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dBContext.Walks.AddAsync(walk);
            await dBContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var walkDomainModel= await dBContext.Walks.FirstOrDefaultAsync(x=>x.Id==id);
            if (walkDomainModel == null)
            {
                return null;
            }

            dBContext.Walks.Remove(walkDomainModel);
            await dBContext.SaveChangesAsync();
            return walkDomainModel;
        }

        public async Task<List<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? shortBy = null, bool isAscending = true, int pageNumber = 1, int pageSize = 1000)
        {
            var walks = dBContext.Walks.Include("Difficulty").Include("Region").AsQueryable();

            //Filtering
            if(string.IsNullOrWhiteSpace(filterOn)==false && string.IsNullOrWhiteSpace(filterQuery)==false)
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
                
            }
            //Sorting
            if (string.IsNullOrWhiteSpace(shortBy) == false)
            {
                if(shortBy.Equals("Name",StringComparison.OrdinalIgnoreCase))
                {
                    walks=isAscending ? walks.OrderBy(x => x.Name):walks.OrderByDescending(x => x.Name);
                }
                else if(shortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks=isAscending ? walks.OrderBy(x => x.LengthInKm):walks.OrderByDescending(x=>x.LengthInKm);
                }
            }
            //Pagination
            var skipResult=(pageNumber-1)*pageSize;


            return await walks.Skip(skipResult).Take(pageSize).ToListAsync();
            //return await dBContext.Walks.Include("Difficulty").Include("Region").ToListAsync();

        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await dBContext.Walks.Include("Difficulty").Include("Region").FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var existingDomainModel=  await dBContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if(existingDomainModel == null)
            {
                return null;
            }

            existingDomainModel.Name = walk.Name;
            existingDomainModel.Description = walk.Description;
            existingDomainModel.LengthInKm = walk.LengthInKm;
            existingDomainModel.WalkImageUrl = walk.WalkImageUrl;
            existingDomainModel.DifficultyId = walk.DifficultyId;
            existingDomainModel.RegionId = walk.RegionId;

            await dBContext.SaveChangesAsync();

            return existingDomainModel;

        }
    }
}
