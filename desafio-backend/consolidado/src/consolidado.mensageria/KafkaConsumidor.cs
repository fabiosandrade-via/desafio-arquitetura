using Confluent.Kafka;
using consolidado.dominio.DTO;
using consolidado.servico.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace consolidado.messagebroker
{
    public class KafkaConsumidor : BackgroundService
    {
        private readonly ILogger<KafkaConsumidor> _logger;
        private readonly IConsumer<Ignore, string> _consumer;
        private readonly IConsolidadoServico _consolidadoServico;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public KafkaConsumidor(ILogger<KafkaConsumidor> logger, IServiceScopeFactory serviceScopeFactory, IOptions<KafkaSettings> settings)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                _consolidadoServico = scope.ServiceProvider.GetRequiredService<IConsolidadoServico>();
            }

            var config = new ConsumerConfig
            {
                BootstrapServers = settings.Value.BootstrapServers,
                GroupId = "grupo-consumidor",
                AutoOffsetReset = AutoOffsetReset.Latest
            };

            _consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            _consumer.Subscribe("kfk-topico-lancamentos");
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Consumidor Kafka iniciado.");
            List<LancamentoGrupoDTO>? lancamentosAgrupados = new List<LancamentoGrupoDTO>();

            await Task.Run(() =>
            {
                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        try
                        {
                            var consumeResult = _consumer.Consume(stoppingToken);
                            List<LancamentoGrupoDTO>? lancamentosConsumidor = null;

                            if (consumeResult.Message.Value != null)
                            {
                                lancamentosConsumidor = JsonConvert.DeserializeObject<List<LancamentoGrupoDTO>>(consumeResult.Message.Value);

                                if (lancamentosConsumidor != null)
                                    lancamentosAgrupados.AddRange(lancamentosConsumidor);
                            }

                            if (lancamentosConsumidor != null)
                            {
                                _logger.LogInformation($"Mensagem recebida: {consumeResult.Message.Value}");

                                if (lancamentosAgrupados.Count > 0)
                                    _ = _consolidadoServico.AdicionarLancamentosAsync(lancamentosAgrupados);
                            }
                        }
                        catch (OperationCanceledException)
                        {
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Erro no consumidor Kafka.");
                }
                finally
                {
                    _consumer.Close();  
                }
            });
        }
        public override void Dispose()
        {
            _consumer.Dispose();
            base.Dispose();
        }
    }
}
