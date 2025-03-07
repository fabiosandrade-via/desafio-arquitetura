namespace consolidado.dominio.Entidades
{
    public class ConsolidadoEntity
    {
        public List<LancamentoEntity> Lancamentos { get; set; } = new();
        public decimal? Acumulado { get; set; }
    }
}
