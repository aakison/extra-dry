using CsvHelper;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Sample.Spa.Backend.SampleData;

public partial class SampleDataService {

    public SampleDataService(SampleContext sampleContext, RegionService regionService)
    {
        database = sampleContext;
        regions = regionService;
    }

    public void PopulateServices()
    {
        database.Sectors.Add(new Sector { 
            State = SectorState.Active,  
            Title = "Standard Electrical Services",
            Description = "Provide licensed electrical works for commercial and residential buildings",
        });
        database.Sectors.Add(new Sector {
            State = SectorState.Active,
            Title = "Standard Plumbing Services",
            Description = "Provide licensed plumbing services for commercial and residential buildings",
        });
        database.Sectors.Add(new Sector {
            State = SectorState.Active,
            Title = "Cleaners",
            Description = "Provide general cleaning services",
        });
        database.Sectors.Add(new Sector {
            State = SectorState.Inactive,
            Title = "Fax Machine Repair",
            Description = "Provides routine maintenance and consulting services on getting the most from the latest in high-tech gear",
        });
        database.SaveChanges();
    }

    public void PopulateTemplates()
    {
        database.Templates.Add(
            new Template {
                Uuid = PseudoRandomGuid(),
                Schema = new ExpandoSchema {
                    TargetType = "Company",
                    Fields = { 
                        new ExpandoField { 
                            DataType = ExpandoDataType.Text, 
                            Label = "Stock Code", 
                            Slug = "asx_code",  
                            IsRequired = false, 
                            MaxLength = 10, 
                            State = ExpandoState.Active 
                        },
                    }
                }
            });
    }

    public void PopulateCompanies(int count)
    {
        var trademarks = new List<string>();
        var services = database.Sectors.ToArray();
        while(trademarks.Count < count) {
            var first = PickRandom(companyPrefixes);
            var last = PickRandom(companySuffixes);
            var name = $"{first}{last}";
            if(!trademarks.Contains(name)) {
                trademarks.Add(name);
                var company = new Company {
                    Uuid = PseudoRandomGuid(),
                    Slug = Slug.RandomWebString(6),
                    Title = name,
                    PrimarySector = PickRandom(services),
                    Status = PickRandom(companyStatuses),
                    AnnualRevenue = random.Next(1_000_000, 3_000_000),
                    SalesMargin = random.Next(100_000, 300_000),
                    IncorporationDate = new DateTime(random.Next(2020, 2021), random.Next(1, 12), random.Next(1, 28)),
                    
                };
                //Randomly populate fields.
                if(company.PrimarySector.Id == 3 ) {
                    company.CustomFields = new ExpandoValues { 
                        { "asx_code", Slug.RandomWebString(3).ToUpper(CultureInfo.CurrentCulture) }
                    };
                }
                //company.Videos.Add(new Video { Title = "Huzzah 1", Uri = "https://www.example.com/huzzah1" });
                //company.Videos.Add(new Video { Title = "Huzzah 2", Uri = "https://www.example.com/huzzah2" });
                database.Companies.Add(company);
            }
        }
        database.SaveChanges();
    }

    public async Task PopulateEmployeesAsync(int count)
    {
        var companies = await database.Companies.Where(e => e.Status == CompanyStatus.Active).ToArrayAsync();
        for(int i = 0; i < count; ++i) {
            var first = PickRandom(firstNames);
            var last = PickRandom(lastNames);
            var employee = new Employee {
                Uuid = PseudoRandomGuid(),
                FirstName = first,
                LastName = last
            };
            if(random.Next(0, 100) < 20) {
                // 20% are no longer employed
                employee.TerminationDate = DateTime.UtcNow.AddDays(-random.Next(1, 1000));
            }
            employee.Employer = PickRandom(companies);
            await employee.OnCreatingAsync();
            database.Employees.Add(employee);
        }
        await database.SaveChangesAsync();
    }

    public void PopulateContents()
    {
        database.Contents.Add(Sample);
        database.SaveChanges();
    }

    private T PickRandom<T>(T[] candidates) => candidates[random.Next(0, candidates.Length)];

    private Guid PseudoRandomGuid()
    {
        // Create a fake Guid, one that is consistently created based on the random seed below, don't use Guid.NewGuid().
        var bytes = new byte[16];
        random.NextBytes(bytes);
        return new Guid(bytes);
    }

