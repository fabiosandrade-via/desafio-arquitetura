using System.Net;

namespace consolidado.dominio.DTO
{
    public class ConsolidadoDTO
    {
        public List<LancamentoDTO> Lancamentos { get; set; } = new();
        public decimal? Acumulado { get; set; }
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public string Mensagem { get; set; }
    }
}
