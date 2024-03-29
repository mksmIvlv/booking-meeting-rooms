﻿using System.Net.Http.Json;
using Application.Interfaces;
using Contracts.Interface;
using Infrastructure.Settings;
using Infrastructure.Settings.Http;
using Microsoft.Extensions.Options;
using Polly;
using Polly.Retry;
using Serilog;

namespace Application.Services;

/// <summary>
/// Сервис для отправки сообщений с помощью Http
/// </summary>
public class HttpService<T> : IPublishBusService<T> where T : IMessage
{
    #region Поля

    /// <summary>
    /// Отправка с помощью Http
    /// </summary>
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Настройки Http
    /// </summary>
    private readonly HttpSettings _settings;

    #endregion

    #region Конструктор

    public HttpService(HttpClient httpClient, IOptions<InfrastructureSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings.Value.HttpSettings;
    }

    #endregion

    #region Метод

    /// <summary>
    /// Отправка сообщений с политиками от Polly
    /// </summary>
    /// <param name="classMessage">Класс - сообщение</param>
    public async Task SendMessageAsync(T classMessage)
    {
        await Policy
            .HandleResult<HttpResponseMessage>(message => !message.IsSuccessStatusCode)
            .WaitAndRetryAsync(new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10)
            }, (exception, timeSpan) =>
            {
                Log.Fatal($"Сообщение не отправленно. Ошибка: {exception}, время ожидания: {timeSpan}.");
            })
            .ExecuteAsync(async () => await _httpClient.PostAsJsonAsync(_settings.Address, classMessage));
    }
    
    /*/// <summary>
    /// Отправка сообщений с политиками от Microsoft Polly
    /// </summary>
    /// <param name="classMessage">Класс - сообщение</param>
    public async Task SendMessageAsync(T classMessage)
    {
        await _httpClient.PostAsJsonAsync(_settings.Address, classMessage);
    }*/

    #endregion
}