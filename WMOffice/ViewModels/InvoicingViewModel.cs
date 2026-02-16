using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WMOffice.Data;
using WMOffice.Models;

namespace WMOffice.ViewModels
{
    public partial class InvoicingViewModel : ObservableObject
    {
        private readonly AppDbContext _context;
        private List<Invoice> _allInvoices = new();

        [ObservableProperty]
        private ObservableCollection<Invoice> _invoices = new();

        [ObservableProperty]
        private string _selectedFilter = "All";

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private string _newInvoiceNumber;
        
        [ObservableProperty]
        private decimal _newAmount;

        [ObservableProperty]
        private DateTimeOffset? _newDate = DateTimeOffset.Now;

        [ObservableProperty]
        private string _newStatus = "Unpaid";

        public ObservableCollection<string> FilterOptions { get; } = new ObservableCollection<string> { "All", "Paid", "Unpaid" };
        public ObservableCollection<string> StatusOptions { get; } = new ObservableCollection<string> { "Paid", "Unpaid" };

        public InvoicingViewModel()
        {
            _context = new AppDbContext();
            // Ensure DB created
            _context.Database.EnsureCreated();
            
            LoadInvoicesAsync();
        }

        partial void OnSelectedFilterChanged(string value)
        {
            ApplyFilter();
        }

        public async void LoadInvoicesAsync()
        {
            if (IsLoading) return;
            IsLoading = true;
            try
            {
                _allInvoices = await _context.Invoices.ToListAsync();
                ApplyFilter();
            }
            catch (Exception ex)
            {
                // In a real app, log or show error
                System.Diagnostics.Debug.WriteLine($"Error loading invoices: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void ApplyFilter()
        {
            if (SelectedFilter == "All")
            {
                Invoices = new ObservableCollection<Invoice>(_allInvoices);
            }
            else
            {
                var filtered = _allInvoices.Where(i => i.Status == SelectedFilter).ToList();
                Invoices = new ObservableCollection<Invoice>(filtered);
            }
        }

        [RelayCommand]
        public async Task AddInvoiceAsync()
        {
            if (string.IsNullOrWhiteSpace(NewInvoiceNumber)) return;

            var invoice = new Invoice
            {
                InvoiceNumber = NewInvoiceNumber,
                Date = NewDate?.DateTime ?? DateTime.Now,
                Amount = NewAmount,
                Status = NewStatus
            };

            try
            {
                _context.Invoices.Add(invoice);
                await _context.SaveChangesAsync();
                
                _allInvoices.Add(invoice);
                ApplyFilter();

                // Reset form
                NewInvoiceNumber = string.Empty;
                NewAmount = 0;
                NewDate = DateTimeOffset.Now;
                NewStatus = "Unpaid";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding invoice: {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task DeleteInvoiceAsync(Invoice invoice)
        {
            if (invoice == null) return;
            
            try
            {
                _context.Invoices.Remove(invoice);
                await _context.SaveChangesAsync();
                
                _allInvoices.Remove(invoice);
                ApplyFilter();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting invoice: {ex.Message}");
            }
        }
    }
}
