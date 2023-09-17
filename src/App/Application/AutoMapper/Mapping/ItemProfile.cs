using Application.Models.Dto;
using AutoMapper;
using Domain.Models;

namespace Application.AutoMapper.Mapping;

public class ItemProfile : Profile
{
    /// <summary>
    /// Маппинг
    /// </summary>
    public ItemProfile()
    {
        CreateMap<Item, ItemDto>()
            .ForMember(dto => dto.Name,
                m => m.MapFrom(e => e.Name))
            .ForMember(dto => dto.Description,
                m => m.MapFrom(e => e.Description));
    }
}