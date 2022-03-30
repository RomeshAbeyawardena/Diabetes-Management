using DiabetesManagement.Shared.Enumerations;

namespace DiabetesManagement.Shared.Contracts
{
    public interface IQueryBuilder
    {
        BuildMode BuildMode { get; set; }
        int? TopAmount { get; set; }
        string Query { get; }

        IQueryBuilder SetBuildMode(BuildMode buildMode);
        IQueryBuilder GenerateUpdateBody<TRequest>(TRequest request);
        IQueryBuilder GenerateWhereClause<TRequest>(TRequest request);
    }

    public interface IQueryBuilder<TModel> : IQueryBuilder
        where TModel : IDbModel
    {
        TModel Model { get; }
    }
}
