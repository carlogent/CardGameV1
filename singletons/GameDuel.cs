using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameDuel : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(" (Start): the Duel has begun!");

        // Make sure all starting variables in the gameState is properly set!




        // Intialize the core game rules in the gameDuelEvents here!
        // > put the draw for turn effect as a card rule inside the livecards if we want it to be reactive! add isAnimated bool to ICard for silent effects

        //calls all the events associated with the game phase 
        //GamePhaseManager.ProcessGamePhase(); //draw phase start -> draw phase
        
        //the game is now in draw phase, do all further processing in update
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            Debug.Log("Player attempting to advance the game phase!");
            //new GamePhaseManager.AdvanceGamePhaseEffect().ConditionalExecute();
            //GamePhaseManager.AdvanceGamePhaseEffect agpEffect =  new GamePhaseManager.AdvanceGamePhaseEffect();

            var agpEffect = new AdvanceGamePhaseEffect(); //only source of this effect is a player command not a card.
            GameTools.ActivateEffect(agpEffect);

            //GameState.defaultCard //activate from default card! use type lol
            //agpEffect.Activate();
            //agpEffect.
            
            //GameTools.ExecuteEffect(agpEffect);


            //TO DO:
            //create draw for turn effect card
            // !!!!!!!!!!!!!!!!!!    CREATE THE CONCEPT OF RULE CARDS ex: add enum type CardType.Rule, giving certain privlages, maybe instead of liveCards put them into a new ruleCards list

            

            /*
            GameState.SetChainStartingSequenceId(GameState.sequenceIdSource); // denote the start of a response effect chain
            agpEffect.SetEffectActivateEvents();
            

            while(true){
                List<ICardEffect> activatableEffects = GameTools.EvaluateCardsConditionals(); //see if any card respond to the new events
                if(activatableEffects.Count > 0){ // note: treats everything as mandatory
                    Debug.Log("found a response to the event! adding events to chain");
                    activatableEffects[0].SetEffectActivateEvents(); //FOR CHAINING TO OCCUR ALL EFFECTS MUST CALL EvaluateCardsConditionals AFTER DOING AN ACTION WHICH CREATES EVENTS.
                }else{
                    //no more responses! start executing effects!
                    Debug.Log("no more responses to chain");
                    GameTools.ExecuteEffectEventChain();
                    break;
                }
            }

            //chain has ended
            GameState.SetChainStartingSequenceId(-1);
            */
        }

        //Debug.Log("update");
    }

    

    void ProcessPlayerInput(){

        //the player inputs need to be checked against rules, since cards can change the rules of the game.

    }


}
            //GamePhaseManager.AdvanceGamePhase();




//when a particular event happens, the Game Duel will call on one of these events, and it will evaluate all conditionals registered by the cards in play,
// > the ones which evaluate to true are organized by time stamp and are executed in order.. use the reference to the card that owns the effect to process the event with a spefic event-object about the event 
// > ex: "if monster is normal summoned, destroy 1 card" OnSummon -> evaluate all card conditionals including that card -> execute function signiture with the EventObject provided by the GameDuel.

// playing a spell card: "if monster is normal summoned, destroy 1 card"
// > triggers OnCardEffect -> (searches for all cards that may have events attached to this, if any of them have a conditional that matches this cards event they will execute): card is "Negate 1 spell per turn" -> triggers the registered event in that card with the event object. -> check for activateable response -> keep building chain ->resolve chain by resolving the registered effects one by one..

//have event return types associated with every event? ex every OnAttackDeclaration triggered card functions must accept a OnAttackDeclarationEvent object (yes)
// a card should be able to register with multiple conditionals and should execute as many times as it needs to (tracked)










