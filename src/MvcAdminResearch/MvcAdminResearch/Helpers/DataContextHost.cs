using MvcAdminResearch.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Design.PluralizationServices;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Web;

namespace MvcAdminResearch.Helpers
{
    public class ModelDbContextHost : MvcAdminResearch.Helpers.IModelContextHost
    {
        //        ContextType	System.Type	The type of the data context.
        public Type ContextType { get; set; }
        //ControllerName	String	The name of the controller class that will be generated.
        public string ControllerName { get; set; }
        //ControllerRootName	String	The name of the controller class excluding the Controller part at the end of the name.
        public string ControllerRootName { get; set; }
        //EntitySetName	String	Name of the property on the data context class containing the set of entities.
        public string EntitySetName { get; set; }
        //ModelType	System.Type	The type of the model class specified in the Add Controller dialog.
        public string ModelTypeName { get; set; }
        public Type ModelType { get; set; }
        //Namespace	String	Namespace that will be used for the generated controller class.
        public string Namespace { get; set; }
        //PrimaryKeys	 PrimaryKey[] 
        public PrimaryKey PrimaryKeys { get; set; }

        public Dictionary<string, RelatedModel> RelatedProperties { get; set; }
        public List<ModelProperty> ModelProperties { get; set; }

        public ModelDbContextHost(Type modelType, Type contextType)
        {
            this.ContextType = contextType;

            var modelTypeNoProxy = ObjectContext.GetObjectType(modelType);//returns poco type if proxy(EF creates proxy for entity models)
            this.ModelType = modelTypeNoProxy;
            this.ModelTypeName = modelTypeNoProxy.Name;

            this.EntitySetName = contextType.GetModelCollectionPropertiesDbContext().Where(p => p.PropertyType.GetGenericArguments()[0].Name == modelTypeNoProxy.Name).FirstOrDefault().Name;

            this.RelatedProperties = contextType.GetRelatedProperties(modelTypeNoProxy);
            this.ModelProperties = ScaffoldHelpers.GetEligibleProperties(modelTypeNoProxy, this);
        }

    }

    public class PrimaryKey
    {
        public string Name { get; set; }
        public string ShortTypeName { get; set; }
        public Type Type { get; set; }

    }

    public class RelatedModel
    {
        public string DisplayPropertyName { get; set; }
        public string EntitySetName { get; set; }
        public string ForeignKeyPropertyName { get; set; }
        public string PrimaryKey { get; set; }
        public string PropertyName { get; set; }
        public string TypeName { get; set; }
        public Type Type { get; set; }
    }

    public class ModelProperty
    {
        public string Name { get; set; }
        public string AssociationName { get; set; }
        public string ValueExpression { get; set; }
        public string ModelValueExpression { get; set; }
        public string ItemValueExpression { get; set; }
        public Type UnderlyingType { get; set; }
        public bool IsPrimaryKey { get; set; }
        public bool IsForeignKey { get; set; }
        public bool IsReadOnly { get; set; }
        public bool Scaffold { get; set; }
    }

    public static class ScaffoldHelpers
    {

        // Change this list to include any non-primitive types you think should be eligible for display/edit
        public static Type[] BindableNonPrimitiveTypes = new[] {
            typeof(string),
            typeof(decimal),
            typeof(Guid),
            typeof(DateTime),
            typeof(DateTimeOffset),
            typeof(TimeSpan),
        };

        public static string[] DisplayPropertyNames = new[]{
            "Name",
            "Subject",
            "LastName",
            "FirstName",
            "Description",            
        };

        public static string[] GetDisplayPropertyNames(Type type, int count)
        {

            var properties = type.GetProperties().Where(p => BindableNonPrimitiveTypes.Contains(p.PropertyType)).Select(p => p.Name).ToList();
            if (properties.Count < count)
            {
                count = properties.Count;
            }

            List<string> selectedProperties = new List<string>();
            int currCnt = 0;
            foreach (var propName in DisplayPropertyNames)
            {
                if (!properties.Contains(propName))
                {
                    continue;
                }

                selectedProperties.Add(propName);
                properties.Remove(propName);
                currCnt++;
                if (currCnt >= count || properties.Count == 0)
                {
                    break;
                }

            }

            if (currCnt < count && properties.Count > 0)
            {
                var restProps = new List<string>();
                foreach (var propName in properties)
                {
                    restProps.Add(propName);
                }

                foreach (var propName in restProps)
                {
                    selectedProperties.Add(propName);
                    properties.Remove(propName);
                    currCnt++;
                    if (currCnt >= count || properties.Count == 0)
                    {
                        break;
                    }
                }
            }

            return selectedProperties.ToArray();
        }

