using lancamento.dominio.DTO;
using lancamento.servico.Interfaces;
using Moq;

namespace lancamento.teste
{
    public class LancamentoServicoTest
    {
        private readonly Mock<ILancamentoServico> _lancamentoServicoMock;
        public LancamentoServicoTest()
        {
            _lancamentoServicoMock = new Mock<ILancamentoServico>();
        }

        [Fact]
        public async Task AdicionarAsync_DeveChamarOMetodoComListaCorreta()
        {
            var lancamentos = new List<LancamentoDTO>
        {
            new LancamentoDTO { Tipo = "Credito", Valor = 100.50m, Descricao = "Venda", Data = "2025-02-28" },
            new LancamentoDTO { Tipo = "Debito", Valor = 50.75m, Descricao = "Compra", Data = "2025-02-28" }
        };

            _lancamentoServicoMock.Setup(s => s.AdicionarAsync(lancamentos))
                .Returns(Task.CompletedTask)
                .Verifiable();

            await _lancamentoServicoMock.Object.AdicionarAsync(lancamentos);

            _lancamentoServicoMock.Verify(s => s.AdicionarAsync(lancamentos), Times.Once);
        }
    }
}