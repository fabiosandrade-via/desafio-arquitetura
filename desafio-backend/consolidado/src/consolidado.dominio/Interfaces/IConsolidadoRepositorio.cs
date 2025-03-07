using consolidado.dominio.Entidades;

namespace consolidado.dominio.Interfaces
{
    public interface IConsolidadoRepositorio
    {
        Task AdicionarConsolidadoAsync(string chave, ConsolidadoEntity consolidadoDiario);
        Task<bool?> ConsolidadoExisteAsync(string chave);
        Task<ConsolidadoEntity?> BuscarConsolidadoAsync(string chave);
        Task ExcluirConsolidadoAsync(string chave);
    }
}