        public static Dictionary<string, RelatedModel> GetRelatedProperties(this Type dbContext, Type modelType)
        {
            var dict = new Dictionary<string, RelatedModel>();

            foreach (var prop in modelType.GetProperties())
            {
                if (ScaffoldHelpers.IsForeignKey(prop, dbContext.GetModelCollectionTypesDbContext().ToArray()))
                {
                    var relatedModel = new RelatedModel();
                    relatedModel.PropertyName = prop.Name;
                    relatedModel.ForeignKeyPropertyName = prop.Name;

                    var getRelatedType = ScaffoldHelpers.GetAssociatedType(prop, dbContext.GetModelCollectionTypesDbContext().ToArray());
                    relatedModel.DisplayPropertyName = GetDisplayPropertyNames(getRelatedType, 1)[0];

                    relatedModel.Type = getRelatedType;

                    //check if property is child collection
                    relatedModel.PrimaryKey = getRelatedType.GetProperties().Where(p => ScaffoldHelpers.IsPrimaryKey(p)).FirstOrDefault().Name;
                    relatedModel.EntitySetName = dbContext.GetProperties()
                                                            .Where(t => t.PropertyType.IsGenericType
                                                                      && t.PropertyType.GetGenericTypeDefinition().Equals(typeof(DbSet<>))
                                                                      && t.PropertyType.GetGenericArguments()[0].Name == getRelatedType.Name)
                                                            .FirstOrDefault()
                                                            .Name;

                    dict.Add(prop.Name, relatedModel);
                }
            }

            return dict;
        }

        public static Dictionary<string, RelatedModel> GetModelParentNavigationProperties(this Type dbContext, Type modelType)
        {
            var dict = new Dictionary<string, RelatedModel>();

            foreach (var prop in modelType.GetProperties())
            {
                var relatedModel = new RelatedModel();
                relatedModel.PropertyName = prop.Name;
                relatedModel.DisplayPropertyName = prop.Name;

                var modelCollectionTypes = dbContext == typeof(DbContext) ? dbContext.GetModelCollectionTypesDbContext() : null;
                //check if property is child collection
                if (modelCollectionTypes.Contains(prop.PropertyType))
                {
                    relatedModel.PrimaryKey = prop.PropertyType.GetProperties().Where(p => ScaffoldHelpers.IsPrimaryKey(p)).FirstOrDefault().Name;
                    relatedModel.EntitySetName = dbContext.GetProperties()
                                                        .Where(t => t.PropertyType.IsGenericType
                                                                  && t.PropertyType.GetGenericTypeDefinition().Equals(typeof(DbSet<>)))
                                                        .FirstOrDefault()
                                                        .Name;

                    dict.Add(prop.Name, relatedModel);
                }
            }

            return dict;
        }

        public static Dictionary<string, RelatedModel> GetModelChildNavigationProperties(this Type dbContext, Type modelType)
        {
            var dict = new Dictionary<string, RelatedModel>();

            foreach (var prop in modelType.GetProperties())
            {
                var relatedModel = new RelatedModel();
                relatedModel.PropertyName = prop.Name;
                relatedModel.DisplayPropertyName = prop.Name;

                var modelCollectionTypes = dbContext == typeof(DbContext) ? dbContext.GetModelCollectionTypesDbContext() : null;
                //check if property is child collection
                if ((prop.PropertyType.IsGenericType)
                    && (prop.PropertyType.GetGenericTypeDefinition().Equals(typeof(IList<>))
                        || prop.PropertyType.GetGenericTypeDefinition().Equals(typeof(ICollection<>))
                        || prop.PropertyType.GetGenericTypeDefinition().Equals(typeof(IEnumerable<>))
                        || prop.PropertyType.GetGenericTypeDefinition().Equals(typeof(IQueryable<>)))
                    )
                {
                    if (dbContext == typeof(DbContext))
                    {
                        var propEntityType = prop.PropertyType.GetGenericArguments()[0];
                        var dbSetProp = dbContext.GetProperties()
                                                .Where(t => t.PropertyType.IsGenericType
                                                         && t.PropertyType.GetGenericTypeDefinition().Equals(typeof(DbSet<>))
                                                         && t.PropertyType.GetGenericArguments().Contains(propEntityType))
                                                .FirstOrDefault();

                        relatedModel.EntitySetName = dbSetProp.Name;
                        relatedModel.PrimaryKey = dbSetProp.PropertyType.GetGenericArguments()[0]
                                                        .GetProperties()
                                                        .Where(p => ScaffoldHelpers.IsPrimaryKey(p))
                                                        .FirstOrDefault()
                                                        .Name;


                        dict.Add(prop.Name, relatedModel);
                    }
                    //else//TODO: Check if dbContext is ObjectContext
                    //    if (dbContext is ObjectContext)
                    //    {
                    //    }
                }

            }

            return dict;
        }


