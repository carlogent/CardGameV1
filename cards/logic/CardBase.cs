using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardBase: ICard
{
    public Player owner;
    
    //since effects are managed by the Card.. we should only access the ICardEffect through ICard functions
    //we need a way to register refreshActivationRules for each effect.. store it in the ICardEffect itself..
    public Dictionary<Type, ICardEffect> effects {get; set;} //
    public CardType cardType {get; set;}
    
    //public List<ICardEffect> refreshEffects {get; set;} // (Deprecated) contains a reference to the effect it refreshes and/or a callback

    public CardBase(Player _owner, CardType type){
        owner = _owner;
        effects = new Dictionary<Type, ICardEffect>();
        cardType = type;
        //refreshEffects = new List<ICardEffect>();
    }

    public Player GetOwner()
    {
        return owner;
    }

    // can be overridden to provide a custom way to refresh a cards effects activations remaining
    public virtual void Refresh()
    {
        foreach ((Type type, ICardEffect effect) in effects)
        {
            effect.Refresh();
        }
        //.. a function that can be called on refresh rule..
    }

    // delete any effect which the conditionals are met, an effect can also be deleted by another effect other then its own condtional
    public void DeleteEffects()
    {
        foreach ((Type type, ICardEffect effect) in effects)
        {
            if(effect.DeleteConditional()){
                effects.Remove(type);
            }
        }
    }


    // this is the default implementation, it asks each effect individually if they can activate, if the parent card needs to coordinate activations then this can be overridden!!
    // List<TriggeredEffect> instead of List<ICardEffect> to determine if it happened in the same instance.. simultanius effects
    public virtual List<ICardEffect> DetermineActivatableEffects() // default implementation, works with cards which effects activations dont care about eachother and operate independantly (activationsRemaining)
    {
        List<ICardEffect> activatableEffects = new List<ICardEffect>();

        //Debug.Log("ICard DetermineActivatableEffects() - " + effects.Count + " effects in list" );

        foreach ((Type type, ICardEffect effect) in effects)
        {
            if(effect.Conditional() && effect.GetActivationsRemaining() > 0 && GameState.nextEffect != effect){
                //Debug.Log("adding activatble effects from the live cards");
                activatableEffects.Add(effect);
            }
        }
        
        return activatableEffects;
    }

}


/// <summary> 
///  IMPLEMENT!!!! List<TriggeredEffect> instead of List<ICardEffect> to determine if it happened in the same instance.. simultaneous effects
/// </summary>
public class TriggeredEffect
{
    public int triggerEventSequenceId; //cannot use cardeffectactivation-sequenceid because not all effects activate... but also effects can create multiple events so which one do we target? must be consistance through ALL cards in the game.. !! maybe all the events in a activation/execution of a cardeffect have the same eventSequenceId (I THINK THATS IT!)
    public ICardEffect cardEffect;

    public TriggeredEffect(int _triggerEventSequenceId, ICardEffect _cardEffect)
    {
        triggerEventSequenceId = _triggerEventSequenceId;
        cardEffect = _cardEffect;
    }
}