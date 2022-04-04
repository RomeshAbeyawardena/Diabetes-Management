using DiabetesManagement.Shared.Attributes;
using DiabetesManagement.Shared.Base;
using System;
using System.Data;
using System.Threading.Tasks;

namespace DiabetesManagement.Api.RequestHandlers.InventoryHistory
{
    using Models = Shared.Models;
    using InventoryFeature = Shared.RequestHandlers.Inventory;
    using InventoryHistoryFeature = Shared.RequestHandlers.InventoryHistory;
    using UserFeature = Shared.RequestHandlers.User;

    [HandlerDescriptor(Commands.SaveInventoryPayload)]
    public class Post : HandlerBase<SaveRequest, Models.InventoryHistory>
    {
        public Post(string connectionString) : base(connectionString)
        {
        }

        public Post(IDbConnection dbConnection, IDbTransaction dbTransaction = null) : base(dbConnection, dbTransaction)
        {
        }

        protected override void Dispose(bool disposing)
        {

        }

        protected override async Task<Models.InventoryHistory> HandleAsync(SaveRequest command)
        {
            var user = await HandlerFactory
                .Execute<UserFeature.GetRequest, Models.User>(UserFeature.Queries.GetUser, new UserFeature.GetRequest
                {
                    UserId = command.UserId
                });

            var userId = user?.UserId;

            if (user == null)
            {
                userId = await HandlerFactory.Execute<UserFeature.SaveCommand, Guid>(UserFeature.Commands.SaveUser, new UserFeature.SaveCommand
                {
                    User = new Models.User
                    {
                        UserId = command.UserId,
                        Created = command.Created
                    }
                });
            }


            var inventory = await HandlerFactory.Execute<InventoryFeature.GetRequest, Models.Inventory>(
                InventoryFeature.Queries.GetInventory,
                new InventoryFeature.GetRequest
                {
                    Type = command.Type,
                    Key = command.Key,
                    UserId = userId.Value,
                });

            var inventoryId = inventory?.InventoryId;

            if (inventory == null)
            {
                inventoryId = await HandlerFactory.Execute<InventoryFeature.SaveCommand, Guid>(
                    InventoryFeature.Commands.SaveInventory,
                    new InventoryFeature.SaveCommand
                    {
                        Inventory = new Models.Inventory
                        {
                            Created = DateTimeOffset.UtcNow,
                            DefaultType = command.Type,
                            Key = command.Key,
                            UserId = command.UserId
                        },
                    });
            }
            else
            {
                await HandlerFactory.Execute<InventoryFeature.SaveCommand, Guid>(InventoryFeature.Commands.UpdateInventory,
                    new InventoryFeature.SaveCommand
                    {
                        Inventory = new Models.Inventory
                        {
                            InventoryId = inventoryId.Value,
                            Modified = DateTimeOffset.UtcNow
                        }
                    });
            }

            if (!inventoryId.HasValue)
            {
                throw new DataException();
            }

            var inventoryHistoryId = await HandlerFactory.Execute<InventoryHistoryFeature.SaveCommand, Guid>(
                    InventoryHistoryFeature.Commands.SaveInventoryHistory,
                    new InventoryHistoryFeature.SaveCommand
                    {
                        InventoryHistory = new Models.InventoryHistory
                        {
                            Created = DateTimeOffset.Now,
                            //Key = command.Key,
                            Type = command.Type,
                            Items = command.Items,
                            InventoryId = inventoryId.Value,
                            //UserId = command.UserId,
                        },
                        CommitOnCompletion = true
                    });

            return await HandlerFactory.Execute<InventoryHistoryFeature.GetRequest, Models.InventoryHistory>(
                    InventoryHistoryFeature.Queries.GetInventoryHistory,
                    new InventoryHistoryFeature.GetRequest
                    {
                        InventoryHistoryId = inventoryHistoryId
                    }
                );
        }
    }
}
