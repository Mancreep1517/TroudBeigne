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

            float spreadAngle = 20f;
            float startAngle = -spreadAngle * (numberOfCards - 1) / 2f;

            for (int i = 0; i < numberOfCards; i++)
            {
                if (cardIndex >= deck.Count) break;

                CardData data = deck[cardIndex];
                cardIndex++;


                float angle = startAngle + i * spreadAngle;
                Quaternion rotation = zone.rotation * Quaternion.Euler(0, 0, angle);

                Vector3 offset = Quaternion.Euler(0, 0, angle) * Vector3.right * 2f;
                Vector3 position = zone.position + zone.rotation * offset;


                CardView card = Instantiate(cardView, position, rotation);
                Card c = new(data);
                card.Setup(c);
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
        view.Setup(drawnCard);
    }
}
