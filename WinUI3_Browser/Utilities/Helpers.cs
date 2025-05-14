using System;

namespace WinUI3_Browser.Utilities
{
    public static class URLHelpers
    {
        public static bool IsUrl(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return false;

            if (!input.Contains("://"))
            {
                input = "https://" + input;
            }

            return Uri.TryCreate(input, UriKind.Absolute, out Uri? uri) &&
                   (uri.Scheme == Uri.UriSchemeHttp ||
                    uri.Scheme == Uri.UriSchemeHttps ||
                    uri.Scheme == Uri.UriSchemeFtp);
        }

        public static string CreateGoogleSearchURL(string q)
        {
            return $"https://google.com/search?q={q}";
        }

        public static bool IsBlankUrl(string url)
        {
            return url.Equals("about:blank", StringComparison.OrdinalIgnoreCase);
        }
    }
}
