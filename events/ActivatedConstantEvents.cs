
//do not use
public class ActivatedConstantAttemptEvent: GameEvent<ActivatedConstantAction> 
{
    public ActivatedConstantAttemptEvent(int eventSequenceId, ActivatedConstantAction action): base(_eventSequenceId: eventSequenceId, _action: action)
    {}
}

//do not use
public class ActivatedConstantActivateEvent: GameEvent<ActivatedConstantAction> 
{
    public ActivatedConstantActivateEvent(int eventSequenceId, ActivatedConstantAction action): base(_eventSequenceId: eventSequenceId, _action: action)
    {}
}

public class ActivatedConstantResolvedEvent: GameEvent<ActivatedConstantAction>
{
    public int? targetEventSequenceId; //the sequence id of the event the ActivatedConstant effect modified 
    public ICardEffect originCardEffect; // the origin cardEffect (check its global id or instance if needed when checking conditionals)

    //this is used so that the effect which does not activate can see that it already modified the triggering event/action

    public ActivatedConstantResolvedEvent(int eventSequenceId, ActivatedConstantAction action): base(_eventSequenceId: eventSequenceId, _action: action, _isNegatable: false)
    {}
}

