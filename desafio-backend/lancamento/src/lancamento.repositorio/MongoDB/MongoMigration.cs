using lancamento.dominio.Entidades;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lancamento.repositorio.MongoDB
{
    public class MongoMigration
    {
        private readonly IMongoDatabase _database;

        public MongoMigration(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task CriarColecaoSeNaoExisteAsync()
        {
            var collectionNames = await _database.ListCollectionNamesAsync();
            var collections = await collectionNames.ToListAsync();

            if (!collections.Contains("lancamentos"))
            {
                await _database.CreateCollectionAsync("lancamentos");
                Console.WriteLine("Coleção 'lancamentos' criada com sucesso.");
            }
            else
            {
                Console.WriteLine("Coleção 'lancamentos' já existe.");
            }
        }

        public async Task AdicionarExemploDataAsync()
        {
            var collection = _database.GetCollection<LancamentoGrupoEntity>("lancamentos");

            var lancamentoGrupo = new LancamentoGrupoEntity
            {
                Id = DateTime.Now,
                Lancamentos = new List<LancamentoEntity>
            {
                new LancamentoEntity
                {
                    Tipo = "Credito",
                    Valor = 1000.50m,
                    Descricao = "Pagamento de serviço",
                    DataLancamento = DateTime.Now
                }
            }
            };

            await collection.InsertOneAsync(lancamentoGrupo);
            Console.WriteLine("Dados de exemplo inseridos com sucesso.");
        }
    }
}
