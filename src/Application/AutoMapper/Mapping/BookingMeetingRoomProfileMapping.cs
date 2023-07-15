using Application.Models.Dto;
using AutoMapper;
using Domain.Models;

namespace Application.AutoMapper.Mapping;

/// <summary>
/// Настройка профиля маппинга для BookingMeetingRoom
/// </summary>
public class BookingMeetingRoomProfileMapping: Profile
{
    /// <summary>
    /// Маппинг
    /// </summary>
    public BookingMeetingRoomProfileMapping()
    {
        CreateMap<BookingMeetingRoom, BookingMeetingRoomDto>()
            .ForMember(dto => dto.DateMeeting,
                m => m.MapFrom(e => e.DateMeeting))
            .ForMember(dto => dto.StartTimeMeeting, 
                m => m.MapFrom(e => e.StartTimeMeeting))
            .ForMember(dto => dto.EndTimeMeeting, 
                m => m.MapFrom(e => e.EndTimeMeeting))
            .ForMember(dto => dto.MeetingRoomDtoId, 
                m => m.MapFrom(e => e.MeetingRoomId));
    }
}