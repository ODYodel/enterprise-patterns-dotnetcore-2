namespace Enterprise.Models
{
    public class WidgetProviders
        : BaseDbModel
    {
        public string OrganizationName { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }


    }
}