using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SpelAvondApp.Application;
using SpelAvondApp.Controllers;
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
        private InschrijvingenController _inschrijvingController;
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
            _inschrijvingController = new InschrijvingenController(_inschrijvingServiceMock.Object, _userManagerMock.Object);
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
        //user story 1

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
        public async Task GeenAvonden_Returns_EmptyList()
        {
            // Arrange
            _avondServiceMock.Setup(s => s.GetAllAvondenAsync()).ReturnsAsync(new List<BordspellenAvond>());

            // Act
            var result = await _controller.Index() as ViewResult;
            var model = result?.Model as List<BordspellenAvond>;

            // Assert
            Assert.IsNotNull(model);
            Assert.AreEqual(0, model.Count);
        }

        //user story 2
        [TestMethod]
        public async Task Create_New_BordspellenAvond_Success()
        {
            // Arrange
            var user = new ApplicationUser { Id = "user1", UserName = "testorganizer", Geboortedatum = new DateTime(2000,1,1) };
            var avond = new BordspellenAvond
            {
                Id = 1,
                OrganisatorId = user.Id,
                Adres = "Teststraat 123",
                MaxAantalSpelers = 5,
                Datum = DateTime.Now,
                Is18Plus = false,
                BiedtLactosevrijeOpties = false,
                BiedtNotenvrijeOpties = true,
                BiedtVegetarischeOpties = true,
                BiedtAlcoholvrijeOpties = true
            };

            _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            _avondServiceMock.Setup(s => s.IsUserEligibleToOrganizeAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(true);
            _avondServiceMock.Setup(s => s.CreateBordspellenAvondAsync(avond, It.IsAny<List<int>>())).Returns(Task.CompletedTask); 
            _avondServiceMock.Setup(s => s.ValidateBordspellenAvond(avond, It.IsAny<List<int>>())).ReturnsAsync(true);

            var tempDataMock = new Mock<ITempDataDictionary>();
            _controller.TempData = tempDataMock.Object;

            // Act
            var result = await _controller.Create(avond, new List<int> { 1, 2 }) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            _avondServiceMock.Verify(s => s.CreateBordspellenAvondAsync(It.IsAny<BordspellenAvond>(), It.IsAny<List<int>>()), Times.Once);
            tempDataMock.VerifySet(tempData => tempData["SuccessMessage"] = "De bordspellenavond is succesvol aangemaakt.");

        }

        [TestMethod]
        public async Task Edit_BordspellenAvond_Success_When_NoInschrijvingen()
        {
            // Arrange
            var user = new ApplicationUser { Id = "user1", UserName = "testorganizer" };
            var avond = new BordspellenAvond
            {
                Id = 1,
                OrganisatorId = user.Id,
                Inschrijvingen = new List<Inschrijving>()
            };

            _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            _avondServiceMock.Setup(s => s.GetAvondByIdAsync(avond.Id)).ReturnsAsync(avond);
            _avondServiceMock.Setup(s => s.UpdateBordspellenAvondAsync(avond, It.IsAny<List<int>>())).Returns(Task.CompletedTask);
            _avondServiceMock.Setup(s => s.UserCanEditOrDeleteAsync(avond.Id, user.Id)).ReturnsAsync(true);
            _avondServiceMock.Setup(s => s.GetAllBordspellenAsync()).ReturnsAsync(new List<Bordspel>
            {
                new Bordspel { Id = 1, Naam = "Catan" },
                new Bordspel { Id = 2, Naam = "Risk" }
            });

            // Act
            var result = await _controller.Edit(avond.Id) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            _avondServiceMock.Verify(s => s.GetAvondByIdAsync(avond.Id), Times.Once);
        }

        [TestMethod]
        public async Task Delete_BordspellenAvond_Success_When_NoInschrijvingen()
        {
            // Arrange
            var user = new ApplicationUser { Id = "user1", UserName = "testorganizer" };
            var avond = new BordspellenAvond
            {
                Id = 1,
                OrganisatorId = user.Id,
                Inschrijvingen = new List<Inschrijving>()
            };

            _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            _avondServiceMock.Setup(s => s.GetAvondByIdAsync(avond.Id)).ReturnsAsync(avond);
            _avondServiceMock.Setup(s => s.DeleteBordspellenAvondAsync(avond.Id)).Returns(Task.CompletedTask);
            _avondServiceMock.Setup(s => s.UserCanEditOrDeleteAsync(avond.Id, user.Id)).ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteConfirmed(avond.Id) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            _avondServiceMock.Verify(s => s.DeleteBordspellenAvondAsync(avond.Id), Times.Once);
        }

        //Foutscenario's

        [TestMethod]
        public async Task Edit_BordspellenAvond_Fails_When_Inschrijvingen_Exist()
        {
            // Arrange
            var user = new ApplicationUser { Id = "user1", UserName = "testorganizer" };
            var avond = new BordspellenAvond
            {
                Id = 1,
                OrganisatorId = user.Id,
                Inschrijvingen = new List<Inschrijving> { new Inschrijving() } // Er zijn inschrijvingen
            };

            _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            _avondServiceMock.Setup(s => s.GetAvondByIdAsync(avond.Id)).ReturnsAsync(avond);
            var tempDataMock = new Mock<ITempDataDictionary>();
            _controller.TempData = tempDataMock.Object;

            var result = await _controller.Edit(avond.Id) as RedirectToActionResult;
        
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            tempDataMock.VerifySet(tempData => tempData["ErrorMessage"] = "Deze avond kan niet worden bewerkt omdat er al inschrijvingen zijn.");
        }
        [TestMethod]
        public async Task Delete_BordspellenAvond_Fails_When_Inschrijvingen_Exist()
        {
            // Arrange
            var user = new ApplicationUser { Id = "user1", UserName = "testorganizer" };
            var avond = new BordspellenAvond
            {
                Id = 1,
                OrganisatorId = user.Id,
                Inschrijvingen = new List<Inschrijving> { new Inschrijving() } // Er zijn inschrijvingen
            };

            _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            _avondServiceMock.Setup(s => s.GetAvondByIdAsync(avond.Id)).ReturnsAsync(avond);
            var tempDataMockDelete = new Mock<ITempDataDictionary>();
            _controller.TempData = tempDataMockDelete.Object;

            // Act
            var result = await _controller.Delete(avond.Id) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            tempDataMockDelete.VerifySet(tempData => tempData["ErrorMessage"] = "Deze avond kan niet worden verwijderd omdat er al inschrijvingen zijn.");

        }

        //User Story 3
        [TestMethod]
        public async Task Minderjarige_Speler_Kan_Deelnemen_Aan_Niet_18Plus_Avond()
        {
            // Arrange
            var user = new ApplicationUser { Id = "user1", UserName = "testplayer", Geboortedatum = new DateTime(2007, 1, 1) }; // Minderjarig
            var avond = new BordspellenAvond
            {
                Id = 1,
                Is18Plus = false, // Niet 18+
                MaxAantalSpelers = 5,
                Bordspellen = new List<Bordspel> { new Bordspel { Id = 1, Naam = "Catan", Is18Plus = false }
                }
            };

            _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            _inschrijvingServiceMock.Setup(s => s.GetAvondMetDieetOptiesAsync(avond.Id)).ReturnsAsync(avond);
            _inschrijvingServiceMock.Setup(s => s.InschrijvenVoorAvondAsync(user.Id, avond.Id, It.IsAny<string>())).ReturnsAsync(true); 
                _inschrijvingServiceMock.Setup(s => s.MagDeelnemenOpBasisVanLeeftijdAsync(user.Id, avond.Id)).ReturnsAsync(true);
            var tempDataMock = new Mock<ITempDataDictionary>();

            var inschrijvingenController = new InschrijvingenController(_inschrijvingServiceMock.Object, _userManagerMock.Object)
            {
                TempData = tempDataMock.Object 
            };

            // Act
            var result = await inschrijvingenController.Inschrijven(avond.Id) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            tempDataMock.VerifySet(tempData => tempData["SuccessMessage"] = "Je bent succesvol ingeschreven voor de bordspellenavond!", Times.Once);

            // Verify inschrijving service call
            _inschrijvingServiceMock.Verify(s => s.InschrijvenVoorAvondAsync(user.Id, avond.Id, "Geen specifieke dieetwensen"), Times.Once);
        }
        [TestMethod]
        public async Task Volwassene_Speler_Kan_Deelnemen_Aan_18Plus_Avond()
        {
            // Arrange
            var user = new ApplicationUser { Id = "user2", UserName = "testadult", Geboortedatum = new DateTime(1990, 1, 1) }; // Volwassen
            var avond = new BordspellenAvond
            {
                Id = 2,
                Is18Plus = true, // 18+ avond
                Bordspellen = new List<Bordspel> { new Bordspel { Id = 1, Naam = "Cards Against Humanity", Is18Plus = true } }
            };

            _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            _inschrijvingServiceMock.Setup(s => s.GetAvondMetDieetOptiesAsync(avond.Id)).ReturnsAsync(avond);
            _inschrijvingServiceMock.Setup(s => s.InschrijvenVoorAvondAsync(user.Id, avond.Id, It.IsAny<string>())).ReturnsAsync(true);
            _inschrijvingServiceMock.Setup(s => s.MagDeelnemenOpBasisVanLeeftijdAsync(user.Id, avond.Id)).ReturnsAsync(true);

            var tempDataMock = new Mock<ITempDataDictionary>();
            var inschrijvingenController = new InschrijvingenController(_inschrijvingServiceMock.Object, _userManagerMock.Object)
            {
                TempData = tempDataMock.Object 
            };


            // Act
            var result = await inschrijvingenController.Inschrijven(avond.Id) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            tempDataMock.VerifySet(tempData => tempData["SuccessMessage"] = "Je bent succesvol ingeschreven voor de bordspellenavond!", Times.Once);
            _inschrijvingServiceMock.Verify(s => s.InschrijvenVoorAvondAsync(user.Id, avond.Id, "Geen specifieke dieetwensen"), Times.Once);

        }
        //Fout scenario's
        [TestMethod]
        public async Task Minderjarige_Speler_Kan_Niet_Deelnemen_Aan_18Plus_Avond()
        {
            // Arrange
            var user = new ApplicationUser { Id = "user3", UserName = "testminor", Geboortedatum = new DateTime(2010, 1, 1) }; // Minderjarig
            var avond = new BordspellenAvond
            {
                Id = 3,
                Is18Plus = true, // 18+ avond
                Bordspellen = new List<Bordspel> { new Bordspel { Id = 1, Naam = "Cards Against Humanity", Is18Plus = true } }
            };

            _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            _inschrijvingServiceMock.Setup(s => s.GetAvondMetDieetOptiesAsync(avond.Id)).ReturnsAsync(avond);
            var tempDataMock = new Mock<ITempDataDictionary>();

            var inschrijvingenController = new InschrijvingenController(_inschrijvingServiceMock.Object, _userManagerMock.Object)
            {
                TempData = tempDataMock.Object
            };

            // Act
            var result = await inschrijvingenController.Inschrijven(avond.Id) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            tempDataMock.VerifySet(tempData => tempData["ErrorMessage"] = "Je bent niet oud genoeg om deel te nemen aan deze 18+ bordspellenavond.", Times.Once);
        }

        [TestMethod]
        public async Task Organisator_Kan_Geen_Niet_18Plus_Avond_Maken_Met_18Plus_Spel()
        {
            // Arrange
            var avond = new BordspellenAvond
            {
                Id = 4,
                Is18Plus = false, 
                Bordspellen = new List<Bordspel> { new Bordspel { Id = 1, Naam = "Cards Against Humanity", Is18Plus = true } } 
            };

            _avondServiceMock.Setup(s => s.CreateBordspellenAvondAsync(It.IsAny<BordspellenAvond>(), It.IsAny<List<int>>())).Returns(Task.CompletedTask);
            _avondServiceMock.Setup(s => s.IsUserEligibleToOrganizeAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(true);
            var tempDataMock = new Mock<ITempDataDictionary>();
            _controller.TempData = tempDataMock.Object;

            // Act
            var result = await _controller.Create(avond, new List<int> { 1 }) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            tempDataMock.VerifySet(tempData => tempData["ErrorMessage"] = "Je kunt geen niet-18+ avond aanmaken met een 18+ spel.", Times.Once);

        }


        //User story 4
        [TestMethod]
        public async Task Speler_Kan_Inschrijven_Als_Spelavond_Niet_Vol_Is()
        {
            // Arrange
            var user = new ApplicationUser { Id = "user1", UserName = "testplayer", Geboortedatum = new DateTime(2000, 1, 1) }; // 24 jaar oud
            var avond = new BordspellenAvond
            {
                Id = 1,
                MaxAantalSpelers = 5,
                Inschrijvingen = new List<Inschrijving>(), // Nog geen inschrijvingen
                Is18Plus = false
            };

            _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            _inschrijvingServiceMock.Setup(s => s.GetAvondMetDieetOptiesAsync(avond.Id)).ReturnsAsync(avond);
            _inschrijvingServiceMock.Setup(s => s.InschrijvenVoorAvondAsync(user.Id, avond.Id, "Geen specifieke dieetwensen")).ReturnsAsync(true);
            _inschrijvingServiceMock.Setup(s => s.MagDeelnemenOpBasisVanLeeftijdAsync(user.Id, avond.Id)).ReturnsAsync(true); // Correcte mock


            var tempDataMock = new Mock<ITempDataDictionary>();
            _inschrijvingController.TempData = tempDataMock.Object;

            // Act
            var result = await _inschrijvingController.Inschrijven(avond.Id) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            tempDataMock.VerifySet(tempData => tempData["SuccessMessage"] = "Je bent succesvol ingeschreven voor de bordspellenavond!", Times.Once);
            _inschrijvingServiceMock.Verify(s => s.InschrijvenVoorAvondAsync(user.Id, avond.Id, "Geen specifieke dieetwensen"), Times.Once);
        }

        [TestMethod]
        public async Task Speler_Kan_Niet_Inschrijven_Als_Spelavond_Vol_Is()
        {
            // Arrange
            var user = new ApplicationUser { Id = "user1", UserName = "testplayer", Geboortedatum = new DateTime(2000, 1, 1) };
            var avond = new BordspellenAvond
            {
                Id = 1,
                MaxAantalSpelers = 5,
                Inschrijvingen = new List<Inschrijving> { new Inschrijving(), new Inschrijving(), new Inschrijving(), new Inschrijving(), new Inschrijving() }, // Vol
                Is18Plus = false
            };

            _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            _inschrijvingServiceMock.Setup(s => s.GetAvondMetDieetOptiesAsync(avond.Id)).ReturnsAsync(avond);
            _inschrijvingServiceMock.Setup(s => s.MagDeelnemenOpBasisVanLeeftijdAsync(user.Id, avond.Id)).ReturnsAsync(true);

            var tempDataMock = new Mock<ITempDataDictionary>();
            _inschrijvingController.TempData = tempDataMock.Object;

            // Act
            var result = await _inschrijvingController.Inschrijven(avond.Id) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            tempDataMock.VerifySet(tempData => tempData["ErrorMessage"] = "Inschrijving mislukt. Mogelijk is het maximale aantal spelers al bereikt.", Times.Once);
        }
        [TestMethod]
        public async Task Speler_Kan_Niet_Meerdere_Avonden_Op_Dezelfde_Dag_Inschrijven()
        {
            // Arrange
            var user = new ApplicationUser { Id = "user1", UserName = "testplayer", Geboortedatum = new DateTime(2000, 1, 1) };
            var avond = new BordspellenAvond
            {
                Id = 1,
                MaxAantalSpelers = 5,
                Is18Plus = false,
                Datum = DateTime.Today
            };

            _userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            _inschrijvingServiceMock.Setup(s => s.HeeftAlIngeschreven(user.Id, avond.Id)).ReturnsAsync(false);
            _inschrijvingServiceMock.Setup(s => s.GetAvondMetDieetOptiesAsync(avond.Id)).ReturnsAsync(avond);
            _inschrijvingServiceMock.Setup(s => s.KanDeelnemenAanAvond(user, avond.Datum)).ReturnsAsync(true); // Al ingeschreven voor een andere avond

            var tempDataMock = new Mock<ITempDataDictionary>();
            _inschrijvingController.TempData = tempDataMock.Object;

            // Act
            var result = await _inschrijvingController.Inschrijven(avond.Id) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            tempDataMock.VerifySet(tempData => tempData["ErrorMessage"] = "Je kunt slechts aan één bordspellenavond per dag deelnemen.", Times.Once);
        }

        //user story 5
    }
}
