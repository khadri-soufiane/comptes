using Comptes_WebAPI.Models;
using Comptes_WebAPI.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Comptes_WebAPI.Controllers;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using FluentAssertions;

namespace CompteXunitTests
{
    public class AccountUnitTestController
    {
        private AccountRepository repository;
        public static DbContextOptions<AccountContext> dbContextOptions { get; }
        public static string connectionString = "server=SOUFIANE-PC\\SQLEXPRESS; Database=AccountManagement; integrated security= true";

        static AccountUnitTestController()
        {
            dbContextOptions = new DbContextOptionsBuilder<AccountContext>()
                .UseSqlServer(connectionString)
                .Options;
        }

        public AccountUnitTestController()
        {
            var context = new AccountContext(dbContextOptions);

            repository = new AccountRepository(context);

        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        #region GetAccouts Tests  

        [Fact]
        public async void Task_GetAccounts_OkResult()
        {
            //Arrange  
            var controller = new ComptesAPIController(repository);

            //Act  
            var data = await controller.GetAccounts();

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void Task_GetAccounts_ResultatExacte_OkResult()
        {
            //Arrange  
            var controller = new ComptesAPIController(repository);

            //Act  
            var data = await controller.GetAccounts();

            //Assert  
            Assert.IsType<OkObjectResult>(data);

            var okResult = data.Should().BeOfType<OkObjectResult>().Subject;
            var account = okResult.Value.Should().BeAssignableTo<List<Account>>().Subject;

            Assert.Equal("wassef@bakkali.com", account[0].Email);
            Assert.Equal("wbakkali", account[0].Username);

            Assert.Equal("adil@lahoui.com", account[1].Email);
            Assert.Equal("alahoui", account[1].Username);
        }

        #endregion


        #region GetById Tests
        [Fact]
        public async void Task_GetAccountById_OkResult()
        {
            //Arrange  
            var controller = new ComptesAPIController(repository);
            var accountId = 1;

            //Act  
            var data = await controller.GetById(accountId);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void Task_GetAccountById_NotFound()
        {
            //Arrange  
            var controller = new ComptesAPIController(repository);
            var accountId = 500;

            //Act  
            var data = await controller.GetById(accountId);

            //Assert  
            Assert.IsType<NotFoundResult>(data);
        }

        [Fact]
        public async void Task_GetAccountById__BadRequestResult()
        {
            //Arrange  
            var controller = new ComptesAPIController(repository);
            int? accountId = null;

            //Act  
            var data = await controller.GetById(accountId   );

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        #endregion


        #region AddAccount Tests

        [Fact]
        public async void Task_AddAccount__OkResult()
        {
            //Arrange  
            var controller = new ComptesAPIController(repository);
            string username = RandomString(7);
            var account = new RegisterAccountViewModel() {
                Name = "Test name",
                Username = username,
                Email = "test@name.com",
                Password = "test",
                RepeatPassword = "test"
            };

            //Act  
            var data = await controller.AddAccount(account);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void Task_AddAccount_Password_NotMatch_BadRequestResult()
        {
            //Arrange  
            var controller = new ComptesAPIController(repository);
            var account = new RegisterAccountViewModel()
            {
                Name = "Test name",
                Username = "testName2",
                Email = "test@name.com",
                Password = "test",
                RepeatPassword = "test1"
            };

            //Act  
            var data = await controller.AddAccount(account);

            //Assert  
            Assert.IsType<BadRequestResult>(data);
        }

        [Fact]
        public async void Task_AddAccount_UsernameExist_BadRequestResult()
        {
            //Arrange  
            var controller = new ComptesAPIController(repository);
            string username = RandomString(7);
            var account = new RegisterAccountViewModel()
            {
                Name = "Test name",
                Username = "alahoui",
                Email = "test@name.com",
                Password = "test",
                RepeatPassword = "test"
            };

            //Act  
            var data = await controller.AddAccount(account);

            //Assert  
            Assert.IsType<BadRequestObjectResult>(data);
        }

        #endregion


        #region Update Account 

        [Fact]
        public async void Task_UpdateAccount_OkResult()
        {
            //Arrange  
            var controller = new ComptesAPIController(repository);
            var accountId = 2011;

            //Act  
            var existingAccount = await controller.GetById(accountId);
            var okResult = existingAccount.Should().BeOfType<OkObjectResult>().Subject;
            var result = okResult.Value.Should().BeAssignableTo<Account>().Subject;

            var account = new EditAccountViewModel();
            account.Id = accountId;
            account.Name = "Update Test Name";
            account.UserName = result.Username;
            account.NewPassword = "test";
            account.NewPasswordAgain = "test";
            account.RegistrationDate = result.RegistrationDateTime;
            account.LastLogin = result.LastLoginDateTime;

            var updatedData = await controller.UpdateAccount(account);

            //Assert  
            Assert.IsType<OkObjectResult>(updatedData);
        }

        [Fact]
        public async void Task_Update_Password_NotMatch_BadRequestResult()
        {
            //Arrange  
            var controller = new ComptesAPIController(repository);
            var accountId = 2011;

            //Act  
            var existingAccount = await controller.GetById(accountId);
            var okResult = existingAccount.Should().BeOfType<OkObjectResult>().Subject;
            var result = okResult.Value.Should().BeAssignableTo<Account>().Subject;

            var account = new EditAccountViewModel();
            account.Id = accountId;
            account.Name = "Update Test Name";
            account.UserName = result.Username;
            account.NewPassword = "test";
            account.NewPasswordAgain = "test1";
            account.RegistrationDate = result.RegistrationDateTime;
            account.LastLogin = result.LastLoginDateTime;

            var updatedData = await controller.UpdateAccount(account);

            //Assert  
            Assert.IsType<BadRequestObjectResult>(updatedData);
        }

        [Fact]
        public async void Task_UpdateAccount_NotFound()
        {
            //Arrange  
            var controller = new ComptesAPIController(repository);
            var accountId = 100000;

            //Act  
            var existingAccount = await controller.GetById(accountId);

            //Assert
            var okResult = existingAccount.Should().BeOfType<NotFoundResult>();

            //var account = new EditAccountViewModel();
            //account.Name = "Update Test Name";
            //account.UserName = result.Username;
            //account.NewPassword = "test";
            //account.NewPasswordAgain = "test1";
            //account.RegistrationDate = result.RegistrationDateTime;
            //account.LastLogin = result.LastLoginDateTime;

            //var updatedData = await controller.UpdateAccount(account);

            ////Assert  
            //Assert.IsType<NotFoundObjectResult>(updatedData);
        }

        #endregion


        #region Delete Account Test

        [Fact]
        public async void Task_DeleteAccount_OkResult()
        {
            //Arrange  
            var controller = new ComptesAPIController(repository);
            //Act  
            string username = RandomString(7);
            var account = new RegisterAccountViewModel()
            {
                Name = "Test name",
                Username = username,
                Email = "test@name.com",
                Password = "test",
                RepeatPassword = "test"
            };

            var existingAccount = await controller.AddAccount(account);
            var okResult = existingAccount.Should().BeOfType<OkObjectResult>().Subject;
            var result = okResult.Value.Should().BeAssignableTo<Account>().Subject;
 
            var data = await controller.Delete(result.Id);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }

        [Fact]
        public async void Task_DeleteAccount_BadRequestResult()
        {
            //Arrange  
            var controller = new ComptesAPIController(repository);
              

            //Act  
            var data = await controller.Delete(17500);

            //Assert  
            Assert.IsType<BadRequestObjectResult>(data);
        }

        #endregion


        #region Login Tests

        [Fact]
        public async void Task_Login_OkResult()
        {
            //Arrange  
            var controller = new ComptesAPIController(repository);
            string username = RandomString(7);
            var account = new LoginAccountViewModel()
            {
                Username = "wbakkali",
                Password = "12",
            };

            //Act  
            var data = await controller.Login(account);

            //Assert  
            Assert.IsType<OkObjectResult>(data);
        }


        [Fact]
        public async void Task_Login_BadRequestResult()
        {
            //Arrange  
            var controller = new ComptesAPIController(repository);
            string username = RandomString(7);
            var account = new LoginAccountViewModel()
            {
                Username = "wbakkali",
                Password = "12345",
            };

            //Act  
            var data = await controller.Login(account);

            //Assert  
            Assert.IsType<BadRequestObjectResult>(data);
        }

        #endregion
    }
}
