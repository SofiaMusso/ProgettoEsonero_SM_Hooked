using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CardManager;
using Unity.Collections;
using UnityEngine;


public class DeckManager : MonoBehaviour
{
    public List<CardData> playerCards = new List<CardData>();
    public List<CardData> deckCards = new List<CardData>();

    private int currentIndex = 0;

    public int maxHandSize;
    public int currentHandSize;

    private HandManager handManager;
    public CardDatabase database;

    public int deckSize;

    private void Start()
    {
        playerCards = new List<CardData>(database.allCards);

        handManager = FindAnyObjectByType<HandManager>();

        // Mescola tutte le carte
        playerCards = playerCards.OrderBy(card => Random.value).ToList();

        for (int i = 0; i < 6; i++)
        {
            DrawCard(handManager);
        }
    }

    private void Update()
    {
        if (handManager != null)
        {
            currentHandSize = handManager.cardsInHand.Count;
        }
    }
    public void DrawCard(HandManager handManager)
    {
        if (playerCards.Count == 0)
        {
            return;
        }
        if (currentHandSize < maxHandSize)
        {
            CardData nextCard = playerCards[currentIndex];
            handManager.AddCardToHand(nextCard);
            currentIndex = (currentIndex + 1) % playerCards.Count;
        }
    }

}
