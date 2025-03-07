using AutoMapper;
using consolidado.dominio.Calculos;
using consolidado.dominio.DTO;
using consolidado.dominio.Entidades;
using consolidado.dominio.Interfaces;
using consolidado.messagebroker;
using consolidado.repository;
using consolidado.servico;
using consolidado.servico.Interfaces;
using Prometheus;
using Serilog;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

builder.WebHost.UseUrls("http://0.0.0.0:80");

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithCorrelationId()
    .WriteTo.Console()
    .WriteTo.File("logs/logConsolidado.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Services.AddScoped<IConsolidadoServico, ConsolidadoServico>();
builder.Services.AddScoped<IFluxoCaixa, FluxoCaixa>();
builder.Services.AddScoped<IConsolidadoRepositorio, ConsolidadoRepositorio>();

builder.Services.Configure<KafkaSettings>(
    builder.Configuration.GetSection("KafkaSettings"));
builder.Services.AddHostedService<KafkaConsumidor>();

var configMapper = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<ConsolidadoEntity, ConsolidadoDTO>()
        .ForMember(dest => dest.Lancamentos, opt => opt.MapFrom(src => src.Lancamentos));
    cfg.CreateMap<LancamentoEntity, LancamentoDTO>();
});

IMapper mapper = configMapper.CreateMapper();
builder.Services.AddSingleton(mapper);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var app = builder.Build();

app.UseCors("AllowAll");

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMetricServer();
app.UseHttpMetrics();

app.MapGet("/", () => "Aplicação cnosolidado diário .NET 8 Monitorada com Prometheus e Grafana!");

app.MapPost("api/lancamentos/consolidado", async (List<LancamentoDTO> lancamentosDTO, IConsolidadoServico servico) =>
{
    try
    {
        await servico.AdicionarLancamentosAsync(lancamentosDTO);
        return HttpStatusCode.OK;
    }
    catch
    {
        return HttpStatusCode.InternalServerError;
    }
})
.WithName("adicionar")
.WithOpenApi();

app.MapGet("api/lancamentos/consolidado", async (string DataConsolidado, IConsolidadoServico servico) =>
{
    ConsolidadoDTO? consolidado = new();
    try
    {
        consolidado = await servico.BuscarConsolidadoAsync(DataConsolidado);

        if (consolidado == null)
        {
            consolidado = new ConsolidadoDTO
            {
                StatusCode = HttpStatusCode.NotFound,
                Mensagem = "Não existe lançamentos para data informada."
            };
        }
        else
        { 
            consolidado.Mensagem = "Lançamentos encontrados.";
        }
    }
    catch(Exception ex)
    {
        if (consolidado != null)
        {
            consolidado.StatusCode = HttpStatusCode.InternalServerError;
            consolidado.Mensagem = ex.Message;
        }
    }

    return consolidado;
})
.WithName("consultar")
.WithOpenApi();

app.Run();
