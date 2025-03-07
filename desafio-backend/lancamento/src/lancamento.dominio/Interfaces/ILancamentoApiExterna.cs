using lancamento.dominio.Entidades;

namespace lancamento.dominio.Interfaces
{
    public interface ILancamentoApiExterna
    {
        Task EnviarLancamentosApiExternaAsync(List<LancamentoEntity> lancamentos);
    }
}