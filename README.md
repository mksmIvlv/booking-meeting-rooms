    В данном проекте рассмотрены следующие технологии:
      1. Infrastructure:
          PostgreSQL, Entity Framework(Fluent Api), Redis,
          gRPC(взаимодействие с другим приложением).

      2. Application:
          MediatR, AutoMapper, RabbitMQ, Kafka, MassTransit
          Http(взаимодействие с другим приложением),
          Polly/Microsoft.Polly.

      3. Presentation:
          Serilog, Swagger, HealthCheck/HealthCheckUI, Jaeger 
          OpenTelemetry/Prometheus, Elastic/Kibana.
    
      4. Tests:
          NSubstitute, Nunit.

      *Все доп сервисы подняты локально в Docker, кроме Prometheus.