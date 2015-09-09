using Demo.Models;
using Demo.Utils;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData.Batch;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Formatter;

namespace Demo
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var odataFormatters = ODataMediaTypeFormatters.Create();
            config.Formatters.InsertRange(0, odataFormatters);

            config.MapODataServiceRoute("odata", null, GetEdmModel(), new DefaultODataBatchHandler(GlobalConfiguration.DefaultServer));
            config.EnsureInitialized();

            Logger.Instance.Info("Service started");
        }
        private static IEdmModel GetEdmModel()
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
            builder.Namespace = "Demos";
            builder.ContainerName = "DefaultContainer";
            builder.EntitySet<Person>("People");
            builder.EntitySet<Trip>("Trips");
            var edmModel = builder.GetEdmModel();
            return edmModel;
        }
    }
}
