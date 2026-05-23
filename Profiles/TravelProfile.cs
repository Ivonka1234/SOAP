using AutoMapper;
using SOAP.DTOs.Location;
using SOAP.DTOs.Trip;
using SOAP.DTOs.TripLocation;
using SOAP.Models;

namespace SOAP.Profiles
{
    public class TravelProfile:Profile 
    {
        public TravelProfile()
        {
            // Trip
            CreateMap<CreateTripDTO, Trip>();
            CreateMap<UpdateTripDTO, Trip>();
            CreateMap<Trip, TripResponseDTO>();

            // Location
            CreateMap<CreateLocationDTO, Location>();
            CreateMap<UpdateLocationDTO, Location>();
            CreateMap<Location, LocationResponseDTO>();

        

            // TripLocation → Response
            CreateMap<TripLocation, TripLocationResponseDto>()
                .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.Location.Id))
                .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.Location.Name))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(src => src.Location.Country))
                .ForMember(dest => dest.VisitDurationHours, opt => opt.MapFrom(src => src.Location.VisitDurationHours))
                .ForMember(dest => dest.EstimatedCost, opt => opt.MapFrom(src => src.Location.EstimatedCost));
        }
    }
}
