using lancamento.dominio.Entidades;
using lancamento.dominio.Interfaces;
using lancamento.repositorio.MongoDB;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace lancamento.repositorio
{
    public class LancamentoRepositorio : ILancamentoRepositorio
    {
        private readonly IMongoCollection<LancamentoGrupoEntity> _collection;
        private readonly ILogger<LancamentoRepositorio> _logger;
        public LancamentoRepositorio(MongoDbContext context, ILogger<LancamentoRepositorio> logger)
        {
            _collection = context.ReqLancamentos;
            _logger = logger;
        }
        public async Task AdicionarLancamentosAsync(LancamentoGrupoEntity lancamentos)
        {
            try
            {
                await _collection.InsertOneAsync(lancamentos);
                _logger.LogDebug("Lançamentos inseridos com sucesso na coleção no MongoDB");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao tentar adicionar os lançamentos no banco de dados.");
            }
        }
        public async Task<LancamentoGrupoEntity> BuscarPorIdAsync(DateTime id)
        {
            try
            {
                return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync() ?? new LancamentoGrupoEntity();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao tentar buscar os lançamentos no banco de dados.");
                throw;
            }
        }
        public async Task ExcluirPorIdAsync(DateTime id)
        {
            try
            {
                await _collection.DeleteOneAsync(x => x.Id == id);
                _logger.LogDebug("Lançamentos excluídos com sucesso na coleção no MongoDB");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao tentar excluir os lançamentos no banco de dados.");
            }
        }        
    }
}
