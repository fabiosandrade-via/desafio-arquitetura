using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using Polly.Wrap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lancamento.servico.Resiliencia
{
    internal static class CriacaoPolicy
    {
        private static AsyncCircuitBreakerPolicy AplicarCircuiBreaker()
        {
            var segundosAberturaCircuito = 6;
            var numeroMaximoExcecoesCircuito = 3;

            return Policy
                    .Handle<Exception>()
                    .CircuitBreakerAsync(numeroMaximoExcecoesCircuito, TimeSpan.FromSeconds(segundosAberturaCircuito),
                        onBreak: (_, _) =>
                        {
                            Console.WriteLine("Aberto (onBreak)", ConsoleColor.Red);
                        },
                        onReset: () =>
                        {
                            Console.WriteLine("Fechado (onReset)", ConsoleColor.Green);
                        },
                        onHalfOpen: () =>
                        {
                            Console.WriteLine("Semi aberto (onHalfOpen)", ConsoleColor.Magenta);
                        });
        }
        private static AsyncRetryPolicy AplicarRety()
        {
            var intervaloRetentativas = new[]
            {
                TimeSpan.FromSeconds(2),
                TimeSpan.FromSeconds(4),
                TimeSpan.FromSeconds(6)
            };

            return Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(
                    sleepDurations: intervaloRetentativas,
                    onRetry: (_, span, retryCount, _) =>
                    {
                        var previousBackgroundColor = Console.BackgroundColor;
                        var previousForegroundColor = Console.ForegroundColor;

                        Console.BackgroundColor = ConsoleColor.Yellow;
                        Console.ForegroundColor = ConsoleColor.Black;

                        Console.Out.WriteLineAsync($" ***** {DateTime.Now:HH:mm:ss} | " +
                            $"Retentativa: {retryCount} | " +
                            $"Tempo de Espera em segundos: {span.TotalSeconds} **** ");

                        Console.BackgroundColor = previousBackgroundColor;
                        Console.ForegroundColor = previousForegroundColor;
                    });
        }
        public static AsyncPolicyWrap AplicarPolitica()
        {
            return Policy.WrapAsync(AplicarCircuiBreaker(), AplicarRety());
        }
    }
}
