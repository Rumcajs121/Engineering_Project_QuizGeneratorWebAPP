namespace QuizService.Domain.Abstraction;

public interface IRepository<TAggregate, TId>
    where TAggregate : IAggregate<TId>
{
    //TODO: Joint method repository
}