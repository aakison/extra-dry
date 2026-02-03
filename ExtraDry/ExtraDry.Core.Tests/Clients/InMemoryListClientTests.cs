namespace ExtraDry.Core.Tests.Clients;

public class InMemoryListClientTests
{
    #region Test Data Classes

    private sealed class TestItem : IUniqueIdentifier
    {
        [Key]
        public Guid Uuid { get; set; }
        
        [Filter]
        public string Name { get; set; } = string.Empty;
        
        [Filter]
        public int Value { get; set; }
        
        public DateTime CreatedDate { get; set; }
    }

    private sealed class SimpleItem
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        
        public string Name { get; set; } = string.Empty;
    }

    #endregion

    #region Constructor Tests

    [Fact]
    public void Constructor_WithItemsOnly_CreatesClient()
    {
        var items = new List<TestItem>
        {
            new() { Uuid = Guid.NewGuid(), Name = "Item1", Value = 10 },
            new() { Uuid = Guid.NewGuid(), Name = "Item2", Value = 20 }
        };

        var client = new InMemoryListClient<TestItem>(items);

        Assert.NotNull(client);
        Assert.Equal(2, client.PageSize);
        Assert.False(client.IsLoading);
        Assert.False(client.IsEmpty);
    }

    [Fact]
    public void Constructor_WithItemsAndFilter_CreatesClient()
    {
        var items = new List<TestItem>
        {
            new() { Uuid = Guid.NewGuid(), Name = "Item1", Value = 10 },
            new() { Uuid = Guid.NewGuid(), Name = "Item2", Value = 20 }
        };
        Func<TestItem, bool> filter = item => item.Value > 15;

        var client = new InMemoryListClient<TestItem>(items, filter);

        Assert.NotNull(client);
        Assert.Equal(2, client.PageSize);
    }

    [Fact]
    public void Constructor_WithEmptyList_CreatesClient()
    {
        var items = new List<TestItem>();

        var client = new InMemoryListClient<TestItem>(items);

        Assert.NotNull(client);
        Assert.Equal(1, client.PageSize); // Math.Max(1, 0) = 1
        Assert.True(client.IsEmpty);
    }

    #endregion

    #region PageSize Tests

    [Theory]
    [InlineData(0, 1)]  // Empty list should return 1
    [InlineData(1, 1)]
    [InlineData(5, 5)]
    [InlineData(100, 100)]
    public void PageSize_ReturnsCorrectValue(int itemCount, int expectedPageSize)
    {
        var items = Enumerable.Range(0, itemCount)
            .Select(i => new TestItem { Uuid = Guid.NewGuid(), Name = $"Item{i}" })
            .ToList();

        var client = new InMemoryListClient<TestItem>(items);

        Assert.Equal(expectedPageSize, client.PageSize);
    }

    #endregion

    #region IsLoading Tests

    [Fact]
    public void IsLoading_AlwaysReturnsFalse()
    {
        var items = new List<TestItem>
        {
            new() { Uuid = Guid.NewGuid(), Name = "Item1" }
        };
        var client = new InMemoryListClient<TestItem>(items);

        Assert.False(client.IsLoading);
    }

    #endregion

    #region IsEmpty Tests

    [Fact]
    public void IsEmpty_WithEmptyList_ReturnsTrue()
    {
        var items = new List<TestItem>();
        var client = new InMemoryListClient<TestItem>(items);

        Assert.True(client.IsEmpty);
    }

    [Fact]
    public void IsEmpty_WithItems_ReturnsFalse()
    {
        var items = new List<TestItem>
        {
            new() { Uuid = Guid.NewGuid(), Name = "Item1" }
        };
        var client = new InMemoryListClient<TestItem>(items);

        Assert.False(client.IsEmpty);
    }

    #endregion

    #region GetItemsAsync Tests

    [Fact]
    public async Task GetItemsAsync_WithEmptyQuery_ReturnsAllItems()
    {
        var items = new List<TestItem>
        {
            new() { Uuid = Guid.NewGuid(), Name = "Item1", Value = 10 },
            new() { Uuid = Guid.NewGuid(), Name = "Item2", Value = 20 },
            new() { Uuid = Guid.NewGuid(), Name = "Item3", Value = 30 }
        };
        var client = new InMemoryListClient<TestItem>(items);
        var query = new Query();

        var result = await client.GetItemsAsync(query, CancellationToken.None);

        Assert.Equal(3, result.Count);
        Assert.Equal(3, result.Total);
        Assert.Equal(3, result.Items.Count());
    }

    [Fact]
    public async Task GetItemsAsync_WithFilter_ReturnsFilteredItems()
    {
        var items = new List<TestItem>
        {
            new() { Uuid = Guid.NewGuid(), Name = "Apple", Value = 10 },
            new() { Uuid = Guid.NewGuid(), Name = "Banana", Value = 20 },
            new() { Uuid = Guid.NewGuid(), Name = "Cherry", Value = 30 }
        };
        var client = new InMemoryListClient<TestItem>(items);
        var query = new Query { Filter = "Value:[10,15)" }; // Range query for values between 10 and 15

        var result = await client.GetItemsAsync(query, CancellationToken.None);

        Assert.Equal(1, result.Count);
        Assert.Single(result.Items);
        Assert.Equal("Apple", result.Items.First().Name);
    }

    [Fact]
    public async Task GetItemsAsync_WithSort_ReturnsSortedItems()
    {
        var items = new List<TestItem>
        {
            new() { Uuid = Guid.NewGuid(), Name = "Charlie", Value = 30 },
            new() { Uuid = Guid.NewGuid(), Name = "Alpha", Value = 10 },
            new() { Uuid = Guid.NewGuid(), Name = "Bravo", Value = 20 }
        };
        var client = new InMemoryListClient<TestItem>(items);
        var query = new Query { Sort = "Name" };

        var result = await client.GetItemsAsync(query, CancellationToken.None);

        var resultItems = result.Items.ToList();
        Assert.Equal("Alpha", resultItems[0].Name);
        Assert.Equal("Bravo", resultItems[1].Name);
        Assert.Equal("Charlie", resultItems[2].Name);
    }

    [Fact]
    public async Task GetItemsAsync_WithSortDescending_ReturnsSortedItemsDescending()
    {
        var items = new List<TestItem>
        {
            new() { Uuid = Guid.NewGuid(), Name = "Alpha", Value = 10 },
            new() { Uuid = Guid.NewGuid(), Name = "Bravo", Value = 20 },
            new() { Uuid = Guid.NewGuid(), Name = "Charlie", Value = 30 }
        };
        var client = new InMemoryListClient<TestItem>(items);
        var query = new Query { Sort = "-Value" };

        var result = await client.GetItemsAsync(query, CancellationToken.None);

        var resultItems = result.Items.ToList();
        Assert.Equal(30, resultItems[0].Value);
        Assert.Equal(20, resultItems[1].Value);
        Assert.Equal(10, resultItems[2].Value);
    }

    [Fact]
    public async Task GetItemsAsync_WithConstructorFilter_AppliesFilter()
    {
        var items = new List<TestItem>
        {
            new() { Uuid = Guid.NewGuid(), Name = "Item1", Value = 10 },
            new() { Uuid = Guid.NewGuid(), Name = "Item2", Value = 20 },
            new() { Uuid = Guid.NewGuid(), Name = "Item3", Value = 30 }
        };
        Func<TestItem, bool> filter = item => item.Value > 15;
        var client = new InMemoryListClient<TestItem>(items, filter);
        var query = new Query();

        var result = await client.GetItemsAsync(query, CancellationToken.None);

        Assert.Equal(2, result.Count);
        Assert.All(result.Items, item => Assert.True(item.Value > 15));
    }

    [Fact]
    public async Task GetItemsAsync_WithFilterAndSort_AppliesBoth()
    {
        var items = new List<TestItem>
        {
            new() { Uuid = Guid.NewGuid(), Name = "Apple", Value = 30 },
            new() { Uuid = Guid.NewGuid(), Name = "Apricot", Value = 10 },
            new() { Uuid = Guid.NewGuid(), Name = "Banana", Value = 20 }
        };
        var client = new InMemoryListClient<TestItem>(items);
        var query = new Query { Filter = "Value:[5,25)", Sort = "Value" }; // Filter and sort by Value

        var result = await client.GetItemsAsync(query, CancellationToken.None);

        var resultItems = result.Items.ToList();
        Assert.Equal(2, resultItems.Count);
        Assert.Equal("Apricot", resultItems[0].Name);
        Assert.Equal("Banana", resultItems[1].Name);
    }

    [Fact]
    public async Task GetItemsAsync_WithCancellationToken_CompletesSuccessfully()
    {
        var items = new List<TestItem>
        {
            new() { Uuid = Guid.NewGuid(), Name = "Item1", Value = 10 }
        };
        var client = new InMemoryListClient<TestItem>(items);
        var query = new Query();
        var cts = new CancellationTokenSource();

        var result = await client.GetItemsAsync(query, cts.Token);

        Assert.Single(result.Items);
    }

    #endregion

    #region TryRefreshItem Tests

    [Fact]
    public void TryRefreshItem_WithMatchPredicate_UpdatesItem()
    {
        var uuid1 = Guid.NewGuid();
        var items = new List<TestItem>
        {
            new() { Uuid = uuid1, Name = "Item1", Value = 10 },
            new() { Uuid = Guid.NewGuid(), Name = "Item2", Value = 20 }
        };
        var client = new InMemoryListClient<TestItem>(items);
        var updatedItem = new TestItem { Uuid = uuid1, Name = "UpdatedItem1", Value = 15 };

        var result = client.TryRefreshItem(updatedItem, item => item.Uuid == uuid1);

        Assert.True(result);
        Assert.Equal("UpdatedItem1", items[0].Name);
        Assert.Equal(15, items[0].Value);
    }

    [Fact]
    public void TryRefreshItem_WithIUniqueIdentifier_UpdatesItem()
    {
        var uuid1 = Guid.NewGuid();
        var items = new List<TestItem>
        {
            new() { Uuid = uuid1, Name = "Item1", Value = 10 },
            new() { Uuid = Guid.NewGuid(), Name = "Item2", Value = 20 }
        };
        var client = new InMemoryListClient<TestItem>(items);
        var updatedItem = new TestItem { Uuid = uuid1, Name = "UpdatedItem1", Value = 15 };

        var result = client.TryRefreshItem(updatedItem);

        Assert.True(result);
        Assert.Equal("UpdatedItem1", items[0].Name);
        Assert.Equal(15, items[0].Value);
    }

    [Fact]
    public void TryRefreshItem_WithNonExistentItem_ReturnsFalse()
    {
        var uuid1 = Guid.NewGuid();
        var items = new List<TestItem>
        {
            new() { Uuid = uuid1, Name = "Item1", Value = 10 }
        };
        var client = new InMemoryListClient<TestItem>(items);
        var updatedItem = new TestItem { Uuid = Guid.NewGuid(), Name = "UpdatedItem", Value = 15 };

        var result = client.TryRefreshItem(updatedItem);

        Assert.False(result);
        Assert.Equal("Item1", items[0].Name); // Original item unchanged
    }

    [Fact]
    public void TryRefreshItem_WithoutPredicateAndNonIUniqueIdentifier_ThrowsException()
    {
        var items = new List<SimpleItem>
        {
            new() { Id = 1, Name = "Item1" }
        };
        var client = new InMemoryListClient<SimpleItem>(items);
        var updatedItem = new SimpleItem { Id = 1, Name = "UpdatedItem" };

        var exception = Assert.Throws<InvalidOperationException>(() =>
            client.TryRefreshItem(updatedItem));

        Assert.Contains("Either a match predicate must be provided", exception.Message);
    }

    [Fact]
    public void TryRefreshItem_WithPredicateOnNonIUniqueIdentifierType_UpdatesItem()
    {
        var items = new List<SimpleItem>
        {
            new() { Id = 1, Name = "Item1" },
            new() { Id = 2, Name = "Item2" }
        };
        var client = new InMemoryListClient<SimpleItem>(items);
        var updatedItem = new SimpleItem { Id = 1, Name = "UpdatedItem1" };

        var result = client.TryRefreshItem(updatedItem, item => item.Id == 1);

        Assert.True(result);
        Assert.Equal("UpdatedItem1", items[0].Name);
    }

    [Fact]
    public void TryRefreshItem_UpdatesCorrectItemInMiddleOfList()
    {
        var uuid1 = Guid.NewGuid();
        var uuid2 = Guid.NewGuid();
        var uuid3 = Guid.NewGuid();
        var items = new List<TestItem>
        {
            new() { Uuid = uuid1, Name = "Item1", Value = 10 },
            new() { Uuid = uuid2, Name = "Item2", Value = 20 },
            new() { Uuid = uuid3, Name = "Item3", Value = 30 }
        };
        var client = new InMemoryListClient<TestItem>(items);
        var updatedItem = new TestItem { Uuid = uuid2, Name = "UpdatedItem2", Value = 25 };

        var result = client.TryRefreshItem(updatedItem);

        Assert.True(result);
        Assert.Equal("Item1", items[0].Name);
        Assert.Equal("UpdatedItem2", items[1].Name);
        Assert.Equal(25, items[1].Value);
        Assert.Equal("Item3", items[2].Name);
    }

    #endregion

    #region FilterAndSort Tests

    [Fact]
    public void FilterAndSort_WithEmptyQuery_ReturnsAllItems()
    {
        var items = new List<TestItem>
        {
            new() { Uuid = Guid.NewGuid(), Name = "Item1", Value = 10 },
            new() { Uuid = Guid.NewGuid(), Name = "Item2", Value = 20 }
        };
        var client = new InMemoryListClient<TestItem>(items);
        var query = new Query();

        var result = client.FilterAndSort(query);

        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void FilterAndSort_WithFilter_ReturnsFilteredItems()
    {
        var items = new List<TestItem>
        {
            new() { Uuid = Guid.NewGuid(), Name = "Apple", Value = 10 },
            new() { Uuid = Guid.NewGuid(), Name = "Banana", Value = 20 },
            new() { Uuid = Guid.NewGuid(), Name = "Apricot", Value = 30 }
        };
        var client = new InMemoryListClient<TestItem>(items);
        var query = new Query { Filter = "Value:[15,35)" }; // Range query

        var result = client.FilterAndSort(query);

        Assert.Equal(2, result.Count);
        Assert.All(result, item => Assert.InRange(item.Value, 15, 34));
    }

    [Fact]
    public void FilterAndSort_WithSort_ReturnsSortedItems()
    {
        var items = new List<TestItem>
        {
            new() { Uuid = Guid.NewGuid(), Name = "Charlie", Value = 30 },
            new() { Uuid = Guid.NewGuid(), Name = "Alpha", Value = 10 },
            new() { Uuid = Guid.NewGuid(), Name = "Bravo", Value = 20 }
        };
        var client = new InMemoryListClient<TestItem>(items);
        var query = new Query { Sort = "Name" };

        var result = client.FilterAndSort(query);

        Assert.Equal("Alpha", result[0].Name);
        Assert.Equal("Bravo", result[1].Name);
        Assert.Equal("Charlie", result[2].Name);
    }

    [Fact]
    public void FilterAndSort_WithConstructorFilter_AppliesFilter()
    {
        var items = new List<TestItem>
        {
            new() { Uuid = Guid.NewGuid(), Name = "Item1", Value = 10 },
            new() { Uuid = Guid.NewGuid(), Name = "Item2", Value = 20 },
            new() { Uuid = Guid.NewGuid(), Name = "Item3", Value = 30 }
        };
        Func<TestItem, bool> filter = item => item.Value >= 20;
        var client = new InMemoryListClient<TestItem>(items, filter);
        var query = new Query();

        var result = client.FilterAndSort(query);

        Assert.Equal(2, result.Count);
        Assert.All(result, item => Assert.True(item.Value >= 20));
    }

    [Fact]
    public void FilterAndSort_WithConstructorFilterAndQueryFilter_AppliesBoth()
    {
        var items = new List<TestItem>
        {
            new() { Uuid = Guid.NewGuid(), Name = "Apple", Value = 10 },
            new() { Uuid = Guid.NewGuid(), Name = "Apricot", Value = 20 },
            new() { Uuid = Guid.NewGuid(), Name = "Banana", Value = 30 }
        };
        Func<TestItem, bool> filter = item => item.Value >= 15;
        var client = new InMemoryListClient<TestItem>(items, filter);
        var query = new Query { Filter = "Value:[18,25)" }; // Query filter for range 18-25

        var result = client.FilterAndSort(query);

        Assert.Single(result);
        Assert.Equal("Apricot", result[0].Name);
        Assert.Equal(20, result[0].Value);
    }

    [Fact]
    public void FilterAndSort_CaseInsensitiveFilter_WorksCorrectly()
    {
        var items = new List<TestItem>
        {
            new() { Uuid = Guid.NewGuid(), Name = "APPLE", Value = 10 },
            new() { Uuid = Guid.NewGuid(), Name = "apple", Value = 20 },
            new() { Uuid = Guid.NewGuid(), Name = "ApPlE", Value = 30 }
        };
        var client = new InMemoryListClient<TestItem>(items);
        var query = new Query { Filter = "Name:apple" };

        var result = client.FilterAndSort(query);

        Assert.Equal(3, result.Count);
    }

    #endregion

    #region Integration Tests

    [Fact]
    public async Task CompleteWorkflow_AddFilterSortRefresh_WorksCorrectly()
    {
        // Arrange: Create initial list
        var uuid1 = Guid.NewGuid();
        var uuid2 = Guid.NewGuid();
        var uuid3 = Guid.NewGuid();
        var items = new List<TestItem>
        {
            new() { Uuid = uuid1, Name = "Apple", Value = 30 },
            new() { Uuid = uuid2, Name = "Banana", Value = 10 },
            new() { Uuid = uuid3, Name = "Cherry", Value = 20 }
        };
        var client = new InMemoryListClient<TestItem>(items);

        // Act & Assert: Get all items sorted
        var query1 = new Query { Sort = "Value" };
        var result1 = await client.GetItemsAsync(query1, CancellationToken.None);
        var sorted = result1.Items.ToList();
        Assert.Equal("Banana", sorted[0].Name);
        Assert.Equal("Cherry", sorted[1].Name);
        Assert.Equal("Apple", sorted[2].Name);

        // Act & Assert: Filter items with range query
        var query2 = new Query { Filter = "Value:[25,35)" };
        var result2 = await client.GetItemsAsync(query2, CancellationToken.None);
        Assert.Single(result2.Items);
        Assert.Equal("Apple", result2.Items.First().Name);

        // Act & Assert: Refresh an item
        var updatedItem = new TestItem { Uuid = uuid2, Name = "Blueberry", Value = 15 };
        var refreshResult = client.TryRefreshItem(updatedItem);
        Assert.True(refreshResult);

        // Act & Assert: Verify the refresh
        var query3 = new Query { Sort = "Value" };
        var result3 = await client.GetItemsAsync(query3, CancellationToken.None);
        var finalSorted = result3.Items.ToList();
        Assert.Equal("Blueberry", finalSorted[0].Name);
        Assert.Equal(15, finalSorted[0].Value);
    }

    #endregion
}
