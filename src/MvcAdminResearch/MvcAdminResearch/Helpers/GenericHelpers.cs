using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Mvc.Html;
using MvcAdminResearch.Models;


namespace MvcAdminResearch.Helpers
{
    public static class GenericHelpers
    {
        public static LambdaExpression PropertyGetLambda(string parameterName, Type parameterType, string propertyName, Type propertyType)
        {
            var parameter = Expression.Parameter(parameterType, parameterName);
            var memberExpression = Expression.Property(parameter, propertyName);
            var lambdaExpression = Expression.Lambda(memberExpression, parameter);
            return lambdaExpression;
        }

    }
}