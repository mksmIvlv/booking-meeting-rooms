﻿using Application.Mediatr.Features.Models;
using Application.Models.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class MeetingRoomController : ControllerBase
{
    #region Поле

    /// <summary>
    /// Медиатор
    /// </summary>
    private readonly IMediator _mediator;

    #endregion

    #region Конструктор
    
    public MeetingRoomController(IMediator mediator)
    {
        _mediator = mediator;
    }

    #endregion

    #region Api - Методы
    
    /// <summary>
    /// Бронирование комнаты
    /// </summary>
    /// <param name="id">Id комнаты</param>
    /// <param name="dateMeeting">Дата брони</param>
    /// <param name="startTimeMeeting">Время начала брони</param>
    /// <param name="endTimeMeeting">Время конца брони</param>
    /// <returns>Данные о бронировании</returns>
    [HttpPost]
    public async Task<BookingMeetingRoomDto> BookingMeetingRoomAsync(Guid id, string dateMeeting, string startTimeMeeting, string endTimeMeeting)
    {
        var bookingMeetingRoomDto = await _mediator.Send(
            new PostBookingMeetingRoomCommand(id, dateMeeting, startTimeMeeting, endTimeMeeting));
        
        await _mediator.Send(new PostBookingNotificationQuery());

        return bookingMeetingRoomDto;
    }
    
    /// <summary>
    /// Расписание комнаты
    /// </summary>
    /// <param name="id">Id комнаты</param>
    /// <returns>Расписание комнаты</returns>
    [HttpGet]
    public async Task<MeetingRoomDto> ScheduleSpecificRoomAsync(Guid id)
    {
        return await _mediator.Send(new GetScheduleSpecificRoomQuery(id));
    }

    #endregion
}