using System.Reflection;

namespace ExtraDry.Swashbuckle;

/// <summary>
/// Document filter that applies to OpenApiDocuments (swagger) to make the DisplayAttribute work
/// when it is applied to a ApiController.  Included when AddExtraDry() used on Swagger options.
/// </summary>
public class DisplayControllerDocumentFilter : IDocumentFilter {

    public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
    {
        LoadDisplayAttributes();
        LoadOriginalPositions(swaggerDoc);

        swaggerDoc.Tags = swaggerDoc.Tags.OrderBy(e => TagOrder(e.Name)).ToList();

        RenameTags(swaggerDoc);
    }

    private void RenameTags(OpenApiDocument swaggerDoc)
    {
        foreach(var attribute in displayAttributes) {
            var newName = attribute.Value.Name;
            if(!string.IsNullOrWhiteSpace(newName)) {
                RenameTag(swaggerDoc, attribute.Key, newName);
            }
        }
    }

    private static void RenameTag(OpenApiDocument swaggerDoc, string oldName, string newName) { 
        foreach(var tag in swaggerDoc.Tags) {
            if(tag.Name == oldName) {
                tag.Name = newName;
            }
        }
        foreach(var path in swaggerDoc.Paths.Values) {
            foreach(var op in path.Operations.Values) {
                foreach(var tag in op.Tags) {
                    if(tag.Name == oldName) {
                        tag.Name = newName;
                    }
                }
            }
        }
    }

    private void LoadOriginalPositions(OpenApiDocument swaggerDoc)
    {
        originalPositions.Clear();
        for(int i = 0; i < swaggerDoc.Tags.Count; i++) {
            originalPositions.Add(swaggerDoc.Tags[i].Name, i);
        }
    }

    private int TagOrder(string name)
    {
        displayAttributes.TryGetValue(name, out var display);
        return display?.GetOrder() ?? 10000 + originalPositions[name];
    }

    private void LoadDisplayAttributes()
    {
        displayAttributes.Clear();
        foreach(var assembly in AppDomain.CurrentDomain.GetAssemblies()) {
            if(assembly.IsDynamic) {
                continue;
            }
            foreach(var type in assembly.ExportedTypes) {
                var controller = type.GetCustomAttribute<ApiControllerAttribute>();
                if(controller != null) {
                    var display = type.GetCustomAttribute<DisplayAttribute>();
                    if(display != null) {
                        var name = type.Name.Replace("Controller", "");
                        displayAttributes.Add(name, display);
                    }
                }
            }
        }
    }

    private readonly Dictionary<string, int> originalPositions = new();

    private readonly Dictionary<string, DisplayAttribute> displayAttributes = new();

}

