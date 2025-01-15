using System.Security.Claims;

using laminas_calisa.Pages.IniciarSesion;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Primitives;

using Moq;

using NToastNotify;

using Supabase.Gotrue;
using Supabase.Storage;

using Xunit;

using Client = Supabase.Client;

namespace laminas_calisa.Tests.Auth;

public class LoginTests
{
    private readonly Mock<Client> _mockClient;
    private readonly Mock<IToastNotification> _mockToastNotification;
    private readonly Mock<ILogger<IniciarSesionIndexModel>> _mockLogger;
    private readonly IniciarSesionIndexModel _pageModel;

    public LoginTests()
    {
        _mockClient = new();
        _mockToastNotification = new();
        _mockLogger = new();

        _pageModel = new IniciarSesionIndexModel(
            _mockLogger.Object,
            _mockClient.Object,
            _mockToastNotification.Object
        );
        
        var httpContext = new DefaultHttpContext();
        var formCollection = new FormCollection(new Dictionary<string, StringValues>
        {
            { "email", "test@example.com" },
            { "pwd", "Test_Pwd123" }
        });
        httpContext.Request.Form = formCollection;
        
        var authServiceMock = new Mock<IAuthenticationService>();
        authServiceMock.Setup(x =>
                x.SignInAsync(It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()))
                .Returns(Task.CompletedTask);
        
        var serviceProviderMock = new Mock<IServiceProvider>();
        serviceProviderMock.Setup(x => 
            x.GetService(typeof(IAuthenticationService)))
            .Returns(authServiceMock.Object);
        
        httpContext.RequestServices = serviceProviderMock.Object;

        _pageModel.PageContext = new Microsoft.AspNetCore.Mvc.RazorPages.PageContext { HttpContext = httpContext };
    }

    [Fact]
    public async Task OnPost_WithValidCredentials_RedirectsToIndex()
    {
        // Arrange
        var mockSession = new Session 
        { 
            User = new User 
            { 
                Id = "user123",
                Email = "test@example.com" 
            },
            AccessToken = "access_token",
            RefreshToken = "refresh_token"
        };

        _mockClient
            .Setup(c => c.Auth.SignIn(
                It.IsAny<string>(), 
                It.IsAny<string>()))
            .ReturnsAsync(mockSession);

        // Act
        var result = await _pageModel.OnPostAsync();

        // Assert
        var redirectResult = Assert.IsType<RedirectToPageResult>(result);
        Assert.Equal("/Dashboard/Index", redirectResult.PageName);
        
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Information,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Successful sign in")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()
            ),
            Times.Once
        );
    }
    
    [Fact]
    public async Task OnPost_WithInvalidEmail_ReturnsPage()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var formCollection = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
        {
            { "email", "invalid-email" },
            { "pwd", "Password123*" }
        });
        httpContext.Request.Form = formCollection;
        _pageModel.PageContext.HttpContext = httpContext;

        // Act
        var result = await _pageModel.OnPostAsync();

        // Assert
        Assert.IsType<PageResult>(result);
        _mockToastNotification.Verify(
            x => x.AddErrorToastMessage("Correo no válido", null),
            Times.Once
        );
    }

    [Fact]
    public async Task OnPost_WithInvalidPassword_ReturnsPage()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        var formCollection = new FormCollection(new Dictionary<string, Microsoft.Extensions.Primitives.StringValues>
        {
            { "email", "test@example.com" },
            { "pwd", "weak" }
        });
        httpContext.Request.Form = formCollection;
        _pageModel.PageContext.HttpContext = httpContext;

        // Act
        var result = await _pageModel.OnPostAsync();

        // Assert
        Assert.IsType<PageResult>(result);
        _mockToastNotification.Verify(
            x => x.AddErrorToastMessage("Contraseña no válida", null),
            Times.Once
        );
    }

    [Fact]
    public async Task OnPost_WithInvalidCredentials_ReturnsPage()
    {
        // Arrange
        _mockClient
            .Setup(c => c.Auth.SignIn(
                It.IsAny<string>(), 
                It.IsAny<string>()))
            .ThrowsAsync(new Exception("Invalid credentials"));

        // Act
        var result = await _pageModel.OnPostAsync();

        // Assert
        Assert.IsType<PageResult>(result);
        _mockToastNotification.Verify(
            x => x.AddErrorToastMessage("Correo o contraseña no válidos", null),
            Times.Once
        );
        _mockLogger.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Failed login attempt")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()
            ),
            Times.Once
        );
    }
}