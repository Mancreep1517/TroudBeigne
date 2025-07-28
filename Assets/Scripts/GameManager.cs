using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private List<CardData> cardDatas;
    [SerializeField] private CardView cardView;
    [SerializeField] private List<Transform> playerZones;
    [SerializeField] private List<int> cardsPerPlayer;
    private List<Card> deck;

    private void Start()
    {
        List<CardData> deck = new(cardDatas);
        Shuffle(deck);

        int cardIndex = 0;

        for (int playerIndex = 0; playerIndex < playerZones.Count; playerIndex++)
        {
            Transform zone = playerZones[playerIndex];
            int numberOfCards = cardsPerPlayer[playerIndex];

            List<(Card card, CardData data, int drawIndex)> playerCards = new();

            for (int i = 0; i < numberOfCards && cardIndex < deck.Count; i++, cardIndex++)
            {
                CardData data = deck[cardIndex];
                Card card = new(data);
                playerCards.Add((card, data, i));
            }

            playerCards.Sort((a, b) =>
            {
                int compare = b.card.Value.CompareTo(a.card.Value);
                if (compare == 0)
                    return b.drawIndex.CompareTo(a.drawIndex);
                return compare;
            });


            float spreadAngle = 10f;

            float startAngle = (playerCards.Count == 1) ? 90f :
                (-spreadAngle * (playerCards.Count - 1) / 2f) + 90f;

            for (int i = 0; i < playerCards.Count; i++)
            {
                float angle = startAngle + i * spreadAngle;
                Quaternion rotation = zone.rotation * Quaternion.Euler(0, 0, angle - 90f);

                Vector3 offset = Quaternion.Euler(0, 0, angle) * Vector3.right * 2f;
                Vector3 position = zone.position + zone.rotation * offset;
                position.z = 0;

                CardView view = Instantiate(cardView, position, rotation);
                int sortingOrder = playerCards.Count - 1 - i;
                view.Setup(playerCards[i].card, sortingOrder);
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
        Card drawnCard = deck[Random.Range(0, deck.Count)];
        deck.Remove(drawnCard);
        CardView view = Instantiate(cardView);

        int sortingOrder = 99;
        view.Setup(drawnCard, sortingOrder);
    }

}
