using System;
using JetBrains.Annotations;
using LegacyApp;
using Xunit;

namespace LegacyApp.Tests;

[TestSubject(typeof(UserService))]
public class UserServiceTest
{
    
    [Fact]
    public void AddUser_Should_Throw_ArgumentException_When_ClientId_Doesnt_Exist()
    {
        //Arrange
        var userService = new UserService();
        //Act & Assert
        Assert.Throws<ArgumentException>(() =>
        {
            var addResult = userService.AddUser("John", "Doe", "johndoe@gmail.com", DateTime.Parse("1982-03-21"), 7);
        });
        
    }
    [Fact]
    public void AddUser_Should_Return_False_When_Date_Is_Not_Correct()
    {
        //Arrange
        var userService = new UserService();
        //Act
        var addResult = userService.AddUser("John", "Doe", "johndoe@gmail.com", DateTime.Now.AddYears(1), 1);
        //Assert
        Assert.False(addResult);
    }
    
    [Fact]
    public void AddUser_Should_Return_False_When_Email_Is_Not_Correct()
    {
        //Arrange
        var userService = new UserService();
        //Act
        var addResult = userService.AddUser("John", "Doe", "johndoegmailcom", DateTime.Parse("1982-03-21"), 1);
        //Assert
        Assert.False(addResult);
    }
    [Fact]
    public void AddUser_Should_Return_False_When_LastName_Is_Missing()
    {
        //Arrange
        var userService = new UserService();
        //Act
        var addResult = userService.AddUser("John", "", "johndoe@gmail.com", DateTime.Parse("1982-03-21"), 1);
        //Assert
        Assert.False(addResult);
    }

    [Fact]
    public void AddUser_Should_Return_False_When_FirstName_Is_Missing()
    {
        //Arrange
        var userService = new UserService();
        //Act
        var addResult = userService.AddUser("", "Doe", "johndoe@gmail.com", DateTime.Parse("1982-03-21"), 1);
        //Assert
        Assert.False(addResult);
    }

    [Fact]
    public void METHOD()
    {
        
    }
}