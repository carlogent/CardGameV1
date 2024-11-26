using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// EVENT
public class AttemptGameEvent<T>: IDuelEvent where T: IGameAction
{
    public int eventSequenceId;
    public T action; // the action associated with this event 

    public AttemptGameEvent(int _eventSequenceId, T _action){
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