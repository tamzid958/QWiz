namespace QWiz.Entities.Abstract;

public interface IAbstractAuditable<TKey, TUser, TUserKey> : IAbstractPersistence<TKey>
{
    public TUserKey? CreatedById { set; get; }

    public TUser? CreatedBy { set; get; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
}