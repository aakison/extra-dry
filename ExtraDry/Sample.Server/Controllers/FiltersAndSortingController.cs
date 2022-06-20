using ExtraDry.Core.Models;
using ExtraDry.Server;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sample.Server.Controllers;

/// <summary>
/// Most API endpoints have the ability to list entities.  When provided, these list methods accept a `filter` query parameter, allowing for filtering to only a subset of entities.
/// 
/// The `filter` parameter allows for both simple and advanced filtering scenarios against a pre-defined set of fields.  The simple version of the filters are suitable for filters that might be seen by average users.  While the advanced filters start to look more complex than users prefer.  The advanced options, however, are suitable for constructing filter view for users with multiple UI controls.
/// 
/// ### Simple Filters
/// Simple filters contain just a space-separated list of terms.  The filter will return all items that match any of the terms on any of the filterable fields.  
/// For example:
/// 
/// * `Toyota Honda` - returns all matches of 'Toyota' _or_ 'Honda' on any filterable fields.
/// * `Toyota Civic` - returns all Toyotas and the Civic from Honda.  Also, anything that has a description that compares itself to a Civic.
/// * `2011` - returns all strings that contain '2011' and numbers that equal 2011 on any filterable fields.
/// 
/// To filter based on phrases, use quotation markers to enclose spaces and any punctuation.  
/// For example:
/// 
/// * `"Toyota Corolla"` - returns all matches of 'Toyota Corolla' on any filterable fields.
/// * `"Toyota Corolla" "Honda Civic"` - returns all matches of 'Toyota Corolla' _or_ 'Honda Civic' on any filterable fields.
/// 
/// ### Advanced Filters
/// Using advanced filters, it is also possible to specify specific fields for filter terms.  Simply provide the field name and a colon before the filter term.
/// For example:
/// 
/// * `make:Toyota model:Corolla` - returns all matches of 'Toyota' in the 'make' field _and_ 'Corolla' in the 'model' field.
/// * `make:Toyota model:""FJ Cruiser""` - returns all matches of 'Toyota' in the 'make' field _and_ 'FJ Cruiser' in the 'model' field, note the use of quotes because of the space in the name 'FJ Cruiser'. 
/// 
/// Advanced filters can also specify a list of alternate values.  This can be done by either listing the field multiple times or by separating the terms using a pipe '|' character.  
/// 
/// For example, all of the following are equivalent and will return all matches of 'Corolla' in the 'model' field union with all matches of 'Prado' in the 'model' field.
/// 
/// * `model:Corolla model:Prado`
/// * `model:"Corolla" model:"Prado"`
/// * `model:Corolla|Prado`
/// * `model:"Corolla"|"Prado"`
/// 
/// ### Range Filters
/// Number and DateTime filterable fields also support ranges.
/// To specify a range, comma separate the lower and upper bounds inside parantheses and/or brackets, e.g. `(0,10)` or `[1,4]`.  
/// Parentheses '(' and ')' indicate exclusive boundaries, while brackets '[' and ']' indicate inclusive boundaries.
/// Either the lower or upper bound (but not both) can be omitted for open ranges.
/// For example, the following are valid range filters:
/// 
/// * `year:[2000,2010)` - All years from 2000 up to and exluding 2010.
/// * `year:[2010,)` - All years from 2010 onwards (no upper bound).
/// * `date:[2020-01-01T00:00:00,2021-01-01T00:00:00)` - Any time in calendar year 2020.
/// * `age:(,18)` - Any age below 18.
/// 
/// ### Filterable Fields
/// Each of the above examples only works on 'filterable fields'.  For performance reasons, not all fields are filterable.  Each endpoint will list the fields that can be filtered on.  Additionally, string filters might be applied differently depending on the content of the filter field. Strings may match either the whole string (equality), the start of the string (starts-with), or anywhere in the string (contains).  
///
/// ### Response Body
/// The response body is a container of both items and metadata about the request.  In particular, it contains:
/// * `created` - a ISO8601 formatted date with the UTC time the query was run.
/// * `filter` - (optional) if a filter was provided for the query, the text of the filter as requested.
/// * `count` - the number of items returned (can be calculated, but reiterated for convenience).
/// * `items` - an array of entities, serialization varies by endpoint. 
/// 
/// ## Try It Out
/// Use the endpoint below to test some filters against a sample database of car make and models - no authentication required.
/// Try the following:
/// * *blank* - get a full list of all cars, no filter applied.
/// * `Toyota` - get a list of vehicles that reference 'Toyota'.
/// * `make:toyota` - get a list of just vehicles that _are_ a toyota.
/// * `make:toy` - empty list, the 'make' field is a full match, example of 'equality' filter.
/// * `model:yaris` - only returns the 'Yaris Cross' and not the 'GT Yaris', example of 'starts-with' filter.
/// * `year:[1960,1990)` - see only the oldest cars.
/// * Look at the JSON produced and mix and match, all automobile fields are filterable.
/// </summary>
[ApiController]
[ApiExplorerSettings(GroupName = ApiGroupNames.Instructions)]
[SkipStatusCodePages]
public class FilteringController : ControllerBase {

    public FilteringController()
    {
        sampleData = new SampleDataService();
    }

    /// <summary>
    /// Filterable list of all cars
    /// </summary>
    [HttpGet("sample-data/filter-cars"), Produces("application/json")]
    [AllowAnonymous]
    public async Task<FilteredCollection<Automobile>> ListFilteredAsync([FromQuery] string? filter)
    {
        var query = new FilterQuery() { Filter = filter };
        return await sampleData.ListAsync(query);
    }

    private readonly SampleDataService sampleData;

}

public class SampleDataService {

    static SampleDataService()
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
            .QueryWith(query)
            .ToFilteredCollectionAsync();
    }

    private readonly static List<Automobile> automobiles;

}

/// <summary>
/// A sample class used for demonstrating the common features of APIs.
/// </summary>
public class Automobile {

    [JsonIgnore]
    [Key]
    public int Id { get; set; }

    /// <summary>
    /// The make (manufacturer's name) of the car.
    /// </summary>
    /// <example>Toyota</example>
    [Filter(FilterType.Equals)]
    public CaselessString Make { get; set; } = string.Empty;

    /// <summary>
    /// The model name of the car.
    /// </summary>
    /// <example>FJ Cruiser</example>
    [Filter(FilterType.StartsWith)]
    public CaselessString Model { get; set; } = string.Empty;

    /// <summary>
    /// The year that the car was first introduced.
    /// </summary>
    /// <example>2011</example>
    [Filter]
    public int Year { get; set; }

    /// <summary>
    /// A description of the intended regional market for the car.
    /// </summary>
    /// <example>Japan and Australasia</example>
    [Filter(FilterType.Contains)]
    public CaselessString Market { get; set; } = string.Empty;

    /// <summary>
    /// A description of the car, as sourced from Wikipedia.
    /// </summary>
    /// <example>Retro-styled body-on-frame mid-size SUV inspired by the Toyota FJ40.</example>
    [Filter(FilterType.Contains)]
    public CaselessString Description { get; set; } = string.Empty;

}

