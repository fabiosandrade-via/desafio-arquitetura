using lancamento.dominio.Entidades;
using lancamento.dominio.Interfaces;
using lancamento.messagebroker;
using Microsoft.Extensions.Options;

namespace lancamento.servico.Resiliencia
{
    internal class AplicacaoResiliencia
    {
        private readonly KafkaProdutor _kafkaProdutor;

        public AplicacaoResiliencia(IOptions<KafkaSettings> kafkaSettings)
        {
            _kafkaProdutor = new KafkaProdutor(kafkaSettings);
        }

        public async Task EnviarLancamentosApiExterna(List<LancamentoGrupoEntity> lancamentosAgrupados, ILancamentoApiExterna apiExterna)
        {
            ExecucaoPolicy politicaExecucao = new ExecucaoPolicy(apiExterna);

            int contLimite = 0;
            int limiteChamadasApi = 3;

            while (contLimite < limiteChamadasApi)
            {
                try
                {
                    await politicaExecucao.ExecutarResilienciaApiAsync(lancamentosAgrupados);
                    break;
                }
                catch
                {
                    contLimite++;
                }
            }

            if (contLimite == limiteChamadasApi)
            {
                await _kafkaProdutor.Enviar(lancamentosAgrupados);
            }
        }
    }
}
