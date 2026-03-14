using ThabeSoft.Ddd.Domain.Entities;

namespace Test.Mocks;

public class TestUser : AggregateRoot<TestUserId>
{
    public string Name { get; private set; }

    public TestUser(TestUserId id, string name) : base(id)
    {
        Name = name;
        AddDomainEvent(new TestUserCreatedEvent(id));
    }

    public void ChangeName(string value) => Name = value;
}

public record TestUserId(Guid Value);