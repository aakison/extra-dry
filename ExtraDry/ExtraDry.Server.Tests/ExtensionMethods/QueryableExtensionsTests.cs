#nullable enable

using ExtraDry.Server;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace ExtraDry.Core.Tests.Server.ExtensionMethods {
    public class QueryableExtensionsTests {

        [Fact]
        public void NoSortSpecifiedUseIdStabilizer()
        {
            var list = new List<IdConventionKey> {
                new IdConventionKey { Id = 1 },
                new IdConventionKey { Id = 3 },
                new IdConventionKey { Id = 2 },
            };
            var query = new FilterQuery();

            var sorted = list.AsQueryable()
                .Sort(query)
                .ToList();

            Assert.Equal(3, sorted.Count);
            Assert.Equal(1, sorted.First().Id);
            Assert.Equal(3, sorted.Last().Id);
        }

        [Fact]
        public void NoSortSpecifiedUseClassNameStabilizer()
        {
            var list = new List<ClassNameConventionKey> {
                new ClassNameConventionKey { ClassNameConventionKeyId = 1 },
                new ClassNameConventionKey { ClassNameConventionKeyId = 3 },
                new ClassNameConventionKey { ClassNameConventionKeyId = 2 },
            };
            var query = new FilterQuery();

            var sorted = list.AsQueryable().Sort(query).ToList();

            Assert.Equal(3, sorted.Count);
            Assert.Equal(1, sorted.First().ClassNameConventionKeyId);
            Assert.Equal(3, sorted.Last().ClassNameConventionKeyId);
        }

        [Fact]
        public void NoSortSpecifiedUseKeyAttributeStabilizer()
        {
            var list = new List<KeyAttributeEntity> {
                new KeyAttributeEntity { PrimaryKey = 1 },
                new KeyAttributeEntity { PrimaryKey = 3 },
                new KeyAttributeEntity { PrimaryKey = 2 },
            };
            var query = new FilterQuery();

            var sorted = list.AsQueryable().Sort(query).ToList();

            Assert.Equal(3, sorted.Count);
            Assert.Equal(1, sorted.First().PrimaryKey);
            Assert.Equal(3, sorted.Last().PrimaryKey);
        }

        [Fact]
        public void NoSortSpecifiedNoStabilizerException()
        {
            var list = new List<NoImplicitStabilizer> {
                new NoImplicitStabilizer { PrimaryKey = 1 },
                new NoImplicitStabilizer { PrimaryKey = 3 },
                new NoImplicitStabilizer { PrimaryKey = 2 },
            };
            var query = new FilterQuery();

            Assert.Throws<DryException>(() => list.AsQueryable().Sort(query));
        }

        [Fact]
        public void NoSortSpecifiedCompositeKeyStabilizerException()
        {
            var list = new List<CompositeKeyStabilizer> {
                new CompositeKeyStabilizer { FirstPartKey = 1 },
                new CompositeKeyStabilizer { FirstPartKey = 3 },
                new CompositeKeyStabilizer { FirstPartKey = 2 },
            };
            var query = new FilterQuery();

            Assert.Throws<DryException>(() => list.AsQueryable().Sort(query));
        }


        public class IdConventionKey {
            public int Id { get; set; }

            public string Payload { get; set; } = string.Empty;
        }

        public class ClassNameConventionKey {
            public int ClassNameConventionKeyId { get; set; }

            public string Payload { get; set; } = string.Empty;
        }

        public class KeyAttributeEntity {

            [Key]
            public int PrimaryKey { get; set; }

            public string Payload { get; set; } = string.Empty;

        }

        public class NoImplicitStabilizer {
            public int PrimaryKey { get; set; }

            public string Payload { get; set; } = string.Empty;
        }


        public class CompositeKeyStabilizer {
            [Key]
            public int FirstPartKey { get; set; }

            [Key]
            public int SecondPartKey { get; set; }

            public string Payload { get; set; } = string.Empty;
        }

    }
}
