namespace consolidado.dominio.DTO
{
    public class LancamentoGrupoDTO
    {
        public DateTime? Id { get; set; } = null;
        public List<LancamentoDTO> Lancamentos { get; set; } = new();
    }
}
