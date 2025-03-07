using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lancamento.dominio.Entidades
{
    public class LancamentoEntity
    {
        public string? Tipo { get; set; }
        public decimal? Valor { get; set; }
        public string? Descricao { get; set; } = string.Empty;
        [BsonDateTimeOptions(Kind = DateTimeKind.Local)]
        public DateTime? DataLancamento { get; set; }
    }
}
