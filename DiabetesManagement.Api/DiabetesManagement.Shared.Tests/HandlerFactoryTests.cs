using AutoMapper;
using DiabetesManagement.Shared.RequestHandlers;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DiabetesManagement.Shared.Tests;
using InventoryRequestHandlers = RequestHandlers.Inventory;
using InventoryHistoryRequestHandlers = RequestHandlers.InventoryHistory;
using UserRequestHandlers = RequestHandlers.User;
public class HandlerFactoryTests 
{
    private HandlerFactory? handlerFactory;
    private Mock<ILogger>? loggerMock;
    private Mock<IDbConnection>? dbConnectionMock;
    private Mock<IMapper>? mapperMock;
    private async Task VerifyIsNotNull(IEnumerable<string> queries)
    {
        foreach (var query in queries)
        {
            var requestHandler = await handlerFactory!.GetRequestHandler(query);

            Assert.IsNotNull(requestHandler);
            Assert.IsNotNull(requestHandler.HandlerFactory);
            Assert.AreSame(handlerFactory, requestHandler.HandlerFactory);
        }
    }

    [SetUp] public void Setup()
    {
        loggerMock = new();
        dbConnectionMock = new();
        mapperMock = new();
        handlerFactory = new(loggerMock.Object, dbConnectionMock.Object, mapperMock.Object);
    }

    [Test] public async Task Ensure_QueryHandlers_exist()
    {
        var queries = new[] {
            InventoryRequestHandlers.Queries.GetInventory,
            InventoryRequestHandlers.Commands.SaveInventory,
            InventoryRequestHandlers.Commands.UpdateInventory,
            InventoryHistoryRequestHandlers.Queries.GetInventoryHistory,
            InventoryHistoryRequestHandlers.Commands.SaveInventoryHistory,
            UserRequestHandlers.Queries.GetUser,
            UserRequestHandlers.Commands.SaveUser,
            InventoryRequestHandlers.Queries.GetInventory,
            InventoryRequestHandlers.Commands.SaveInventory,
            InventoryRequestHandlers.Commands.UpdateInventory,
            InventoryHistoryRequestHandlers.Queries.GetInventoryHistory,
            InventoryHistoryRequestHandlers.Commands.SaveInventoryHistory,
            UserRequestHandlers.Queries.GetUser,
            UserRequestHandlers.Commands.SaveUser
        };

        await VerifyIsNotNull(queries);
    }

    [Test] public void GetQueryHandlers_throws_InvalidOperationException()
    {
        var query = "Invalid-Query";
        Assert.ThrowsAsync<InvalidOperationException>(async() => await handlerFactory!.GetRequestHandler(query));
    }
}