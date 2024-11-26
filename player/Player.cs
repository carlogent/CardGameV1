using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player{
    public Field field;
    public PlayerDuelStats stats;
    public DeckZone deck;
    public string name;
    public Owner ownerId;

    //for testing:
    public int cardsInHand;


    public Player(Vector3 _originLocation, string _name, Owner _ownerId){
        field = new Field(_originLocation);
        stats = new PlayerDuelStats();
        name = _name;
        cardsInHand = 1;
        ownerId = _ownerId;
    }

}



