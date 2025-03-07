namespace lancamento.dominio.DTO
{
    public class LancamentoDTO
    {
        public string Tipo { get; set; } = string.Empty;
        public decimal? Valor { get; set; }
        public string? Descricao { get; set; } = string.Empty;
        public string? Data { get; set; } = string.Empty;
    }
}
