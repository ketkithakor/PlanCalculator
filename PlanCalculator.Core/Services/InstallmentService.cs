﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PlanCalculator.Core.AggregatesModel.InstallmentAggregate;
using PlanCalculator.Core.AggregatesModel.PlanAggregate;
namespace PlanCalculator.Core.Services
{
    public class InstallmentService : IInstallmentService
    {
        private IPlanRepository _planRepository;
        public InstallmentService(IPlanRepository planRepository)
        {
            _planRepository = planRepository;
        }
        public async Task<List<Installment>> GetInstallmentsAsync(decimal purchasePrice, DateTime purchaseDate)
        {
            var planList= await _planRepository.GetPlansByPriceAsync(purchasePrice);
            List<Installment> installments=new List<Installment>();
            foreach (Plan p in planList)
            {
                installments.Add(new Installment(purchasePrice,purchaseDate,p.DepositPercent,p.InstallmentInterval,p.InstallmentPeriod));
            }
            return installments;

        }
    }
}
