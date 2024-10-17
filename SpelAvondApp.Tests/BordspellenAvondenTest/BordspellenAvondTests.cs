using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SpelAvondApp.Application;
using SpelAvondApp.Domain.Models;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SpelAvondApp.Tests.BordspellenAvondenTest
{
    [TestClass]
    public class BordspellenAvondTests
    {
        private Mock<IBordspellenAvondService> _avondServiceMock;
        private Mock<IInschrijvingService> _inschrijvingServiceMock;
        private Mock<UserManager<ApplicationUser>> _userManagerMock;
        private BordspellenAvondController _controller;
        private ApplicationUser _user;

        [TestInitialize]
        public void Setup()
        {
            _avondServiceMock = new Mock<IBordspellenAvondService>();
            _inschrijvingServiceMock = new Mock<IInschrijvingService>();

            _userManagerMock = MockUserManager();
            _user = new ApplicationUser { Id = "user1", UserName = "testuser" };

            _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(_user);
            _userManagerMock.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(_user.Id);

            _controller = new BordspellenAvondController(_avondServiceMock.Object, _userManagerMock.Object, _inschrijvingServiceMock.Object);
        }

        private Mock<UserManager<ApplicationUser>> MockUserManager()
        {
            var store = new Mock<IUserStore<ApplicationUser>>();
            return new Mock<UserManager<ApplicationUser>>(
                store.Object, null, null, null, null, null, null, null, null);
        }

        [TestMethod]
        public async Task Index_Geeft_AlleAvondenTerug()
        {
            var allAvonden = new List<BordspellenAvond>
            {
                new BordspellenAvond { Id = 1, OrganisatorId = "user1" },
                new BordspellenAvond { Id = 2, OrganisatorId = "user2" }
            };
            _avondServiceMock.Setup(s => s.GetAllAvondenAsync()).ReturnsAsync(allAvonden);

   
            var result = await _controller.Index() as ViewResult;
            var model = result?.Model as List<BordspellenAvond>;

            // Assert
            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Count);
            Assert.IsTrue(model.Any(a => a.OrganisatorId == "user1"));
            Assert.IsTrue(model.Any(a => a.OrganisatorId == "user2"));
        }

        [TestMethod]
        public async Task BeheerdersOverzicht_Geeft_GeorganiseerdeAVondenTerug()
        {
            // Arrange
            var userId = "user1";
            var organizedAvonden = new List<BordspellenAvond>
    {
        new BordspellenAvond
        {
            Id = 1,
            OrganisatorId = userId,
            Datum = DateTime.Now,
            Adres = "Teststraat 123",
            MaxAantalSpelers = 5,
            Is18Plus = false,
            BiedtLactosevrijeOpties = true,
            BiedtNotenvrijeOpties = true,
            BiedtVegetarischeOpties = false,
            BiedtAlcoholvrijeOpties = true
        },
        new BordspellenAvond
        {
            Id = 2,
            OrganisatorId = userId,
            Datum = DateTime.Now,
            Adres = "Testlaan 456",
            MaxAantalSpelers = 5,
            Is18Plus = false,
            BiedtLactosevrijeOpties = false,
            BiedtNotenvrijeOpties = true,
            BiedtVegetarischeOpties = true,
            BiedtAlcoholvrijeOpties = true
        }
    };

            _userManagerMock.Setup(um => um.GetUserId(It.IsAny<ClaimsPrincipal>())).Returns(userId);
            _avondServiceMock.Setup(s => s.GetAvondenByOrganisatorAsync(userId)).ReturnsAsync(organizedAvonden);
            _inschrijvingServiceMock.Setup(s => s.GetAvondWithInschrijvingenAndUserNamesAsync(It.IsAny<int>()))
            .ReturnsAsync((int id) => organizedAvonden.FirstOrDefault(a => a.Id == id));

            var result = await _controller.BeheerdersOverzicht() as ViewResult;


            Assert.IsNotNull(result, "Expected ViewResult, but got null.");

            var model = result.Model as List<BordspellenAvond>;

            Assert.IsNotNull(model, "Expected model to be a list of BordspellenAvond, but got null.");
            Assert.AreEqual(2, model.Count, "Expected model to contain exactly 2 BordspellenAvond items.");

      
            foreach (var avond in model)
            {
                if (avond == null)
                {
                    Console.WriteLine("Null BordspellenAvond found in model.");
                }
                else
                {
                    Console.WriteLine($"Avond ID: {avond.Id}, OrganisatorId: {avond.OrganisatorId}");
                }
            }

            Assert.IsTrue(model.All(a => a != null && a.OrganisatorId == userId), "Expected all avonden to be organized by user1.");
        }


        [TestMethod]
        public async Task MijnIngeschrevenAvonden_Lijst()
        {
            var joinedAvonden = new List<BordspellenAvond>
            {
                new BordspellenAvond { Id = 1 },
                new BordspellenAvond { Id = 2 }
            };
            _avondServiceMock.Setup(s => s.GetAvondenWaarIngeschrevenAsync(_user.Id)).ReturnsAsync(joinedAvonden);

            var result = await _controller.MijnIngeschrevenAvonden() as ViewResult;
            var model = result?.Model as List<BordspellenAvond>;

            Assert.IsNotNull(model);
            Assert.AreEqual(2, model.Count);
        }

        // Foutscenario's

        [TestMethod]
        public async Task Index_NoAvonden_ReturnsEmptyList()
        {
            _avondServiceMock.Setup(s => s.GetAllAvondenAsync()).ReturnsAsync(new List<BordspellenAvond>());

            var result = await _controller.Index() as ViewResult;
            var model = result?.Model as List<BordspellenAvond>;

            Assert.IsNotNull(model);
            Assert.AreEqual(0, model.Count, "Expected no avonden to be available.");
        }

        [TestMethod]
        public async Task BeheerdersOverzicht_NoOrganizedAvonden_ReturnsEmptyList()
        {
            _avondServiceMock.Setup(s => s.GetAvondenByOrganisatorAsync(_user.Id)).ReturnsAsync(new List<BordspellenAvond>());

            var result = await _controller.BeheerdersOverzicht() as ViewResult;
            var model = result?.Model as List<BordspellenAvond>;

            Assert.IsNotNull(model);
            Assert.AreEqual(0, model.Count, "Expected no organized avonden for the user.");
        }

        [TestMethod]
        public async Task MijnIngeschrevenAvonden_NoJoinedAvonden_ReturnsEmptyList()
        {
            _avondServiceMock.Setup(s => s.GetAvondenWaarIngeschrevenAsync(_user.Id)).ReturnsAsync(new List<BordspellenAvond>());

            var result = await _controller.MijnIngeschrevenAvonden() as ViewResult;
            var model = result?.Model as List<BordspellenAvond>;

            Assert.IsNotNull(model);
            Assert.AreEqual(0, model.Count, "Expected no joined avonden for the user.");
        }
    }
}
