using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using Moq;

namespace CommonTestUtilities.Cache;

public class IDistributedCacheBuilder
{
    private readonly Mock<IDistributedCache> _mock = new();

    public IDistributedCacheBuilder()
    {
        _mock.Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((byte[]?)null);
    }

    public IDistributedCacheBuilder WithCachedProducts<T>(T data)
    {
        var json = JsonSerializer.SerializeToUtf8Bytes(data);

        _mock.Setup(c => c.GetAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(json);

        return this;
    }

    public IDistributedCache Build() => _mock.Object;
}
