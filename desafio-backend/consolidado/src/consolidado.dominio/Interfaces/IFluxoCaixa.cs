using consolidado.dominio.DTO;
using consolidado.dominio.Entidades;

namespace consolidado.dominio.Interfaces
{
    public interface IFluxoCaixa
    {
        Task<ConsolidadoEntity> ObterConsolidadoDiario(List<LancamentoDTO> lancamentos, DateTime data);
    }
}