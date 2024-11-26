using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field
{
    public Vector3 originLocation;
    public CardPileZone graveyard;
    public CardZone[] monsterZones;
    public CardZone[] spellTrapZones;
    public CardZone fieldSpellZone;
    public DeckZone deckZone;

    public Field(Vector3 _originLocation){
        originLocation = _originLocation;
        graveyard = new CardPileZone(originLocation);

        monsterZones =  new CardZone[]{
            new CardZone(originLocation, null), // originLocation for testing
            new CardZone(originLocation, null),
            new CardZone(originLocation, null),
            new CardZone(originLocation, null),
            new CardZone(originLocation, null)
        };

        spellTrapZones =  new CardZone[]{
            new CardZone(originLocation, null),
            new CardZone(originLocation, null),
            new CardZone(originLocation, null),
            new CardZone(originLocation, null),
            new CardZone(originLocation, null)
        };

        fieldSpellZone = new CardZone(originLocation, null);
        deckZone = new DeckZone(originLocation);
    }
}