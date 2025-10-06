namespace QuizService.Domain.Abstraction;

public abstract class Entity<T>:IEntity<T>
{
    public DateTime? CreateTime { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? LastModified { get; set; }
    public string? LastModifiedBy { get; set; }
    public T Id { get; set; }
}