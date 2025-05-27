using NUnit.Framework;

using Moq;

using DatingWebApp.Controllers;

using DatingWebApp.Interfaces;

using DatingWebApp.Entities;

using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using DatingWebApp.Extensions;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using static Microsoft.ApplicationInsights.MetricDimensionNames.TelemetryContext;
using static System.Net.WebRequestMethods;
using NUnit.Framework.Legacy;



namespace DatingWebApp.Tests

{

    public class UsersControllerTests

    {

        private UsersController _controller = null!;

        private Mock<IUnitOfWork> _unitOfWorkMock = null!;

        private Mock<IPhotoRepository> _photoRepoMock = null!;

        private Mock<IUserRepository> _userRepoMock = null!;

        private Mock<IPhotoService> _photoServiceMock = null!;



        [SetUp]

        public void Setup()

        {

            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _photoRepoMock = new Mock<IPhotoRepository>();

            _userRepoMock = new Mock<IUserRepository>();

            _photoServiceMock = new Mock<IPhotoService>();



            _unitOfWorkMock.Setup(u => u.PhotoRepository).Returns(_photoRepoMock.Object);

            _unitOfWorkMock.Setup(u => u.UserRepository).Returns(_userRepoMock.Object);

            _unitOfWorkMock.Setup(u => u.Complete()).ReturnsAsync(true);



            _controller = new UsersController(_unitOfWorkMock.Object, null!, _photoServiceMock.Object);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {

                new Claim(ClaimTypes.NameIdentifier, "testuser"),

                new Claim(ClaimTypes.Name, "testuser")

            }, "mock"));



            _controller.ControllerContext = new ControllerContext
            {

                HttpContext = new DefaultHttpContext { User = user }

            };

        }


        [Test]

        public async Task DeletePhoto_Should_RemovePhoto_IfNotMain()

        {

            // Arrange

            var photo = new Photo
            {

                Id = 1,

                IsMain = false,

                Url = "https://nekilink.com/test.jpg"

            };



        var user = new AppUser

        {

            Id = 1,

            UserName = "testuser",

            KnownAs = "Test",

            Gender = "M",

            City = "vakuf",

            Country = "BiH",

            Photos = new List<Photo> { photo }
        };



            _photoRepoMock.Setup(p => p.GetPhotoById(1)).ReturnsAsync(photo);

            _userRepoMock.Setup(u => u.GetUserByUsernameAsync("testuser")).ReturnsAsync(user);



        // Act

        var result = await _controller.DeletePhoto(1);



        // Assert

            ClassicAssert.IsInstanceOf<OkResult>(result);

            ClassicAssert.IsFalse(user.Photos.Contains(photo));

        }

}

}