    public async Task<RegionLoadStats> PopulateRegionsAsync(string[] countryFilter, bool includeSubdivisions, bool includeLocalities)
    {
        using var reader = new StreamReader(@".\SampleData\Countries.csv", Encoding.GetEncoding("ISO-8859-1"));
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var countries = csv.GetRecords<Country>().ToList();
        var re = await regions.ListAsync(new PageQuery { Take = int.MaxValue });
        var items = re.Items.ToList();
        var world = await PopulateWorldAsync(items);

        var maxSibling = items.Where(e => e.Level == RegionLevel.Country).Max(e => e.Lineage);
        foreach(var country in countries) {
            if(countryFilter.Length == 0 || countryFilter.Contains(country.Alpha2Code)) {
                var countryRegion = await PopulateCountry(items, country, world, maxSibling);
                maxSibling = countryRegion.Lineage;
            }
        }
        database.ChangeTracker.Clear();

        if(includeSubdivisions) {
            await PopulateSubdivisions(items);
        }

        var stats = new RegionLoadStats(countries.Count);
        return stats;
    }

    public async Task PopulateSubdivisions(List<Region> knownRegions)
    {
        // File seems to be UTF-8 but when using that encoding, the latin-1 characters are lost.
        // ISO-8859-1 brings in Latin-1 but doesn't handle multi-byte UTF-8 characters.
        using var reader = new StreamReader(@".\SampleData\Subdivisions.csv", Encoding.GetEncoding("ISO-8859-1"));
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var subdivisions = csv.GetRecords<Subdivision>().ToList();

        try {
            var loadedCountries = knownRegions.Where(e => e.Level == RegionLevel.Country).ToList();
            foreach(var country in loadedCountries) {
                var countrySubs = subdivisions.Where(e => e.Country == country.Slug).ToList();
                var lastSibling = knownRegions.Where(e => e.Parent == country).Max(e => e.Lineage);
                foreach(var sub in countrySubs) {
                    var subRegion = await PopulateSubdivision(knownRegions, sub, country, lastSibling);
                    lastSibling = subRegion.Lineage;
                }
            }
        }
        catch(Exception ex) {
            Console.WriteLine(ex.Message);
        }
    }

    public async Task<Region> PopulateSubdivision(List<Region> knownRegions, Subdivision subdivision, Region parent, HierarchyId? lastSibling)
    {
        var slug = $"{subdivision.Country}-{subdivision.Code}";
        var subRegion = knownRegions.FirstOrDefault(e => e.Slug == slug);
        if(subRegion == null) {
            var name = RemoveSubdivisionName().Replace(subdivision.Name, "").Trim();
            subRegion = new Region {
                Description = name,
                Level = RegionLevel.Subdivision,
                Lineage = parent.Lineage.GetDescendant(lastSibling, null),
                Parent = parent,
                Slug = slug,
                Title = name,
                Uuid = Guid.NewGuid(),
            };
            knownRegions.Add(subRegion);
            await regions.CreateAsync(subRegion);
        }
        return subRegion;
    }

    public async Task<Region> PopulateCountry(List<Region> knownRegions, Country country, Region parent, HierarchyId? lastSibling)
    {
        var countryRegion = knownRegions.FirstOrDefault(e => e.Slug == country.Alpha2Code);
        if(countryRegion == null) {
            countryRegion = new Region {
                Description = country.Name.Trim(),
                Level = RegionLevel.Country,
                Lineage = parent.Lineage.GetDescendant(lastSibling, null),
                Parent = parent,
                Slug = country.Alpha2Code,
                Title = country.Name.Trim(),
                Uuid = Guid.NewGuid(),
            };
            knownRegions.Add(countryRegion);
            await regions.CreateAsync(countryRegion);
        }
        return countryRegion;
    }

    public async Task<Region> PopulateWorldAsync(List<Region> knownRegions)
    {
        var world = knownRegions.FirstOrDefault(e => e.Slug == "all");
        if(world == null) {
            world = new Region {
                Description = "World",
                Level = RegionLevel.Global,
                Lineage = HierarchyId.GetRoot(),
                Slug = "all",
                Title = "World",
                Uuid = Guid.NewGuid(),
            };
            knownRegions.Add(world);
            await regions.CreateAsync(world);
        }
        return world;
    }

    public record Country (string Name, string Alpha2Code, string Alpha3Code, int NumericCode, string Iso3166Code, string Independent);
    public record Subdivision (string Country, string Code, string Name, string TypeName);
    private readonly Random random = new(123);

    private readonly CompanyStatus[] companyStatuses = [CompanyStatus.Inactive, CompanyStatus.Deleted, CompanyStatus.Active, CompanyStatus.Active, CompanyStatus.Active, CompanyStatus.Active, CompanyStatus.Active, CompanyStatus.Active, CompanyStatus.Active, CompanyStatus.Active, CompanyStatus.Active, CompanyStatus.Active, CompanyStatus.Active];

    private readonly string[] companyPrefixes = [ "High Tide", "Tempest", "Jupiter", "Cyclor", "Ant", "Jungle",
        "Grotto", "Ace", "Wood", "Ceas", "Jet" ];

    private readonly string[] companySuffixes = [ " Lighting", " Arts", "ation", "arts", "tainment", "search", "gate",
        "worth", " Microsystems", " Electronics", " King" ];

