using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// EVENT
public class GameEvent<T>: IDuelEvent where T: IGameAction
{
    public bool negated;
    public bool isNegatable;
    public int eventSequenceId;
    public T action; // the action associated with this event 

    public GameEvent(int _eventSequenceId, T _action, bool _isNegatable = true){
        eventSequenceId = _eventSequenceId;
        action = _action;
        negated = false;
        isNegatable = _isNegatable;
    }

    public IGameAction GetAction(){
        return action;
    }
    
    public int GetEventSequenceId(){
        return eventSequenceId;
    }
}