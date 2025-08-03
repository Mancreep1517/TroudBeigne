using System.Collections;
using UnityEngine;

public class CardView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer frontRenderer;
    [SerializeField] private SpriteRenderer cardBackRenderer;
    [SerializeField] private float flipSpeed = 300f;
    private CardPlay cardPlay;
    private Card card;

    private bool isFaceUp = false;
    private bool isFlipping = false;

    private void Start()
    {
        cardPlay = GetComponentInParent<CardPlay>();
    }

    public void Setup(Card card, int sortingOrder)
    {
        this.card = card;

        frontRenderer.sprite = card.Sprite;

        string sortingLayer = frontRenderer.sortingLayerName;
        frontRenderer.sortingLayerName = sortingLayer;
        cardBackRenderer.sortingLayerName = sortingLayer;

        frontRenderer.sortingOrder = sortingOrder;
        cardBackRenderer.sortingOrder = sortingOrder;

        int layer = gameObject.layer;
        frontRenderer.gameObject.layer = layer;
        cardBackRenderer.gameObject.layer = layer;
    }

    public void UpdateFaceUp(bool faceUp)
    {
        isFaceUp = faceUp;
        UpdateVisibility();
    }

    public void FlipCardAnimated()
    {
        if (!isFlipping)
        {
            StartCoroutine(FlipCoroutine());
        }
    }

    private IEnumerator FlipCoroutine()
    {
        isFlipping = true;

        float halfway = 90f;
        float currentY = 0f;
        float elapsed = 0f;
        float duration = halfway / flipSpeed;

        Vector3 baseEuler = transform.localEulerAngles;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float yRotation = Mathf.Lerp(currentY, halfway, elapsed / duration);
            transform.localEulerAngles = new Vector3(baseEuler.x, yRotation, baseEuler.z);
            yield return null;
        }

        isFaceUp = !isFaceUp;
        UpdateVisibility();

        elapsed = 0f;
        currentY = 90f;
        duration = 90f / flipSpeed;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float yRotation = Mathf.Lerp(currentY, 0f, elapsed / duration);
            transform.localEulerAngles = new Vector3(baseEuler.x, yRotation, baseEuler.z);
            yield return null;
        }

        transform.localEulerAngles = baseEuler;
        isFlipping = false;
    }

    private void UpdateVisibility()
    {
        frontRenderer.gameObject.SetActive(isFaceUp);
        cardBackRenderer.gameObject.SetActive(!isFaceUp);
    }
}
