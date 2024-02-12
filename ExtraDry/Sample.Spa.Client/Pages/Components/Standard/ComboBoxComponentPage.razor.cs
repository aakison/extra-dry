using ExtraDry.Blazor;
using ExtraDry.Blazor.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Sample.Shared;

namespace Sample.Spa.Client.Pages.Components.Standard;

public partial class ComboBoxComponentPage : ComponentBase, IListItemViewModel<Sector> {

    public string Title(Sector item) => item.Title;

    public string Description(Sector item) => item.Description;

    private string Icon { get; set; } = "sectors";

    public string BasicPlaceholder { get; set; } = "filter...";

    public string AdvancedPlaceholder { get; set; } = "find...";

    private bool BasicSort { get; set; } = true;

    private bool AdvancedSort { get; set; } = true;

    private bool BasicGroup { get; set; }

    private Sector? BasicValue { get; set; }

    private Sector? AdvancedValue { get; set; }

    private List<Sector> Data { get; set; } = new();

    private SectorListService SectorService { get; set; } = new();

    private string MoreItemsTemplate { get; set; } = "plus {0} more...";

    private static string GroupName(Sector? item) => item?.Group ?? "unnamed";

    protected override void OnInitialized()
    {
        Data.Add(new Sector { Title = "Catering", Description = "Give people food.", Group = "Other" });
        Data.Add(new Sector { Title = "Courier", Description = "asdf", Group = "Other" });
        Data.Add(new Sector { Title = "Civil Engineer", Description = "asdf", Group = "Other" });
        Data.Add(new Sector { Title = "Insulation Installer", Description = "NULL", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "Commercial Manager", Description = "NULL", Group = "Other" });
        Data.Add(new Sector { Title = "Vehicle Service", Description = "NULL", Group = "Other" });
        Data.Add(new Sector { Title = "Tiler", Description = "desc", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "Plumber - Hydraulics", Description = "NULL", Group = "Plumbing" });
        Data.Add(new Sector { Title = "Gutter Cleaner", Description = "NULL", Group = "Cleaning" });
        Data.Add(new Sector { Title = "Mould Removalist", Description = "NULL", Group = "Cleaning" });
        Data.Add(new Sector { Title = "Plumber - General", Description = "NULL", Group = "Plumbing" });
        Data.Add(new Sector { Title = "Window Cleaner", Description = "NULL", Group = "Cleaning" });
        Data.Add(new Sector { Title = "Electrician", Description = "NULL", Group = "Electrical" });
        Data.Add(new Sector { Title = "WH&S Officer (OHS)", Description = "NULL", Group = "Other" });
        Data.Add(new Sector { Title = "Graffiti Removalist", Description = "NULL", Group = "Other" });
        Data.Add(new Sector { Title = "Surveyor", Description = "NULL", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "Landlord", Description = "NULL", Group = "Other" });
        Data.Add(new Sector { Title = "Geotechnical Engineer", Description = "NULL", Group = "Other" });
        Data.Add(new Sector { Title = "Waste Removal", Description = "NULL", Group = "Cleaning" });
        Data.Add(new Sector { Title = "Mechanical Engineer", Description = "NULL", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "IT Technician", Description = "NULL", Group = "Other" });
        Data.Add(new Sector { Title = "Landscaper", Description = "NULL", Group = "Gardening/Landscaping" });
        Data.Add(new Sector { Title = "Roof Tiler", Description = "NULL", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "Gardener", Description = "NULL", Group = "Gardening/Landscaping" });
        Data.Add(new Sector { Title = "Safety Officer", Description = "NULL", Group = "Fire, Safety & Security" });
        Data.Add(new Sector { Title = "Refrigeration Mechanic", Description = "NULL", Group = "HVAC" });
        Data.Add(new Sector { Title = "Site Manager", Description = "NULL", Group = "Other" });
        Data.Add(new Sector { Title = "Carpet Layer", Description = "NULL", Group = "Other" });
        Data.Add(new Sector { Title = "Concreter", Description = "NULL", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "Arborist - Tree Surgeon", Description = "NULL", Group = "Gardening/Landscaping" });
        Data.Add(new Sector { Title = "Garage Door Mechanic ", Description = "NULL", Group = "Electrical" });
        Data.Add(new Sector { Title = "Rubbish Removalist", Description = "NULL", Group = "Cleaning" });
        Data.Add(new Sector { Title = "Plumber - Roof", Description = "NULL", Group = "Plumbing" });
        Data.Add(new Sector { Title = "Security", Description = "NULL", Group = "Fire, Safety & Security" });
        Data.Add(new Sector { Title = "Plasterer", Description = "NULL", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "Builder", Description = "NULL", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "Auditor", Description = "NULL", Group = "Other" });
        Data.Add(new Sector { Title = "Quantity Surveyor", Description = "NULL", Group = "Other" });
        Data.Add(new Sector { Title = "Boiler Maker", Description = "NULL", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "Demolition Service", Description = "NULL", Group = "Other" });
        Data.Add(new Sector { Title = "Cleaner", Description = "NULL", Group = "Cleaning" });
        Data.Add(new Sector { Title = "Fitter & Turner", Description = "NULL", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "Scaffolder", Description = "NULL", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "Handyman", Description = "NULL", Group = "Other" });
        Data.Add(new Sector { Title = "Estimator", Description = "NULL", Group = "Other" });
        Data.Add(new Sector { Title = "Restumper", Description = "NULL", Group = "Other" });
        Data.Add(new Sector { Title = "Field Services Technician", Description = "NULL", Group = "Other" });
        Data.Add(new Sector { Title = "Engineer", Description = "NULL", Group = "Other" });
        Data.Add(new Sector { Title = "Painter", Description = "NULL", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "Tenant", Description = "NULL", Group = "Other" });
        Data.Add(new Sector { Title = "Glazier", Description = "NULL", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "Draughting Services", Description = "NULL", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "Insurance Assesor", Description = "NULL", Group = "Other" });
        Data.Add(new Sector { Title = "Renderer", Description = "NULL", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "Interior Designer", Description = "NULL", Group = "Other" });
        Data.Add(new Sector { Title = "Asbestos Removal", Description = "NULL", Group = "Cleaning" });
        Data.Add(new Sector { Title = "Pest Control", Description = "NULL", Group = "Other" });
        Data.Add(new Sector { Title = "Antenna Installer", Description = "NULL", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "Concierge", Description = "NULL", Group = "Other" });
        Data.Add(new Sector { Title = "Carpenter - Cabinet Maker", Description = "NULL", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "Architect", Description = "NULL", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "Carpenter - General", Description = "NULL", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "Airconditioning & HVAC ", Description = "NULL", Group = "HVAC" });
        Data.Add(new Sector { Title = "Property Valuer", Description = "NULL", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "Project Manager", Description = "NULL", Group = "Other" });
        Data.Add(new Sector { Title = "Managing Agent", Description = "NULL", Group = "Other" });
        Data.Add(new Sector { Title = "Bricklayer", Description = "NULL", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "Auto Mechanic", Description = "NULL", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "Paver", Description = "NULL", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "Building Manager", Description = "NULL", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "Auto Electrician", Description = "NULL", Group = "Electrical" });
        Data.Add(new Sector { Title = "Fencing", Description = "NULL", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "Builder - Bricklayer", Description = "NULL", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "Builder - Fencer", Description = "NULL", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "Builder - Plasterer", Description = "NULL", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "Carpenter (inc. Joiner & Cabinet Maker) - There's always one that thinks it's too good to be limited to the maximum character rules.", Description = "NULL", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "Finishing & Covering", Description = "NULL", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "Fitter and Welder", Description = "NULL", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "Flooring - General Repairs", Description = "NULL", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "Flooring - Tiler", Description = "NULL", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "Glazier - Showers", Description = "NULL", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "Glazier - Windows", Description = "NULL", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "Locksmith", Description = "NULL", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "Roofing", Description = "NULL", Group = "Building & Construction" });
        Data.Add(new Sector { Title = "Cleaner - Carpets", Description = "NULL", Group = "Cleaning" });
        Data.Add(new Sector { Title = "Cleaner - Drains and Gutter", Description = "NULL", Group = "Cleaning" });
        Data.Add(new Sector { Title = "Cleaner - General", Description = "NULL", Group = "Cleaning" });
        Data.Add(new Sector { Title = "Electrician - Doors", Description = "NULL", Group = "Electrical" });
        Data.Add(new Sector { Title = "Electrician - Equipment", Description = "NULL", Group = "Electrical" });
        Data.Add(new Sector { Title = "Electrician - General", Description = "NULL", Group = "Electrical" });
        Data.Add(new Sector { Title = "Electrician - Lift mechanic", Description = "NULL", Group = "Electrical" });
        Data.Add(new Sector { Title = "Electrician - Test & Tag", Description = "NULL", Group = "Electrical" });
        Data.Add(new Sector { Title = "Fire Services - Alarm Testing & Monitoring", Description = "NULL", Group = "Fire, Safety & Security" });
        Data.Add(new Sector { Title = "Fire Services - Fire Protection Equipment Technici", Description = "NULL", Group = "Fire, Safety & Security" });
        Data.Add(new Sector { Title = "Fire Services - Training", Description = "NULL", Group = "Fire, Safety & Security" });
        Data.Add(new Sector { Title = "Fire Services - Wet and Dry", Description = "NULL", Group = "Fire, Safety & Security" });
        Data.Add(new Sector { Title = "Security - Access", Description = "NULL", Group = "Fire, Safety & Security" });
        Data.Add(new Sector { Title = "Security - Patrols & Monitoring", Description = "NULL", Group = "Fire, Safety & Security" });
        Data.Add(new Sector { Title = "Gardener - Arborist", Description = "NULL", Group = "Gardening/Landscaping" });
        Data.Add(new Sector { Title = "Gardener - General", Description = "NULL", Group = "Gardening/Landscaping" });
        Data.Add(new Sector { Title = "Gardener - Landcaping", Description = "NULL", Group = "Gardening/Landscaping" });
        Data.Add(new Sector { Title = "Airconditioning & Mechanical Services", Description = "NULL", Group = "HVAC" });
        Data.Add(new Sector { Title = "Airconditioning & Refrigeration mechanic", Description = "NULL", Group = "HVAC" });
        Data.Add(new Sector { Title = "Handyperson", Description = "NULL", Group = "Other" });
        Data.Add(new Sector { Title = "I.T -  Technician", Description = "NULL", Group = "Other" });
        Data.Add(new Sector { Title = "I.T - Telephone", Description = "NULL", Group = "Other" });
        Data.Add(new Sector { Title = "Pest Control - Technician", Description = "NULL", Group = "Other" });
        Data.Add(new Sector { Title = "Removalist - Asbestos", Description = "NULL", Group = "Other" });
        Data.Add(new Sector { Title = "Removalist - Mould", Description = "NULL", Group = "Other" });
        Data.Add(new Sector { Title = "Removalist - Waste", Description = "NULL", Group = "Other" });
        Data.Add(new Sector { Title = "Plumber - Drainer/Drainlayer", Description = "NULL", Group = "Plumbing" });
        Data.Add(new Sector { Title = "Plumber - Gas", Description = "NULL", Group = "Plumbing" });
        Data.Add(new Sector { Title = "Plumber - HVAC", Description = "NULL", Group = "Plumbing" });
        Data.Add(new Sector { Title = "Caulker", Description = "", Group = "Plumbing" });

        SectorService.Initialize(Data);
    }

    /// <summary>
    /// Provides list of sectors as a service with a max size of 20 and simulating a slow connection.
    /// </summary>
    public class SectorListService : IListService<Sector> {

        public void Initialize(IEnumerable<Sector> sectors)
        {
            Sectors = sectors.ToList();
        }

        public string UriTemplate { get; set; } = string.Empty;

        public object[] UriArguments { get; set; } = null!;

        public int PageSize { get; set; } = 20;

        public int MaxLevel { get; set; }

        private List<Sector> Sectors { get; set; } = new();

        public async ValueTask<ItemsProviderResult<Sector>> GetItemsAsync(CancellationToken cancellationToken = default)
        {
            var query = Sectors.Take(PageSize);
            var result = new ItemsProviderResult<Sector>(query, Sectors.Count);
            cancellationToken.ThrowIfCancellationRequested();
            await Task.Delay(500, cancellationToken);
            return result;
        }

        public ValueTask<ItemsProviderResult<Sector>> GetItemsAsync(Query query, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<ListItemsProviderResult<Sector>> GetListItemsAsync(Query query, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
