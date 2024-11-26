using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICard{
    //public List<ICardEffect> effects {get; set;} 
    
    //CHANGE BACK TO TYPE
    //public Dictionary<int, ICardEffect> effects {get; set;} // final to execute, creates events and responses [we use a dictionary to get the effect, an ICard should never have two of the same effect so this is okay (actually a constraint)]
    public Dictionary<Type, ICardEffect> effects {get; set;}


    // first to execute, creates no events.. if we pass in Refresh-ICardEffects into the ICardEffects then they will self register here, for all others that we do not pass in Refresh-ICardEffects we must manage their effect activationsRemaining manually in the ICard. 
    // (ICard can add its own Refresh-ICardEffects to refreshEffects which can callback its own overridden Refresh() function which can be hard coded to handle all the non-independant ICardEffects)
    //public List<ICardEffect> refreshEffects {get; set;} 

    public List<ICardEffect> DetermineActivatableEffects();
    public void DeleteEffects();
    public Player GetOwner();
    public void Refresh();
}