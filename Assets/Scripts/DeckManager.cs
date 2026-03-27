using CardManager;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DeckManager : MonoBehaviour
{
    public List<CardData> allCards = new List<CardData>();

    private int currentIndex = 0;

    public int maxHandSize;
    public int currentHandSize;

    private HandManager handManager;
    public CardDatabase database;

    private void Start()
    {
        allCards = new List<CardData>(database.allCards);

        handManager = FindAnyObjectByType<HandManager>();

        for(int i = 0; i < 6; i++)
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
        if (allCards.Count == 0)
        {
            return;
        }
        if (currentHandSize < maxHandSize)
        {
            CardData nextCard = allCards[currentIndex];
            handManager.AddCardToHand(nextCard);
            currentIndex = (currentIndex + 1) % allCards.Count;
        }
    }

}
