
public class DrawAttemptEvent: GameEvent<DrawAction>  //replace AttemptGameEvent<DrawAction> with GameEvent<DrawAction>?? cleaner but less flexible.. last on priority
{
    public DrawAttemptEvent(int eventSequenceId, DrawAction action): base(_eventSequenceId: eventSequenceId, _action: action)
    {}
}

public class DrawActivateEvent: GameEvent<DrawAction> 
{
    public DrawActivateEvent(int eventSequenceId, DrawAction action): base(_eventSequenceId: eventSequenceId, _action: action)
    {}
}

public class DrawResolvedEvent: GameEvent<DrawAction>
{
    public DrawResolvedEvent(int eventSequenceId, DrawAction action): base(_eventSequenceId: eventSequenceId, _action: action, _isNegatable: false)
    {}
}

