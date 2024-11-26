public class GameActionBase{
    public ICardEffect originCardEffect;
    public int effectActivationSequenceId;
    public int actionSequenceId;

    public GameActionBase(ICardEffect _originCardEffect, int _effectActivationSequenceId)
    {
        originCardEffect = _originCardEffect;
        effectActivationSequenceId = _effectActivationSequenceId;
        actionSequenceId = GameState.GenerateUniqueSequenceId();
    }
}
