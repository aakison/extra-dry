#nullable enable

using ExtraDry.Blazor.Components.Internal;
using System;
using Xunit;

namespace ExtraDry.Blazor.Tests.Internals {
    public class SelectionSetTests
    {
        [Fact]
        public void IsEmpty()
        {
            var set = new SelectionSet();

            Assert.False(set.Any());
            Assert.False(set.All());
            Assert.False(set.Single());
            Assert.Empty(set.Items);
        }


        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SelectOne(bool multi)
        {
            var set = new SelectionSet() { MultipleSelect = multi };
            var obj = new object();

            set.Add(obj);

            Assert.True(set.Any());
            Assert.True(set.Single());
            Assert.False(set.All());
        }

        [Fact]
        public void ExclusiveImplementationSelectOne()
        {
            var set = new SelectionSet() { MultipleSelect = true };
            var obj1 = new object();

            set.SelectAll();
            set.Add(obj1);

            Assert.True(set.Any());
            Assert.True(set.All());
            Assert.False(set.Single());
            Assert.True(set.Contains(new object()));
            Assert.True(set.Contains(obj1));
        }

        [Fact]
        public void SingleSelectSelectMany()
        {
            var set = new SelectionSet() { MultipleSelect = false };
            var obj1 = new object();
            var obj2 = new object();

            set.Add(obj1);
            set.Add(obj2);

            Assert.True(set.Any());
            Assert.False(set.All());
            Assert.True(set.Single());
            Assert.True(set.Contains(obj2));
            Assert.False(set.Contains(obj1));
        }

        [Fact]
        public void MultiSelectSelectMany()
        {
            var set = new SelectionSet() { MultipleSelect = true };
            var obj1 = new object();
            var obj2 = new object();

            set.Add(obj1);
            set.Add(obj2);

            Assert.True(set.Any());
            Assert.False(set.All());
            Assert.False(set.Single());
            Assert.True(set.Contains(obj2));
            Assert.True(set.Contains(obj1));
        }

        [Fact]
        public void ExclusiveImplementationMultiSelectDeselect()
        {
            var set = new SelectionSet() { MultipleSelect = true };
            var obj1 = new object();

            set.SelectAll();
            set.Remove(obj1);

            Assert.True(set.Any());
            Assert.False(set.All());
            Assert.False(set.Single());
            Assert.True(set.Contains(new object()));
            Assert.False(set.Contains(obj1));
        }


        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SingleSelectClear(bool multi)
        {
            var set = new SelectionSet() { MultipleSelect = multi };
            var obj1 = new object();
            var obj2 = new object();
            set.Add(obj1);
            set.Add(obj2);

            set.Clear();

            Assert.False(set.Any());
            Assert.False(set.All());
            Assert.False(set.Single());
            Assert.Empty(set.Items);
        }

        [Fact]
        public void CheckDuplicates()
        {
            var set = new SelectionSet();
            var obj1 = new object();

            set.Add(obj1);
            set.Add(obj1);

            Assert.True(set.Any());
            Assert.False(set.All());
            Assert.True(set.Single());
            Assert.Single(set.Items);
        }

        [Fact]
        public void SelectAllMultiSelect()
        {
            var set = new SelectionSet() { MultipleSelect = true };
            var obj1 = new object();

            set.SelectAll();

            Assert.True(set.Any());
            Assert.True(set.All());
            Assert.False(set.Single());
            Assert.True(set.Contains(obj1));
        }

        [Fact]
        public void SelectAllSingleSelect()
        {
            var set = new SelectionSet() { MultipleSelect = false };
            var obj1 = new object();

            Assert.Throws<InvalidOperationException>(() => set.SelectAll());
        }

    }
}
