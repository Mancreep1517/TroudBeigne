using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public bool hasBeenPlayed;
    public int handIndex;
    private GameManager gm;

    void Start()
    {
        gm = FindFirstObjectByType<GameManager>(); // Modern replacement for FindObjectOfType
    }

    private void OnMouseDown()
    {
        if (!hasBeenPlayed)
        {
            transform.position += Vector3.up * 5;
            hasBeenPlayed = true;
            StartCoroutine(MoveToDiscardPileDelayed(2f)); // Coroutine instead of Invoke
        }
    }

    private IEnumerator MoveToDiscardPileDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        MoveToDiscardPile();
    }

    void MoveToDiscardPile()
    {
        gm.discardPile.Add(this);
        gameObject.SetActive(false);
    }

    void Update()
    {
    }
}
