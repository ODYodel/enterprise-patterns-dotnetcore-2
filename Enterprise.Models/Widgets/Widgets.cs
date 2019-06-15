using System;
using System.Collections.Generic;
using System.Text;
using Enterprise.Models.Enumerations;

namespace Enterprise.Models
{
    public class Widgets
        : BaseDbModel
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public WidgetTypes WidgetTypes { get; set; }
        public WidgetProviders WidgetProviders { get; set; }
        public WidgetResources WidgetResources { get; set; }

    }
}
