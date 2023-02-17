using System;
using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Resting.API;
using Xunit;

namespace RestShop.Tests
{
  public class HttpClientFixture : IDisposable
  {
    public HttpClientFixture() => this.Client = new WebApplicationFactory<Startup>().CreateClient();

    public void Dispose() => this.Client.Dispose();
    public HttpClient Client { get; private set; }
  }

  [CollectionDefinition(nameof(HttpClientCollection))]
  public class HttpClientCollection : ICollectionFixture<HttpClientFixture>
  {
    // This class has no code, and is never created. Its purpose is simply
    // to be the place to apply [CollectionDefinition] and all the
    // ICollectionFixture<> interfaces.
  }
}
