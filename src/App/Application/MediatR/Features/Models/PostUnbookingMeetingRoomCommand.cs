using Application.Mediatr.Interfaces.Commands;
using MediatR;

namespace Application.Mediatr.Features.Models;

/// <summary>
/// Команда для разбронирования комнат
/// </summary>
public class PostUnbookingMeetingRoomCommand : ICommand<Unit> { }