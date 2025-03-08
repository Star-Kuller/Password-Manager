using System.Net;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using PasswordManager.API.Middlewares;
using PasswordManager.API.Middlewares.Exceptions;
using PasswordManager.API.Middlewares.Exceptions.Handlers;

namespace PasswordManager.Tests.UnitTest.Middlewares;

public class ExceptionsHandlerMiddlewareTests
{
    private readonly Mock<ILogger<ExceptionsHandlerMiddleware>> _mockLogger = new();
    private readonly Mock<RequestDelegate> _mockRequestDelegate = new();
    private readonly Mock<HttpContext> _mockHttpContext = new();
    private readonly Mock<IResponseWriter> _mockResponseWriter = new();
    
    [Fact]
    public async Task ExceptionHandler_doesnt_interfere_with_work()
    {
        //Arrange
        var middleware = new ExceptionsHandlerMiddleware(_mockRequestDelegate.Object);
        
        //Act
        await middleware.Invoke(_mockHttpContext.Object, _mockLogger.Object, _mockResponseWriter.Object);

        //Assert
        _mockRequestDelegate.Verify(
            next 
                => next.Invoke(_mockHttpContext.Object),
            Times.Once);
    }
    
    [Fact]
    public async Task ExceptionHandler_catch_ValidationException()
    {
        //Arrange
        var validationException = new ValidationException(new List<ValidationFailure>
        {
            new("TestPropertyName1", "TestErrorMessage1"),
            new("TestPropertyName1", "TestErrorMessage2"),
            new("TestPropertyName2", "TestErrorMessage3")
        });
        _mockRequestDelegate
            .Setup(rd => rd.Invoke(It.IsAny<HttpContext>()))
            .ThrowsAsync(validationException);
        
        var middleware = new ExceptionsHandlerMiddleware(_mockRequestDelegate.Object);
        
        //Act
        await middleware.Invoke(_mockHttpContext.Object, _mockLogger.Object, _mockResponseWriter.Object);

        //Assert
        var expectedResponse = new List<ValidationExceptionHandler.Response>
        {
            new("TestPropertyName1", ["TestErrorMessage1", "TestErrorMessage2"]),
            new("TestPropertyName2", ["TestErrorMessage3"])
        };
        
        _mockResponseWriter.VerifySet(
            writer => 
                writer.HttpContext = _mockHttpContext.Object,
            Times.Once);
        _mockResponseWriter.Verify(
            writer => writer.WriteAsync(
                HttpStatusCode.BadRequest,
                It.Is<IEnumerable<ValidationExceptionHandler.Response>>(response
                    => response.SequenceEqual(expectedResponse, new ValidationExceptionResponseComparer()))
                ),
            Times.Once);
    }
    
    [Fact]
    public async Task ExceptionHandler_catch_unhandled_Exception()
    {
        //Arrange
        var exception = new Exception("TestMessage");
        _mockRequestDelegate
            .Setup(rd => rd.Invoke(It.IsAny<HttpContext>()))
            .ThrowsAsync(exception);
        
        var middleware = new ExceptionsHandlerMiddleware(_mockRequestDelegate.Object);
        
        //Act
        await middleware.Invoke(_mockHttpContext.Object, _mockLogger.Object, _mockResponseWriter.Object);

        //Assert
        _mockLogger.Verify(
            logger => 
                logger.Log(
                    LogLevel.Critical,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    exception,
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
        
        _mockResponseWriter.VerifySet(
            writer => 
                writer.HttpContext = _mockHttpContext.Object,
            Times.Once);
        
        _mockResponseWriter.Verify(
            writer => writer.WriteAsync(
                HttpStatusCode.InternalServerError
            ),
            Times.Once);
    }

    private class ValidationExceptionResponseComparer : IEqualityComparer<ValidationExceptionHandler.Response>
    {
        public bool Equals(ValidationExceptionHandler.Response? x, ValidationExceptionHandler.Response? y)
        {
            return x!.Property == y!.Property &&
                   x.Messages.SequenceEqual(y.Messages);
        }

        public int GetHashCode(ValidationExceptionHandler.Response obj)
        {
            return HashCode.Combine(obj.Property, obj.Messages);
        }
    }
}