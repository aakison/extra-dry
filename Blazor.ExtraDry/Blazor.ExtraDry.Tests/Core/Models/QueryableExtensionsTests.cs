#nullable enable

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace Blazor.ExtraDry.Tests.Core.Models {
    public class QueryableExtensionsTests {

        [Fact]
        public void NoSortSpecifiedUseIdStabalizer()
        {
            var list = new List<IdConventionKey> {
                new IdConventionKey { Id = 1 },
                new IdConventionKey { Id = 3 },
                new IdConventionKey { Id = 2 },
            };
            var query = new FilterQuery();
            
            var sorted = list.AsQueryable().Sort(query).ToList();

            Assert.Equal(3, sorted.Count);
            Assert.Equal(1, sorted.First().Id);
            Assert.Equal(3, sorted.Last().Id);
        }

        [Fact]
        public void NoSortSpecifiedUseClassNameStabalizer()
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
        public void NoSortSpecifiedUseKeyAttributeStabalizer()
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
        public void NoSortSpecifiedNoStabalizerException()
        {
            var list = new List<NoImplicitStabalizer> {
                new NoImplicitStabalizer { PrimaryKey = 1 },
                new NoImplicitStabalizer { PrimaryKey = 3 },
                new NoImplicitStabalizer { PrimaryKey = 2 },
            };
            var query = new FilterQuery();

            Assert.Throws<DryException>(() => list.AsQueryable().Sort(query));
        }

        [Fact]
        public void NoSortSpecifiedCompositeKeyStabalizerException()
        {
            var list = new List<CompositeKeyStabalizer> {
                new CompositeKeyStabalizer { FirstPartKey = 1 },
                new CompositeKeyStabalizer { FirstPartKey = 3 },
                new CompositeKeyStabalizer { FirstPartKey = 2 },
            };
            var query = new FilterQuery();

            Assert.Throws<DryException>(() => list.AsQueryable().Sort(query));
        }

        [Fact]
        public void NoSortSpecifiedUseSpecifiedStabalizer()
        {
            var list = new List<CompositeKeyStabalizer> {
                new CompositeKeyStabalizer { FirstPartKey = 1 },
                new CompositeKeyStabalizer { FirstPartKey = 3 },
                new CompositeKeyStabalizer { FirstPartKey = 2 },
            };
            var query = new FilterQuery { Stabalizer = nameof(CompositeKeyStabalizer.FirstPartKey) };

            var sorted = list.AsQueryable().Sort(query).ToList();

            Assert.Equal(3, sorted.Count);
            Assert.Equal(1, sorted.First().FirstPartKey);
            Assert.Equal(3, sorted.Last().FirstPartKey);
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

        public class NoImplicitStabalizer {
            public int PrimaryKey { get; set; }

            public string Payload { get; set; } = string.Empty;
        }


        public class CompositeKeyStabalizer {
            [Key]
            public int FirstPartKey { get; set; }

            [Key]
            public int SecondPartKey { get; set; }

            public string Payload { get; set; } = string.Empty;
        }

    }
}
