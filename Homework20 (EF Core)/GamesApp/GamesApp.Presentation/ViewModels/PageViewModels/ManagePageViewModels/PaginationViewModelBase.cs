using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GamesApp.Domain.Entities;
using GamesApp.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace GamesApp.Presentation.ViewModels.PageViewModels.ManagePageViewModels;

public partial class PaginationViewModelBase<TEntity> : ObservableObject where TEntity : class, IEntity
{
    [ObservableProperty]
    public partial int CurrentPage { get; set; } = 1;

    [ObservableProperty]
    public partial int TotalPages { get; set; }

    public ObservableCollection<TEntity> Entities { get; set; } = new();

    public bool CanGoPrevious => CurrentPage > 1;
    public bool CanGoNext => CurrentPage < TotalPages;
    public string CurrentPageText => $"Page {CurrentPage} / {TotalPages}";

    private const int PageSize = 10;

    protected IQueryable<TEntity> EntitiesSet;
    protected readonly DbContext Context;

    public PaginationViewModelBase(GamesAppContext context, IQueryable<TEntity> entitiesSet)
    {
        Context = context;
        EntitiesSet = entitiesSet;
        context.SavedChanges += async (s, e) => await UpdatePageAsync();

        UpdatePageAsync();
    }

    private async Task UpdatePageAsync()
    {
        TotalPages = Math.Max(1, (int)Math.Ceiling((double)await EntitiesSet.CountAsync() / PageSize));
        if (CurrentPage > TotalPages) CurrentPage = TotalPages;

        Entities.Clear();
        var items = await EntitiesSet
            .OrderBy(c => c.Name)
            .Skip((CurrentPage - 1) * PageSize)
            .Take(PageSize)
            .ToListAsync();

        foreach (var item in items)
            Entities.Add(item);

        OnPropertyChanged(nameof(CanGoPrevious));
        OnPropertyChanged(nameof(CanGoNext));
        OnPropertyChanged(nameof(CurrentPageText));
    }

    [RelayCommand]
    private async Task NextPage()
    {
        if (CanGoNext)
        {
            CurrentPage++;
            await UpdatePageAsync();
        }
    }

    [RelayCommand]
    private async Task PreviousPage()
    {
        if (CanGoPrevious)
        {
            CurrentPage--;
            await UpdatePageAsync();
        }
    }
}
