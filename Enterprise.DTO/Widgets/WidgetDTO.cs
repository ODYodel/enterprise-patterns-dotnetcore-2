using System;
using System.Collections.Generic;
using System.Text;
using Enterprise.Models;
using Enterprise.Models.Enumerations;
using Microsoft.AspNetCore.Http;

namespace Enterprise.DTO
{
    public class WidgetDTO : BaseDbModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public WidgetTypes WidgetTypes { get; set; }
        public WidgetProvidersDTO WidgetProviders { get; set; }
        public WidgetResourcesDTO WidgetResources { get; set; }
        public IFormFile FeatureImage { get; set; }
    }
}
