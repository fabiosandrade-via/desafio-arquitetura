using AutoMapper;
using lancamento.dominio.DTO;
using lancamento.dominio.Entidades;
using lancamento.dominio.Interfaces;
using lancamento.messagebroker;
using lancamento.servico.Interfaces;
using lancamento.servico.Resiliencia;
using lancamento.servico.Uteis;
using Microsoft.Extensions.Options;

namespace lancamento.servico
{
    public class LancamentoServico : ILancamentoServico
    {
        private readonly ILancamentoRepositorio _repositorio;
        private readonly ILancamentoApiExterna _apiExterna;
        private readonly IMapper _mapper;
        private readonly IOptions<KafkaSettings> _settings;

        public LancamentoServico(ILancamentoRepositorio repositorio, ILancamentoApiExterna apiExterna, IMapper mapper, IOptions<KafkaSettings> settings)
        {
            _repositorio = repositorio;
            _apiExterna = apiExterna;
            _mapper = mapper;
            _settings = settings;
        }
        public async Task AdicionarAsync(List<LancamentoDTO> lancamentosDTO)
        {
            var lancamentosEntity = _mapper.Map<List<LancamentoEntity>>(lancamentosDTO);
            List<LancamentoGrupoEntity> lancamentosAgrupados = await new ClassificaLancamentos(lancamentosEntity).RetornaLancamentosAgrupados();
            List<LancamentoGrupoEntity> lancamentosAgrupadosEntity = new List<LancamentoGrupoEntity>();

            foreach (var lancamentoAgrupado in lancamentosAgrupados)
            {
                DateTime? dataOriginal = lancamentoAgrupado.Id?.Date;
                lancamentoAgrupado.Id = dataOriginal;
                DateTime? dataLancamento = dataOriginal;

                if (lancamentoAgrupado.Id.HasValue)
                    dataLancamento = lancamentoAgrupado.Id.Value.Date;

                LancamentoGrupoEntity req = await _repositorio.BuscarPorIdAsync(dataLancamento);

                if (req.Id != null)
                {
                    await _repositorio.ExcluirPorIdAsync(dataLancamento);
                    lancamentoAgrupado.Lancamentos.AddRange(req.Lancamentos);
                }

                lancamentosAgrupadosEntity.Add(lancamentoAgrupado);
                await _repositorio.AdicionarLancamentosAsync(lancamentoAgrupado);
            }

            await new AplicacaoResiliencia(_settings).EnviarLancamentosApiExterna(lancamentosAgrupadosEntity, _apiExterna);
        }
    }
}
