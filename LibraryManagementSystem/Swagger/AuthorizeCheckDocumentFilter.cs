using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace LibraryManagement.Swagger;

public class AuthorizeCheckDocumentFilter : IDocumentFilter
{
    private readonly string _scope;

    public AuthorizeCheckDocumentFilter(string scope)
    {
        _scope=scope;
    }

    public void Apply(OpenApiDocument document, DocumentFilterContext context)
    {
        foreach (var apiDescription in context.ApiDescriptions)
        {
            var hasAthorize= apiDescription.ActionDescriptor.EndpointMetadata
            .OfType<AuthorizeAttribute>()
            .Any();

            if(!hasAthorize) continue;
            var path= "/"+apiDescription.RelativePath!.TrimEnd('?');
            var method= apiDescription.HttpMethod!.ToLower();

            if(document.Paths.TryGetValue(path, out var pathItem))
            {
                var operation = method switch
                {
                    "get"=> pathItem.Operations?.GetValueOrDefault(HttpMethod.Get),
                    "post"=> pathItem.Operations?.GetValueOrDefault(HttpMethod.Post),
                    "put"=> pathItem.Operations?.GetValueOrDefault(HttpMethod.Put),
                    "delete"=>pathItem.Operations?.GetValueOrDefault(HttpMethod.Delete),
                    _=> null
                };
                if (operation != null)
                {
                   operation.Security = new List<OpenApiSecurityRequirement>
                    {
                        new OpenApiSecurityRequirement
                        {{new OpenApiSecuritySchemeReference("oauth2", document), new List<string> { _scope }}}
                    };
                }
            }
        }
    }
}