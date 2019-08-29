using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using PlanCalculator.Core.AggregatesModel.PlanAggregate;
namespace PlanCalculator.Infrastructure.Repositories
{
    public class PlanRepository : IPlanRepository
    {
        private List<Plan> _planList;
        public PlanRepository()
        {
            _planList = GetAllPlans();
        }
        public async Task<IEnumerable<Plan>> GetPlansByPriceAsync(decimal purchasePrice)
        {
            return await Task.FromResult(_planList.FindAll(p => p.FromPrice <= purchasePrice && p.ToPrice >= purchasePrice));            
        }
        private List<Plan> GetAllPlans()
        {
            return new List<Plan>() {
                new Plan{ FromPrice=100,ToPrice=1000,DepositPercent=20,InstallmentInterval=15,InstallmentPeriod=5},
                new Plan{ FromPrice=100,ToPrice=1000,DepositPercent=30,InstallmentInterval=15,InstallmentPeriod=4},
                new Plan{ FromPrice=1000,ToPrice=10000,DepositPercent=20,InstallmentInterval=30,InstallmentPeriod=4}
            };
        }
        
    }
}
