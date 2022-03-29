using DiabetesManagement.Shared.Enumerations;

namespace DiabetesManagement.Shared.Contracts
{
    public interface IQueryBuilder<TModel>
        where TModel : IDbModel
    {
        TModel Model { get;  }
        BuildMode BuildMode { get; set; }

        int? TopAmount { get; set; }
        string Query { get; }
    }
}
