using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//the reason i dont make this more generic is because the card creators know exactly the events that need to be called or subscribed to, and i want them to know exactly what data to pass in.
public class DuelEventData<TAttempt, TActivate, TResolved> where TAttempt : IDuelEvent where TActivate : IDuelEvent where TResolved : IDuelEvent
{
    // because we will always know the type of the specific event data i dont want to make generic since we would have to cast later.. but this means we can not reinforce the need for these functions which we absolutly do. we need this level of flexibility because the card effects can be extremely flexible i believe, also they know exactly what kind of event to produce!.
    public ObservableList<TAttempt> Attempt;    //Activate events execute the ICardEffect's Effect
    public ObservableList<TActivate> Activate; //Activate events execute the ICardEffect's Effect
    public ObservableList<TResolved> Resolved; //Resolved events are only used to confirm a successfull activation

    public DuelEventData(){
        Attempt = new ObservableList<TAttempt>();

        Activate = new ObservableList<TActivate>();

        Resolved = new ObservableList<TResolved>();
    }

    // must add all effect events in one batch.
    /*
    public void AddAttemptEvents(List<TAttempt> events){
        if(events.Count != 0){
            Attempt.AddRange(events);
            AttemptChangeFlag = true;
        }
    }

    public void RemoveAttemptEvent(TAttempt events){
        Attempt.Remove(events);
    }

    // must add all effect events in one batch.
    public void AddActivateEvents(List<TActivate> events){
        if(events.Count != 0){
            Activate.AddRange(events);
            ActivateChangeFlag = true;
        }
    }

    public void RemoveActivateEvent(TActivate events){
        Activate.Remove(events);
    }

    // must add all effect events in one batch.
    public void AddResolvedEvents(List<TResolved> events){
        if(events.Count != 0){
            Resolved.AddRange(events);
            ResolvedChangeFlag = true;
        }
    }

    public void RemoveResolvedEvent(TResolved events){
        Resolved.Remove(events);
    }
    */

}







// written mostly by GPT
public class ObservableList<T>
{
    private List<T> _list = new List<T>();
    public (bool check, int index) isAppended; // if check is true, the list has been changed since last processing, index denotes the index where the first unprocessed event is located.

    public event Action<T> ItemAdded;

    public void Add(T item)
    {
        _list.Add(item);

        //change the flag
        if(isAppended.check == false)
        {
            isAppended.check = true;
            isAppended.index = _list.Count-1;
        }
    }


    // You can add other List<TAttempt> methods like Remove, Clear, etc., if needed.
    public void Remove(T item)
    {
        _list.Remove(item);
    }

    public int Count => _list.Count;

    public T this[int index]
    {
        get { return _list[index]; }
        set { _list[index] = value; }
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _list.GetEnumerator();
    }

    // And other List<TAttempt> methods as needed
}