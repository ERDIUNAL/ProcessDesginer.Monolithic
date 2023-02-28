using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Crea.Core.CrossCuttingConcerns.Exceptions.HttpProblemDetails;

internal static class ProblemDetailsExtensions
{
    public static string AsJson(this ProblemDetails details)
    {
        return JsonConvert.SerializeObject(details);
    }
}
