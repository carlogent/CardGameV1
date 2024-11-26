
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeckZone{
    public Vector3 location;
    public List<ICard> cards;
    public DeckZone(Vector3 _location){
        location = _location;
        cards = new List<ICard>();
    }
}