using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManagementApplication.Models
{
    public class FilterView
    {
        public DateTime? SelectedStartDate { get; set; }
        public DateTime? SelectedEndDate { get; set; }

        public string[] SelectedNumbers { get; set; }
        public string[] SelectedProviders { get; set; }
        public string[] SelectedName { get; set; }
        public string[] SelectedUnit { get; set; }

        public List<SelectListItem> Orders { get; set; }
        public List<SelectListItem> Providers { get; set; }
        public List<SelectListItem> ItemNames { get; set; }
        public List<SelectListItem> ItemUnits { get; set; }

        public List<OrderProviderItem> OrderProviderItems { get; set; }

    }
}
