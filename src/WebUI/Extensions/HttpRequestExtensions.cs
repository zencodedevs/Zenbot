namespace ZenAchitecture.WebUI.Extensions
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Globalization;
    using System.Linq;

    public static class HttpRequestExtensions
    {
  
        public static string GetSysLanguage(this HttpRequest request)
        {
            request.Headers.TryGetValue("x-sys-language", out var _params);

            if (_params.Any())
            {
                try
                {
                    CultureInfo.GetCultureInfo(_params.First().Trim());
                }
                catch (CultureNotFoundException)
                {
                    return null;
                }
                return _params.First().Trim();
            }

            return null;
        }

 
        public static Guid? GetSysTenant(this HttpRequest request)
        {
            request.Headers.TryGetValue("x-tenant-id", out var _params);

            if (_params.Any())
            {
                Guid.TryParse(_params.First().Trim(), out Guid _guid);
                return _guid != Guid.Empty ? _guid : null;
            }

            return null;
        }



    }
}
