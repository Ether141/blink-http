namespace BlinkHttp.Handling
{
    internal static class StaticHtmlResources
    {
        internal const string ErrorPageTemplate = 
            """
            <!DOCTYPE html>
            <html lang="en">
            <head>
                <meta charset="UTF-8">
                <meta name="viewport" content="width=device-width, initial-scale=1.0">
                <title>404 Not Found</title>
                <style>
                    @import url('https://fonts.googleapis.com/css2?family=Inter:wght@400;700&display=swap');

                    * {
                        margin: 0;
                        padding: 0;
                        box-sizing: border-box;
                    }

                    body {
                        font-family: 'Inter', sans-serif;
                        background-color: #121212;
                        color: #f5f5f5;
                        display: flex;
                        justify-content: center;
                        align-items: center;
                        height: 100vh;
                        margin: 0;
                    }

                    .container {
                        text-align: center;
                        background-color: #1e1e1e;
                        padding: 60px 70px;
                        box-shadow: 0 4px 15px rgba(0, 0, 0, 0.4);
                    }

                    h1 {
                        font-size: 3rem;
                        font-weight: 700;
                        margin-bottom: 10px;
                    }

                    p {
                        font-size: 1rem;
                        color: #bdbdbd;
                    }
                </style>
            </head>
            <body>
                <div class="container">
                    <h1>{header}</h1>
                    <p>{description}</p>
                </div>
            </body>
            </html>
            """;

        internal static string GetErrorPageHtml(string header, string description) => ErrorPageTemplate.Replace("{header}", header).Replace("{description}", description);

        internal static string GetErrorPageNotFound() => GetErrorPageHtml("🔎 404 - Not Found", "The requested page could not be found on this server. It may have been moved or deleted.");

        internal static string GetErrorPageInternalError() => GetErrorPageHtml("🔴 500 - Internal Error", "Something went wrong on the server. Please try again later.");

        internal static string GetErrorPageUnauthorizedError() => GetErrorPageHtml("🔐 401 - Unauthorized", "You need to log in to access this page.");

        internal static string GetErrorPageForbiddenError() => GetErrorPageHtml("🔒 403 - Forbidden", "You don't have permission to access this page.");
    }
}
