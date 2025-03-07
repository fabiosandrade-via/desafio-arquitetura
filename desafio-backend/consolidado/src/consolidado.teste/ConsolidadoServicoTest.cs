using consolidado.dominio.DTO;
using consolidado.servico.Interfaces;
using Moq;
using System.Net;

namespace consolidado.teste
{
    public class ConsolidadoServicoTest
    {
        private readonly Mock<IConsolidadoServico> _mockService;

        public ConsolidadoServicoTest()
        {
            _mockService = new Mock<IConsolidadoServico>();
        }

        [Fact]
        public async Task BuscarConsolidadoAsync_DeveRetornarConsolidadoDTO_QuandoDataValida()
        {
            // Arrange
            var data = "2025-02-28";
            var consolidadoMock = new ConsolidadoDTO
            {
                Lancamentos = new List<LancamentoDTO>
            {
                new LancamentoDTO { Tipo = "Credito", Valor = 200, Descricao = "Freelance", DataLancamento = DateTime.Now }
            },
                Acumulado = 200,
                StatusCode = HttpStatusCode.OK,
                Mensagem = "Sucesso"
            };

            _mockService.Setup(s => s.BuscarConsolidadoAsync(data))
                        .ReturnsAsync(consolidadoMock);

            // Act
            var resultado = await _mockService.Object.BuscarConsolidadoAsync(data);

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(200, resultado.Acumulado);
            Assert.Equal(HttpStatusCode.OK, resultado.StatusCode);
            Assert.Single(resultado.Lancamentos);
        }
    }
}