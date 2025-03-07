using lancamento.dominio.DTO;

namespace lancamento.servico.Interfaces
{
    public interface ILancamentoServico
    {
        Task AdicionarAsync(List<LancamentoDTO> lancamentosDTO);
    }
}