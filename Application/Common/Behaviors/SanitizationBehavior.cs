using MediatR;
using System.Reflection;
using Application.Common.Attributes;
using Ganss.Xss;

public class SanitizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly HtmlSanitizer _sanitizer;

    public SanitizationBehavior()
    {
        _sanitizer = new HtmlSanitizer();
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var propsToSanitize = typeof(TRequest).GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead &&
                        p.CanWrite &&
                        p.PropertyType == typeof(string) &&
                        Attribute.IsDefined(p, typeof(SanitizeAttribute)));

        foreach (var prop in propsToSanitize)
        {
            var original = (string?)prop.GetValue(request);
            if (!string.IsNullOrWhiteSpace(original))
            {
                var sanitized = _sanitizer.Sanitize(original);
                prop.SetValue(request, sanitized);
            }
        }

        return await next();
    }
}
