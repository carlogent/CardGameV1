using System.Collections.Generic;

public class EventChain: SingletonBase<EventChain> 
{
    public List<IDuelEvent> chain; //pretty useless i think

    public EventChain(){
        chain = new List<IDuelEvent>();
    }
}