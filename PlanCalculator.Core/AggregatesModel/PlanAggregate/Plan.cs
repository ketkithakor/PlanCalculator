using System;
using System.Collections.Generic;
using System.Text;

namespace PlanCalculator.Core.AggregatesModel.PlanAggregate
{
    public class Plan
    {
        public decimal FromPrice { get; set; }
        public decimal ToPrice { get; set; }
        public decimal DepositPercent { get; set; }
        public int InstallmentInterval { get; set; }
        public int InstallmentPeriod { get; set; }

       
    }
}
