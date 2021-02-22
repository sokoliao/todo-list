using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Service.Extensions
{
    public static class HttpResponseExtensions
    {
        public async static Task WriteAsJsonAsync<TDto>(this HttpResponse response, TDto data)
        {
            var options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;
            options.Converters.Add(new JsonStringEnumConverter());
            await response.WriteAsync(JsonSerializer.Serialize(data, options));
        }
    }
}
