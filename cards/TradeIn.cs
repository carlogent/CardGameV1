using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class TradeIn: CardBase, ICard //example
{
    int id = 1;
    CardType type = CardType.Spell; 
    string name = "Trade In";

    //custom activate variables for the opperation of this card, used in unique hardcoded logic
    int originalActivations = 1;

    Dictionary<Type, ICardEffect> ICard.effects { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    //A CARD SHOULD KEEP TRACK OF ALL THE ACTIVATIONS OF ITS EFFECTS

    public TradeIn(Player _owner): base(_owner)
    {
        // Register effects and their conditionals here !
        effects.Add(typeof(Draw_2_card_effect), new Draw_2_card_effect(this));
        refreshEffects.Add(new RefreshOnTurnChangeEffect(Refresh, this)); // instead of calling the card-effect base-class refresh(), we call refresh here allowing us to coordinate the refreshing of the different effects for more complex effects
        //effects.Add(new Draw_2_card_effect(this));
        //..
    }

    //manually handle refreshing of effects (currently no automatic ways anyways)
    public override void Refresh()
    {
        // refresh all effects here... card effects cannot be refreshed by themselves and need the refreshing process to start from the card.. this is beneficial because there are effects on the same card which depend on each other
        effects[typeof(Draw_2_card_effect)].activationsRemaining = effects[typeof(Draw_2_card_effect)].originalActivations;
    }

    public override List<ICardEffect> DetermineActivatableEffects() //custom implementation since the class doesnt know if the effect activations depend on eachother (ex: only 1 effect can be activated)
    {
        List<ICardEffect> activatableEffects = new List<ICardEffect>();

        foreach ((Type type, ICardEffect effect) in effects)
        {
            //if(effect.dependency == EffectDependency.Independant){} //use this when you want to seperate out effects that are not concidered the Main effects on a card and are not subject to special activation conditions
            if(effect.ActivateConditional() && effect.GetActivationsRemaining() > 1){
                activatableEffects.Add(effect);
            }
        }
        
        return activatableEffects;
    }

    
    public class Draw_2_card_effect: CardEffectBase, ICardEffect 
    {
        //if _dependency: EffectDependency.Managed means we do not ask for a refresh effect
        public Draw_2_card_effect(ICard card): base(_card: card, _dependency: EffectDependency.Managed, _originalActivations: 1, _isAnimated: false)
        {}



        public override bool ActivateConditional(){
            return false;
        }

        public override void Activate(int cardeffectActivationChainIndex)
        {
            throw new NotImplementedException();
        }

        public override void Effect(int cardEffectActivationChainIndex)
        {
            throw new NotImplementedException();
        }
    }
}
*/