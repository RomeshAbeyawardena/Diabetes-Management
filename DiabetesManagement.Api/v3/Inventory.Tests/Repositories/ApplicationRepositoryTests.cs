using Inventory.Core.Features.Application;
using Inventory.Persistence.Repositories;
using Inventory.Models;
using Inventory.Contracts;
using Inventory.Features.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Tests.Repositories;

public class ApplicationRepositoryTests
{
    private ApplicationRepository? sut;

    private Mock<IDbContextProvider>? dbContextProviderMock;
    private Mock<IClockProvider>? clockProvider;
    private Mock<IConfiguration>? configurationMock;
    [SetUp]
    public void SetUp()
    {
        dbContextProviderMock = new();
        configurationMock = new();
        clockProvider = new();

        //dbContextProviderMock.Setup(s => s.GetDbContext<InventoryDbContext>()).Returns(new InventoryDbContext(options));
        //sut = new (dbContextProviderMock.Object, new ApplicationSettings(configurationMock.Object), clockProvider.Object);
    }

    [Test]
    public async Task Save()
    {
        //await sut.Save(new Application { }, CancellationToken.None);
    }
}
