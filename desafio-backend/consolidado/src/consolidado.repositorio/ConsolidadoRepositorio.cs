
using consolidado.dominio.Entidades;
using consolidado.dominio.Interfaces;
using consolidado.repositorio.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace consolidado.repository
{
    public class ConsolidadoRepositorio : IConsolidadoRepositorio
    {
        private RedisDbContext _redisDbContext;
        private readonly ILogger<ConsolidadoRepositorio> _logger;
        public ConsolidadoRepositorio(ILogger<ConsolidadoRepositorio> logger, IConfiguration configuration)
        {
            var redisConnectionString = configuration["Redis:Connection"];
            
            if (string.IsNullOrEmpty(redisConnectionString))
            {
                throw new ArgumentNullException(nameof(redisConnectionString), "A conexão ao Redis não pode ser nula ou vazia.");
            }

            _redisDbContext = new RedisDbContext(redisConnectionString);
            _logger = logger;
        }
        public async Task AdicionarConsolidadoAsync(string chave, ConsolidadoEntity consolidadoDiario)
        {
            try
            {
                await _redisDbContext.AdicionarConsolidadoAsync(chave, consolidadoDiario);
                _logger.LogInformation($"Consolidado adicionado no banco de dados redis com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao adicionar consolidado ao banco de dados redis: {ex.Message}");
            }
        }
        public async Task<bool?> ConsolidadoExisteAsync(string chave)
        {
            try
            {
                _logger.LogInformation($"Verificando se consolidado existe no banco de dados redis.");
                return await _redisDbContext.ExisteConsolidadoAsync(chave);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao verificar se consolidado existe no banco de dados redis: {ex.Message}");
                return null;
            }
        }
        public async Task<ConsolidadoEntity?> BuscarConsolidadoAsync(string chave)
        {
            try
            {
                _logger.LogInformation($"Buscando consolidado no banco de dados redis.");
                return await _redisDbContext.BuscarConsolidadoAsync(chave);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao buscar consolidado no banco de dados redis: {ex.Message}");
                return null;
            }
        }
        public async Task ExcluirConsolidadoAsync(string chave)
        {
            try
            {
                await _redisDbContext.ExcluirConsolidadoAsync(chave);
                _logger.LogInformation($"Consolidado excluído do banco de dados redis com sucesso.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao excluir consolidado do banco de dados redis: {ex.Message}");
            }
        }
    }
}
