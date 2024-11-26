using System;
using System.Collections;
using System.Collections.Generic;


public static class GameDuelEvents
{
    /*
    List<Action<GamePhaseChangeEvent>> OnGamePhaseChangeEvents; //name events -> observers
    List<Action<ICard, ICard>> OnAttackDeclarationEvents;
    List<Action<ICard, ICard>> OnDamageStepEvents;
    List<Action<ICard>> OnCardEffectEvent;
    List<Action<List<ICard>>> OnSummonEvents;
    List<Action<List<ICard>>> OnDrawEvents;
    List<Action<List<ICard>>> OnDiscardEvents;
    List<Action<List<ICard>>> OnCardDestructionEvents;
    List<Action<List<ICard>>> OnSentToGravyardEvents;
    List<Action<List<ICard>>> OnSummonedFromGraveyardEvents;
    */


    //public DuelEventData<SummonAttemptEvent, SummonActivateEvent, SummonResolvedEvent> OnSummon;

    public static DuelEventData<GamePhaseChangeAttemptEvent, GamePhaseChangeActivateEvent, GamePhaseChangeResolvedEvent> OnGamePhaseChange;
    public static DuelEventData<DrawAttemptEvent, DrawActivateEvent, DrawResolvedEvent> OnDraw;


    // special event pool used for events that do not activate, used to make sure thats these effects dont execute on the same conditionals !all ActivatedConstant effects produce events, the conditionals must check these produced events in their respective pools to see if the card activated already to a specific event.. but not all of them produce these events..
    //public static DuelEventData<ActivatedConstantAttemptEvent, ActivatedConstantActivateEvent, ActivatedConstantResolvedEvent> OnActivatedConstantEffect; //really only care about the resolved event




    //public DuelEventData<NegateAttemptEvent, NegateActivateEvent, NegateResolvedEvent> OnNegate;



    // IDuelEvent
    // instead of list of functions, we can have each event as a class which contain its attempt, activate, resolved versions of its event subscriber list. ex: OnSummon event -> onSummon.Attempt , onSummon.Activate, onSummon.Resolved

    static GameDuelEvents(){

        //OnSummon = new DuelEventData<SummonAttemptEvent, SummonActivateEvent, SummonResolvedEvent>();
        OnGamePhaseChange = new DuelEventData<GamePhaseChangeAttemptEvent, GamePhaseChangeActivateEvent, GamePhaseChangeResolvedEvent>();
        OnDraw = new DuelEventData<DrawAttemptEvent, DrawActivateEvent, DrawResolvedEvent>();
        //OnGamePhaseChange = new DuelEventData<ActivateGameEvent<GamePhaseChangeActivateEvent> , ResolvedGameEvent<GamePhaseChangeResolvedEvent>>();
        //OnNegate = new DuelEventData<NegateAttemptEvent, NegateActivateEvent, NegateResolvedEvent>();
    }

}

