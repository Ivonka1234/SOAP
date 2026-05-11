using Microsoft.EntityFrameworkCore;
using SOAP.Data;
using SOAP.Models;

namespace SOAP.Repository
{

    public class LocationRepository : ILocationRepository
    {
        private readonly AppDbContext _context;

        public LocationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Location location)
        {
            await _context.Locations.AddAsync(location);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Location>> GetAllAsync()
        {
            return await _context.Locations.ToListAsync();
        }

        public async Task<Location?> GetByIdAsync(Guid id)
        {
            return await _context.Locations.FindAsync(id);
        }

        public async Task UpdateAsync(Location entity)
        {
            _context.Locations.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Locations.FindAsync(id);

            if (entity != null)
            {
                _context.Locations.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
