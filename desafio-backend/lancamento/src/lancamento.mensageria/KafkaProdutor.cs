using Confluent.Kafka;
using lancamento.dominio.Entidades;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace lancamento.messagebroker
{
    public class KafkaProdutor
    {
        private readonly IOptions<KafkaSettings> _settings;
        public KafkaProdutor(IOptions<KafkaSettings> settings)
        {
            _settings = settings;
        }
        public async Task Enviar(List<LancamentoGrupoEntity> lancamentos)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = _settings.Value.BootstrapServers
            };

            using var producer = new ProducerBuilder<Null, string>(config).Build();
            var message = JsonConvert.SerializeObject(lancamentos);

            await producer.ProduceAsync("kfk-topico-lancamentos", new Message<Null, string> { Value = message });
            Console.WriteLine($"Enviado: {message}");

            producer.Flush(TimeSpan.FromSeconds(10));
        }
    }
}