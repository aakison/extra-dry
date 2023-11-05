using CsvHelper;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace Sample.Server.SampleData;

public class SampleDataService {

    public SampleDataService(SampleContext sampleContext)
    {
        database = sampleContext;
    }

    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Not static to be aligned with other methods.")]
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
                            IsRequired = true, 
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
                    Code = Slug.RandomWebString(6),
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
                        { "asx_code", Slug.RandomWebString(3).ToUpper() }
                    };
                }
                //company.Videos.Add(new Video { Title = "Huzzah 1", Uri = "https://www.example.com/huzzah1" });
                //company.Videos.Add(new Video { Title = "Huzzah 2", Uri = "https://www.example.com/huzzah2" });
                database.Companies.Add(company);
            }
        }
        database.SaveChanges();
    }

    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Might need instance in future.")]
    public async Task PopulateRegionsAsync(RegionService service)
    {

        // Tier 1
        var allRegions = new Region { Uuid = Guid.NewGuid(), Slug = "all", Title = "All Regions", Description = "The World", Level = RegionLevel.Global };

        var auRegion = new Region { Parent = allRegions, Slug = "AU", Title = "Australia", Description = "Australia", Level = RegionLevel.Country};
        var nzRegion = new Region { Parent = allRegions, Slug = "NZ", Title = "New Zealand", Description = "New Zealand", Level = RegionLevel.Country };

        // Tier 2
        var vicRegion = new Region { Parent = auRegion, Slug = "AU-VIC", Title = "Victoria", Description = "Victoria, Australia", Level = RegionLevel.Subdivision };
        var qldRegion = new Region { Parent = auRegion, Slug = "AU-QLD", Title = "Queensland", Description = "Queensland, Australia", Level = RegionLevel.Subdivision };
        var nswRegion = new Region { Parent = auRegion, Slug = "AU-NSW", Title = "New South Wales", Description = "NSW, Australia", Level = RegionLevel.Subdivision };
        var actRegion = new Region { Parent = auRegion, Slug = "AU-ACT", Title = "Canberra", Description = "Australian Capital Territory", Level = RegionLevel.Subdivision };
        var tasRegion = new Region { Parent = auRegion, Slug = "AU-TAS", Title = "Tasmania", Description = "Tasmania", Level = RegionLevel.Subdivision };
        var saRegion = new Region { Parent = auRegion, Slug = "AU-SA", Title = "South Australia", Description = "South Australia", Level = RegionLevel.Subdivision };
        var ntRegion = new Region { Parent = auRegion, Slug = "AU-NT", Title = "Northern Territory", Description = "Northern Territory", Level = RegionLevel.Subdivision };
        var waRegion = new Region { Parent = auRegion, Slug = "AU-WA", Title = "Western Australia", Description = "Western Australia", Level = RegionLevel.Subdivision };

        var aukRegion = new Region { Parent = nzRegion, Slug = "NZ-AUK", Title = "Auckland", Description = "Auckland, NZ", Level = RegionLevel.Subdivision };
        var tkiRegion = new Region { Parent = nzRegion, Slug = "NZ-TKI", Title = "Taranaki", Description = "Taranaki, NZ", Level = RegionLevel.Subdivision };

        // Tier 3
        var melbRegion = new Region { Parent = vicRegion, Slug = "AU-VIC-Melbourne", Title = "Melbourne City", Description = "Melbourne, Victoria, Australia", Level = RegionLevel.Locality };
        var brisRegion = new Region { Parent = qldRegion, Slug = "AU-QLD-Brisbane", Title = "Brisbane", Description = "Brisbane", Level = RegionLevel.Locality };
        var redRegion = new Region { Parent = qldRegion, Slug = "AU-QLD-Redlands", Title = "Redlands", Description = "City of Redlands", Level = RegionLevel.Locality };

        var baseRegions = new Region[] { allRegions, auRegion, nzRegion, vicRegion, qldRegion, nswRegion, actRegion, tasRegion, saRegion, ntRegion, waRegion, aukRegion, tkiRegion, melbRegion, brisRegion, redRegion };

        foreach(var region in baseRegions) {
            await service.CreateAsync(region);
        }

    }

    public async Task PopulateArbitaryRegions(RegionService service, int countryCount, int divisionCount, int subdivisionCount)
    {
        var allRegions = new List<Region>();

        var topRegion = new Region { Uuid = Guid.NewGuid(), Slug = "global", Title = "Global", Description = "Top", Level = RegionLevel.Global };
        await service.CreateAsync(topRegion);

        for(int i = 0; i < countryCount; i++) {
            var country = new Region { Uuid = Guid.NewGuid(), Parent = topRegion, Slug = $"country-{i}", Title = $"Country {i}", Level = RegionLevel.Country };
            allRegions.Add(country);

            for(int j = 0; j < divisionCount; j++) {
                var division = new Region { Uuid = Guid.NewGuid(), Parent = country, Slug = $"div-{j}-country-{i}", Title = $"Division {j} Country {i}", Level = RegionLevel.Subdivision };
                allRegions.Add(division);

                for(int k = 0; k < subdivisionCount; k++) {
                    var subdivision = new Region { Uuid = Guid.NewGuid(), Parent = division, Slug = $"subdiv-{k}-div-{j}-country-{i}", Title = $"SubDiv {k} Div {j} Country {i}", Level = RegionLevel.Locality };
                    allRegions.Add(subdivision);
                }
            }
        }
        foreach(var region in allRegions.OrderBy(e => e.Slug)) {
            await service.CreateAsync(region);
        }
    }

    public void PopulateEmployees(int count)
    {
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
            database.Employees.Add(employee);
        }
        database.SaveChanges();
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

    public Task<RegionLoadStats> PopulateRegionsAsync(RegionService regions, string? country, bool includeSubdivisions, bool includeLocalities)
    {
        using var reader = new StreamReader(@".\SampleData\Countries.csv");
        using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
        var records = csv.GetRecords<Country>();
        var re = regions.ListAsync(new PageQuery { Take = int.MaxValue });
        return Task.FromResult(new RegionLoadStats(records.Count()));
    }

    public record Country (string Name, string Alpha2Code, string Alpha3Code, int NumericCode, string Iso3166Code, string Independent);
    private readonly Random random = new(123);

    private readonly CompanyStatus[] companyStatuses = { CompanyStatus.Inactive, CompanyStatus.Deleted, CompanyStatus.Active, CompanyStatus.Active, CompanyStatus.Active, CompanyStatus.Active, CompanyStatus.Active, CompanyStatus.Active, CompanyStatus.Active, CompanyStatus.Active, CompanyStatus.Active, CompanyStatus.Active, CompanyStatus.Active };

    private readonly string[] companyPrefixes = { "High Tide", "Tempest", "Jupiter", "Cyclor", "Ant", "Jungle",
        "Grotto", "Ace", "Wood", "Ceas", "Jet" };

    private readonly string[] companySuffixes = { " Lighting", " Arts", "ation", "arts", "tainment", "search", "gate",
        "worth", " Microsystems", " Electronics", " King" };

    private readonly string[] firstNames = {"James", "John", "Robert", "Michael", "William", "David", "Richard",
        "Joseph", "Thomas", "Charles",  "Christopher", "Daniel", "Matthew", "Anthony", "Donald", "Mark",
        "Paul",  "Steven", "Andrew", "Kenneth", "Joshua", "Kevin", "Brian", "George", "Edward", "Ronald",
        "Timothy", "Jason", "Jeffrey", "Ryan", "Jacob", "Gary", "Nicholas", "Eric", "Jonathan", "Stephen",
        "Larry", "Justin", "Scott", "Brandon", "Benjamin", "Samuel", "Frank", "Gregory", "Raymond",
        "Alexander", "Patrick", "Jack", "Dennis", "Jerry", "Mary", "Patricia", "Jennifer", "Linda",
        "Elizabeth", "Barbara", "Susan", "Jessica", "Sarah", "Karen", "Nancy", "Lisa", "Margaret", "Betty",
        "Sandra", "Ashley", "Dorothy", "Kimberly", "Emily", "Donna", "Michelle", "Carol", "Amanda",
        "Melissa", "Deborah", "Stephanie", "Rebecca", "Laura", "Sharon", "Cynthia", "Kathleen", "Amy",
        "Shirley", "Angela", "Helen", "Anna", "Brenda", "Pamela", "Nicole", "Samantha", "Katherine",
        "Emma", "Ruth", "Christine", "Catherine", "Debra", "Rachel", "Carolyn", "Janet", "Virginia" };


    private readonly string[] lastNames = {"Smith", "Johnson", "Williams", "Brown", "Jones", "Miller", "Davis",
        "Garcia", "Rodriguez", "Wilson", "Martinez", "Anderson", "Taylor", "Thomas", "Hernandez", "Moore",
        "Martin", "Jackson", "Thompson", "White", "Lopez", "Lee", "Gonzalez", "Harris", "Clark", "Lewis",
        "Robinson", "Walker", "Perez", "Hall", "Young", "Allen", "Sanchez", "Wright", "King", "Scott",
        "Green", "Baker", "Adams", "Nelson", "Hill", "Ramirez", "Campbell", "Mitchell", "Roberts", "Carter",
        "Phillips", "Evans", "Turner", "Torres", "Parker", "Collins", "Edwards", "Stewart", "Flores",
        "Morris", "Nguyen", "Murphy", "Rivera", "Cook", "Rogers", "Morgan", "Peterson", "Cooper", "Reed",
        "Bailey", "Bell", "Gomez", "Kelly", "Howard", "Ward", "Cox", "Diaz", "Richardson", "Wood", "Watson",
        "Brooks", "Bennett", "Gray", "James", "Reyes", "Cruz", "Hughes", "Price", "Myers", "Long", "Foster",
        "Sanders", "Ross", "Morales", "Powell", "Sullivan", "Russell", "Ortiz", "Jenkins", "Gutierrez",
        "Perry", "Butler", "Barnes", "Fisher" };

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

}

public record RegionLoadStats(int countries);
