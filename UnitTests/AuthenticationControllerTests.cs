using System;
using Xunit;
using BookStore.Api.Identity.Controllers;
using BookStore.Api.Identity.UnitTests.Fixtures;
using BookStore.Api.Identity.Requests;
using BookStore.Api.Identity.Responses;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookStore.Api.Identity.Models;

namespace BookStore.Api.Identity.UnitTests
{
    public class AuthenticationControllerTests : IDisposable
    {
        private AuthenticationController _authController;

        public AuthenticationControllerTests()
        {
            _authController = new AuthenticationController(
                new UserServiceFixture(), new AppSettingsFixture());
        }

        public void Dispose()
        {
        }

        [Theory]
        [InlineData(1, "dzy", "dzy", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9")]
        public async Task Authenticate_Succeeds(long id, string userName, string password, string jwtToken)
        {
            var r = await _authController.Post(new AuthenticationRequest { UserName = userName, Password = password });
            Assert.IsType<OkObjectResult>(r.Result);
            var or = r.Result as OkObjectResult;
            Assert.Equal(200, or.StatusCode);
            Assert.IsType<AuthenticationResponse>(or.Value);
            var res = or.Value as AuthenticationResponse;
            Assert.Equal(id, res.Id);
            Assert.Equal(userName, res.Name);
            var parts = res.JwtToken.Split('.');
            Assert.Equal(3, parts.Length);
            Assert.Equal(jwtToken, parts[0]);
        }

        [Theory]
        [InlineData("abc", "abc")]
        public async Task Authenticate_Fails(string userName, string password)
        {
            var r = await _authController.Post(new AuthenticationRequest { UserName = userName, Password = password });
            Assert.IsType<BadRequestObjectResult>(r.Result);
            var or = r.Result as BadRequestObjectResult;
            Assert.Equal(400, or.StatusCode);
            Assert.IsType<Error>(or.Value);
            var res = or.Value as Error;
            Assert.Equal("UserName or Password is incorrect", res.ErrorMessage);
        }

        [Theory]
        [InlineData(2, "ly", "ly", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9")]
        public async Task Register_Succeeds(long id, string userName, string password, string jwtToken)
        {
            var r = await _authController.Post(new RegisterRequest { UserName = userName, Password = password });
            Assert.IsType<OkObjectResult>(r.Result);
            var or = r.Result as OkObjectResult;
            Assert.Equal(200, or.StatusCode);
            Assert.IsType<AuthenticationResponse>(or.Value);
            var res = or.Value as AuthenticationResponse;
            Assert.Equal(id, res.Id);
            Assert.Equal(userName, res.Name);
            var parts = res.JwtToken.Split('.');
            Assert.Equal(3, parts.Length);
            Assert.Equal(jwtToken, parts[0]);
        }
    }
}
