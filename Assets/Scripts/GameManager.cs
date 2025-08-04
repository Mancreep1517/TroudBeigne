using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<CardData> cardDatas;
    [SerializeField] private CardView cardView;
    [SerializeField] private CardPlay cardPlay;
    [SerializeField] private List<Transform> playerZones;
    [SerializeField] private List<int> cardsPerPlayer;
    [SerializeField] private Transform centerPile;
    private List<Card> deck;

    public List<CardPlay> selectedCards = new();
    public List<CardPlay> playedCards = new();

    public int turn = 0;

    private void Start()
    {
        StartCoroutine(DistributeCards());
    }

    private IEnumerator DistributeCards()
    {
        List<CardData> workingDeck = new(cardDatas);
        Shuffle(workingDeck);
        int cardIndex = 0;

        foreach (Transform zone in playerZones)
        {
            zone.transform.Translate(0.0f, -18.5f, 0.0f);
        }

        List<List<(Card card, CardData data, int drawIndex)>> playerHands = new();
        List<List<(Card card, CardData data, int drawIndex)>> distributionOrders = new();
        for (int i = 0; i < playerZones.Count; i++)
        {
            playerHands.Add(new List<(Card, CardData, int)>());
            distributionOrders.Add(new List<(Card, CardData, int)>());
        }

        for (int player = 0; player < playerZones.Count; player++)
        {
            int count = cardsPerPlayer[player];
            for (int i = 0; i < count && cardIndex < workingDeck.Count; i++, cardIndex++)
            {
                CardData data = workingDeck[cardIndex];
                Card card = new(data);
                playerHands[player].Add((card, data, i));
            }

            distributionOrders[player] = new List<(Card, CardData, int)>(playerHands[player]);
            distributionOrders[player].Sort((a, b) =>
            {
                int compare = a.card.Value.CompareTo(b.card.Value); // tri croissant pour distribution
                return compare == 0 ? a.drawIndex.CompareTo(b.drawIndex) : compare;
            });

            playerHands[player].Sort((a, b) =>
            {
                int compare = b.card.Value.CompareTo(a.card.Value); // tri décroissant pour affichage
                return compare == 0 ? b.drawIndex.CompareTo(a.drawIndex) : compare;
            });
        }

        int maxCards = 0;
        foreach (var hand in playerHands)
            maxCards = Mathf.Max(maxCards, hand.Count);

        for (int i = 0; i < maxCards; i++)
        {
            for (int playerIndex = 0; playerIndex < playerZones.Count; playerIndex++)
            {
                if (i >= playerHands[playerIndex].Count) continue;

                var (card, _, drawIndex) = distributionOrders[playerIndex][i];
                Transform zone = playerZones[playerIndex];

                float spreadAngle = (playerIndex == 0) ? 4f : 1f;
                int totalCards = playerHands[playerIndex].Count;

                float startAngle = (totalCards == 1) ? 90f :
                    (-spreadAngle * (totalCards - 1) / 2f) + 90f;

                int displayIndex = totalCards - 1 - i;

                float angle = startAngle + displayIndex * spreadAngle;
                Quaternion targetRot = zone.rotation * Quaternion.Euler(0, 0, angle - 90f);
                Vector3 offset = Quaternion.Euler(0, 0, angle) * Vector3.right * 2f;
                Vector3 targetPos = zone.position + zone.rotation * (offset * 10);
                targetPos.z = 0;

                CardView view = Instantiate(cardView, centerPile.position, Quaternion.identity);
                CardPlay play = view.GetComponent<CardPlay>();
                play.playerNumber = playerIndex;

                int sortingOrder = i;
                view.Setup(card, sortingOrder);
                view.UpdateFaceUp(false);

                bool flipDuringMove = (playerIndex == 0);
                StartCoroutine(view.MoveToPosition(targetPos, targetRot, 0.3f, flipDuringMove));
                yield return new WaitForSeconds(0.1f);
            }
        }
    }


    private void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    public void DrawCard()
    {
        selectedCards.Clear();
        foreach (CardPlay cp in FindObjectsOfType<CardPlay>())
        {
            if (cp.isSelected && !cp.isPlayed)
            {
                selectedCards.Add(cp);
            }
        }

        if (selectedCards.Count == 0)
        {
            Debug.Log("Il doit y avoir au moins une carte de sélectionné.");
            return;
        }

        int refValue = selectedCards[0].GetComponentInParent<CardView>().Card.Value;
        foreach (var cp in selectedCards)
        {
            if (cp.GetComponentInParent<CardView>().Card.Value != refValue)
            {
                Debug.Log("Toutes les cartes doivent avoir la même valeur.");
                return;
            }
        }

        float spacing = 1.5f;
        float totalWidth = (selectedCards.Count - 1) * spacing;
        float startOffset = -totalWidth / 2f;

        for (int i = 0; i < selectedCards.Count; i++)
        {
            CardPlay cp = selectedCards[i];
            Vector3 offset = centerPile.position + Vector3.right * (startOffset + i * spacing);

            cp.isPlayed = true;
            playedCards.Add(cp);

            Quaternion centerRot = Quaternion.identity;
            cp.StartCoroutine(cp.MoveToPosition(offset, 0.4f));
        }

        selectedCards.Clear();
    }
}