using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//this is where we keep track of variables that may be used by card effects.. the rules are the logic while the duelstats is the memory
public class PlayerDuelStats //PlayerTrackedDuelStatistics
{
    public int availableNormalSummons;
    //public List<CardRecord> summons_this_duel; // should just check the list for events..
    //public List<CardRecord> summons_this_turn; // should just check the list for events..
    //public List<CardRecord> destroyed_this_duel; // should just check the list for events..
    //public List<CardRecord> destroyed_this_turn; // should just check the list for events.. if 3 of your "xxx" monsters were destroyed this turn...

    public PlayerDuelStats(){
        availableNormalSummons = 1;
        /*
        summons_this_duel = new List<CardRecord>();
        summons_this_turn = new List<CardRecord>();
        destroyed_this_duel = new List<CardRecord>();
        destroyed_this_turn = new List<CardRecord>();
        */
    }

}
