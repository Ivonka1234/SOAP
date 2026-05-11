using Microsoft.EntityFrameworkCore;
using SOAP.Data;
using SOAP.Models;

namespace SOAP.Repository
    {
        public class TripLocationRepository : ITripLocationRepository
        {
            private readonly AppDbContext _context;

            public TripLocationRepository(AppDbContext context)
            {
                _context = context;
            }

            public async Task<List<TripLocation>> GetByTripIdAsync(Guid tripId)
            {
                return await _context.TripLocations
                    .Include(tl => tl.Location)
                    .Where(tl => tl.TripId == tripId)
                    .ToListAsync();
            }

            public async Task AddAsync(TripLocation tripLocation)
            {
                await _context.TripLocations.AddAsync(tripLocation);
                await _context.SaveChangesAsync();
            }

            public async Task DeleteAsync(Guid tripId, Guid locationId)
            {
                var entity = await _context.TripLocations
                    .FirstOrDefaultAsync(tl => tl.TripId == tripId && tl.LocationId == locationId);

                if (entity != null)
                {
                    _context.TripLocations.Remove(entity);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }

