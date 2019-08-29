using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PlanCalculator.Core.AggregatesModel.InstallmentAggregate
{
    public interface IInstallmentService
    {
        Task<List<Installment>> GetInstallmentsAsync(decimal purchasePrice, DateTime purchaseDate);
    }
}
