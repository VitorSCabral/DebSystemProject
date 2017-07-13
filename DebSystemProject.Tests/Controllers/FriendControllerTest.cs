using DebSystemProject.Controllers;
using NUnit.Framework;
using DebSystemProject.Models;
using Moq;
using static DebSystemProject.Models.Repository.FriendRepository;
using System.Web.Http;
using System.Net;
using System.Net.Http;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;

namespace DebSystemProject.Tests.Controllers
{
    [TestFixture]
    public class FriendControllerTest
    {
        private Mock<IFriendRepository> friendRepository;
        private Friend getFriend;
        private FriendViewModel postFriend;

        [SetUp]
        public void TestInit()
        {
            friendRepository = new Mock<IFriendRepository>();
            getFriend = new Friend();

            getFriend.Id = 1;
            getFriend.Name = "Vitor";
            getFriend.Sex = "M";
            getFriend.Age = 28;

            postFriend = new FriendViewModel();

            postFriend.Name = "Vitor Cabral";
            postFriend.Sex = "M";
            postFriend.Age = "28";
        }       

        [Test]
        public void GetFriendById()
        {
            // Arrange                           
            friendRepository.Setup(x => x.GetFriendById(1)).Returns(getFriend);
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "friend" } });
            var controller = new FriendController(friendRepository.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:1175/api/Friend/FriendId?friendId={friendId}")
                {
                    Properties =
                {
                    { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                    { HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
                }
                }
            };

            // Act            
            var response = controller.GetFriendById(1);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public void CreateFriend()
        {
            // Arrange
            friendRepository.Setup(c => c.Insert(It.IsAny<Friend>())).Returns(true);
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "friend" } });
            var controller = new FriendController(friendRepository.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:1175/api/Friend/CreateFriend")
                {
                    Properties =
                {
                    { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                    { HttpPropertyKeys.HttpRouteDataKey, httpRouteData }                   
                }
                }
            };

            // Act
            var response = controller.CreateFriend(postFriend);

            // Assert
            Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        }

        [Test]
         public void UpdateFriend()
         {
            // Arrange
            friendRepository.Setup(x => x.GetFriendById(1)).Returns(getFriend);
            friendRepository.Setup(c => c.Update(It.IsAny<Friend>(), It.IsAny<int>())).Returns(true);            
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "friend" } });
            var controller = new FriendController(friendRepository.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Put, "http://localhost:1175/api/Friend/UpdateFriend?friendId={friendId}")
                {
                    Properties =
                {
                    { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                    { HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
                }
                }
            };

            // Act
            var response = controller.UpdateFriend(postFriend, 1);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [Test]
        public void DeleteFriend()
        {
            // Arrange
            friendRepository.Setup(x => x.GetFriendById(1)).Returns(getFriend);
            friendRepository.Setup(c => c.DeleteObject(It.IsAny<Friend>(), It.IsAny<bool>())).Returns(true);
            var httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);
            var httpRouteData = new HttpRouteData(httpConfiguration.Routes["DefaultApi"],
                new HttpRouteValueDictionary { { "controller", "friend" } });
            var controller = new FriendController(friendRepository.Object)
            {
                Request = new HttpRequestMessage(HttpMethod.Delete, "http://localhost:1175/api/Friend/DeleteFriend/{friendId}")
                {
                    Properties =
                {
                    { HttpPropertyKeys.HttpConfigurationKey, httpConfiguration },
                    { HttpPropertyKeys.HttpRouteDataKey, httpRouteData }
                }
                }
            };

            // Act
            var response = controller.DeleteFriendById(1);

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
