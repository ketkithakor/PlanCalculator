using System;
using System.Threading.Tasks;
using PlanCalculator.Core.AggregatesModel.PlanAggregate;
using PlanCalculator.Core.AggregatesModel.InstallmentAggregate;
using PlanCalculator.Core.Services;
using PlanCalculator.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Collections.Generic;

namespace PlanCalculator.Console
{
    class Program
    {
        private static IServiceProvider _serviceProvider;
        static void Main(string[] args)
        {
            PlanCalculatorMainApp().GetAwaiter();
        }
        private static async Task PlanCalculatorMainApp()
        {
            await RegisterServices();

            decimal pPrice = await IsValidPurchasePrice(await EnterPurchasePrice());
            if (pPrice == 0) { return; }

            DateTime pDate = await IsValidPurchaseDate(await EnterPurchaseDate());
            if ( pDate == DateTime.MinValue) { return; }

            await DisplayInstallments(pPrice, pDate);
        }

        private static async Task DisplayInstallments(decimal pPrice, DateTime pDate)
        {
            var installmentService = _serviceProvider.GetService<IInstallmentService>();
            var installments = await installmentService.GetInstallmentsAsync(pPrice, pDate);

            if (installments != null && installments.Count > 0)
            {
                foreach (Installment i in installments)
                {
                    System.Console.WriteLine("--------------------");
                    System.Console.WriteLine("Deposit Amount:" + i.GetDepositAmount());
                    System.Console.WriteLine("Installment Amount:" + i.GetInstallmentAmount());
                    var pdates = i.PlanmentDates();
                    System.Console.WriteLine("Planment Dates:");
                    foreach (DateTime d in pdates)
                    {
                        System.Console.WriteLine(d.ToString("dd/MM/yyyy"));
                    }
                    System.Console.WriteLine("--------------------");
                }
            }
            else
            {
                System.Console.WriteLine("Plans not offered for this price.");
            }
        }
        private static async Task RegisterServices()
        {
            await Task.FromResult(_serviceProvider = new ServiceCollection()
                .AddTransient<IPlanRepository, PlanRepository>()
                .AddTransient<IInstallmentService, InstallmentService>()
                .BuildServiceProvider());
        }
        private static async Task<decimal> IsValidPurchasePrice(string purchasePrice)
        {
            decimal pPrice;
            if (!Decimal.TryParse(purchasePrice, out pPrice))
            {
                System.Console.WriteLine("Invalid purchase price. Price should be number only (example: 100).");
            }
            return await Task.FromResult(pPrice);
        }


        private static async Task<DateTime> IsValidPurchaseDate(string purchaseDate)
        {
            DateTime pDate;
            if (!DateTime.TryParse(purchaseDate, out pDate))
            {
                System.Console.WriteLine("Invalid purchase date. It should be like 15/07/2019");
            }
            return await Task.FromResult(pDate);
        }

        private static async Task<string> EnterPurchasePrice()
        {
            System.Console.WriteLine("Enter purchase price:");
            return await Task.FromResult(System.Console.ReadLine());
        }

        private static async Task<string> EnterPurchaseDate()
        {
            System.Console.WriteLine("Enter purchase date:");
            return await Task.FromResult(System.Console.ReadLine());
        }
    }
}
