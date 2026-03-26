using CardManager;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class DeckManager : MonoBehaviour
{
    public List<CreatureCard> allCards = new List<CreatureCard>();

    private int currentIndex = 0;

    private void Start()
    {
        HandManager hand = FindAnyObjectByType<HandManager>();
        for (int i = 0; i < 6; i++)
        {
            DrawCard(hand);
        }
    }
    public void DrawCard(HandManager handManager)
    {
        if (allCards.Count == 0)
        {
            return;
        }

        CreatureCard nextCard = allCards[currentIndex];
        handManager.AddCardToHand (nextCard);
        currentIndex = (currentIndex + 1) % allCards.Count;
    }
}
