using lancamento.dominio.Entidades;

namespace lancamento.dominio.Interfaces
{
    public interface ILancamentoRepositorio
    {
        Task AdicionarLancamentosAsync(LancamentoGrupoEntity lancamentos);
        Task<LancamentoGrupoEntity> BuscarPorIdAsync(DateTime id);
        Task ExcluirPorIdAsync(DateTime id);
    }
}