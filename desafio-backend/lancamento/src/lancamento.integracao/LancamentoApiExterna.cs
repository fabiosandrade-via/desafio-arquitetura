using lancamento.dominio.Entidades;
using lancamento.dominio.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace lancamento.integracao
{
    public class LancamentoApiExterna : ILancamentoApiExterna
    {
        private readonly ILogger<LancamentoApiExterna> _logger;
        private readonly IConfiguration _configuration;
        public LancamentoApiExterna(ILogger<LancamentoApiExterna> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public async Task EnviarLancamentosApiExternaAsync(List<LancamentoEntity> lancamentos)
        {
            var json = JsonConvert.SerializeObject(lancamentos);

            using (var client = new HttpClient())
            {
                try
                {
                    var url = _configuration["ApiExterna:Url"];
                    _logger.LogInformation("Chamando API externa: {Url}", url);
                    var response = await client.PostAsync(
                        url,
                         new StringContent(json, Encoding.UTF8, "application/json"));

                    var resposta = response.Content.ReadAsStringAsync();
                    _logger.LogInformation("Lançamentos enviados para a API externa com sucesso.");
                }
                catch (Exception ex)
                {
                    _logger.LogError($"Erro ao enviar lançamentos para a API externa. {ex.Message}");
                    throw;
                }                    
            }
        }
    }
}
