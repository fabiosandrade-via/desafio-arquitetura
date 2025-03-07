using consolidado.dominio.Entidades;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Text.Json;

namespace consolidado.repositorio.Redis
{
    public class RedisDbContext
    {
        private readonly IDatabase _db;

        public RedisDbContext(string conexaoRedis)
        {
            var redis = ConnectionMultiplexer.Connect(conexaoRedis);
            _db = redis.GetDatabase();
        }
        public async Task AdicionarConsolidadoAsync(string chave, ConsolidadoEntity data)
        {
            string jsonData = JsonSerializer.Serialize(data);
            await _db.StringSetAsync(chave, jsonData);
        }
        private async Task<ConsolidadoEntity?> ObterConsolidadoAsync(string chave)
        {
            var jsonData = await _db.StringGetAsync(chave);
            return jsonData.HasValue ? JsonSerializer.Deserialize<ConsolidadoEntity>(jsonData!) : null;
        }
        public async Task<bool?> ExisteConsolidadoAsync(string chave)
        {
            return await ObterConsolidadoAsync(chave) == null ? false : true;
        }
        public async Task<ConsolidadoEntity?> BuscarConsolidadoAsync(string chave)
        {
            return await ObterConsolidadoAsync(chave);
        }
        public async Task<bool> ExcluirConsolidadoAsync(string chave)
        {
            return await Task.FromResult(_db.KeyDelete(chave));
        }
    }
}
