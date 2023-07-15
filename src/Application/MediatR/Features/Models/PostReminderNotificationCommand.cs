using Application.Mediatr.Interfaces.Commands;
using MediatR;

namespace Application.Mediatr.Features.Models;

/// <summary>
/// Команда для отправки напоминания о бронировании
/// </summary>
public class PostReminderNotificationCommand : ICommand<Unit> { }