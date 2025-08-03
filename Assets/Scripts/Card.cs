using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card
{
    private readonly CardData cardData;

    public Card(CardData cardData)
    {
        this.cardData = cardData;
        Value = cardData.Value;
        Effect = cardData.Effect;
    }

    public Sprite Sprite => cardData.FrontSprite;
    public int Value { get; set; }
    public string Effect { get; set; }

    public void PerformEffect()
    {
        Debug.Log("Value is " + Value + " with an effect of " + Effect);
    }
}
