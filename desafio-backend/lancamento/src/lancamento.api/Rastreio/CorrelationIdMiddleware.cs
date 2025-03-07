namespace lancamento.api.Rastreio
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (!context.Request.Headers.ContainsKey("X-Correlation-ID"))
            {
                context.Request.Headers["X-Correlation-ID"] = Guid.NewGuid().ToString();
            }

            context.Items["CorrelationId"] = context.Request.Headers["X-Correlation-ID"];
            await _next(context);
        }
    }
}
