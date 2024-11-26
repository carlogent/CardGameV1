using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//used as a interface for the functionality of the game duel, handles event raising and subscribing 
public class GameTools: SingletonBase<GameTools>
{
    


    //resets all card effects remaining activations to be their original numbers if applicable 
    public static void RefreshActivationsRemaining(Player player){
        //..implement
        foreach (ICard card in GameState.liveCards)
        {
           // card.
        }
    }


    public static void TurnChangeActions(Player turnPlayer){
        //..implement everything that happens on turn change
        Debug.Log("TurnChangeActions");
        GameState.turnNumber += 1;
        GameState.turnStartSequenceNumberList.Add(GameState.eventSequenceIdSource+1); //THE LIST which keeps track of the amount of turns (indexes) and the sequence number at the start of the turn (this allows for searching the event pools within a turn by searching a range of sequence numbers)
        //GameState.turnSequenceNumberStartDict.Add(GameState.turnNumber, GameState.eventSequenceIdSource-1); // helps cards search effect chains by giving bounds
        //GameState.SetChainStartingSequenceId(-1);

        // add the latest eventchain index as the turn start, this helps card effects shorten the amount of things to check
        //RefreshActivationsRemaining(turnPlayer);
    }

    // ----------------------------------------------------------------------------   testing a train of thought..  ------------------------------------------------------------------------------------------------------------------------------
    public static void  EvaluateCardsEffects()
    {
        int numberOfCards = GameState.liveCards.Count;
        List<ICardEffect> activatableEffects = new List<ICardEffect>();
        List<ICardEffect> activatableEffects_Temp;
         
        // ! get all card effects that are activatable 
        for (int i = 0; i < numberOfCards; i++)
        {
            // delete any effects that should be deleted here
            GameState.liveCards[i].DeleteEffects();

            //get activatable effects from the live cards here
            activatableEffects_Temp = GameState.liveCards[i].DetermineActivatableEffects(); // a card can have multiple activatable effects //CRASHES 

            if(activatableEffects_Temp.Count > 0){
                activatableEffects.AddRange(activatableEffects_Temp);
            };
        }

        // ! for all card effects that are ActivatedConstant, execute them immediatly, this should change the event such that they do not acitvate anymore / conditional should check if it already activated
        foreach (ICardEffect workableEffect in activatableEffects)
        {
            if(workableEffect.activationState == ActivationState.ConstantActivated){
                workableEffect.Effect(-1); // -1 because this is not actually needing the chain..
                activatableEffects.Remove(workableEffect); 
            }
        }


        // now we have all the possible activatable effects, sort them by the current rules (the returned effects are all the effects that can be activated at THIS point ex: only effects that can activate by the player with priority, with sufficient spell speed, etc.)
        activatableEffects = FilterSortActivatableEffectsByPriorityRules(activatableEffects);

        // if no effects that can activate/execute
        if(activatableEffects.Count == 0){ return;}
        
        //check if it is optional or mandatory (will not have a mix of both thus process different based on first returned)
        if(activatableEffects[0].requirementType == RequirementType.Mandatory)
        {
            foreach (ICardEffect effect in activatableEffects)
            {
                if(effect.activationState == ActivationState.ConstantActivated)
                {
                    ExecuteEffect(effect);
                }
                else //ActivationState.Activating (means it activates and thus gets put into the activation chain)
                { 
                    ActivateEffect(effect);
                }
            }
        }
        else //activatableEffects[0].requirementType == RequirementType.Optional (must ask turn player which he would like to execute)
        {
            Player priorityPlayer = GetPlayerWithPriority();
            priorityPlayer.ChooseEffect(activatableEffects);
            //what about effects that trigger at the same time?????????
            //make scenerio about card effect destroying all monsters on feild and each monster triggering an effect on destruction
        }


        //process each effect based on their activation state and requirements 
        foreach (ICardEffect effect in activatableEffects)
        {
            if(effect.activationState == ActivationState.ConstantActivated)
            {
                ExecuteEffect(effect);
            }
            else //ActivationState.Activatable
            { 
                ActivateEffect(effect);
            }
        }
        
        



    }

