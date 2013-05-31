using System;
namespace MvcAdminResearch.Helpers
{
    public interface IModelContextHost
    {
        Type ContextType { get; set; }
        string ControllerName { get; set; }
        string ControllerRootName { get; set; }
        string EntitySetName { get; set; }
        
        System.Collections.Generic.List<ModelProperty> ModelProperties { get; set; }
        Type ModelType { get; set; }
        string ModelTypeName { get; set; }
        string Namespace { get; set; }
        PrimaryKey PrimaryKeys { get; set; }
        
        System.Collections.Generic.Dictionary<string, RelatedModel> RelatedProperties { get; set; }
    }
}
