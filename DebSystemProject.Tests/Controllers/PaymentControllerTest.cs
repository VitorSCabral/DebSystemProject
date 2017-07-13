using NUnit.Framework;
using Moq;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using System;
using static DebSystemProject.Models.Repository.PaymentRepository;
using DebSystemProject.Models;
using DebSystemProject.Controllers;


namespace DebSystemProject.Tests.Controllers
{
    [TestFixture]
    public class PaymentControllerTest
    {
        private Mock<IPaymentRepository> paymentRepository;
        private Payment getPayment;
        private PaymentViewModel postPayment;

        [SetUp]
        public void TestInit()
        {
            paymentRepository = new Mock<IPaymentRepository>();
            getPayment = new Payment();

            getPayment.Id = 1;
            getPayment.Value = 200;
            getPayment.Date = DateTime.Now;
            getPayment.FriendIdIn = 1;
            getPayment.FriendIdOut = 2;

            postPayment = new PaymentViewModel();

            postPayment.Value = "300";
            postPayment.Date = "06/07/2017";
            postPayment.FriendNameIn = "Vitor";
            postPayment.FriendNameOut = "Thais";

        }

        [Test]
        public void CreatePayment()
        {
            // Arrange
            paymentRepository.Setup(p => p.Insert(It.IsAny<Debt>())).Returns(true);
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "payment" } });
            var controller = new PaymentController(paymentRepository.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:1175/api/Payment/CreatePayment?debtId={debtId}")
                {
                    Properties =
                {
                    { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                    { HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
                }
                }
            };

            // Act
            var response = controller.CreatePayment(1);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [Test]
        public void UpdatePayment()
        {
            // Arrange
            paymentRepository.Setup(x => x.GetPaymentById(1)).Returns(getPayment);
            paymentRepository.Setup(c => c.Update(It.IsAny<Payment>(), It.IsAny<int>())).Returns(true);
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "payment" } });
            var controller = new PaymentController(paymentRepository.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Put, "http://localhost:1175/api/Payment/UpdatePayment?paymentId={paymentId}")
                {
                    Properties =
                {
                    { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                    { HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
                }
                }
            };

            // Act
            var response = controller.UpdatePayment(postPayment, 1);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public void GetPaymentById()
        {
            // Arrange                           
            paymentRepository.Setup(x => x.GetPaymentById(1)).Returns(getPayment);
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "payment" } });
            var controller = new PaymentController(paymentRepository.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:1175/api/Payment/PaymentById?paymentId={paymentId}")
                {
                    Properties =
                {
                    { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                    { HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
                }
                }
            };

            // Act            
            var response = controller.GetPaymentById(1);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public void DeletePayment()
        {
            // Arrange
            paymentRepository.Setup(x => x.GetPaymentById(1)).Returns(getPayment);
            paymentRepository.Setup(c => c.DeleteObject(It.IsAny<Payment>(), It.IsAny<bool>())).Returns(true);
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "payment" } });
            var controller = new PaymentController(paymentRepository.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Delete, "http://localhost:1175/api/Payment/DeletePayment/{paymentId}")
                {
                    Properties =
                {
                    { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                    { HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
                }
                }
            };

            // Act
            var response = controller.DeletePaymentById(1);

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}