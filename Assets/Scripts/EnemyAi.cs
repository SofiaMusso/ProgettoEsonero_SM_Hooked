using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public DeckManager deck;
    public HandManager hand;
    public List<HabitatSlotManager> enemySlots;

    public AudioSource placeDownCardSfx;

    public void PlayTurn()
    {
        StartCoroutine(PlayRoutine());
    }

    IEnumerator PlayRoutine()
    {
        yield return new WaitForSeconds(1f);

        Debug.Log("Enemy Turn Start");

        deck.DrawCard(hand);

        yield return new WaitForSeconds(1f);

        List<GameObject> sortedCards = hand.cardsInHand

            .OrderByDescending(c =>
            {
                var d = c.GetComponent<CardDisplay>();
                return d.currentDamage + d.currentHealth;
            })
            .ToList();

        foreach (GameObject card in sortedCards)
        {
            var display = card.GetComponent<CardDisplay>();
            var cardData = display.cardData;

            if (PlayerData.enemyDroplets < cardData.cost)
            {
                continue;
            }

            HabitatSlotManager bestSlot = FindBestSlot(display);

            if (bestSlot != null)
            {
                PlaceCardAI(card, bestSlot);
                PlayerData.enemyDroplets -= cardData.cost;

                yield return new WaitForSeconds(0.5f);
            }
        }

        yield return new WaitForSeconds(1f);

        Debug.Log("Enemy Turn End");

        TurnManager.Instance.EndTurn();
    }
    HabitatSlotManager FindBestSlot(CardDisplay card)
    {
        for (int i = 0; i < enemySlots.Count; i++)
        {
            var slot = enemySlots[i];

            if (!slot.CanPlaceOnHabitat(card) || slot.HasCreature())
                continue;

            var playerSlot = TurnManager.Instance.boardManager.playerSlots[i];

            if (!playerSlot.HasCreature())
                return slot;
        }

        return enemySlots.FirstOrDefault(s => !s.HasCreature());
    }

    void PlaceCardAI(GameObject card, HabitatSlotManager slot)
    {
        CardMovement movement = card.GetComponent<CardMovement>();
        CardDisplay display = card.GetComponent<CardDisplay>();

        hand.cardsInHand.Remove(card);

        slot.PlaceCardAI(card);
        placeDownCardSfx.Play();

        if (movement != null)
        {
            movement.enabled = false;
            CardMovement.isDragging = false;
            CardMovement.currentCardInPlay = null;
        }

        Debug.Log("Enemy played: " + display.cardData.cardName);
    }
}
