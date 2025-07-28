using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardView : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private SpriteRenderer frontRenderer;     // Avant
    [SerializeField] private SpriteRenderer backRenderer;      // Arrière-plan de face
    [SerializeField] private SpriteRenderer cardBackRenderer;  // Dos

    private Card card;
    private bool isFaceUp = true;

    public void Setup(Card card, int sortingOrder)
    {
        this.card = card;

        frontRenderer.sprite = card.Sprite;
        backRenderer.sprite = card.SpriteBack;

        UpdateVisibility();

        string sortingLayer = frontRenderer.sortingLayerName;

        frontRenderer.sortingLayerName = sortingLayer;
        backRenderer.sortingLayerName = sortingLayer;
        cardBackRenderer.sortingLayerName = sortingLayer;

        frontRenderer.sortingOrder = sortingOrder + 1;
        backRenderer.sortingOrder = sortingOrder;
        cardBackRenderer.sortingOrder = sortingOrder;

        int layer = gameObject.layer;
        frontRenderer.gameObject.layer = layer;
        backRenderer.gameObject.layer = layer;
        cardBackRenderer.gameObject.layer = layer;
    }



    public void Flip(bool faceUp)
    {
        isFaceUp = faceUp;
        UpdateVisibility();
    }

    private void UpdateVisibility()
    {
        frontRenderer.gameObject.SetActive(isFaceUp);
        backRenderer.gameObject.SetActive(isFaceUp);
        cardBackRenderer.gameObject.SetActive(!isFaceUp);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        card.PerformEffect();
        Destroy(gameObject);
    }
}