using System;
using System.Collections.Generic;
using System.Text;

namespace PlanCalculator.Core.AggregatesModel.InstallmentAggregate
{
    public class Installment
    {
        private decimal _purchasePrice;
        private decimal _depositPercent;
        private int _installmentPeriod;
        private int _installmentInterval;
        private DateTime _purchaseDate;
        
        public Installment(decimal purchasePrice, DateTime purchaseDate, decimal depositPercent, int installmentInterval, int installmentPeriod)
        {
            _purchasePrice = purchasePrice;
            _depositPercent = depositPercent;
            _installmentPeriod = installmentPeriod;
            _installmentInterval = installmentInterval;
            _purchaseDate = purchaseDate;
        }

        public decimal GetDepositAmount()
        {
            return (_purchasePrice * _depositPercent) / 100;
        }

        public decimal GetInstallmentAmount()
        {
            return (_purchasePrice - GetDepositAmount()) / _installmentPeriod;
        }

        public IEnumerable<DateTime> PlanmentDates()
        {            
            DateTime purDate=_purchaseDate;
            for (int i = 0; i < _installmentPeriod; i++)
            {                
                purDate = purDate.AddDays(_installmentInterval);
                yield return purDate;         
            }           
        }
    }
}
