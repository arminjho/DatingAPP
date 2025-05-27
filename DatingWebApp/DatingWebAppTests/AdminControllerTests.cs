using NUnit.Framework;
using Moq;
using DatingWebApp.Controllers;
using DatingWebApp.Interfaces;
using DatingWebApp.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Collections.Generic;
using NUnit.Framework.Legacy;
using static System.Net.WebRequestMethods;
using AutoMapper;

namespace DatingWebApp.Tests
{
    public class AdminControllerTests
    {
        private Mock<IUnitOfWork> _unitOfWorkMock = null!;
        private Mock<IPhotoRepository> _photoRepoMock = null!;
        private Mock<IUserRepository> _userRepoMock = null!;
        private Mock<IMapper> _mapperMock = null!;
        private AdminController _controller = null!;

        [SetUp]
        public void Setup()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _photoRepoMock = new Mock<IPhotoRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _mapperMock= new Mock<IMapper>();   

            _unitOfWorkMock.Setup(u => u.PhotoRepository).Returns(_photoRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.UserRepository).Returns(_userRepoMock.Object);
            _unitOfWorkMock.Setup(u => u.Complete()).ReturnsAsync(true);


            _controller = new AdminController(null!, _unitOfWorkMock.Object, null!, null!, mapper: _mapperMock.Object);

        }

        [Test]
        public async Task ApprovePhoto_Should_SetIsApprovedToTrue_And_SetAsMainIfNone()
        {
            // Arrange
            var photo = new Photo { Id = 1, Url="https://nekilink.com/" ,IsApproved = false };
            var user = new AppUser
            {
                Id = 1,
                Photos = new List<Photo> { photo },
                KnownAs="tester",
                City="Gornji Vakuf",
                Country="BiH",
                Gender="musko"
                
            };

            _photoRepoMock.Setup(r => r.GetPhotoById(1)).ReturnsAsync(photo);
            _userRepoMock.Setup(r => r.GetUserByPhotoId(1)).ReturnsAsync(user);

            // Act
            var result = await _controller.ApprovePhoto(1);

            // Assert
            ClassicAssert.IsInstanceOf<OkResult>(result);
            ClassicAssert.IsTrue(photo.IsApproved);
            ClassicAssert.IsTrue(photo.IsMain); // should be set as main if user had none


        }

       
    }
}