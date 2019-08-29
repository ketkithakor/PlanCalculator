using System;
using System.Collections.Generic;
using System.Text;
using PlanCalculator.Core.Services;
using PlanCalculator.Core.AggregatesModel.PlanAggregate;
using Moq;
using Xunit;
using System.Threading.Tasks;

namespace PlanCalculator.UnitTests
{
    public class InstallmentServiceTest
    {
        private readonly Mock<IPlanRepository> _planRepositoryMock;
        public InstallmentServiceTest()
        {
            _planRepositoryMock = new Mock<IPlanRepository>();
        }

        [Fact]
        public async Task Get_Installment()
        {
            //Arrange
            decimal purchasePrice = 100;
            DateTime purchaseDate = System.DateTime.Now;

            _planRepositoryMock.Setup(pr => pr.GetPlansByPriceAsync(It.IsAny<decimal>())).ReturnsAsync(GetPlans());

            //Act
            var installmentService = new InstallmentService(_planRepositoryMock.Object);
            var result = await installmentService.GetInstallmentsAsync(purchasePrice, purchaseDate);

            //Assert
            _planRepositoryMock.Verify(pr => pr.GetPlansByPriceAsync(It.IsAny<decimal>()), Times.Once);
            Assert.Equal(2, result.Count);            
            
        }

        private List<Plan> GetPlans()
        {
            return new List<Plan>() {
                new Plan{ FromPrice=100,ToPrice=1000,DepositPercent=20,InstallmentInterval=15,InstallmentPeriod=5},
                new Plan{ FromPrice=100,ToPrice=1000,DepositPercent=30,InstallmentInterval=15,InstallmentPeriod=4},                
            };
        }
    }
}
