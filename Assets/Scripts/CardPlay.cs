using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardPlay : MonoBehaviour, IPointerClickHandler
{
    private CardView cardView;
    private GameManager gameManager;

    public int playerNumber;
    private bool isSelected = false;
    private Coroutine moveCoroutine;

    private void Start()
    {
        cardView = GetComponentInParent<CardView>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(gameManager.turn == 0 && playerNumber == 0)
        {
            CardUpDown();
        }
    }

    private void CardUpDown()
    {
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        float distance = 1f;
        Vector3 targetPosition;

        if (!isSelected)
        {
            targetPosition = transform.position + transform.up * distance;
        }
        else
        {
            targetPosition = transform.position - transform.up * distance;
        }

        moveCoroutine = StartCoroutine(MoveToPosition(targetPosition, 0.4f)); // durée en secondes

        isSelected = !isSelected;
    }

    private IEnumerator MoveToPosition(Vector3 target, float duration)
    {
        Vector3 start = transform.position;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            t = 1f - Mathf.Pow(1f - t, 3);

            transform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }

        transform.position = target;
    }
}
