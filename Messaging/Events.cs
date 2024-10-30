namespace Messaging
{
    public class SomethingHappened
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = default!;
    }

    public class SomethingElseHappened
    {
        public Guid Id { get; init; }
        public string Name { get; init; } = default!;
    }
}
