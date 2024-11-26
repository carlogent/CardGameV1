using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolvedGameEvent<T>: IDuelEvent where T: IGameAction
{
    public int eventSequenceId;
    public readonly T action; 

    public ResolvedGameEvent(int _eventSequenceId, T _action){
        eventSequenceId = _eventSequenceId;
        action = _action;
    }

    public IGameAction GetAction(){
        return action;
    }
    public int GetEventSequenceId(){
        return eventSequenceId;
    }
}