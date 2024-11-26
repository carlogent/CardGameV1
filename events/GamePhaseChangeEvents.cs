public class GamePhaseChangeAttemptEvent: GameEvent<ChangeGamePhaseAction> 
{
    public  GamePhase originalPhase;
    public  GamePhase targetPhase;

    public GamePhaseChangeAttemptEvent(int eventSequenceId, ChangeGamePhaseAction action, GamePhase _originalPhase,  GamePhase _targetPhase): base(_eventSequenceId: eventSequenceId, _action: action)
    {
        originalPhase = _originalPhase;
        targetPhase = _targetPhase;
    }
}

public class GamePhaseChangeActivateEvent: GameEvent<ChangeGamePhaseAction> 
{
    public GamePhase originalPhase;
    public  GamePhase targetPhase;

    public GamePhaseChangeActivateEvent(int eventSequenceId, ChangeGamePhaseAction action, GamePhase _originalPhase,  GamePhase _targetPhase): base(_eventSequenceId: eventSequenceId, _action: action)
    {
        originalPhase = _originalPhase;
        targetPhase = _targetPhase;
    }
}

public class GamePhaseChangeResolvedEvent: GameEvent<ChangeGamePhaseAction>
{
    public GamePhase originalPhase;
    public  GamePhase targetPhase;
    public GamePhaseChangeResolvedEvent(int eventSequenceId, ChangeGamePhaseAction action, GamePhase _originalPhase,  GamePhase _targetPhase): base(_eventSequenceId: eventSequenceId, _action: action)
    {
        originalPhase = _originalPhase;
        targetPhase = _targetPhase;
    }
}