using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using System.Text.Json.Serialization;
using BxTestTask.Controllers;
using System.Text.Json;
using BxTestTask.Stores;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace ServerTests.integration
{
  public class TodoTaskControllerTests
  {
    [Fact]
    public async Task ShouldGetAll()
    {
      // Arrange

      using var server = new WebAppFixture(TodoTaskControllerTests.tasks);
      using var client = server.CreateClient();

      // Act

      var response = await client.GetAsync("tasks/all");
      var result = await DeserializeAsync<IEnumerable<TodoTaskModel>>(response.Content);

      // Assert

      Assert.True(response.IsSuccessStatusCode);
      Assert.Collection(result, 
        model => AssertEqualToEntity(model, tasks[0]),
        model => AssertEqualToEntity(model, tasks[1]));
    }

    [Fact]
    public async Task ShouldCreateNew()
    {
      // Arrange

      using var server = new WebAppFixture(TodoTaskControllerTests.tasks);
      using var client = server.CreateClient();

      // Act

      var response = await client.PostAsync(
        "tasks/new", 
        Serialize(new
          {
            name = "Buy an avocado",
            priority = 1,
            status = "Completed"
          }));
      var result = await DeserializeAsync<CreateTodoTaskResultModel>(response.Content);

      // Assert

      Assert.True(response.IsSuccessStatusCode);
      Assert.NotNull(result);
      Assert.NotEqual(Guid.Empty, result.Id);
    }

    [Fact]
    public async Task ShouldReturn400WhenNewTaskInvalid()
    {
      // Arrange

      using var server = new WebAppFixture(TodoTaskControllerTests.tasks);
      using var client = server.CreateClient();

      // Act

      var response = await client.PostAsync(
        "tasks/new", 
        Serialize(new
          {
            name = "Buy an avocado",
            priority = 0,
            status = "Completed"
          }));
      var result = await DeserializeAsync<CreateTodoTaskResultModel>(response.Content);

      // Assert

      Assert.Equal(400, (int)response.StatusCode);
    }

    [Fact]
    public async Task ShouldUpdate()
    {
      // Arrange

      using var server = new WebAppFixture(TodoTaskControllerTests.tasks);
      using var client = server.CreateClient();

      // Act

      var response = await client.PutAsync(
        $"tasks/update/{tasks[0].Id}", 
        Serialize(new
          {
            name = "Buy an avocado",
            priority = 1,
            status = "Completed"
          }));

      // Assert

      Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task ShouldReturn404WhenUpdateMissingTask()
    {
      // Arrange

      using var server = new WebAppFixture(TodoTaskControllerTests.tasks);
      using var client = server.CreateClient();

      // Act

      var response = await client.PutAsync(
        $"tasks/update/{Guid.NewGuid()}", 
        Serialize(new
          {
            name = "Buy an avocado",
            priority = 1,
            status = "Completed"
          }));

      // Assert

      Assert.Equal(404, (int)response.StatusCode);
    }

    [Fact]
    public async Task ShouldReturn400WhenUpdateInvalid()
    {
      // Arrange

      using var server = new WebAppFixture(TodoTaskControllerTests.tasks);
      using var client = server.CreateClient();

      // Act

      var response = await client.PutAsync(
        $"tasks/update/{tasks[0].Id}", 
        Serialize(new
          {
            name = tasks[1].Name,
            priority = 1,
            status = "Completed"
          }));

      // Assert

      Assert.Equal(400, (int)response.StatusCode);
    }

    [Fact]
    public async Task ShouldDelete()
    {
      // Arrange

      using var server = new WebAppFixture(TodoTaskControllerTests.tasks);
      using var client = server.CreateClient();

      // Act

      var response = await client.DeleteAsync($"tasks/delete/{tasks[0].Id}");

      // Assert

      Assert.True(response.IsSuccessStatusCode);
    }

    [Fact]
    public async Task ShouldReturn404WhenDeleteMissingTask()
    {
      // Arrange

      using var server = new WebAppFixture(TodoTaskControllerTests.tasks);
      using var client = server.CreateClient();

      // Act

      var response = await client.DeleteAsync($"tasks/delete/{Guid.NewGuid()}");

      // Assert

      Assert.Equal(404, (int)response.StatusCode);
    }

    [Fact]
    public async Task ShouldReturn400WhenDeleteInvalid()
    {
      // Arrange

      using var server = new WebAppFixture(TodoTaskControllerTests.tasks);
      using var client = server.CreateClient();

      // Act

      var response = await client.DeleteAsync($"tasks/delete/{tasks[1].Id}");

      // Assert

      Assert.Equal(400, (int)response.StatusCode);
    }

    public static void AssertEqualToEntity(TodoTaskModel model, TodoTaskEntity entity)
    {
      Assert.NotNull(model);
      Assert.NotNull(entity);
      Assert.Equal(entity.Id, model.Id);
      Assert.Equal(entity.Name, model.Name);
      Assert.Equal(entity.Priority, model.Priority);
      Assert.Equal((int)entity.Status, (int)model.Status);
    }

    public static HttpContent Serialize<T>(T data)
    {
      var options = new JsonSerializerOptions();
      options.Converters.Add(new JsonStringEnumConverter());
      return new StringContent(
        JsonSerializer.Serialize<T>(data, options), 
        Encoding.UTF8, 
        "application/json");
    }

    public static async Task<T> DeserializeAsync<T>(HttpContent content)
    {
      var options = new JsonSerializerOptions();
      options.PropertyNameCaseInsensitive = true;
      options.Converters.Add(new JsonStringEnumConverter());
      return await JsonSerializer.DeserializeAsync<T>(
        await content.ReadAsStreamAsync(), 
        options);
    }

    private static readonly TodoTaskEntity[] tasks = new TodoTaskEntity[2] 
    {
      new TodoTaskEntity
      {
        Id = Guid.NewGuid(),
        Name = "Make a toast",
        Priority = 1,
        Status = TodoTaskEntityStatus.Completed
      },
      new TodoTaskEntity
      {
        Id = Guid.NewGuid(),
        Name = "Eat a toast",
        Priority = 1,
        Status = TodoTaskEntityStatus.NotStarted
      }
    };
  }
}