using AutoMapper;
using consolidado.dominio.DTO;
using consolidado.dominio.Entidades;
using consolidado.dominio.Interfaces;
using consolidado.servico.Interfaces;
using System.Globalization;

namespace consolidado.servico
{
    public class ConsolidadoServico : IConsolidadoServico
    {
        private readonly IFluxoCaixa _fluxoCaixa;
        private readonly IConsolidadoRepositorio _consolidadoRepositorio;
        private readonly IMapper _mapper;
        public ConsolidadoServico(IFluxoCaixa fluxoCaixa, IConsolidadoRepositorio consolidadoRepositorio, IMapper mapper)
        {
            _fluxoCaixa = fluxoCaixa;
            _consolidadoRepositorio = consolidadoRepositorio;
            _mapper = mapper;
        }
        public async Task AdicionarLancamentosAsync(List<LancamentoGrupoDTO> lancamentoAgrupado)
        {
            foreach (var lancamento in lancamentoAgrupado)
            {
                if (lancamento.Id.HasValue)
                {
                    ConsolidadoEntity consolidadoEntity = await _fluxoCaixa.ObterConsolidadoDiario(lancamento.Lancamentos, lancamento.Id.Value);
                    string dataFormatada = lancamento.Id.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);

                    if (await _consolidadoRepositorio.ConsolidadoExisteAsync(dataFormatada) == true)
                    {
                        await _consolidadoRepositorio.ExcluirConsolidadoAsync(dataFormatada);
                    }

                    await _consolidadoRepositorio.AdicionarConsolidadoAsync(dataFormatada, consolidadoEntity);
                }
            }
        }
        public async Task<ConsolidadoDTO?> BuscarConsolidadoAsync(string data)
        {
            ConsolidadoEntity? consolidadoEntity = await _consolidadoRepositorio.BuscarConsolidadoAsync(data);

            if (consolidadoEntity == null)
            {
                return null;
            }

            ConsolidadoDTO? consolidadoDTO = _mapper.Map<ConsolidadoDTO>(consolidadoEntity);
            return consolidadoDTO;
        }
    }
}
