using BookCatalog.Core.Models;
using BookCatalog.Core.Services;

namespace BookCatalog.Tests.NUnit;

public class SqliteDataServiceTest
{
    private const string dbPath = @"D:\Development\Projects\C#\BookCatalog\BookCatalog.Tests.NUnit\Data\";

    private static readonly List<BookItem> entries = new()
    {
        new BookItem
        {
            Id = 1,
            Category = "Computing",
            Title = "Advanced Guide To Python 3 Programming - Hunt",
            Path = @"D:\TEMP\Books\Computing & Technical\Advanced Guide To Python 3 Programming - Hunt.pdf",
            UsageCount = 0
        },
        new BookItem
        {
            Id = 2,
            Category = "Computing",
            Title = "ActiveX Programming with Visual C++ - Que",
            Path = @"D:\TEMP\Books\Computing & Technical\ActiveX Programming with Visual C++ - Que.pdf",
            UsageCount = 0
        },
        new BookItem
        {
            Id = 3,
            Category = "Computing",
            Title = "Adaptive Code via C# - McClean",
            Path = @"D:\TEMP\Books\Computing & Technical\Adaptive Code via C# - McClean.pdf",
            UsageCount = 0
        },
        new BookItem
        {
            Id = 4,
            Category = "Computing",
            Title = "Advanced  Metaprogramming in Classic C++ - Gennaro",
            Path = @"D:\TEMP\Books\Computing & Technical\Advanced  Metaprogramming in Classic C++ - Gennaro.pdf",
            UsageCount = 0
        },
        new BookItem
        {
            Id = 5,
            Category = "Computing",
            Title = "Advanced Analytics in Power BI with R and Python - Wade",
            Path = @"D:\TEMP\Books\Computing & Technical\Advanced Analytics in Power BI with R and Python - Wade.pdf",
            UsageCount = 0
        },
        new BookItem
        {
            Id = 6,
            Category = "Computing",
            Title = "Advanced C and C++ Compiling - Stevanovic",
            Path = @"D:\TEMP\Books\Computing & Technical\Advanced C and C++ Compiling - Stevanovic.pdf",
            UsageCount = 0
        },
        new BookItem
        {
            Id = 7,
            Category = "Computing",
            Title = "Advanced C# Programming - Kimmel",
            Path = @"D:\TEMP\Books\Computing & Technical\Advanced C# Programming - Kimmel.pdf",
            UsageCount = 0
        },
        new BookItem
        {
            Id = 8,
            Category = "Computing",
            Title = "Advanced C++ Programming Cookbook - Quinn",
            Path = @"D:\TEMP\Books\Computing & Technical\Advanced C++ Programming Cookbook - Quinn.pdf",
            UsageCount = 0
        },
        new BookItem
        {
            Id = 9,
            Category = "Computing",
            Title = "Advanced DotNET Remoting - Rammer",
            Path = @"D:\TEMP\Books\Computing & Technical\Advanced DotNET Remoting - Rammer.pdf",
            UsageCount = 0
        },
        new BookItem
        {
            Id = 10,
            Category = "Computing",
            Title = "A Beginner's Guide To Python 3 Programming - Hunt",
            Path = @"D:\TEMP\Books\Computing & Technical\A Beginner's Guide To Python 3 Programming - Hunt.pdf",
            UsageCount = 0
        },
    };

    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestAddItem()
    {
        // Arrange
        var dataService = new SqliteDataService
        {
            DbPath = $"{dbPath}{Path.GetRandomFileName()}"
        };
        _ = dataService.InitializeDataAsync();

        var book = entries[0];

        // Act
        var id = dataService.AddItemAsync(book).Result;

        // Assert
        Assert.That(id, Is.EqualTo(1));
    }

    [Test]
    public void TestGetItem()
    {
        var dataService = new SqliteDataService
        {
            DbPath = $"{dbPath}{Path.GetRandomFileName()}"
        };
        _ = dataService.InitializeDataAsync();

        var book = entries[0];

        var id = dataService.AddItemAsync(book).Result;

        var target = dataService.GetItemAsync(id).Result;
        Assert.Multiple(() =>
        {
            Assert.That(book.Id, Is.EqualTo(target.Id));
            Assert.That(book.Category, Is.EqualTo(target.Category));
            Assert.That(book.Title, Is.EqualTo(target.Title));
            Assert.That(book.Path, Is.EqualTo(target.Path));
            Assert.That(book.UsageCount, Is.EqualTo(target.UsageCount));
        });
    }

    [Test]
    public void TestGetItems()
    {
        var dataService = new SqliteDataService
        {
            DbPath = $"{dbPath}{Path.GetRandomFileName()}"
        };
        _ = dataService.InitializeDataAsync();

        foreach (var entry in entries)
        {
            _ = dataService.AddItemAsync(entry).Result;
        }

        var targets = dataService.GetItemsAsync().Result;

        foreach (var target in targets)
        {
            Assert.Multiple(() =>
            {
                Assert.That(entries[target.Id - 1].Id, Is.EqualTo(target.Id));
                Assert.That(entries[target.Id - 1].Category, Is.EqualTo(target.Category));
                Assert.That(entries[target.Id - 1].Title, Is.EqualTo(target.Title));
                Assert.That(entries[target.Id - 1].Path, Is.EqualTo(target.Path));
                Assert.That(entries[target.Id - 1].UsageCount, Is.EqualTo(target.UsageCount));
            });
        }
    }

    [Test]
    public void TestDeleteItem()
    {
        // Arrange
        var dataService = new SqliteDataService
        {
            DbPath = $"{dbPath}{Path.GetRandomFileName()}"
        };
        _ = dataService.InitializeDataAsync();

        foreach (var entry in entries)
        {
            _ = dataService.AddItemAsync(entry).Result;
        }

        // Act
        var targets = dataService.GetItemsAsync().Result;
        Assert.That(entries, Has.Count.EqualTo(targets.Count));

        var book = entries[0];

        var result = dataService.DeleteItemAsync(book).Result;
        Assert.That(result, Is.True);

        targets = dataService.GetItemsAsync().Result;

        Assert.That(entries.Count - 1, Is.EqualTo(targets.Count));

        var found = false;
        foreach (var target in targets)
        {
            if (target.Title == book.Title)
            {
                found = true; break;
            }
        }

        Assert.That(found, Is.False);
    }

    [Test]
    public void TestUpdateItem()
    {
        // Arrange
        var dataService = new SqliteDataService
        {
            DbPath = $"{dbPath}{Path.GetRandomFileName()}"
        };
        _ = dataService.InitializeDataAsync();

        foreach (var entry in entries)
        {
            _ = dataService.AddItemAsync(entry).Result;
        }

        // Act
        foreach (var entry in entries)
        {
            entry.Category = "Computing & Technical";
            _ = dataService.UpdateItemAsync(entry);
        }

        var targets = dataService.GetItemsAsync().Result;

        // Assert
        for (var i = 0; i < targets.Count; i++)
        {
            var target = targets[i];
            var entry = entries[i];
            Assert.That(entry.Category, Is.EqualTo(target.Category));
        }
    }
}
