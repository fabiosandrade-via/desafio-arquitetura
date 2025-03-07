using AutoMapper;
using lancamento.api.Rastreio;
using lancamento.dominio.DTO;
using lancamento.dominio.Entidades;
using lancamento.dominio.Interfaces;
using lancamento.integracao;
using lancamento.messagebroker;
using lancamento.repositorio;
using lancamento.repositorio.MongoDB;
using lancamento.servico;
using lancamento.servico.Interfaces;
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

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ILancamentoRepositorio, LancamentoRepositorio>();
builder.Services.AddScoped<ILancamentoApiExterna, LancamentoApiExterna>();
builder.Services.AddScoped<ILancamentoServico, LancamentoServico>();

Log.Logger = new LoggerConfiguration()
    .Enrich.FromLogContext()
    .Enrich.WithCorrelationId()
    .WriteTo.Console()
    .WriteTo.File("logs/logLancamentos.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Services.Configure<MongoDbSettings>(
    builder.Configuration.GetSection("MongoDbSettings"));

builder.Services.AddSingleton<MongoDbContext>();

builder.Services.Configure<KafkaSettings>(
    builder.Configuration.GetSection("KafkaSettings"));
builder.Services.AddSingleton<KafkaSettings>();

var configMapper = new MapperConfiguration(cfg =>
{
    cfg.CreateMap<string, DateTime?>().ConvertUsing<MappingStringDateTime>();

    cfg.CreateMap<LancamentoDTO, LancamentoEntity>()
        .ForMember(dest => dest.DataLancamento, opt => opt.MapFrom(src => src.Data));
});

IMapper mapper = configMapper.CreateMapper();
builder.Services.AddSingleton(mapper);

var app = builder.Build();

app.UseCors("AllowAll");

app.UseMiddleware<CorrelationIdMiddleware>();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI();

app.UseMetricServer();
app.UseHttpMetrics();

app.MapGet("/", () => "Aplicação lançamentos .NET 8 Monitorada com Prometheus e Grafana!");

app.MapPost("api/lancamentos", async (List<LancamentoDTO> lancamentosDTO, ILancamentoServico servico) =>
{
    try
    {
        await servico.AdicionarAsync(lancamentosDTO);
        return HttpStatusCode.OK;
    }
    catch
    {
        return HttpStatusCode.InternalServerError;
    }
})
.WithName("adicionar")
.WithOpenApi();

app.Run();
