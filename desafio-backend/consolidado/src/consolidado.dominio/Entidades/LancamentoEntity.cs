using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace consolidado.dominio.Entidades
{
    public class LancamentoEntity
    {
        public string? Tipo { get; set; }
        public decimal? Valor { get; set; }
        public string? Descricao { get; set; } = string.Empty;
        public DateTime? DataLancamento { get; set; }
    }
}
