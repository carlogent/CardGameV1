
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardZone{
    public Vector3 location;
    public ICard? card;

    public CardZone(Vector3 _location, ICard? _card){
        location = _location;
        card = _card;
    }
}