using lancamento.dominio.Entidades;
using lancamento.dominio.Interfaces;

namespace lancamento.servico.Resiliencia
{
    internal class ExecucaoPolicy
    {
        private readonly ILancamentoApiExterna _apiExterna;
        public ExecucaoPolicy(ILancamentoApiExterna apiExterna)
        {
            _apiExterna = apiExterna;
        }
        public async Task ExecutarResilienciaApiAsync(List<LancamentoEntity> lancamentosEntity)
        {
            await CriacaoPolicy.AplicarPolitica().ExecuteAsync(() => _apiExterna.EnviarLancamentosApiExternaAsync(lancamentosEntity));
        }
    }
}
