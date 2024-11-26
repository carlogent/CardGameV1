using System.Collections.Generic;

public class CardEffectActivation
{
    public ICardEffect cardEffect;
    public bool negated; //both the action event, and card effect can be negated.
    public int effectActivationSequenceId; // currently this is derived from the eventSequenceIdSource.. should we have a different source for this?

    //public List<IGameAction> actions; //modify these actions, these references are pretty useless i think not accessed from here but instead from the event pools

    public CardEffectActivation(ICardEffect _cardEffect, int _effectActivationSequenceId){ //, List<IGameAction> _actions
        cardEffect = _cardEffect;
        effectActivationSequenceId = _effectActivationSequenceId;
        //actions = _actions;
    } 
    
}