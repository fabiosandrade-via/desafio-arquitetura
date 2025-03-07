namespace consolidado.dominio.DTO
{
    public class LancamentoDTO
    {
        public string? Tipo { get; set; }
        public decimal? Valor { get; set; }
        public string? Descricao { get; set; } = string.Empty;
        public DateTime? DataLancamento { get; set; }
    }
}
