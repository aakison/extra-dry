using ExtraDry.Server;

namespace Sample.Server.Instructions;

public class InstructionDataService {

    static InstructionDataService()
    {
        automobiles = new List<Automobile> {
            new Automobile { Make = "Toyota", Model = "Avalon", Year = 1994, Market = "North America and China", Description = "Full-size sedan mainly produced and marketed in North America and China. Hybrid powertrain is available. All-wheel drive models are exclusively sold in North America." },
            new Automobile { Make = "Toyota", Model = "Camry", Year = 1982, Market = "Global", Description = "Mid-size sedan (D-segment) marketed globally. Hybrid powertrain is optional." },
            new Automobile { Make = "Toyota", Model = "Mirai", Year = 2014, Market = "Global", Description = "Fuel-cell/hydrogen executive sedan." },
            new Automobile { Make = "Toyota", Model = "Prius", Year = 1997, Market = "Global", Description = "Hybrid/plug-in hybrid compact liftback (C-segment). The first mass-marketed hybrid electric car." },
            new Automobile { Make = "Toyota", Model = "Corolla", Year = 1966, Market = "Global", Description = "Compact hatchback (C-segment). Successor to the Auris. Called the Corolla Sport in Japan. Hybrid powertrain is optional." },
            new Automobile { Make = "Toyota", Model = "Passo", Year = 2004, Market = "Japan", Description = "Subcompact hatchback positioned below the Yaris. Rebadged Daihatsu Boon, marketed primarily in Japan." },
            new Automobile { Make = "Toyota", Model = "GR Yaris", Year = 2020, Market = "Global (except North America)", Description = "High-performance, three-door version of the Yaris (XP210), mass-produced as a homologation model for the FIA World Rally Championship." },
            new Automobile { Make = "Toyota", Model = "4Runner", Year = 1984, Market = "North America", Description = "Body-on-frame mid-size SUV based on the Tacoma, marketed primarily in North America. Third-row seating is optional." },
            new Automobile { Make = "Toyota", Model = "C-HR", Year = 2016, Market = "Global", Description = "Subcompact crossover based on the Corolla platform. Hybrid powertrain is optional." },
            new Automobile { Make = "Toyota", Model = "FJ Cruiser", Year = 2010, Market = "Middle East", Description = "Retro-styled body-on-frame mid-size SUV inspired by the Toyota FJ40." },
            new Automobile { Make = "Toyota", Model = "Land Cruiser Prado", Year = 1984, Market = "Global (except North America)", Description = "Mid-size body-on-frame SUV, smaller than the full-size Land Cruiser. Available in long-wheelbase 5-door and short-wheelbase 3-door body styles." },
            new Automobile { Make = "Toyota", Model = "Yaris Cross", Year = 2020, Market = "Japan, Europe and Australasia", Description = "Subcompact crossover based on the Yaris platform, primarily marketed in Europe, Japan, and Australasia. Hybrid powertrain is optional." },
            new Automobile { Make = "Honda", Model = "Brio", Year = 2011, Market = "Southeast Asia", Description = "Entry-level hatchback, currently only produced in Indonesia for several Southeast Asian markets." },
            new Automobile { Make = "Honda", Model = "City", Year = 1981, Market = "Southeast Asia, South America[1]", Description = "Hatchback version of the City subcompact car. The newest model replaced the third-generation Fit/Jazz in some emerging markets." },
            new Automobile { Make = "Honda", Model = "Civic", Year = 1972, Market = "Global", Description = "Hatchback version of the Civic compact car.  Largest competitor to the Toyota Corolla on the market." },
            new Automobile { Make = "Honda", Model = "e", Year = 2019, Market = "Europe and Japan", Description = "Battery-electric retro-styled subcompact hatchback/supermini." },
            new Automobile { Make = "Honda", Model = "Fit/Jazz/Life", Year = 2001, Market = "Global (except North America)", Description = "Practicality-oriented subcompact hatchback/supermini. Hybrid and e:HEV available." },
            new Automobile { Make = "Honda", Model = "Accord", Year = 1976, Market = "Global (except Europe)", Description = "Mid-size sedan. Also available as the Inspire in China. Hybrid available." },
            new Automobile { Make = "Honda", Model = "City", Year = 1996, Market = "Global (except Europe and North America)", Description = "Subcompact/compact sedan. The latest generation is destined for emerging markets. Hybrid or e:HEV available." },
            new Automobile { Make = "Honda", Model = "Civic/Integra", Year = 1972, Market = "Global", Description = "The Honda Civic is a compact sedan. It's the oldest continuous nameplate used in a Honda automobile." },
            new Automobile { Make = "Honda", Model = "Freed", Year = 2008, Market = "Japan", Description = "Two or three-row Mini MPV with sliding doors for the Japanese market. Hybrid available." },
            new Automobile { Make = "Honda", Model = "Mobilio", Year = 2001, Market = "Southeast Asia", Description = "Three-row entry-level mini MPV engineered for the Indonesian market. Based on the Brio platform." },
            new Automobile { Make = "Honda", Model = "Odyssey (North America)", Year = 1994, Market = "North America", Description = "Three-row minivan with sliding doors engineered for the North American market, exported throughout the Americas and Middle East." },
            new Automobile { Make = "Honda", Model = "Shuttle", Year = 2011, Market = "Japan", Description = "Two-row station wagon version of the Fit/Jazz mainly for the Japanese market. Hybrid available." },
            new Automobile { Make = "Honda", Model = "CR-V", Year = 1995, Market = "Global", Description = "Compact crossover SUV. Available as a two-row and three-row in select markets. Hybrid and PHEV available." },
            new Automobile { Make = "Honda", Model = "Pilot", Year = 2002, Market = "North America", Description = "Three-row mid-size crossover SUV mainly for the North American market." },
        };
    }

    public async Task<FilteredCollection<Automobile>> ListAsync(FilterQuery query)
    {
        return await automobiles
            .AsQueryable()
            .ForceStringComparison(StringComparison.OrdinalIgnoreCase)
            .QueryWith(query)
            .ToFilteredCollectionAsync();
    }

    private readonly static List<Automobile> automobiles;

}