        public static string Plularize(string word)
        {
            PluralizationService pluralizationService;

            var culture = Thread.CurrentThread.CurrentCulture.Name;
            try
            {
                pluralizationService = PluralizationService.CreateService(new CultureInfo(culture));
            }
            catch (NotImplementedException)
            {
                // Unsupported culture
                Debug.WriteLine(string.Format("Cannot pluralize '{0}' because culture '{1}' is not yet supported by System.Data.Entity.Design.PluralizationServices. Leaving the word unpluralized.", word, culture));
                return null;
            }

            return pluralizationService.Pluralize(word);
        }

        /// <summary>
        /// Get models from DbContext type
        /// </summary>
        /// <param name="dbContextType">DbContext type</param>
        /// <returns></returns>
        public static List<Type> GetModelCollectionTypesDbContext(this Type dbContextType)
        {
            var dbSetTypes = dbContextType.GetProperties()
                                      .Where(t => t.PropertyType.IsGenericType
                                               && t.PropertyType.GetGenericTypeDefinition().Equals(typeof(DbSet<>)))
                                      .Select(p => p.PropertyType.GetGenericArguments()[0]);
            return dbSetTypes.ToList();
        }

        public static List<PropertyInfo> GetModelCollectionPropertiesDbContext(this Type dbContextType)
        {
            var dbSetTypes = dbContextType.GetProperties()
                                      .Where(t => t.PropertyType.IsGenericType
                                               && t.PropertyType.GetGenericTypeDefinition().Equals(typeof(DbSet<>)))
                                      ;
            return dbSetTypes.ToList();
        }

        // Call this to determine if property has scaffolding enabled
        public static bool Scaffold(PropertyInfo property)
        {
            foreach (object attribute in property.GetCustomAttributes(true))
            {
                var scaffoldColumn = attribute as ScaffoldColumnAttribute;
                if (scaffoldColumn != null && !scaffoldColumn.Scaffold)
                {
                    return false;
                }
            }
            return true;
        }

        // Call this to determine if the property represents a primary key. Change the
        // code to change the definition of primary key.
        public static bool IsPrimaryKey(PropertyInfo property)
        {
            if (string.Equals(property.Name, "id", StringComparison.OrdinalIgnoreCase))
            {  // EF Code First convention
                return true;
            }

            if (string.Equals(property.Name, property.DeclaringType.Name + "id", StringComparison.OrdinalIgnoreCase))
            {  // EF Code First convention
                return true;
            }

            foreach (object attribute in property.GetCustomAttributes(true))
            {
                if (attribute is KeyAttribute)
                {  // WCF RIA Services and EF Code First explicit
                    return true;
                }

                var edmScalar = attribute as EdmScalarPropertyAttribute;
                if (edmScalar != null && edmScalar.EntityKeyProperty)
                {  // EF traditional
                    return true;
                }

                //var column = attribute as ColumnAttribute;
                //if (column != null && column.IsPrimaryKey)
                //{  // LINQ to SQL
                //    return true;
                //}
            }

            return false;
        }

