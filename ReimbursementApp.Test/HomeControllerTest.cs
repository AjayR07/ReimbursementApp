using Microsoft.AspNetCore.Mvc;
using ReimbursementApp.API.Controllers.v2;

namespace ReimbursementApp.Test;

public class HomeControllerTest
{
    [Fact]
    public void  HealthCheck()
    {
        // Arrange
        var homeController = new HomeController();
        
        // Act
        var actionResult = homeController.Get();

        // Assert
        Assert.IsType<OkObjectResult>(actionResult.Result);
        var result = actionResult.Result as OkObjectResult;
        Assert.Equal("Welcome to Reimbursement System V2.0",result.Value);
        
    }
}