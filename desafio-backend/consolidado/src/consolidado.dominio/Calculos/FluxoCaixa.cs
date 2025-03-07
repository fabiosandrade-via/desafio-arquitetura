using consolidado.dominio.DTO;
using consolidado.dominio.Entidades;
using consolidado.dominio.Interfaces;
using static consolidado.dominio.Enumeradores.EnumTipoLancamento;

namespace consolidado.dominio.Calculos
{
    public class FluxoCaixa : IFluxoCaixa
    {
        private decimal? CalcularSaldo(List<LancamentoDTO> lancamentos, DateTime data)
        {
            return lancamentos.Where(l => l.DataLancamento.HasValue && l.DataLancamento.Value.Date == data.Date)
                              .Sum(l => string.Equals(l.Tipo, TipoLancamento.Credito.ToString(), StringComparison.OrdinalIgnoreCase) ? l.Valor : -l.Valor);
        }
        public async Task<ConsolidadoEntity> ObterConsolidadoDiario(List<LancamentoDTO> lancamentos, DateTime data)
        {
            var saldo = CalcularSaldo(lancamentos, data);

            return await Task.FromResult(new ConsolidadoEntity
            {
                Lancamentos = lancamentos.Select(l => new LancamentoEntity
                {
                    Tipo = l.Tipo,
                    Valor = l.Valor,
                    Descricao = l.Descricao,
                    DataLancamento = l.DataLancamento
                }).ToList(),
                Acumulado = saldo
            });
        }
    }
}
