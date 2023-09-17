using Application.Models.Dto;
using AutoMapper;
using Domain.Models;

namespace Application.AutoMapper.Mapping;

public class ItemInMeetingRoomProfile : Profile
{
    /// <summary>
    /// Маппинг
    /// </summary>
    public ItemInMeetingRoomProfile()
    {
        CreateMap<ItemInMeetingRoom, ItemInMeetingRoomDto>()
            .ForMember(dto => dto.Item,
                m => m.MapFrom(e => e.Item))
            .ForMember(dto => dto.Price,
                m => m.MapFrom(e => e.ItemPrice));
    }
}