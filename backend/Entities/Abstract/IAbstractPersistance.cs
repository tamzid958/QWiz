namespace QWiz.Entities.Abstract;

public interface IAbstractPersistence<TKey>
{
    public TKey Id { get; set; }
}