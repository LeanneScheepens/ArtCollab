using Xunit;
using Moq;
using Logic.Managers;
using Logic.Models;
using Logic.ViewModels;
using Logic.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;


namespace ArtCollab.Test;


public class UsermanagerTests
{
    //If the Username already excist
    [Fact]
    public void CreateUser_ShouldThrow_WhenUsernameAlreadyExists()
    {
        var mockRepo = new Mock<IUserRepository>();

       // make sure the username already excist
        mockRepo.Setup(repo => repo.GetUserByName("existinguser"))
                .Returns(new User(1, "existinguser", "test@test.com", "hashedpwd", null, null));

        var userManager = new UserManager(mockRepo.Object);

        var viewModel = new RegisterViewModel
        {
            Name = "existinguser",
            Email = "test@test.com",
            Password = "SomePassword1234",
            ConfirmPassword = "SomePassword1234"
        };


        var exception = Assert.Throws<ArgumentException>(() => userManager.CreateUser(viewModel, Role.Artist));
        Assert.Equal("Username already exists.", exception.Message);
    }

    //if validation fails
    [Fact]
    public void CreateUser_ShouldThrow_WhenViewModelIsInvalid()
    {
        var mockRepo = new Mock<IUserRepository>();
        var userManager = new UserManager(mockRepo.Object);

        var viewModel = new RegisterViewModel
        {
            Name = "", // shouldn't work
            Email = "invalidemail",
            Password = "pwd",
            ConfirmPassword = "pwd"
        };

        var exception = Assert.Throws<ArgumentException>(() => userManager.CreateUser(viewModel, Role.Artist));

        Assert.Contains("Validation error", exception.Message);
    }
    //Valid user is saved 
    [Fact]
    public void CreateUser_ShouldCallRepository_WhenValid()
    {
        var mockRepo = new Mock<IUserRepository>();
        mockRepo.Setup(repo => repo.GetUserByName(It.IsAny<string>())).Returns((User)null);

        var userManager = new UserManager(mockRepo.Object);

        var viewModel = new RegisterViewModel
        {
            Name = "newuser",
            Email = "test@test.com",
            Password = "SomePassword1234",
            ConfirmPassword = "SomePassword1234"
        };

        userManager.CreateUser(viewModel, Role.Artist);

        mockRepo.Verify(repo => repo.CreateUser(It.IsAny<User>()), Times.Once);
    }
    //if the viewmodel is null
    [Fact]
    public void CreateUser_ShouldThrow_WhenViewModelIsNull()
    {
        var mockRepo = new Mock<IUserRepository>();
        var userManager = new UserManager(mockRepo.Object);

        Assert.Throws<ArgumentNullException>(() => userManager.CreateUser(null, Role.Artist));
    }

    //password check,this is in the viewmodel and not in the manager
    [Fact]
    public void Validate_ShouldReturnError_WhenPasswordsDoNotMatch()
    {
  
        var viewModel = new RegisterViewModel
        {
            Name = "testuser",
            Email = "test@test.com",
            Password = "SomePassword1234",
            ConfirmPassword = "DifferentPassword1234"
        };


        var results = viewModel.Validate(new ValidationContext(viewModel)).ToList();

        Assert.Contains(results, r => r.ErrorMessage == "Passwords do not match.");
    }


}

