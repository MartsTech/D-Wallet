namespace ComponentTests;

using System;

public sealed class WebApplicationFactoryFixture : IDisposable
{
    public WebApplicationFactoryFixture(WebApplicationFactory webApplicationFactory)
    {
        WebApplicationFactory = webApplicationFactory;
    }

    public WebApplicationFactory WebApplicationFactory { get; }

    public void Dispose()
    {
        WebApplicationFactory?.Dispose();
    }
}