using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Card Data")]

public class CardData : ScriptableObject
{
    [field: SerializeField] public Sprite FrontSprite { get; private set; }
    [field: SerializeField] public Sprite BackSprite { get; private set; }
    [field: SerializeField] public int Value { get; private set; }
    [field: SerializeField] public string Effect { get; private set; }
}
