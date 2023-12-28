using CommunityToolkit.Mvvm.ComponentModel;
using Dapper.Contrib.Extensions;

namespace BookCatalog.Core.Models;

public class BookItem : ObservableObject
{
    [Key]
    public int Id
    {
        get; set;
    }

    public string Category
    {
        get; set;
    }

    public string Title
    {
        get; set;
    }

    public string Path
    {
        get; set;
    }

    public int UsageCount
    {
        get; set;
    }

    public override string ToString() => $"{Title}";
}
