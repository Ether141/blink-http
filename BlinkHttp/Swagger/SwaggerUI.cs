using BlinkHttp.Routing;

namespace BlinkHttp.Swagger;

internal class SwaggerUI
{
    private readonly string title;
    private readonly string version;
    private readonly string url;

    private readonly OpenApiGenerator generator;

    internal const string HtmlTemplate = """
        <!DOCTYPE html>
        <html>
        <head>
          <title>Swagger UI</title>
          <link rel="stylesheet" type="text/css" href="https://unpkg.com/swagger-ui-dist/swagger-ui.css" />
        </head>
        <body>
        <div id="swagger-ui"></div>
        <script src="https://unpkg.com/swagger-ui-dist/swagger-ui-bundle.js"></script>
        <script>
        const ui = SwaggerUIBundle({
            url: '/{url}',
            dom_id: '#swagger-ui'
        });
        </script>
        </body>
        </html>
        """;

    internal SwaggerUI(string url, string title, string version, EndpointMetadata[] metadata)
    {
        this.url = url;
        this.title = title;
        this.version = version;

        generator = new OpenApiGenerator(title, version, metadata);
    }

    internal string GetHtml() => HtmlTemplate.Replace("{url}", url);

    internal string? GetJson() => generator.Json;

    internal void GenerateJson(IReadOnlyCollection<IRoutesCollection> routes) => generator.GenerateJson(routes);
}
