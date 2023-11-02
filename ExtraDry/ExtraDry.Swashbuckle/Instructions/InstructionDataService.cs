using ExtraDry.Server;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Claims;
using System.Security.Cryptography;
using System;
using Microsoft.EntityFrameworkCore;

namespace ExtraDry.Swashbuckle.Instructions;

public class InstructionDataService {

    public async Task<FilteredCollection<Automobile>> ListAsync(FilterQuery query)
    {
        return await automobiles
            .AsQueryable()
            .ForceStringComparison(StringComparison.OrdinalIgnoreCase)
            .QueryWith(query)
            .ToFilteredCollectionAsync();
    }

    public async Task<FilteredCollection<Automobile>> ListAsync(SortQuery query)
    {
        return await automobiles
            .AsQueryable()
            .ForceStringComparison(StringComparison.OrdinalIgnoreCase)
            .QueryWith(query)
            .ToFilteredCollectionAsync();
    }

    public async Task<PagedCollection<Automobile>> ListAsync(PageQuery query)
    {
        return await automobiles
            .AsQueryable()
            .ForceStringComparison(StringComparison.OrdinalIgnoreCase)
            .QueryWith(query)
            .ToPagedCollectionAsync();
    }

    public async Task<Statistics<Automobile>> RetrieveStatsAsync(FilterQuery query)
    {
        return await automobiles
            .AsQueryable()
            .ForceStringComparison(StringComparison.OrdinalIgnoreCase)
            .QueryWith(query)
            .ToStatisticsAsync();
    }

    public async Task<HierarchyCollection<Animal>> ListHierarchyAsync(HierarchyQuery query)
    {
        return await Animals
            .AsQueryable()
            .ForceStringComparison(StringComparison.OrdinalIgnoreCase)
            .QueryWith(query)
            .ToHierarchyCollectionAsync();
    }

    public string RandomAffirmation() => affirmations[random.Next(affirmations.Count)];

    private readonly Random random = new();