    public static void ActivateEffect(ICardEffect effect)
    {
        int sequenceId = GameState.GenerateUniqueSequenceId();

        //add the effect to the effect activation chain to be executed later
        //GameState.chainStartSequenceNumberList.Add(new ChainSequence(){chainSequenceNumberEnd = sequenceId}); // denote the start of a effect chain
        
        //add the effect to the effect activation chain to be executed later
        CardEffectActivationChain.Instance.chain.Add(new CardEffectActivation(effect, sequenceId)); // ! needs actions but these should be stored inside the card??? we not even acessing them from here, probs should be accessing them from their native event -> event origin action
        
        //Debug.Log("ExecuteEffect.. activating effect");

        effect.Activate(sequenceId); //activating the effect here
    }

    public static void ExecuteEffect(ICardEffect effect)
    {
        int sequenceId = GameState.GenerateUniqueSequenceId();
        effect.Effect(sequenceId);

        //does not get added to the CardEffectActivationChain because it does not activate, and cannot be interacted with
    }


    // ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


    // ----------------------------------------------------------------------------   testing a train of thought.. Version 2  ------------------------------------------------------------------------------------------------------------------------------
    public static void DetermineNextEffect(){
        //get all live cards
        GameState.players[0]
    }

    // ------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

    


    // should be DrawCardsForTurn? there should be no generic 1 call funcition for anything, since a cards created event-datas must all add their events to the lists -> check if any cards can chain to the events -> then process its chain backwards until finaly the event can be executed...
    // thus the correct way to execute an effect is to (activate registered effect in card class): -> record events + record events in chain -> loop through list of all cards in play and see which resolves all their conditionals for each effect -> if one is found then add events -> check cards.. -> if no execute all effects backwards
    public static List<ICardEffect> EvaluateCardsConditionals( bool executePriority = false)
    {

        

        int numberOfCards = GameState.liveCards.Count;

        /*

        // ! execute any refresh Effects for every card.. ---------- n(deprecated because all refresh effects should be registered to a trigger)
        for (int i = 0; i < numberOfCards; i++)
        {
            foreach (var refreshEffect in GameState.liveCards[i].refreshEffects)
            {
                if(refreshEffect.Conditional()){ // refresh effects activate immediatly and create no events (i will never care about refresh events, it is a background game system )
                    Debug.Log("EvaluateCardsConditionals - executing refresh effects");
                    refreshEffect.Effect(-1); //this has no chain number..
                }
            }
        }

        // ! execute any delete effects on every ICardEffect in every ICard.. delete effects are from the DeleteConditional and thats it --------- (SHOULD BE DEPRECATED, should be checked at the same time as the conditionals..)
        for (int i = 0; i < numberOfCards; i++)
        {

            //remove the refresh classified card effects whose DeleteConditional trigger
            for (int x = 0; x < GameState.liveCards[i].refreshEffects.Count(); x++)
            {
                if(GameState.liveCards[i].refreshEffects[x].DeleteConditional())
                {
                    Debug.Log("EvaluateCardsConditionals - removing refresh effects");
                    GameState.liveCards[i].refreshEffects.RemoveAt(x);
                    x--;// we removed this index so we go back one to maintain spot
                }
            }

            // remove the regular card effects whose DeleteConditional trigger (such as ICardEffects created by ICardEffects resolving - Activated Continuous Effects)
            List<Type> keysToRemove = new List<Type>();
            foreach (var effect in GameState.liveCards[i].effects)
            {
                if(effect.Value.DeleteConditional()){
                    keysToRemove.Add(effect.Key);
                }
            }

            for (int y = 0; y < keysToRemove.Count; y++)
            {
                Debug.Log("EvaluateCardsConditionals - removing regular card effects");
                GameState.liveCards[i].effects.Remove(keysToRemove[y]);
            }
        }*/
        

        // Now we begin actually processing the regular effects which create events and can be responded to.
        List<ICardEffect> activatableEffects = new List<ICardEffect>();
        List<ICardEffect> activatableEffects_Temp;
        

         
        for (int i = 0; i < numberOfCards; i++)
        {
            // delete any effects that should be deleted here

            //get activatable effects from the live cards here
            activatableEffects_Temp = GameState.liveCards[i].DetermineActivatableEffects(); // a card can have multiple activatable effects //CRASHES 

            if(activatableEffects_Temp.Count > 0){
                //Debug.Log("foudn atleast 1 from live cards");
                activatableEffects.AddRange(activatableEffects_Temp);
            };
        }
        

        activatableEffects = FilterSortActivatableEffectsByPriorityRules(activatableEffects); //non optional effects go first, check variable - implement!

        /* //use this if we want to immediatly execute the first activatable effect with the highest priority. 
        if(executePriority && activatableEffects.Count > 0){ // note: treats everything as mandatory
            activatableEffects[0].Effect(); //FOR CHAINING TO OCCUR ALL EFFECTS MUST CALL EvaluateCardsConditionals AFTER DOING AN ACTION WHICH CREATES EVENTS.
        }*/


        //Debug.Log("EvaluateCardsConditionals - returning effects: " + activatableEffects.Count());
        return activatableEffects;
    } 


