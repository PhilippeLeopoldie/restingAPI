using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using Resting.API.Controllers;
using Xunit;

namespace RestShop.Tests
{

  [Collection(nameof(HttpClientCollection))]
  public class PresidentAPITests
  {
    readonly HttpClient _client;
    const string BASE_URL = "/api/Presidents";
    const string GETMANY_URL = BASE_URL;
    private string GETONE_URL = $"{BASE_URL}/b769d25a-86dc-4ec6-a022-dfa4112354f9";
    const string POST_URL = BASE_URL;
    private string PUT_URL = $"{BASE_URL}/b769d25a-86dc-4ec6-a022-dfa4112354f9";

    AddPresidentRequest ADD_PRESIDENT_DATA = new AddPresidentRequest
    {
      Name = "Marcus",
      From = "2042",
      To = "2043",
    };
    private StringContent ToJson(object content) =>
      new StringContent(JsonConvert.SerializeObject(content), Encoding.UTF8, "application/json");

    private async Task<President> getPresident(HttpResponseMessage response) =>
      JsonConvert.DeserializeObject<President>(await response.Content.ReadAsStringAsync());

    public PresidentAPITests(HttpClientFixture fixture)
    {
      _client = fixture.Client;
    }

    [Fact]
    public async Task errornous_url_returns_error()
    {
      // act
      var response = await _client.GetAsync("/api");

      // assert
      response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task getMany_returns_Ok()
    {
      // act
      var response = await _client.GetAsync(GETMANY_URL);

      // assert
      response.IsSuccessStatusCode.Should().BeTrue();
      response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact(Skip = "To not flood with failing tests")]
    public async Task getMany_returns_JSON()
    {
      // act
      var response = await _client.GetAsync(GETMANY_URL);

      // assert
      response.Content.Headers.ContentType.MediaType.Should().Be("application/json");
    }

    [Fact(Skip = "To not flood with failing tests")]
    public async Task getMany_returns_presidents()
    {
      // act
      var response = await _client.GetAsync(GETMANY_URL);

      // assert
      var products = JsonConvert.DeserializeObject<List<President>>(await response.Content.ReadAsStringAsync());
      products.Should().HaveCountGreaterThan(3);
    }

    [Fact(Skip = "To not flood with failing tests")]
    public async Task getOne_returns_OK()
    {
      // act
      var response = await _client.GetAsync(GETONE_URL);

      // assert
      response.IsSuccessStatusCode.Should().BeTrue();
      response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact(Skip = "To not flood with failing tests")]
    public async Task getOne_returns_JSON()
    {
      // act
      var response = await _client.GetAsync(GETONE_URL);

      // assert
      response.Content.Headers.ContentType.MediaType.Should().Be("application/json");
    }

    [Fact(Skip = "To not flood with failing tests")]
    public async Task getOne_returns_correctData()
    {
      // act
      var response = await _client.GetAsync(GETONE_URL);

      // assert
      var product = JsonConvert.DeserializeObject<President>(await response.Content.ReadAsStringAsync());
      product.Name.Should().Be("Donald Trump");
    }


    [Fact(Skip = "To not flood with failing tests")]
    public async Task getOne_fails_for_non_existing()
    {
      // act
      var response = await _client.GetAsync($"{BASE_URL}/-1");

      // assert
      response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }


    [Fact(Skip = "To not flood with failing tests")]
    public async Task post_returns_OK()
    {
      // act
      var response = await _client.PostAsync(POST_URL, ToJson(ADD_PRESIDENT_DATA));

      // assert
      response.IsSuccessStatusCode.Should().BeTrue();
      response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact(Skip = "To not flood with failing tests")]
    public async Task post_returns_JSON()
    {
      // act
      var response = await _client.PostAsync(POST_URL, ToJson(ADD_PRESIDENT_DATA));

      // assert
      response.Content.Headers.ContentType.MediaType.Should().Be("application/json");
    }

    [Fact(Skip = "To not flood with failing tests")]
    public async Task post_returns_correct_location_header()
    {
      // act
      var response = await _client.PostAsync(POST_URL, ToJson(ADD_PRESIDENT_DATA));

      // assert
      var president = await getPresident(response);
      response.Headers.Location.PathAndQuery.ToLowerInvariant().Should().Contain(BASE_URL.ToLowerInvariant());
      response.Headers.Location.PathAndQuery.Should().Contain(president.Id);
    }

    [Fact(Skip = "To not flood with failing tests")]
    public async Task post_returns_president_data()
    {
      // act
      var response = await _client.PostAsync(POST_URL, ToJson(ADD_PRESIDENT_DATA));

      // assert
      var president = await getPresident(response);
      president.Name.Should().Be(ADD_PRESIDENT_DATA.Name);
      president.From.Should().Be(ADD_PRESIDENT_DATA.From);
      president.To.Should().Be(ADD_PRESIDENT_DATA.To);
    }

    [Fact(Skip = "To not flood with failing tests")]
    public async Task post_stores_president_data()
    {
      // arrange
      var postResponse = await _client.PostAsync(POST_URL, ToJson(ADD_PRESIDENT_DATA));

      // act
      var response = await _client.GetAsync(postResponse.Headers.Location.PathAndQuery);

      // assert
      var president = await getPresident(response);
      president.Name.Should().Be(ADD_PRESIDENT_DATA.Name);
      president.From.Should().Be(ADD_PRESIDENT_DATA.From);
      president.To.Should().Be(ADD_PRESIDENT_DATA.To);
    }

    [Fact(Skip = "To not flood with failing tests")]
    public async Task post_returns_400_for_no_post_data()
    {
      // act
      var response = await _client.PostAsync(POST_URL, ToJson(null));

      // assert
      response.IsSuccessStatusCode.Should().BeFalse();
      response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact(Skip = "To not flood with failing tests")]
    public async Task post_returns_400_for_no_name_in_post_data()
    {
      // arrange
      var postWithNoName = new AddPresidentRequest { From = "1999", To = "2002" };

      // act
      var response = await _client.PostAsync(POST_URL, ToJson(postWithNoName));

      // assert
      response.IsSuccessStatusCode.Should().BeFalse();
      response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      var jsonResponse = await response.Content.ReadAsStringAsync();
      jsonResponse.Should().Contain("The Name field is required");
    }

    [Fact(Skip = "To not flood with failing tests")]
    public async Task post_returns_400_for_no_from_in_post_data()
    {
      // arrange
      var postWithNoFrom = new AddPresidentRequest { Name = "Marcus", To = "2002" };

      // act
      var response = await _client.PostAsync(POST_URL, ToJson(postWithNoFrom));

      // assert
      response.IsSuccessStatusCode.Should().BeFalse();
      response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      var jsonResponse = await response.Content.ReadAsStringAsync();
      jsonResponse.Should().Contain("The From field is required");
    }

    [Fact(Skip = "To not flood with failing tests")]
    public async Task put_returns_OK()
    {
      // arrange
      var getResponse = await _client.GetAsync(GETONE_URL);
      var trump = await getPresident(getResponse);
      var request = new UpdatePresidentRequest
      {
        Name = trump.Name,
        From = trump.From,
        To = "2016",
      };

      // act
      var response = await _client.PutAsync(PUT_URL, ToJson(request));

      // assert
      response.IsSuccessStatusCode.Should().BeTrue();
      response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact(Skip = "To not flood with failing tests")]
    public async Task put_returns_JSON()
    {
      // arrange
      var getResponse = await _client.GetAsync(GETONE_URL);
      var trump = await getPresident(getResponse);
      var request = new UpdatePresidentRequest
      {
        Name = trump.Name,
        From = trump.From,
        To = "2016",
      };

      // act
      var response = await _client.PutAsync(PUT_URL, ToJson(request));

      // assert
      response.Content.Headers.ContentType.MediaType.Should().Be("application/json");
    }

    [Fact(Skip = "To not flood with failing tests")]
    public async Task put_returns_update_resource()
    {
      // arrange
      var getResponse = await _client.GetAsync(GETONE_URL);
      var trump = await getPresident(getResponse);
      var request = new UpdatePresidentRequest
      {
        Name = "...",
        From = trump.From,
        To = "2016",
      };

      // act
      var response = await _client.PutAsync(PUT_URL, ToJson(request));

      // assert
      var president = await getPresident(response);
      president.Name.Should().Be(request.Name);
      president.From.Should().Be(request.From);
      president.To.Should().Be(request.To);
    }


    [Fact(Skip = "To not flood with failing tests")]
    public async Task put_updates_data_in_database()
    {
      // arrange
      var getResponse = await _client.GetAsync(GETONE_URL);
      var trump = await getPresident(getResponse);
      var request = new UpdatePresidentRequest
      {
        Name = trump.Name,
        From = trump.From,
        To = "2016",
      };
      var putResponse = await _client.PutAsync(PUT_URL, ToJson(request));

      // act
      var response = await _client.GetAsync(GETONE_URL);

      // assert
      var president = await getPresident(response);
      president.Name.Should().Be(request.Name);
      president.From.Should().Be(request.From);
      president.To.Should().Be(request.To);
    }

    [Fact(Skip = "To not flood with failing tests")]
    public async Task put_returns_400_for_no_post_data()
    {
      // act
      var response = await _client.PutAsync(PUT_URL, ToJson(null));

      // assert
      response.IsSuccessStatusCode.Should().BeFalse();
      response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact(Skip = "To not flood with failing tests")]
    public async Task put_returns_404_for_no_id_found()
    {
      // arrange
      var putRequest = new UpdatePresidentRequest
      {
        Name = "Trump",
        From = "2016",
        To = "2016",
      };

      // act
      var response = await _client.PutAsync($"{BASE_URL}/-1", ToJson(putRequest));

      // assert
      response.IsSuccessStatusCode.Should().BeFalse();
      response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(Skip = "To not flood with failing tests")]
    public async Task put_returns_400_for_no_name_in_post_data()
    {
      // arrange
      var putWithNoName = new UpdatePresidentRequest
      {
        From = "2016",
        To = "2016",
      };

      // act
      var response = await _client.PutAsync(PUT_URL, ToJson(putWithNoName));

      // assert
      response.IsSuccessStatusCode.Should().BeFalse();
      response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      var jsonResponse = await response.Content.ReadAsStringAsync();
      jsonResponse.Should().Contain("The Name field is required");
    }

    [Fact(Skip = "To not flood with failing tests")]
    public async Task put_returns_400_for_no_from_in_post_data()
    {
      // arrange
      var putWithNoFrom = new UpdatePresidentRequest
      {
        Name = "Apa",
        To = "2016",
      };

      // act
      var response = await _client.PutAsync(PUT_URL, ToJson(putWithNoFrom));

      // assert
      response.IsSuccessStatusCode.Should().BeFalse();
      response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
      var jsonResponse = await response.Content.ReadAsStringAsync();
      jsonResponse.Should().Contain("The From field is required");
    }

    [Fact(Skip = "To not flood with failing tests")]
    public async Task delete_returns_not_content()
    {
      // arrange
      var postResponse = await _client.PostAsync(POST_URL, ToJson(ADD_PRESIDENT_DATA));
      var url = postResponse.Headers.Location.PathAndQuery;

      // act
      var response = await _client.DeleteAsync(url);

      // assert
      response.IsSuccessStatusCode.Should().BeTrue();
      response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact(Skip = "To not flood with failing tests")]
    public async Task delete_deletes_data_in_database()
    {
      // arrange
      var postResponse = await _client.PostAsync(POST_URL, ToJson(ADD_PRESIDENT_DATA));
      var president = await getPresident(postResponse);
      var url = postResponse.Headers.Location.PathAndQuery;
      var deleteResponse = await _client.DeleteAsync(url);

      // act
      var response = await _client.GetAsync($"{BASE_URL}/{president.Id}");

      // assert
      response.IsSuccessStatusCode.Should().BeFalse();
      response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact(Skip = "To not flood with failing tests")]
    public async Task delete_returns_no_content_for_non_existing_id()
    {
      // act
      var response = await _client.DeleteAsync($"{BASE_URL}/-1");

      // assert
      response.IsSuccessStatusCode.Should().BeTrue();
      response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
  }
}
