using MarketAPI.Domain.Messaging;
using Moq;

namespace CommonTestUtilities.Repositories;

public class IMessagePublisherBuilder
{
    private readonly Mock<IMessagePublisher> _mock = new();

    public IMessagePublisher Build() => _mock.Object;
}