        // Call this to determine if the property represents a primary key. Change the
        // code to change the definition of primary key.
        public static bool IsForeignKey(PropertyInfo property, Type[] dbContextModels)
        {
            string idSuffix = "id";
            if (property.Name.EndsWith(idSuffix, StringComparison.OrdinalIgnoreCase)
                && dbContextModels.Count(t => t.Name.Equals(property.Name.Substring(0, property.Name.Length - idSuffix.Length), StringComparison.OrdinalIgnoreCase)) > 0)
            {
                return true;
            }

            //TODO: handle ForeignKeyAttribute check
            //foreach (object attribute in property.GetCustomAttributes(true))
            //{
            //    if (attribute is ForeignKeyAttribute)
            //    {  // WCF RIA Services and EF Code First explicit
            //        return true;
            //    }
            //}

            return false;
        }

        public static Type GetAssociatedType(PropertyInfo property, Type[] dbContextModels)
        {
            string idSuffix = "id";
            if (property.Name.EndsWith(idSuffix, StringComparison.OrdinalIgnoreCase)
                )
            {
                string possibleModelType = property.Name.Substring(0, property.Name.Length - idSuffix.Length);
                var modelType = dbContextModels.FirstOrDefault(t => t.Name.Equals(possibleModelType, StringComparison.OrdinalIgnoreCase));
                if (modelType != null)
                {
                    return modelType;
                }
            }

            //TODO: handle ForeignKeyAttribute check
            //foreach (CustomAttributeData attribute in property.GetCustomAttributesData())
            //{
            //    if (attribute.AttributeType == typeof(ForeignKeyAttribute))
            //    {  // WCF RIA Services and EF Code First explicit
            //        return attribute.;
            //    }
            //}

            return null;
        }

        // This will return the primary key property name, if and only if there is exactly
        // one primary key. Returns null if there is no PK, or the PK is composite.
        public static string GetPrimaryKeyName(Type type, IModelContextHost host)
        {
            IEnumerable<string> pkNames = GetPrimaryKeyNames(type, host);
            return pkNames.Count() == 1 ? pkNames.First() : null;
        }

        // This will return all the primary key names. Will return an empty list if there are none.
        public static IEnumerable<string> GetPrimaryKeyNames(Type type, IModelContextHost host)
        {
            return GetEligibleProperties(type, host).Where(mp => mp.IsPrimaryKey).Select(mp => mp.Name);
        }

        // Call this to determine if the property represents a foreign key.
        //public static bool IsForeignKey(PropertyInfo property, DataContextHost host)
        //{
        //    return host.RelatedProperties.ContainsKey(property.Name);
        //}

        // A foreign key, e.g. CategoryID, will have a value expression of Category.CategoryID
        public static string GetValueExpressionSuffix(PropertyInfo property, IModelContextHost host)
        {
            RelatedModel propertyModel;
            host.RelatedProperties.TryGetValue(property.Name, out propertyModel);

            return propertyModel != null ? propertyModel.PropertyName + "." + propertyModel.DisplayPropertyName : property.Name;
        }

        // A foreign key, e.g. CategoryID, will have an association name of Category
        public static string GetAssociationName(PropertyInfo property, IModelContextHost host)
        {
            RelatedModel propertyModel;
            host.RelatedProperties.TryGetValue(property.Name, out propertyModel);

            return propertyModel != null ? propertyModel.PropertyName : property.Name;
        }

        // Helper
        public static List<ModelProperty> GetEligibleProperties(Type type, IModelContextHost host)
        {
            List<ModelProperty> results = new List<ModelProperty>();

            foreach (PropertyInfo prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                Type underlyingType = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                if (prop.GetGetMethod() != null && prop.GetIndexParameters().Length == 0 && IsBindableType(underlyingType))
                {
                    string valueExpression = GetValueExpressionSuffix(prop, host);

                    results.Add(new ModelProperty
                    {
                        Name = prop.Name,
                        AssociationName = GetAssociationName(prop, host),
                        ValueExpression = valueExpression,
                        ModelValueExpression = "Model." + valueExpression,
                        ItemValueExpression = "item." + valueExpression,
                        UnderlyingType = underlyingType,
                        IsPrimaryKey = IsPrimaryKey(prop),
                        IsForeignKey = IsForeignKey(prop, host.ContextType.GetModelCollectionTypesDbContext().ToArray()),
                        IsReadOnly = prop.GetSetMethod() == null,
                        Scaffold = Scaffold(prop)
                    });
                }
            }

            return results;
        }

        // Helper
        public static bool IsBindableType(Type type)
        {
            return type.IsPrimitive || BindableNonPrimitiveTypes.Contains(type);
        }
    }
}