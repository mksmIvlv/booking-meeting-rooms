using Application.Models.Dto;
using AutoMapper;
using Domain.Models;

namespace Application.AutoMapper.Mapping;

/// <summary>
/// Настройка профиля маппинга для MeetingRoom
/// </summary>
public class MeetingRoomProfileMapping: Profile
{
    /// <summary>
    /// Маппинг
    /// </summary>
    public MeetingRoomProfileMapping()
    {
        CreateMap<MeetingRoom, MeetingRoomDto>()
            .ForMember(dto => dto.Id, 
                m => m.MapFrom(e => e.IdRoom))
            .ForMember(dto => dto.Name, 
                m => m.MapFrom(e => e.NameRoom))
            .ForMember(dto => dto.Description, 
                m => m.MapFrom(e => e.DescriptionRoom))
            .ForMember(dto => dto.BookingMeetingRoomsDto, 
                m => m.MapFrom(e => e.BookingMeetingRooms));
    }
}