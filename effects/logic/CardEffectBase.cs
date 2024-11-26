using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#nullable enable

public abstract class CardEffectBase: ICardEffect //maybe make an effect base.. instead of cardEffect to make it more generic
{
    // here we keep the core variables
    public ICard? ownerCard {get; set;}
    public ActivationState activationState {get; set;}
    public SpellSpeed spellSpeed {get; set;}
    public bool negatable {get; set;}
    public RequirementType requirementType {get; set;}



    //here we keep card effect specific data instead of using an extended data (EXTENDED DATA)
    public int originalActivations {get; set;}
    public int activationsRemaining {get; set;} //refreshed and controlled by the Card that owns this effect
    public EffectDependency dependency {get; set;} 
    public bool isAnimated {get; set;}
    public bool deleteAfterEffect;
    //public List<Action<int>> activateFunctions; //move to base
    //public List<Action<int>> effectFunctions; //move to base

    //maybe _dependency can be determined if the _refreshEffect is null or not
    public CardEffectBase(ICard? _card, ActivationState _activationState, RequirementType _requirementType, SpellSpeed _speed, EffectDependency _dependency, int _originalActivations, bool _isAnimated, bool _deleteAfterEffect = false){
        Debug.Log("2.2");
        ownerCard = _card;
        originalActivations = _originalActivations;
        activationsRemaining = _originalActivations;
        requirementType = _requirementType;
        activationState = _activationState;
        spellSpeed = _speed;
        dependency = _dependency;
        isAnimated = _isAnimated;
        deleteAfterEffect = _deleteAfterEffect; //using this way as a way to create deletion effects that can delete itself after triggering once - else needs an endless chain of deletion effects..

        Debug.Log("CardEffectBase");
    }

    public ICard? GetCard()
    {
        return ownerCard;
    }

    public virtual void Refresh()
    {
        //.. optionally override this and add logic to refresh the card effect
        Debug.Log("ICardEffect (Base) Refreshing now!");
        activationsRemaining = originalActivations;
    }

    

    public int GetActivationsRemaining(){
        return activationsRemaining;
    }
    public void SetActivationsRemaining(int _activationsRemaining){
        activationsRemaining = _activationsRemaining;
    }


    public virtual bool DeleteConditional()
    {
        // return do not delete by default
        return false; 
    }

    public abstract bool Conditional();


    public abstract void Activate(int cardeffectActivationChainIndex);


    public abstract void Effect(int cardEffectActivationChainIndex);

    /* use coroutines instead
    // for base class - this is whats called when executing an effect
    public void Effect(int cardEffectActivationChainIndex, int effectFunctionIndex = 0)
    {
        effectFunctions[effectFunctionIndex](cardEffectActivationChainIndex); // usefull for callbacks!

        if(effectFunctionIndex == effectFunctions.Count-1){
            //this is the last effect function so go back to executing chain..
            // FINISH IMPLEMENTING!!
        }
    }

     // for base class - this is whats called when activating an effect
    public void Activate(int cardEffectActivationChainIndex, int activateFunctionIndex = 0) //give chance to register events before execution 
    {
        activateFunctions[activateFunctionIndex](cardEffectActivationChainIndex); // usefull for callbacks!

        if(activateFunctionIndex == activateFunctions.Count-1){
            //this is the last activate function so go back to building chain..
            GameTools.BuildChain();
        }
    }
    */


}