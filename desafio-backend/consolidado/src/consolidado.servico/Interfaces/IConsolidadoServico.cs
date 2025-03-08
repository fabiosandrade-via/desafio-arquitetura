using consolidado.dominio.DTO;
using consolidado.dominio.Entidades;

namespace consolidado.servico.Interfaces
{
    public interface IConsolidadoServico
    {
        Task AdicionarLancamentosAsync(List<LancamentoGrupoDTO> lancamentoAgrupado);
        Task<ConsolidadoDTO?> BuscarConsolidadoAsync(string data);
    }
}