using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#nullable enable

// Tracks everything about the game 
public static class GameState //make ALL OTHER COMMONLY ACCESSED CLASS INSTANCE singletons!
{
    public static Player defaultOwner; //used for default cards
    public static ICard defaultCard; // used to attach effects (rules with execution) which have no owners such as game-effects
    public static GamePhase gamePhase; // current game-phase
    public static int turnNumber; // current turn number
    public static int priorityNumber; // everytime a player does an action that creates events (an action that changes the state), this number gets incremented signifying that the other player has priority (odd is player 1, even is player 2)
    public static List<ICard> liveCards;
    public static Player[] players; //only be 2, players[0] is turn 1 player
    public static int eventSequenceIdSource; //most granular id, given to every single event/action, only used to order of activations, etc.
    //public int chainStartingSequenceId; // (-1 = no effect so far)  this chain
    public static List<int> turnStartSequenceNumberList; //Index: turnNumber  Value: SequenceNumberStart

    //this is set by the executor system

    public static List<ChainSequence> chainStartSequenceNumberList; //Index: turnNumber  Value: SequenceNumberStart //if end is null that means the chain is not completed yet
    public static int lastChainIndex = -1;

    public static EventChain duelEventChain; //recordds all events of the duel, does NOT record GUI events only DUEL events, depracated possibly not usefull, use GameDuelEvents static
    public static CardEffectActivationChain effectActivationChain; // do we keep the constant activated effects here to? when they trigger?

    // the next effect in the chain, settable by an effect
    public static ICardEffect? nextEffect;

    // trigger effects dont activate, they just run a callback when an event happens, this is used for generic non-event functions like refreshing the activations of an effect or refreshing certain gamestate things.
    public static Dictionary<DuelTriggerType, TriggerEffect> triggers;

    public static void RegisterCallbackToTrigger(DuelTriggerType triggerType, Action callback){
        triggers[triggerType].subscribers += callback;
    }

    public static void RemoveCallbackToTrigger(DuelTriggerType triggerType, Action callback){
        triggers[triggerType].subscribers -= callback;
    }


    static GameState(){
        gamePhase = GamePhase.DrawPhaseStart;
        turnNumber = 1;

        players = new Player[2];
        players[0] = new Player(new Vector3(0, 0,0), "Pippy", Owner.PlayerOne ); //the first person is turn 1 player
        players[1] = new Player(new Vector3(1, 0,0), "Garlock", Owner.PlayerTwo);

        eventSequenceIdSource = 0;
        duelEventChain = new EventChain();
        chainStartSequenceNumberList = new List<ChainSequence>();
        effectActivationChain = new CardEffectActivationChain();

        turnStartSequenceNumberList = new List<int>
        {
            eventSequenceIdSource
        };

        defaultOwner = new Player(new Vector3(999f,999f,999f), "Game Manager Player", Owner.GameMechanic);
        defaultCard = new DefaultCard(defaultOwner);

        //registering effects here (should do it in the card likley..)
        
        //defaultCard.effects.Add(typeof(DrawPhaseStartToDrawPhaseEffect), new DrawPhaseStartToDrawPhaseEffect());

        //adding core game rules..
        //defaultCard.effects.Add(0, new DrawForTurnEffect());

        //testing, list of example cards in play
        liveCards = new List<ICard>(){
            defaultCard
        };

        // must be processed specificly, not part of livecards
        triggers = new Dictionary<DuelTriggerType, TriggerEffect>();

        // add the on-turn-change trigger effect.
        OnTurnChangeTriggerEffect refreshOnTurnChangeEffect = new OnTurnChangeTriggerEffect();
        triggers.Add(DuelTriggerType.OnTurnChangeResolved, new TriggerEffect(refreshOnTurnChangeEffect));

        //for each trigger check if can, then if can, immedietly raise event!
        //triggers[DuelTrigger.OnTurnChangeResolved].RaiseEvent();

        gamePhase = GamePhase.DrawPhaseStart;

        priorityNumber = 1; //player 1 starts with priority

        //reference default card.. this is for the start of the game.. game will start in draw-phase-start and things will respond if they cant and then execute this.
        nextEffect = defaultCard.effects[typeof(DrawPhaseStartToDrawPhaseEffect)];


        // MAKE SURE EVERYTHING IS SETUP FOR DRAW-PHASE-START
    }


    


    public static Player GetTurnPlayer(){
         if (turnNumber % 2 == 0){
            return players[1];
         }else{
            return players[0]; //the start player is always first in array, thus starts on turn 1, thus all odd turns are player 1.
         }
    }

    public static int GetTurnPlayerIndex(){
        return turnNumber % 2;
    }

    public static EventChain GetEffectEventChain(){
        return duelEventChain;
    }

    public static int GenerateUniqueSequenceId(){
        return eventSequenceIdSource++;
    }

    public static void SetGamePhase(GamePhase _gamePhase){
        gamePhase = _gamePhase;
    }


    public static GamePhase  GetGamePhase(){
        return gamePhase;
    }

    public static ICardEffect?  GetNextEffect(){
        return nextEffect;
    }

    //public static void SetChainStartingSequenceId(int sequenceId){
    //    GameState.chainStartingSequenceId = sequenceId;
    //}



    //also keep information about things such as current attacking monster, current recepient monster, current effect activated, 
}