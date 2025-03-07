using AutoMapper;
using lancamento.dominio.DTO;
using lancamento.dominio.Entidades;
using lancamento.dominio.Interfaces;
using lancamento.messagebroker;
using lancamento.servico.Interfaces;
using lancamento.servico.Resiliencia;
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
            var dataLanc = DateTime.Today;

            LancamentoGrupoEntity req = await _repositorio.BuscarPorIdAsync(dataLanc);

            if (req.Id != null)
            {
                await _repositorio.ExcluirPorIdAsync(dataLanc);
                lancamentosEntity.AddRange(req.Lancamentos);
            }

            await _repositorio.AdicionarLancamentosAsync(new LancamentoGrupoEntity() { Id = dataLanc, Lancamentos = lancamentosEntity});
            await new AplicacaoResiliencia(_settings).EnviarLancamentosApiExterna(lancamentosEntity, _apiExterna);
        }       
    }
}
