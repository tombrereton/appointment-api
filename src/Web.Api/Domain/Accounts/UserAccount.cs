using Appointer.Web.Api.Common;
using Appointer.Web.Api.Domain.Calendars;
using CommunityToolkit.Diagnostics;

namespace Appointer.Web.Api.Domain.Accounts;

public sealed class UserAccount : Entity
{
    private UserAccount(Guid id, string fullName, bool isActive, bool isDeleted)
    {
        Id = id;
        FullName = fullName;
        IsActive = isActive;
        IsDeleted = isDeleted;
    }

    private UserAccount()
    {
    }

    public string FullName { get; private set; }
    public List<Calendar> Calendars { get; private set; } = [];
    public bool IsActive { get; private set; }
    public bool IsDeleted { get; private set; }

    public static UserAccount Create(string fullName)
    {
        var userAccount = new UserAccount(Guid.NewGuid(), fullName, true, false);

        userAccount.RaiseDomainEvent(new UserAccountCreatedDomainEvent(userAccount.Id));

        return userAccount;
    }

    public void AddCalendar(Calendar calendar)
    {
        Guard.IsNotNull(calendar, nameof(calendar));
        Calendars.Add(calendar);

        RaiseDomainEvent(new DefaultCalendarAddedDomainEvent(this.Id, calendar.Id));
    }
}