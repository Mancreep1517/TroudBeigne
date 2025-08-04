using System.Collections;
using UnityEngine;

public class CardView : MonoBehaviour
{
    [SerializeField] private SpriteRenderer frontRenderer;
    [SerializeField] private SpriteRenderer cardBackRenderer;
    [SerializeField] private float flipSpeed = 600f;
    private CardPlay cardPlay;
    private Card card;
    public Card Card => card;

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

        Vector3 currentEuler = transform.localEulerAngles;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            currentEuler = transform.localEulerAngles;
            float yRotation = Mathf.Lerp(currentY, halfway, elapsed / duration);
            transform.localEulerAngles = new Vector3(currentEuler.x, yRotation, currentEuler.z);
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
            currentEuler = transform.localEulerAngles;
            float yRotation = Mathf.Lerp(currentY, 0f, elapsed / duration);

            transform.localEulerAngles = new Vector3(currentEuler.x, yRotation, currentEuler.z);
            yield return null;
        }

        currentEuler = transform.localEulerAngles;
        transform.localEulerAngles = new Vector3(currentEuler.x, 0f, currentEuler.z);
        isFlipping = false;
    }

    private void UpdateVisibility()
    {
        frontRenderer.gameObject.SetActive(isFaceUp);
        cardBackRenderer.gameObject.SetActive(!isFaceUp);
    }

    public IEnumerator MoveToPosition(Vector3 targetPos, Quaternion targetRot, float duration, bool flipDuring = false)
    {
        Vector3 startPos = transform.position;
        Quaternion startRot = transform.rotation;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            t = 1f - Mathf.Pow(1f - t, 3);

            transform.position = Vector3.Lerp(startPos, targetPos, t);
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t);
            yield return null;
        }

        transform.position = targetPos;
        transform.rotation = targetRot;

        if (flipDuring)
        {
            FlipCardAnimated();
        }
    }

}