using System.Net;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Bicep.Core.Emit;
using System.Web;
using Bicep.Core;

public class Deploy
{
    private readonly ILogger logger;
    private readonly BicepCompiler compiler;

    public Deploy(ILoggerFactory loggerFactory, BicepCompiler compiler)
    {
        logger = loggerFactory.CreateLogger<Deploy>();
        this.compiler = compiler;
    }

    [Function("Deploy")]
    public async Task<HttpResponseData> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
    {
        try
        {
            var query = HttpUtility.ParseQueryString(req.Url.Query);
            if (query["uri"] is not {} uriString)
            {
                logger.LogInformation("Failed to parse uri query parameter");
                return req.CreateResponse(HttpStatusCode.BadRequest);
            }

            var uri = new Uri(uriString);
            var compilation = await compiler.CreateCompilation(uri, skipRestore: true);

            var emitter = new TemplateEmitter(compilation.GetEntrypointSemanticModel());

            using var stringWriter = new StringWriter();
            var emitResult = emitter.Emit(stringWriter);

            if (emitResult.Status == EmitStatus.Failed)
            {
                logger.LogInformation("Bicep compilation failed");
                var failResponse = req.CreateResponse(HttpStatusCode.BadRequest);
                await failResponse.WriteAsJsonAsync(emitResult.Diagnostics);
                return failResponse;
            }

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteString(stringWriter.ToString());
            return response;
        }
        catch (Exception exception)
        {
            logger.LogError($"Caught unhandled exception: {exception}");
            throw;
        }
    }
}