    // CURRENTLY WHERE WE ARE HERE DEVLOPING
    
    


    //execute effect with responses (assuming conditional met for the effect and were processed before hand.)
    public static void ActivateEffectSingle(ICardEffect effect){ //this should not exist.. a manual activation should not exist!!!!!!!!!!!!!!!!!!!!!, remove this in favour of a generic building/executing loop 
        Debug.Log("ExecuteEffect..");

        int sequenceId = GameState.GenerateUniqueSequenceId();


        GameState.chainStartSequenceNumberList.Add(new ChainSequence(){chainSequenceNumberEnd = sequenceId}); // denote the start of a effect chain
        //GameState.SetChainStartingSequenceId(sequenceId); // denote the start of a response effect chain
        
        //add the effect to the effect activation chain to be executed later
        CardEffectActivationChain.Instance.chain.Add(new CardEffectActivation(effect, sequenceId)); // ! needs actions but these should be stored inside the card??? we not even acessing them from here, probs should be accessing them from their native event -> event origin action
        
        //Debug.Log("ExecuteEffect.. activating effect");

        effect.Activate(sequenceId); //activating the effect here
        
        BuildCardEffectActivationChain();

        ExecuteCardEffectActivationChain(startingIndex: CardEffectActivationChain.Instance.chain.Count-1); //EXECUTE CHAIN!

        
        //there may be a card that is interested in the resolve events of the chain, therefore we evaluate contionals once again
        

        //Debug.Log("check for any triggers after chain..");
        List<ICardEffect> activatableEffects = GameTools.EvaluateCardsConditionals(); 
        if(activatableEffects.Count > 0){
            Debug.Log("ExecuteEffect after chain..");
            GameTools.ActivateEffect(activatableEffects[0]);
        }

        // here we check if a "Next Effect" exists (could also be called "Post Chain Next Effect")
        // this allows to have guarenteed effects that activate/execute after a effect has activated, used only by the game mechanics (so far)..
    }

    // when user input (call back) during bulding chain, call this function after finishing Activate() and its extending callback functions
    public static void BuildCardEffectActivationChain()
    {
        List<ICardEffect> activatableEffects = new List<ICardEffect>();
        while(true){
            //Debug.Log("ABOUT TO EVALUATE");
            activatableEffects = GameTools.EvaluateCardsConditionals(); //see if any card respond to the new events //REMOVED FOR TESTING
            Debug.Log("Execute Effects.. activatableEffects count: " + activatableEffects.Count());

            if(activatableEffects.Count > 0){ // note: treats everything as mandatory for now
                //Debug.Log("found a response to the event! adding events to chain");
                int sequenceId = GameState.GenerateUniqueSequenceId();
                CardEffectActivationChain.Instance.chain.Add(new CardEffectActivation(activatableEffects[0], sequenceId));
                activatableEffects[0].Activate(sequenceId); //FOR CHAINING TO OCCUR ALL EFFECTS MUST CALL EvaluateCardsConditionals AFTER DOING AN ACTION WHICH CREATES EVENTS.
            }else{
                //no more responses! start executing effects!
                //Debug.Log("no more responses to chain");
                break;
            }

        }

        //chain has ended
        //GameState.SetChainStartingSequenceId(-1);
    }