    private readonly string[] firstNames = ["James", "John", "Robert", "Michael", "William", "David", "Richard",
        "Joseph", "Thomas", "Charles",  "Christopher", "Daniel", "Matthew", "Anthony", "Donald", "Mark",
        "Paul",  "Steven", "Andrew", "Kenneth", "Joshua", "Kevin", "Brian", "George", "Edward", "Ronald",
        "Timothy", "Jason", "Jeffrey", "Ryan", "Jacob", "Gary", "Nicholas", "Eric", "Jonathan", "Stephen",
        "Larry", "Justin", "Scott", "Brandon", "Benjamin", "Samuel", "Frank", "Gregory", "Raymond",
        "Alexander", "Patrick", "Jack", "Dennis", "Jerry", "Mary", "Patricia", "Jennifer", "Linda",
        "Elizabeth", "Barbara", "Susan", "Jessica", "Sarah", "Karen", "Nancy", "Lisa", "Margaret", "Betty",
        "Sandra", "Ashley", "Dorothy", "Kimberly", "Emily", "Donna", "Michelle", "Carol", "Amanda",
        "Melissa", "Deborah", "Stephanie", "Rebecca", "Laura", "Sharon", "Cynthia", "Kathleen", "Amy",
        "Shirley", "Angela", "Helen", "Anna", "Brenda", "Pamela", "Nicole", "Samantha", "Katherine",
        "Emma", "Ruth", "Christine", "Catherine", "Debra", "Rachel", "Carolyn", "Janet", "Virginia" ];


    private readonly string[] lastNames = ["Smith", "Johnson", "Williams", "Brown", "Jones", "Miller", "Davis",
        "Garcia", "Rodriguez", "Wilson", "Martinez", "Anderson", "Taylor", "Thomas", "Hernandez", "Moore",
        "Martin", "Jackson", "Thompson", "White", "Lopez", "Lee", "Gonzalez", "Harris", "Clark", "Lewis",
        "Robinson", "Walker", "Perez", "Hall", "Young", "Allen", "Sanchez", "Wright", "King", "Scott",
        "Green", "Baker", "Adams", "Nelson", "Hill", "Ramirez", "Campbell", "Mitchell", "Roberts", "Carter",
        "Phillips", "Evans", "Turner", "Torres", "Parker", "Collins", "Edwards", "Stewart", "Flores",
        "Morris", "Nguyen", "Murphy", "Rivera", "Cook", "Rogers", "Morgan", "Peterson", "Cooper", "Reed",
        "Bailey", "Bell", "Gomez", "Kelly", "Howard", "Ward", "Cox", "Diaz", "Richardson", "Wood", "Watson",
        "Brooks", "Bennett", "Gray", "James", "Reyes", "Cruz", "Hughes", "Price", "Myers", "Long", "Foster",
        "Sanders", "Ross", "Morales", "Powell", "Sullivan", "Russell", "Ortiz", "Jenkins", "Gutierrez",
        "Perry", "Butler", "Barnes", "Fisher" ];

    private Content Sample => new() {
        Title = "Sample",
        Uuid = PseudoRandomGuid(),
        Layout = new ContentLayout {
            Sections = {
                new ContentSection {
                    Layout = SectionLayout.DoubleWeightedLeft,
                    Theme = ContentTheme.Light,
                    Containers = {
                        new ContentContainer {
                            Id = PseudoRandomGuid(),
                            Html = "<div>Hello Blazor</div>",
                            Padding = ContentPadding.Single,
                        },
                        new ContentContainer {
                            Id = PseudoRandomGuid(),
                            Html = "<div>Hello Blazor</div>",
                            Alignment = ContentAlignment.MiddleCenter,
                            Padding = ContentPadding.Single,
                        },
                    }
                },
                new ContentSection {
                    Layout = SectionLayout.DoubleWeightedRight,
                    Theme = ContentTheme.Dark,
                    Containers = {
                        new ContentContainer {
                            Id = PseudoRandomGuid(),
                            Html = "<div>Hello Blazor</div>",
                            Padding = ContentPadding.Single,
                        },
                        new ContentContainer {
                            Id = PseudoRandomGuid(),
                            Html = "<div>Hello Blazor</div>",
                            Alignment = ContentAlignment.BottomCenter,
                            Padding = ContentPadding.Single,
                        },
                    }
                },
                new ContentSection {
                    Layout = SectionLayout.Single,
                    Containers = {
                        new ContentContainer {
                            Id = PseudoRandomGuid(),
                            Html = "<div>Hello Blazor</div>",
                            Padding = ContentPadding.Single,
                        },
                    }
                },
            }
        }
    };

    private readonly SampleContext database;

    private readonly RegionService regions;

    [GeneratedRegex("(\\[.*\\])|(\\(.*\\))")]
    private static partial Regex RemoveSubdivisionName();
}

public record RegionLoadStats(int Countries);
