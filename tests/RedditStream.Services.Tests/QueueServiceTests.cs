using Autofac.Extras.Moq;
using Microsoft.Extensions.Options;
using Moq;
using RedditStream.Services.Configuration;
using RedditStream.Services.Interfaces;

namespace RedditStream.Services.Tests;

public class QueueServiceTests
{
    private readonly AutoMock _autoMock;

    public QueueServiceTests()
    {
        _autoMock = AutoMock.GetLoose();

        _autoMock.Mock<IOptions<RedditOptions>>()
            .Setup(x => x.Value)
            .Returns(new RedditOptions
            {
                StorageAccountConnectionString = "TestConnectionString"
            });
    }

    [Fact]
    public async Task QueueService_QueueMessageAsync_Success()
    {
        //Arrange
        var service = _autoMock.Create<QueueService>();

        //Act
        string queueName = "testQueue";
        string message = "testMessage";

        await service.QueueMessageAsync(queueName, message);

        //Assert
        _autoMock.Mock<IQueueClientService>()
            .Verify(x => x.CreateIfNotExistsAsync(queueName, It.IsAny<CancellationToken>()), Times.Once());

        _autoMock.Mock<IQueueClientService>()
            .Verify(x => x.SendMessageAsync(message, It.IsAny<TimeSpan>(), It.IsAny<CancellationToken>()), Times.Once());

    }
}