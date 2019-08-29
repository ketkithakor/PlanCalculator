﻿using System;
using System.Threading.Tasks;
using PlanCalculator.Core.AggregatesModel.PlanAggregate;
using PlanCalculator.Core.AggregatesModel.InstallmentAggregate;
using PlanCalculator.Core.Services;
using PlanCalculator.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.IO;
using Serilog;

namespace PlanCalculator.Console
{
    class Program
    {
        private static IServiceProvider _serviceProvider;
        public static IConfiguration Configuration { get; } = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
        static void Main(string[] args)
        {
            try
            {                
                PlanCalculatorMainApp().GetAwaiter();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "ERROR: Terminated unexpectedly!");                
                throw ex;
            }
            finally
            {
                Log.CloseAndFlush();
                DisposeServices();
            }
            
        }
        private static async Task ConfigureLogging()
        {
            //configure logging
            await Task.FromResult(Log.Logger = new LoggerConfiguration()
              .ReadFrom.Configuration(Configuration)
              .CreateLogger());
        }
        private static async Task PlanCalculatorMainApp()
        {
            await RegisterServices();
            await ConfigureLogging();

            Log.Information("Start application");

            decimal pPrice = await IsValidPurchasePrice(await EnterPurchasePrice());
            if (pPrice == 0) { return; }

            DateTime pDate = await IsValidPurchaseDate(await EnterPurchaseDate());
            if ( pDate == DateTime.MinValue) { return; }

            await DisplayInstallments(pPrice, pDate);
        }

        private static async Task DisplayInstallments(decimal pPrice, DateTime pDate)
        {
            try {
                var installmentService = _serviceProvider.GetService<IInstallmentService>();
                var installments = await installmentService.GetInstallmentsAsync(pPrice, pDate);

                if (installments != null && installments.Count > 0)
                {
                    foreach (Installment i in installments)
                    {
                        System.Console.WriteLine("--------------------");
                        System.Console.WriteLine("Deposit Amount:" + i.GetDepositAmount());
                        System.Console.WriteLine("Installment Amount:" + i.GetInstallmentAmount());
                        var pdates = i.PaymentDates();
                        System.Console.WriteLine("Payment Dates:");
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
            catch (Exception ex)
            {
                Log.Error(ex, "ERROR in DisplayInstallments method");
                throw ex;
            }
            
        }
        private static async Task RegisterServices()
        {
            await Task.FromResult(_serviceProvider = new ServiceCollection()
                .AddSingleton(Configuration)
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

        private static void DisposeServices()
        {
            if (_serviceProvider == null)
            {
                return;
            }
            if (_serviceProvider is IDisposable)
            {
                ((IDisposable)_serviceProvider).Dispose();
            }
        }
    }
}
