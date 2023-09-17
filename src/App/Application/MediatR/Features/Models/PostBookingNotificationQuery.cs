using Application.Mediatr.Interfaces.Queries;
using MediatR;

namespace Application.Mediatr.Features.Models;

/// <summary>
/// Запрос на отправку оповещения о бронировании
/// </summary>
public class PostBookingNotificationQuery : IQuery<Unit> { }