    private List<Animal> Animals {
        get {
            var animals = new List<Animal>() {
                new Animal { Lineage = HierarchyId.Parse("/"), Title = "Animals" },
                new Animal { Lineage = HierarchyId.Parse("/1/"), Title = "Vertebrates" },
                new Animal { Lineage = HierarchyId.Parse("/1/1/"), Title = "Mammals" },
                new Animal { Lineage = HierarchyId.Parse("/1/1/1/"), Title = "Human", Description = "A bipedal primate belonging to the species Homo sapiens, known for its advanced cognitive abilities and civilizations."},
                new Animal { Lineage = HierarchyId.Parse("/1/1/2/"), Title = "Dolphin", Description = "A highly intelligent aquatic mammal known for its playful behavior and use of complex vocalizations." },
                new Animal { Lineage = HierarchyId.Parse("/1/1/3/"), Title = "Elephant", Description = "A large herbivorous mammal with a trunk, long tusks, and flapping ears, native to Africa and Asia." },
                new Animal { Lineage = HierarchyId.Parse("/1/2/"), Title = "Birds" },
                new Animal { Lineage = HierarchyId.Parse("/1/2/1/"), Title = "Sparrow", Description = "A small, often brown - colored bird known for its melodious songs and wide distribution across the world." },
                new Animal { Lineage = HierarchyId.Parse("/1/2/2/"), Title = "Eagle", Description = "A large bird of prey with a powerful build, keen eyesight, and the ability to fly at high altitudes."},
                new Animal { Lineage = HierarchyId.Parse("/1/2/3/"), Title = "Penguin", Description = "A flightless bird adapted for life in the water with a streamlined body and flippers for swimming."},
                new Animal { Lineage = HierarchyId.Parse("/1/3/"), Title = "Fish" },
                new Animal { Lineage = HierarchyId.Parse("/1/3/1/"), Title = "Salmon", Description = "An anadromous fish known for its incredible migrations from freshwater to saltwater and back for spawning." },
                new Animal { Lineage = HierarchyId.Parse("/1/3/2/"), Title = "Shark", Description = "A powerful predatory fish with a streamlined body, cartilaginous skeleton, and multiple rows of sharp teeth." },
                new Animal { Lineage = HierarchyId.Parse("/1/3/3/"), Title = "Clownfish", Description = "A brightly colored fish known for its symbiotic relationship with sea anemones." },
                new Animal { Lineage = HierarchyId.Parse("/1/4/"), Title = "Amphibians" },
                new Animal { Lineage = HierarchyId.Parse("/1/4/1/"), Title = "Frog", Description = "A small amphibian with smooth skin, strong legs for jumping, and a life cycle that includes both aquatic and terrestrial stages." },
                new Animal { Lineage = HierarchyId.Parse("/1/4/2/"), Title = "Salamander", Description = "A slender amphibian that resembles a lizard and often has an aquatic larval stage."},
                new Animal { Lineage = HierarchyId.Parse("/1/5/"), Title = "Reptiles" },
                new Animal { Lineage = HierarchyId.Parse("/1/5/1/"), Title = "Crocodile", Description = "A large predatory reptile with a powerful jaw, long tail, and armored skin, typically found in freshwater habitats." },
                new Animal { Lineage = HierarchyId.Parse("/1/5/2/"), Title = "Turtle", Description = "A reptile with a bony or cartilaginous shell protecting its body, both in water and on land." },
                new Animal { Lineage = HierarchyId.Parse("/1/5/3/"), Title = "Snake", Description = "A legless reptile with a long, flexible body and scales, some of which are venomous." },
                new Animal { Lineage = HierarchyId.Parse("/2/"), Title = "Invertebrates" },
                new Animal { Lineage = HierarchyId.Parse("/2/1/"), Title = "Arthropods" },
                new Animal { Lineage = HierarchyId.Parse("/2/1/1/"), Title = "Insects" },
                new Animal { Lineage = HierarchyId.Parse("/2/1/1/1/"), Title = "Ant", Description = "A small, social insect known for its organized colonies, strength relative to size, and ability to work collectively." },
                new Animal { Lineage = HierarchyId.Parse("/2/1/1/2/"), Title = "Butterfly", Description = " A colorful, winged insect that undergoes a metamorphic life cycle starting from a caterpillar to a pupa and finally an adult." },
                new Animal { Lineage = HierarchyId.Parse("/2/1/1/3/"), Title = "Bee", Description = "A flying insect known for its role in pollination and its ability to produce honey in complex social colonies." },
                new Animal { Lineage = HierarchyId.Parse("/2/1/2/"), Title = "Arachnids" },
                new Animal { Lineage = HierarchyId.Parse("/2/1/2/1/"), Title = "Spider", Description = "An eight - legged arthropod known for spinning silk webs to catch its prey." },
                new Animal { Lineage = HierarchyId.Parse("/2/1/2/2/"), Title = "Scorpion", Description = "An arthropod with a segmented tail tipped with a venomous stinger, primarily found in desert habitats." },
                new Animal { Lineage = HierarchyId.Parse("/2/2/"), Title = "Mollusks" },
                new Animal { Lineage = HierarchyId.Parse("/2/2/1/"), Title = "Snail", Description = "A slow - moving mollusk with a coiled shell, known for its secretion of slime." },
                new Animal { Lineage = HierarchyId.Parse("/2/2/2/"), Title = "Octopus", Description = "A cephalopod mollusk with eight tentacles, known for its intelligence and ability to change color." },
                new Animal { Lineage = HierarchyId.Parse("/2/2/3/"), Title = "Clam", Description = "A bivalve mollusk with two hinged shells, often buried in sand or mud." },
                new Animal { Lineage = HierarchyId.Parse("/2/3/"), Title = "Annelids" },
                new Animal { Lineage = HierarchyId.Parse("/2/3/1/"), Title = "Earthworm", Description = "A segmented annelid that lives underground and plays a vital role in soil aeration and decomposition." },
                new Animal { Lineage = HierarchyId.Parse("/2/4/"), Title = "Cnidarians" },
                new Animal { Lineage = HierarchyId.Parse("/2/4/1/"), Title = "Jellyfish", Description = "A free-swimming marine cnidarian with a gelatinous, umbrella-shaped bell and trailing tentacles, some of which can deliver painful stings." },
                new Animal { Lineage = HierarchyId.Parse("/2/4/2/"), Title = "Coral", Description = "Marine invertebrates that live in colonies and are known for building calcium carbonate skeletons that form coral reefs." },
            };

            for(int i = 0; i < animals.Count(); i++) {
                var animal = animals[i];
                animal.Id = i + 1;
                animal.Parent = animals.SingleOrDefault(e => e.Lineage == animal.Lineage.GetAncestor(1));
                animal.Slug = animal.Title.ToLower().Replace(" ", "-");
            }
            return animals;
        }
    }

    private readonly List<Automobile> automobiles = new() {
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

    private readonly List<string> affirmations = new() {
        "I experience gratitude for everything I have in my life",
        "I always receive exactly what I ask for and appreciate that",
        "My life is filled with an abundance of goodness",
        "Gratitude brings me into a harmonious relationship with everyone",
        "Happiness expands within me",
        "Joy floods my thoughts and my life",
        "I release all negativity and hold joy in my heart",
        "I am overcome with gratitude for the bliss that fills my life",
        "I accept the good that is flowing into my life",
        "I display a natural sense of wonder",
        "My whole world is viewed through the eyes of wonder and excitement",
        "I view life through innocence, purity and curiosity",
        "Everything is mysterious and absolutely incredible",
        "I am worthy to receive the unlimited offerings of the Universe",
        "I affirm only the best for myself and others",
        "I am so grateful for supportive friends and a loving family",
        "I appreciate everything I have in my life",
        "I am always open to receive more blessings",
        "I see the beauty in nature that surrounds me",
        "I give thanks for the helpful people that guide me in this life journey",
        "I am creatively expressing my highest potential",
        "I am a powerful body, a powerful mind, and a powerful soul",
        "I am a channel for love and healing",
        "Everything I need comes to me",
        "All is well in my life",
    };

}
