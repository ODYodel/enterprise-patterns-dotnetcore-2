using System;
using System.Collections.Generic;
using System.Text;
using Enterprise.DTO;
using Enterprise.Models;
using Enterprise.Models.Enumerations;

namespace Enterprise.DTO
{
    public class WidgetGridDTO
        : BaseDbModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int? WidgetProviderId { get; set; }
        public string WidgetTypes { get; set; }
        public string FeatureImagePath { get; set; }

    }
}
