using AutoMapper;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.Dto;

namespace NZWalks.API.Mapping;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Region, RegionDto>().ReverseMap();
        CreateMap<AddRegionRequestDto, Region>().ReverseMap();
        CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();
        CreateMap<AddWalkRequestDto,Walk>().ReverseMap();
        CreateMap<WalkDto, Walk>().ReverseMap();
        CreateMap<DifficultyDto, Difficulty>().ReverseMap();
        CreateMap<UpdateWalkRequestDto, Walk>().ReverseMap();
    }
}