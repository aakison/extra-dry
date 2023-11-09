using ExtraDry.Server.Tests.Models;
using ExtraDry.Server.Tests.WarehouseTests;

namespace ExtraDry.Server.Tests;

public static class Samples {

    public static List<Model> Models => new() {
        new Model { Id = 1, Name = "Alpha", Soundex = "A410", Type = ModelType.Greek, Notes = "Common with phonetic" },
        new Model { Id = 2, Name = "Beta", Soundex = "B300", Type = ModelType.Greek },
        new Model { Id = 3, Name = "Gamma", Soundex = "G500", Type = ModelType.Greek },
        new Model { Id = 4, Name = "Delta", Soundex = "D430", Type = ModelType.Greek, Notes = "Common with phonetic" },
        new Model { Id = 5, Name = "Epsilon", Soundex = "E124", Type = ModelType.Greek },
        new Model { Id = 6, Name = "Zeta", Soundex = "Z300", Type = ModelType.Greek },
        new Model { Id = 7, Name = "Alpha", Soundex = "A410", Type = ModelType.Phonetic, Notes = "Common with Greek" },
        new Model { Id = 8, Name = "Bravo", Soundex = "B610", Type = ModelType.Phonetic },
        new Model { Id = 9, Name = "Charlie", Soundex = "C640", Type = ModelType.Phonetic },
        new Model { Id = 10, Name = "Delta", Soundex = "D430", Type = ModelType.Phonetic, Notes = "Common with Greek" },
        new Model { Id = 11, Name = "Echo", Soundex = "E200", Type = ModelType.Phonetic },
        new Model { Id = 12, Name = "Foxtrot", Soundex = "F236", Type = ModelType.Phonetic },
        new Model { Id = 13, Name = "Foxxy", Soundex = "F200", Type = ModelType.Hendrix, Notes = "Jimi" },
    };


    public static List<Region> Regions {
        get {
            if(regions == null) {
                // Tier 1
                var allRegions = new Region { Uuid = Guid.NewGuid(), Slug = "all", Title = "All Regions", Description = "The World", Level = RegionLevel.Global, Lineage = HierarchyId.Parse("/") };

                var auRegion = new Region { Parent = allRegions, Slug = "AU", Title = "Australia", Description = "Australia", Level = RegionLevel.Country, Lineage = HierarchyId.Parse("/1/") };
                var nzRegion = new Region { Parent = allRegions, Slug = "NZ", Title = "New Zealand", Description = "New Zealand", Level = RegionLevel.Country, Lineage = HierarchyId.Parse("/2/") };

                // Tier 2
                var vicRegion = new Region { Parent = auRegion, Slug = "AU-VIC", Title = "Victoria", Description = "Victoria, Australia", Level = RegionLevel.Division, Lineage = HierarchyId.Parse("/1/1/") };
                var qldRegion = new Region { Parent = auRegion, Slug = "AU-QLD", Title = "Queensland", Description = "Queensland, Australia", Level = RegionLevel.Division, Lineage = HierarchyId.Parse("/1/2/") };
                var nswRegion = new Region { Parent = auRegion, Slug = "AU-NSW", Title = "New South Wales", Description = "NSW, Australia", Level = RegionLevel.Division, Lineage = HierarchyId.Parse("/1/3/") };
                var actRegion = new Region { Parent = auRegion, Slug = "AU-ACT", Title = "Canberra", Description = "Australian Capital Territory", Level = RegionLevel.Division, Lineage = HierarchyId.Parse("/1/4/") };
                var tasRegion = new Region { Parent = auRegion, Slug = "AU-TAS", Title = "Tasmania", Description = "Tasmania", Level = RegionLevel.Division, Lineage = HierarchyId.Parse("/1/5/") };
                var saRegion = new Region { Parent = auRegion, Slug = "AU-SA", Title = "South Australia", Description = "South Australia", Level = RegionLevel.Division, Lineage = HierarchyId.Parse("/1/6/") };
                var ntRegion = new Region { Parent = auRegion, Slug = "AU-NT", Title = "Northern Territory", Description = "Northern Territory", Level = RegionLevel.Division, Lineage = HierarchyId.Parse("/1/7/") };
                var waRegion = new Region { Parent = auRegion, Slug = "AU-WA", Title = "Western Australia", Description = "Western Australia", Level = RegionLevel.Division, Lineage = HierarchyId.Parse("/1/8/") };

                var aukRegion = new Region { Parent = nzRegion, Slug = "NZ-AUK", Title = "Auckland", Description = "Auckland, NZ", Level = RegionLevel.Division, Lineage = HierarchyId.Parse("/2/1/") };
                var tkiRegion = new Region { Parent = nzRegion, Slug = "NZ-TKI", Title = "Taranaki", Description = "Taranaki, NZ", Level = RegionLevel.Division, Lineage = HierarchyId.Parse("/2/2/") };

                // Tier 3
                var melbRegion = new Region { Parent = vicRegion, Slug = "AU-VIC-Melbourne", Title = "Melbourne City", Description = "Melbourne, Victoria, Australia", Level = RegionLevel.Subdivision, Lineage = HierarchyId.Parse("/1/1/1/") };
                var brisRegion = new Region { Parent = qldRegion, Slug = "AU-QLD-Brisbane", Title = "Brisbane", Description = "Brisbane", Level = RegionLevel.Subdivision, Lineage = HierarchyId.Parse("/1/2/1/") };
                var redRegion = new Region { Parent = qldRegion, Slug = "AU-QLD-Redlands", Title = "Redlands", Description = "City of Redlands", Level = RegionLevel.Subdivision, Lineage = HierarchyId.Parse("/1/2/2/") };

                regions = new List<Region> { allRegions, auRegion, nzRegion, vicRegion, qldRegion, nswRegion, actRegion, tasRegion, saRegion, ntRegion, waRegion, aukRegion, tkiRegion, melbRegion, brisRegion, redRegion };
            }
            return regions;
        }

    }
    private static List<Region>? regions;

}
