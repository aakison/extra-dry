using ExtraDry.Server;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace ExtraDry.Swashbuckle.Instructions;

public class InstructionDataService
{
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

    [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Service contract should be instance.")]
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

    private static List<Animal> Animals {
        get {
            var animals = new List<Animal>() {
                new() { Lineage = HierarchyId.Parse("/"), Title = "Animals", Uuid = Guid.Parse("123e4567-e89b-12d3-a456-426614174000") },
                new() { Lineage = HierarchyId.Parse("/1/"), Title = "Vertebrates", Uuid = Guid.Parse("0d3a53a7-31b8-4b1d-b4d6-49c5a3e6e3b6") },
                new() { Lineage = HierarchyId.Parse("/1/1/"), Title = "Mammals", Uuid = Guid.Parse("c2ab8a87-5f08-4097-a6aa-4bfbf6e2c1d7") },
                new() { Lineage = HierarchyId.Parse("/1/1/1/"), Title = "Human", Description = "A bipedal primate belonging to the species Homo sapiens, known for its advanced cognitive abilities and civilizations.", Uuid = Guid.Parse("0de2c3a4-5762-4f4d-9cd9-5e5f6e7f8e9f")},
                new() { Lineage = HierarchyId.Parse("/1/1/2/"), Title = "Dolphin", Description = "A highly intelligent aquatic mammal known for its playful behavior and use of complex vocalizations.", Uuid = Guid.Parse("92e4e8d1-d9a2-4e4b-8f8d-ea1f99a2a8e1") },
                new() { Lineage = HierarchyId.Parse("/1/1/3/"), Title = "Elephant", Description = "A large herbivorous mammal with a trunk, long tusks, and flapping ears, native to Africa and Asia.", Uuid = Guid.Parse("b195fabc-6a67-41d6-ae5b-3a1eb1a3c1a2") },
                new() { Lineage = HierarchyId.Parse("/1/2/"), Title = "Birds", Uuid = Guid.Parse("e58a2a8c-8c32-4b8b-8e02-f9a3ee4a5c6f") },
                new() { Lineage = HierarchyId.Parse("/1/2/1/"), Title = "Sparrow", Description = "A small, often brown - colored bird known for its melodious songs and wide distribution across the world.", Uuid = Guid.Parse("5f217c3b-9d3c-4e19-b079-1d3b1c9e4f2b") },
                new() { Lineage = HierarchyId.Parse("/1/2/2/"), Title = "Eagle", Description = "A large bird of prey with a powerful build, keen eyesight, and the ability to fly at high altitudes.", Uuid = Guid.Parse("7d19ea1b-ee5b-4b2b-bd7d-f650e7e4b9b2")},
                new() { Lineage = HierarchyId.Parse("/1/2/3/"), Title = "Penguin", Description = "A flightless bird adapted for life in the water with a streamlined body and flippers for swimming.", Uuid = Guid.Parse("b7388bda-6f7e-4fcb-89ef-ec1a3da56b25") },
                new() { Lineage = HierarchyId.Parse("/1/3/"), Title = "Fish", Uuid = Guid.Parse("2fc1f5ae-7b35-460a-9dac-91bcfcf2b991") },
                new() { Lineage = HierarchyId.Parse("/1/3/1/"), Title = "Salmon", Description = "An anadromous fish known for its incredible migrations from freshwater to saltwater and back for spawning.", Uuid = Guid.Parse("4de4db5c-4b9d-4f60-9b86-f1f1a6e1ee28") },
                new() { Lineage = HierarchyId.Parse("/1/3/2/"), Title = "Shark", Description = "A powerful predatory fish with a streamlined body, cartilaginous skeleton, and multiple rows of sharp teeth.", Uuid = Guid.Parse("52a3bbd8-2f5f-4a76-9f64-7522b4b2f1a7") },
                new() { Lineage = HierarchyId.Parse("/1/3/3/"), Title = "Clownfish", Description = "A brightly colored fish known for its symbiotic relationship with sea anemones.", Uuid = Guid.Parse("d5d2d69d-7891-47a2-bbc9-ae28b55614b4") },
                new() { Lineage = HierarchyId.Parse("/1/4/"), Title = "Amphibians", Uuid = Guid.Parse("3b8fd845-5717-486e-9f5d-52c214a2a8b3") },
                new() { Lineage = HierarchyId.Parse("/1/4/1/"), Title = "Frog", Description = "A small amphibian with smooth skin, strong legs for jumping, and a life cycle that includes both aquatic and terrestrial stages.", Uuid = Guid.Parse("fbe4b5ac-4f0b-41ea-9dcb-e9c9d8f7c2f3") },
                new() { Lineage = HierarchyId.Parse("/1/4/2/"), Title = "Salamander", Description = "A slender amphibian that resembles a lizard and often has an aquatic larval stage.", Uuid = Guid.Parse("9b9f4823-9d60-4b7a-9285-557f6963a4a4") },
                new() { Lineage = HierarchyId.Parse("/1/5/"), Title = "Reptiles", Uuid = Guid.Parse("cda23b3b-9823-4b1f-8f8b-a1d9c7c390f1") },
                new() { Lineage = HierarchyId.Parse("/1/5/1/"), Title = "Crocodile", Description = "A large predatory reptile with a powerful jaw, long tail, and armored skin, typically found in freshwater habitats.", Uuid = Guid.Parse("74e5678e-e90c-47d3-8c65-8574f5b3a9f2") },
                new() { Lineage = HierarchyId.Parse("/1/5/2/"), Title = "Turtle", Description = "A reptile with a bony or cartilaginous shell protecting its body, both in water and on land.", Uuid = Guid.Parse("516f25d6-8b3f-4bbc-bc4a-9c6dfd75a177") },
                new() { Lineage = HierarchyId.Parse("/1/5/3/"), Title = "Snake", Description = "A legless reptile with a long, flexible body and scales, some of which are venomous.", Uuid = Guid.Parse("b4e4fce4-8eac-4e5a-85ea-d3abd56fd07f") },
                new() { Lineage = HierarchyId.Parse("/2/"), Title = "Invertebrates", Uuid = Guid.Parse("1131fd2c-36d7-4e25-abb1-393b1a5d8f41") },
                new() { Lineage = HierarchyId.Parse("/2/1/"), Title = "Arthropods", Uuid = Guid.Parse("85aad67a-ceda-4b97-afe7-3b3901b8c8eb") },
                new() { Lineage = HierarchyId.Parse("/2/1/1/"), Title = "Insects", Uuid = Guid.Parse("195f8b9a-ffcd-486b-932b-5ea1f8c0dd2f") },
                new() { Lineage = HierarchyId.Parse("/2/1/1/1/"), Title = "Ant", Description = "A small, social insect known for its organized colonies, strength relative to size, and ability to work collectively.", Uuid = Guid.Parse("f3265afa-b23b-4cb8-94be-df635e4eaf2f") },
                new() { Lineage = HierarchyId.Parse("/2/1/1/2/"), Title = "Butterfly", Description = " A colorful, winged insect that undergoes a metamorphic life cycle starting from a caterpillar to a pupa and finally an adult.", Uuid = Guid.Parse("e50b1c78-3c8f-44a9-bf3e-e1f9582c9e66") },
                new() { Lineage = HierarchyId.Parse("/2/1/1/3/"), Title = "Bee", Description = "A flying insect known for its role in pollination and its ability to produce honey in complex social colonies.", Uuid = Guid.Parse("a1848ad7-2b1e-4f3e-b17f-f9b5f4a60f02") },
                new() { Lineage = HierarchyId.Parse("/2/1/2/"), Title = "Arachnids", Uuid = Guid.Parse("2e5b596f-98a4-4f7a-8d2e-91b9ebe2e956") },
                new() { Lineage = HierarchyId.Parse("/2/1/2/1/"), Title = "Spider", Description = "An eight - legged arthropod known for spinning silk webs to catch its prey.", Uuid = Guid.Parse("48d407a7-fd38-467b-9b68-5e3dd89b6f0a") },
                new() { Lineage = HierarchyId.Parse("/2/1/2/2/"), Title = "Scorpion", Description = "An arthropod with a segmented tail tipped with a venomous stinger, primarily found in desert habitats.", Uuid = Guid.Parse("5e2fd632-47a1-4a80-ae47-d1a3a9fde5c2") },
                new() { Lineage = HierarchyId.Parse("/2/2/"), Title = "Mollusks", Uuid = Guid.Parse("e24b6d65-dbb0-4f7b-9b9d-ee5fcef0c8ae") },
                new() { Lineage = HierarchyId.Parse("/2/2/1/"), Title = "Snail", Description = "A slow - moving mollusk with a coiled shell, known for its secretion of slime.", Uuid = Guid.Parse("3c76e4a5-f91e-4b65-9e22-fefb2c1d1f5a") },
                new() { Lineage = HierarchyId.Parse("/2/2/2/"), Title = "Octopus", Description = "A cephalopod mollusk with eight tentacles, known for its intelligence and ability to change color.", Uuid = Guid.Parse("6f23b6e1-cf1e-4d36-bf5b-a7bbe8b3d9e1") },
                new() { Lineage = HierarchyId.Parse("/2/2/3/"), Title = "Clam", Description = "A bivalve mollusk with two hinged shells, often buried in sand or mud.", Uuid = Guid.Parse("0bbcee59-07a0-4f4c-92e2-dbb80c7ea1a3") },
                new() { Lineage = HierarchyId.Parse("/2/3/"), Title = "Annelids", Uuid = Guid.Parse("84d6c9a1-87ed-4f1b-bdc7-8b8e4f50b3d2") },
                new() { Lineage = HierarchyId.Parse("/2/3/1/"), Title = "Earthworm", Description = "A segmented annelid that lives underground and plays a vital role in soil aeration and decomposition.", Uuid = Guid.Parse("7f82bcf3-6d16-4302-ae13-1d3293e64a4f") },
                new() { Lineage = HierarchyId.Parse("/2/4/"), Title = "Cnidarians", Uuid = Guid.Parse("6890dfb6-1d34-4b29-a6b5-8cc5e5a69dbf") },
                new() { Lineage = HierarchyId.Parse("/2/4/1/"), Title = "Jellyfish", Description = "A free-swimming marine cnidarian with a gelatinous, umbrella-shaped bell and trailing tentacles, some of which can deliver painful stings.", Uuid = Guid.Parse("49c5e995-5168-4b1e-8b8e-3e290ab85e54") },
                new() { Lineage = HierarchyId.Parse("/2/4/2/"), Title = "Coral", Description = "Marine invertebrates that live in colonies and are known for building calcium carbonate skeletons that form coral reefs.", Uuid = Guid.Parse("e3b7ed48-4283-4344-8ec1-4aaf3c6f2254") },
            };
            for(int i = 0; i < animals.Count; i++) {
                var animal = animals[i];
                animal.Id = i + 1;
                animal.Parent = animals.SingleOrDefault(e => e.Lineage == animal.Lineage.GetAncestor(1));
                animal.Slug = animal.Title.ToLower(CultureInfo.CurrentCulture).Replace(" ", "-");
            }
            return animals;
        }
    }

    private readonly List<Automobile> automobiles = [
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
    ];

    private readonly List<string> affirmations = [
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
    ];
}
