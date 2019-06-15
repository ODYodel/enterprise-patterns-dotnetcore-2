using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Enterprise.Models.Enumerations
{
    public enum WidgetTypes
    {
        [Description("Toy")]
        Toy = 1,
        [Description("Electronic")]
        Electronic = 2,
        [Description("HomeGood")]
        Homegood = 3
    }
}
