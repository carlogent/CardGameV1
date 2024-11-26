using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class DrawAction: GameActionBase, IGameAction
{
    //private readonly Player player;
    public int numberOfDraws;
    DrawAttemptEvent attemptEvent;
    DrawActivateEvent activateEvent;
    DrawResolvedEvent resolvedEvent;
    Player targetPlayer; 

    public DrawAction(ICardEffect originCardEffect,  int cardEffectActivationSequenceId, Player _player ,int _numberOfDraws):
    base(_originCardEffect: originCardEffect, _effectActivationSequenceId: cardEffectActivationSequenceId)
    {
        numberOfDraws = _numberOfDraws;
        targetPlayer = _player;
    }

    public bool CanAction(){
        
        // makes sure enough cards in deck.

        // create attempt event and add it to the queue
        attemptEvent = new DrawAttemptEvent(GameState.GenerateUniqueSequenceId(), this); //needs to be new sequence number or effects wont recognize something new in the queue
        GameDuelEvents.OnDraw.Attempt.Add(attemptEvent); 

        // process the new events
        GameTools.EvaluateCardsConditionals();

        //if the attempt not negated by a effect and if player has cards in their deck to draw
        if(!attemptEvent.negated && targetPlayer.deck.cards.Count > 0){
            return true;
        }else{
            return false;
        }
    }

    public void SetEvents(){
        activateEvent = new DrawActivateEvent(GameState.GenerateUniqueSequenceId(), this); //should be draw for turn event?
        GameDuelEvents.OnDraw.Activate.Add(activateEvent);
    }

    public void Action(){
        targetPlayer.cardsInHand += numberOfDraws; //do this for testing
        resolvedEvent = new DrawResolvedEvent(GameState.GenerateUniqueSequenceId(), this);
        GameDuelEvents.OnDraw.Resolved.Add(resolvedEvent);
    }
}









