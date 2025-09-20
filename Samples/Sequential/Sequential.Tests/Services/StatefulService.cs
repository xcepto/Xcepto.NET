namespace Sequential.Tests.Services;

public class StatefulService
{
    public State State { get; private set; } = State.A;
    public void Advance()
    {
        switch (State)
        {
            case State.A:
                State = State.B;
                break;
            case State.B:
                State = State.C;
                break;
            case State.C:
                throw new ArgumentOutOfRangeException();
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}

public enum State
{
    A,
    B,
    C
}