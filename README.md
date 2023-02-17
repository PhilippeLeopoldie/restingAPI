# Resting API

In this test we want you to create a RESTful API, using WebAPI.

The resource you will work with are American presidents and your RESTful resource should be located at `/api/presidents`.

The data for each president is stored in-memory and can be retrieved through the `IPresidentRepository` interface.

Here is one president represented as JSON:

```json
{
  "id": "e8584982-6c32-4408-870f-e2712f735fce",
  "from": "2009",
  "to": "2017",
  "name": "Barack Obama"
}
```

For each president the following rules apply:

- `id` is required. The next id can created with `Guid.NewGuid().ToString()`
- `name` is required
- `from` is required and should be the year as a YYYY-string, for example `2010`
- `to` is not required as some presidents has not ended their tenure ... yet.

RESTful means that you should adhere to the REST style and supply endpoints for:

- Create (POST) - creating a new president... without riots, please!
- Read (GET) - two endpoints; one to get one president and one that lists all presidents.
- Update (PUT) - to update president data. Remember that PUT replaces the data with the payload you send it, where PATCH updates individual fields. PATCH is not part of this test.
- Delete (DELETE) - to delete one president.

Let the REST constraints guide you in how to structure the endpoints.
Ensure to return the correct status codes, location and content-type headers, as we have talked about during the week. For each endpoint return the data that is created, or updated, in JSON-format.

We want you to demonstrate that you can write RESTful APIs and return appropriate results, status codes and potential error messages from your API endpoints.

## Handling errors

Your Web API should never crash, but handle any potential errors in a graceful manner. Use the status codes where you can, to help the client to understand what has happened, if an error has happened.

## A word on the "database"

The database is just in-memory and accessed via the `PresidentRepository`, and can be injected in the constructor of a controller (we have set this up in the IoC container for you, see `Startup.cs`)

This means that you can inject the `IPresidentRepository` into the the controller and it will resolve to `PresidentRepository` at runtime, like this:

```c#
private IPresidentRepository _repo;
public PresidentsController(IPresidentRepository repo)
{
  _repo = repo;
}
```

Since the database is "in-memory" remember that any changes you make to the data will go away between test runs. But we will seed the database with a list of the latests presidents for you. See the `SeedDatabase`-class.

## Boiler plate code and scripts

We have supplied a lot of integration-tests in the `Resting.Tests` project for the WebAPI. We encourage you to write more tests.

A lot of these tests are skipped by default since you would otherwise have a lot of failing tests to go through. Change:

```c#
[Fact(Skip = "To not flood with failing tests")]
```

to

```c#
[Fact]
```

to un-skip the test, which include the test to run with `dotnet test`.

We have also created some starting points for you,

- the initialization of the controller (but no Routing-values...)
- the `President`-data class that is stored in the database
- when creating new Presidents, the data should be sent via the `AddPresidentRequest`-class.
- when updating a president, the data should be sent via the `UpdatePresidentRequest`-class.

## Handing in your solution

Please submit your `Solution.cs` file to your Google Drive in a new directory called `restingAPI` inside the directory with your name. Ensure that all code is written in the `Solution.cs` file (this is a requirement from our weekend-test-correcting tool).

## FAQ

Should ID be included in the request body or part of the url, when making a put request to the api?

> It should be in the url. All other data should be in the body!
