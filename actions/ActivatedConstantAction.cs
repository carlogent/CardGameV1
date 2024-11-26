using System;
using System.Collections;
using System.Collections.Generic;


//a unique action simply used to fullfill the requirements of the special ActivatedConstantEvents
public class ActivatedConstantAction: GameActionBase, IGameAction
{
    public ActivatedConstantAction(ICardEffect originCardEffect,  int cardEffectActivationSequenceId):
    base(_originCardEffect: originCardEffect, _effectActivationSequenceId: cardEffectActivationSequenceId)
    {}

    public bool CanAction(){
        throw new Exception("this kind of card effect should not check if it can through here?? maybe");
    }

    public void SetEvents(){
        throw new Exception("this kind of card effect should not create events through here?? maybe");
    }

    public void Action(){
        throw new Exception("this kind of card effect should not do actions through here?? maybe");
    }
}