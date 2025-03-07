using lancamento.dominio.Entidades;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace lancamento.repositorio.MongoDB
{
    public class MongoDbContext
    {
        private readonly IMongoDatabase _database;
        public MongoDbContext(IOptions<MongoDbSettings> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);
            _database = client.GetDatabase(settings.Value.DatabaseName);
        }

        public IMongoCollection<LancamentoGrupoEntity> ReqLancamentos
            => _database.GetCollection<LancamentoGrupoEntity>("ReqLancamentos");
    }
}