    public static void ExecuteCardEffectActivationChain(int startingIndex)
    { 
        Debug.Log("executing effect chain!!!..");
        

        int chainStartingSequenceId = GameState.chainStartSequenceNumberList[^1].chainSequenceNumberStart; 
        //int startingIndex = GetIndexOfEffectActivationWithSequenceID(CardEffectActivationChain.Instance.chain, chainStartingSequenceId);

        //Debug.Log("ExecuteActionChain.. eventChain-LastIndex: " + lastIndex + "  || going backwards down the list until finding chainStartingSequenceId of: " + chainStartingSequenceId);

       // Debug.Log("GameState.chainStartingEventSequenceId: " + chainStartingEventSequenceId + " | current effect chain count: " + (eventChainLastIndex+1) );
        while(true){ //execute the chain backwards >= or >?

            //if the sequence number of the card effect activation is larger then the chain starting sequence id then it happened in the chain and should be executed
            if(chainStartingSequenceId <= CardEffectActivationChain.Instance.chain[startingIndex].effectActivationSequenceId){
                //execute the card effect
                CardEffectActivationChain.Instance.chain[startingIndex].cardEffect.Effect(startingIndex);
                //we dont do action.Action() on each action here because we want the handwritten card effect to orchestrate the actions and their effects. references to the actions are only in the chain for other card effects to have a change to modify the actions.
            }else{
                //finished executing the chain.
                break;
            }

            startingIndex--;

            if(startingIndex < 0){ break; }
        }

        GameState.chainStartSequenceNumberList[^1].chainSequenceNumberEnd =  GameState.eventSequenceIdSource;


        



    }


    //keeps track of the chain
    /*
    public static void ResolveResponseChain() { //must only every be called in an empty chain
        GameState.SetChainStartingSequenceId(EffectEventChain.Instance.chain.Count-1);

        EvaluateCardsConditionals(executePriority: true);

        GameState.SetChainStartingSequenceId(-1);
    }*/

    public static Player GetPlayerWithPriority()
    {
        if(GameState.priorityNumber % 2 == 0){
            return GameState.players[0];
        }

        return GameState.players[1];
    }


    public static List<ICardEffect> FilterSortActivatableEffectsByPriorityRules(List<ICardEffect> activatableEffects)
    {
        //..implement

        // filter: only effects from player with priority 
        // filter: only effects which have the same or greater spell speed then last activated effect
        // filter: only effects which have the greatest requirementType (Mandatory > Optional)

        //simulatanious effects are shown as a regular option to activate, however when a simultanious effect is activated, it does NOT pass priority, and only simultanious effects of the same eventID can be subsequently choosen.
        // -> this can be done by when a simultanious effect is chosen (noted by checking all triggeredeffects and seeing if multiple have identicle eventIds exist), 
        // method 1. registers a "simultanious last activated effect instance list" this is reviewed when filtering, if this list is more than one then immediatly offer those effects along with the rest of the found activatable effects.
        //  if a effect that is not part of that list is selected then the "simultanious last activated effect instance list" is updated with the new effect thus loosing the window to activate these effects/OR allowing priority to be passed to other player before the owner of the simulatnious effects can trigger them again.


        return activatableEffects;
    }

    public static int GetIndexOfEffectActivationWithSequenceID(List<CardEffectActivation> list, int sequenceID)
    {
        for (int i = list.Count - 1; i >= 0; i--)
        {
            if (list[i].effectActivationSequenceId == sequenceID)
            {
                return i;
            }
        }

        throw new KeyNotFoundException("could not find a effect activation with the matching sequence ID!");

    }


/*
    public static List<ICard> DrawCards(DrawActivateEvent drawEvent){
        Debug.Log("GameTools: drawing " + drawEvent.numberOfDraws + " cards!");

        //for testing purposes, usually get card from deck object..
        List<ICard> cardsToDraw = new List<ICard>(){
            //new TradeIn(GameState.GetTurnPlayer())
        };

        //.. implement!

        return cardsToDraw;
    }

    public static void DrawCards_test(DrawActivateEvent drawEvent){
        //Debug.Log("DrawCards_test");
        //Debug.Log("DrawCards_test2 "  + drawEvent);
        //Debug.Log("GameTools: drawing " + drawEvent.numberOfDraws + " cards!");

        GameState.GetTurnPlayer().cardsInHand += drawEvent.numberOfDraws;
    }
*/
    /*
    public void DrawCards(EffectChain chain, Player targetPlayer, byte numberOFDraws){
        //select random card from deck to draw.. normally there is an order and it is preserved but for testing purposes we are just spawning a card
        List<ICard> cardsToDraw = new List<ICard>(){
            new TradeIn(gameState)
        };

        // ! overhaul the events to be objects that carry 3 event subscriber lists: Attempt, Activate, Resolved allowing for more flexibility on when effects can activate
        //gameDuelEvents.OnDraw.AttemptEvent(new OnDrawAttemptEventData(1)); //should have already attempted..

        gameDuelEvents.OnDraw.ActivateEvent(chain, new OnDrawAttemptEventData(1), ); //should i pass an effect chain into the activate effect functions? allowing the conditionals and effects to add themselves to the chain?



    }
    */


}