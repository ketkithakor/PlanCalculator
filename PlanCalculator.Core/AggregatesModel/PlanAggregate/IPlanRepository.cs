using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace PlanCalculator.Core.AggregatesModel.PlanAggregate
{
    public interface IPlanRepository
    {
        Task<IEnumerable<Plan>> GetPlansByPriceAsync(decimal purchasePrice);
    }
}
