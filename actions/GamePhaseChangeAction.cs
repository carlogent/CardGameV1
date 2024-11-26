using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ChangeGamePhaseAction: GameActionBase, IGameAction
{
    //private readonly Player player;
    GamePhase input_originalGamePhase;
    GamePhase input_targetGamePhase;

    GamePhaseChangeAttemptEvent attemptEvent;
    GamePhaseChangeActivateEvent activateEvent;
    GamePhaseChangeResolvedEvent resolvedEvent;
    public ChangeGamePhaseAction(ICardEffect originCardEffect,  int cardEffectActivationSequenceId, GamePhase _originalGamePhase, GamePhase _targetGamePhase):
    base(_originCardEffect: originCardEffect, _effectActivationSequenceId: cardEffectActivationSequenceId)
    {
        input_originalGamePhase = _originalGamePhase;
        input_targetGamePhase = _targetGamePhase;
    }

    // should be player/owner agnostic, should only check for things that it ABSOLUTLY needs to function
    public bool CanAction()
    {
        // anything about the state the says it cant be done? maybe if currently playing animation..

        // create attempt event and add it to the queue
        attemptEvent = new GamePhaseChangeAttemptEvent(GameState.GenerateUniqueSequenceId(), this, input_originalGamePhase, input_targetGamePhase); //needs to be new sequence number or effects wont recognize something new in the queue
        GameDuelEvents.OnGamePhaseChange.Attempt.Add(attemptEvent); 

        // process the new events
        GameTools.EvaluateCardsConditionals();

        //if the attempt not negated by a effect and if player has cards in their deck to draw
        if(!attemptEvent.negated){
            return true;
        }else{
            return false;
        }
    }

    public void SetEvents()
    {
        activateEvent = new GamePhaseChangeActivateEvent(GameState.GenerateUniqueSequenceId(), this, input_originalGamePhase, input_targetGamePhase);
        GameDuelEvents.OnGamePhaseChange.Activate.Add(activateEvent);
    }

    public void Action()
    {
        if(CanAction()) //this check needed here??
        {
            GameState.gamePhase = EnumExtensions.GetNextEnum(GameState.GetGamePhase());
            resolvedEvent = new GamePhaseChangeResolvedEvent(GameState.GenerateUniqueSequenceId(), this, activateEvent.originalPhase, activateEvent.targetPhase);
            GameDuelEvents.OnGamePhaseChange.Resolved.Add(resolvedEvent);
        }
    }
}







