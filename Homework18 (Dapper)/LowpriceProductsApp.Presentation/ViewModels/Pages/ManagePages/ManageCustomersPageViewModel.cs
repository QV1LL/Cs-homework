using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LowpriceProductsApp.Domain.Entities;
using LowpriceProductsApp.Domain.Enums;
using LowpriceProductsApp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LowpriceProductsApp.Presentation.ViewModels.Pages.ManagePages;

public partial class ManageCustomersPageViewModel : ObservableObject
{
    [ObservableProperty]
    public partial string Name { get; set; } = string.Empty;
    [ObservableProperty]
    public partial Gender Gender { get; set; }
    [ObservableProperty]
    public partial string Email {  get; set; } = string.Empty;
    [ObservableProperty]
    public partial City City { get; set; } = null!;


    [ObservableProperty]
    public partial bool IsErrorVisible { get; set; } = false;

    [ObservableProperty]
    public partial string ErrorMessage { get; set; } = string.Empty;

    public Customer? SelectedCustomer
    {
        get => field;
        set
        {
            SetProperty(ref field, value);

            IsCustomerSelected = field != null;

            if (IsCustomerSelected)
            {
                Name = field?.Name ?? string.Empty;
                Gender = field!.Gender;
                Email = field?.Email ?? string.Empty;
                City = Cities.First(c => c.Id == field!.City.Id);
            }
        }
    }

    [ObservableProperty]
    public partial bool IsCustomerSelected { get; set; }

    public ObservableCollection<Customer> Customers { get; set; } = new();
    public List<City> Cities { get; } = new ();
    public IEnumerable<Gender> Genders => Enum.GetValues<Gender>();

    private readonly ICustomersRepository _customersRepository;

    public ManageCustomersPageViewModel(
        ICustomersRepository customersRepository,
        ICitiesRepository citiesRepository)
    {
        _customersRepository = customersRepository;
        Cities.AddRange(citiesRepository.GetAll());

        UpdateCollection();
    }

    [RelayCommand]
    public void Add(object? parameter)
    {
        try
        {
            var customer = new Customer
            {
                Name = Name,
                Gender = Gender,
                Email = Email,
                City = City,
            };

            if (_customersRepository.Find(c => c.Email == customer.Email) != null)
                throw new ArgumentException($"Customer with email {customer.Email} is already exist");

            _customersRepository.Add(customer);
            UpdateCollection();
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
            IsErrorVisible = true;
        }
    }

    [RelayCommand]
    public void Update(object? parameter)
    {
        try
        {
            if (SelectedCustomer == null)
                throw new ArgumentNullException("No one customer is selected");

            var customerToUpdate = new Customer
            {
                Id = SelectedCustomer.Id,
                Name = Name,
                Gender = Gender,
                Email = Email,
                City = City,
            };

            _customersRepository.Add(customerToUpdate);
            UpdateCollection();
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
            IsErrorVisible = true;
        }
    }

    [RelayCommand]
    public void Delete(object? parameter)
    {
        try
        {
            if (SelectedCustomer == null)
                throw new ArgumentNullException("No one customer is selected");

            _customersRepository.Remove(SelectedCustomer);
            UpdateCollection();
        }
        catch (Exception e)
        {
            ErrorMessage = e.Message;
            IsErrorVisible = true;
        }
    }

    private void UpdateCollection()
    {
        Customers.Clear();
        foreach (var customer in _customersRepository.GetAll())
            Customers.Add(customer);

        IsErrorVisible = false;
    }
}
