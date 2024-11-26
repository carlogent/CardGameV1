
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPileZone{
    public Vector3 location;
    public List<ICard> cards;

    public CardPileZone(Vector3 _location){
        location = _location;
        cards = new List<ICard>();
    }
}
