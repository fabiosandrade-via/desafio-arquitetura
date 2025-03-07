using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lancamento.dominio.Entidades
{
    public class LancamentoGrupoEntity
    {
        [BsonId]
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? Id { get; set; } = null;
        public List<LancamentoEntity> Lancamentos { get; set; } = new();
    }
}
