// MappingProfiles.cs is a class that defines the mapping profiles for AutoMapper.
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using Contracts;

// This namespace contains helper classes for handling requests in the AuctionService
namespace AuctionService.RequestHelpers
{
    // This class is used to define the mapping profiles for AutoMapper
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Mapping from Auction entity to AuctionDto and including Item entity in the mapping
            CreateMap<Auction, AuctionDto>().IncludeMembers(x => x.Item);

            // Mapping from Item entity to AuctionDto
            CreateMap<Item, AuctionDto>();

            // Mapping from CreateAuctionDto to Auction entity
            // The Item property of Auction is mapped from the entire source object
            CreateMap<CreateAuctionDto, Auction>()
                .ForMember(d => d.Item, o => o.MapFrom(s => s));

            // Mapping from CreateAuctionDto to Item entity
            CreateMap<CreateAuctionDto, Item>();

            // Mapping from AuctionDto to AuctionCreated contract
            CreateMap<AuctionDto, AuctionCreated>();

            // Mapping from Auction entity to AuctionUpdated contract and including Item entity in the mapping
            CreateMap<Auction, AuctionUpdated>().IncludeMembers(a => a.Item);

            // Mapping from Item entity to AuctionUpdated contract
            CreateMap<Item, AuctionUpdated>();
        }
    }
}