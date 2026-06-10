using Microsoft.EntityFrameworkCore;
using SOAP.Data;
using SOAP.Models;

namespace SOAP.Repository
{
    public class TripRepository : ITripRepository
    {
        private readonly AppDbContext _context;

        public TripRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Trip trip)
        {
            await _context.Trips.AddAsync(trip);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Trip>> GetAllAsync()
        {
            return await _context.Trips
                .Include(t => t.TripLocations)
                .ThenInclude(tl => tl.Location)
                .ToListAsync();
        }

        public async Task<List<Trip>> GetByUserIdAsync(string userId)
        {
            return await _context.Trips
                .Where(t => t.UserId == userId)
                .Include(t => t.TripLocations)
                .ThenInclude(tl => tl.Location)
                .ToListAsync();
        }

        public async Task<bool> BelongsToUserAsync(Guid tripId, string userId)
        {
            return await _context.Trips.AnyAsync(t => t.Id == tripId && t.UserId == userId);
        }

        public async Task<Trip?> GetByIdAsync(Guid id)
        {
            return await _context.Trips
                .Include(t => t.TripLocations)
                .ThenInclude(tl => tl.Location)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task UpdateAsync(Trip entity)
        {
            _context.Trips.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var entity = await _context.Trips.FindAsync(id);
            if (entity != null)
            {
                _context.Trips.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
