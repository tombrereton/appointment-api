using Web.Api.Common;
using Web.Api.Domain.Accounts;

namespace Web.Api.Domain.Calendars;

public sealed class Calendar : Entity
{
    private Calendar(Guid id, string name)
    {
        Id = id;
        Name = name;
    }

    private Calendar()
    {
    }

    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public Guid UserAccountId { get; private set; }
    public UserAccount UserAccount { get; private set; } = null!;

    public static Calendar Create(string fullName)
    {
        var calendar = new Calendar(Guid.NewGuid(), fullName);

        // userAccount.RaiseDomainEvent(new UserAccountCreatedDomainEvent(userAccount.Id));

        return calendar;
    }
}
