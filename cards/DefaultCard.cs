using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;

public class DefaultCard: CardBase
{
    int id = 1;
    CardType type = CardType.Spell; 
    string name = "Default Game Card";
    int originalActivations = 1;


    
    // this is only used for the game mechanics
    public DefaultCard(Player _owner): base(_owner, CardType.GameMechanic)
    {
        // Register effects and their conditionals here !
        //..

        //draw for turn effect and refresh..

        effects.Add(typeof(DrawForTurnEffect), new DrawForTurnEffect());
        effects.Add(typeof(DrawPhaseStartToDrawPhaseEffect), new DrawPhaseStartToDrawPhaseEffect());
        effects.Add(typeof(DrawForTurnEffect), new DrawForTurnEffect());
        effects.Add(typeof(DrawPhaseToMainPhaseEffect), new DrawPhaseToMainPhaseEffect());
        //refreshEffects.Add(new RefreshOnTurnChangeEffect(effects[typeof(DrawForTurnEffect)].Refresh, this));
        
    }

    /*
    public override List<ICardEffect> DetermineActivatableEffects() // default implementation, works with cards which effects activations dont care about eachother and operate independantly (activationsRemaining)
    {
        //.. custom implementation
    }*/

}