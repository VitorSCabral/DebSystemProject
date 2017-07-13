using DebSystemProject.Controllers;
using NUnit.Framework;
using DebSystemProject.Models;
using Moq;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using static DebSystemProject.Models.Repository.DebtRepository;
using System;
using System.Collections.Generic;

namespace DebSystemProject.Tests.Controllers
{
    [TestFixture]
    public class DebtControllerTest
    {
        private Mock<IDebtRepository> debtRepository;
        private Debt getDebt1;
        private Debt getDebt2;
        private DebtViewModel postDebt;
        private IList<Debt> debtList;

        [SetUp]
        public void TestInit()
        {
            debtRepository = new Mock<IDebtRepository>();
            getDebt1 = new Debt();

            getDebt1.Id = 1;
            getDebt1.Value = 200;
            getDebt1.Date = DateTime.Now;
            getDebt1.Description = "Outback";
            getDebt1.FriendIdIn = 1;
            getDebt1.FriendIdOut = 2;

            getDebt2 = new Debt();

            getDebt2.Id = 2;
            getDebt2.Value = 300;
            getDebt2.Date = DateTime.Now;
            getDebt2.Description = "BK";
            getDebt2.FriendIdIn = 1;
            getDebt2.FriendIdOut = 2;

            postDebt = new DebtViewModel();

            postDebt.Value = "300";
            postDebt.Date = "06/07/2017";
            postDebt.Description = "BK";
            postDebt.FriendNameIn = "Vitor";
            postDebt.FriendNameOut = "Thais";

            debtList = new List<Debt>();

            debtList.Add(getDebt1);
            debtList.Add(getDebt2);

        }

        [Test]
        public void CreateDebt()
        {
            // Arrange
            debtRepository.Setup(d => d.Insert(It.IsAny<Debt>())).Returns(true);
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "debt" } });
            var controller = new DebtController(debtRepository.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:1175/api/Debt/CreateDebt")
                {
                    Properties =
                {
                    { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                    { HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
                }
                }
            };

            // Act
            var response = controller.CreateDebt(postDebt);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [Test]
        public void GetListByFriendName()
        {
            // Arrange                           
            debtRepository.Setup(x => x.GetDebtListByFriendName("Vitor")).Returns(debtList);
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "debt" } });
            var controller = new DebtController(debtRepository.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:1175/api/Debt/ListDebt?name={name}")
                {
                    Properties =
                {
                    { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                    { HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
                }
                }
            };

            // Act            
            var response = controller.GetListByFriendName("Vitor");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public void GetDebtById()
        {
            // Arrange                           
            debtRepository.Setup(x => x.GetDebtById(1)).Returns(getDebt1);
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "debt" } });
            var controller = new DebtController(debtRepository.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:1175/api/Debt/DebtById?debtId={debtId}")
                {
                    Properties =
                {
                    { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                    { HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
                }
                }
            };

            // Act            
            var response = controller.GetDebtById(1);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public void UpdateDebt()
        {
            // Arrange
            debtRepository.Setup(x => x.GetDebtById(1)).Returns(getDebt1);
            debtRepository.Setup(c => c.Update(It.IsAny<Debt>(), It.IsAny<int>())).Returns(true);
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "debt" } });
            var controller = new DebtController(debtRepository.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Put, "http://localhost:1175/api/Debt/UpdateDebt?debtId={debtId}")
                {
                    Properties =
                {
                    { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                    { HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
                }
                }
            };

            // Act
            var response = controller.UpdateDebt(postDebt, 1);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public void DeleteDebt()
        {
            // Arrange
            debtRepository.Setup(x => x.GetDebtById(1)).Returns(getDebt1);
            debtRepository.Setup(c => c.DeleteObject(It.IsAny<Debt>(), It.IsAny<bool>())).Returns(true);
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "debt" } });
            var controller = new DebtController(debtRepository.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Delete, "http://localhost:1175/api/Debt/DeleteDebt/{debtId}")
                {
                    Properties =
                {
                    { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                    { HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
                }
                }
            };

            // Act
            var response = controller.DeleteDebtById(1);

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
