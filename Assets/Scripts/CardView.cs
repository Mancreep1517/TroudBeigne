using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private SpriteRenderer backRenderer;
    [SerializeField] private SpriteRenderer frontRenderer;

    private Card card;
    private bool isFaceUp = true;

    public void Setup(Card card)
    {
        this.card = card;
        backRenderer.sprite = card.SpriteBack;
        frontRenderer.sprite = card.Sprite;
        UpdateVisibility();
    }

    public void Flip (bool faceUp)
    {
        isFaceUp = faceUp;
        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        frontRenderer.gameObject.SetActive(isFaceUp);
        backRenderer.gameObject.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        card.PerformEffect();
        Destroy(gameObject);
    }
}
