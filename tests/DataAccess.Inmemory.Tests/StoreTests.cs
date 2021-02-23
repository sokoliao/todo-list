using System;
using System.Linq;
using System.Threading.Tasks;
using DataAccess.Abstraction;
using DataAccess.Inmemory.Stores;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace DataAccess.Inmemory.Tests
{
    public class StoreTests
    {
        [Fact]
        public async Task ShouldGetTasks()
        {
            //  Arrange

            var storeOptions = new Mock<IOptions<TodoTaskStoreOption>>();
            storeOptions
                .SetupGet(m => m.Value)
                .Returns(new TodoTaskStoreOption
                {
                    InitialSet = StoreTests.tasks.ToArray()
                });

            var store = new TodoTaskStore(storeOptions.Object);

            // Act

            var result = await store.Get();

            // Assert

            Assert.Equal(StoreTests.tasks, result);
        }

        [Fact]
        public async Task ShouldSetTasks()
        {
            //  Arrange

            var storeOptions = new Mock<IOptions<TodoTaskStoreOption>>();
            storeOptions
                .SetupGet(m => m.Value)
                .Returns(new TodoTaskStoreOption
                {
                    InitialSet = StoreTests.tasks.ToArray()
                });

            var store = new TodoTaskStore(storeOptions.Object);

            // Act

            await store.Set(existing => existing.Take(1));
            var result = await store.Get();

            // Assert

            Assert.Equal(Enumerable.Empty<TodoTaskEntity>().Append(StoreTests.tasks[0]), result);
        }

        private static readonly TodoTaskEntity[] tasks = new TodoTaskEntity[2]
        {
            new TodoTaskEntity
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Make a toast",
                Priority = 1,
                Status = TodoTaskEntityStatus.NotStarted
            },
            new TodoTaskEntity
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Eat a toast",
                Priority = 1,
                Status = TodoTaskEntityStatus.NotStarted
            }
        };
    }
}