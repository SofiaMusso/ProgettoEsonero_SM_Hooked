using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CardManager;

public class HabitatSlot : MonoBehaviour, IDropHandler
{
    [Header("Slot Settings")]
    public CreatureCard.CardType slotType;

    private GameObject currentCard;
    private bool cardIsPlaced;

    public List<GameObject> cardsOnBoard = new List<GameObject>();
    public HandManager handManager;

    void Awake()
    {
        if (handManager == null)
        {
            handManager = FindAnyObjectByType<HandManager>();
        }
    }
    public bool CanPlaceOnHabitat(CardDisplay cardDisplay)
    {
        if (cardDisplay.cardData is CreatureCard creatureCard)
        {
            return creatureCard.cardType.Contains(slotType);
        }
        else if (cardDisplay.cardData is LandCard landCard)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedObject = eventData.pointerDrag;

        if (droppedObject == null) return;

        CardDisplay cardDisplay = droppedObject.GetComponent<CardDisplay>();
        CardMovement cardMovement = droppedObject.GetComponent<CardMovement>();

        if (cardDisplay == null) return;

        if (CanPlaceOnHabitat(cardDisplay) && CanPayCard(cardDisplay))
        {
            PlaceCard(droppedObject, cardMovement);
        }
        else
        {
            RejectCard(cardMovement);
        }
    }

    private void PlaceCard(GameObject card, CardMovement movement)
    {
        if (!cardIsPlaced)
        {
            cardIsPlaced = true;

            currentCard = card;
            handManager.cardsInHand.Remove(card);
            cardsOnBoard.Add(card);

            card.transform.SetParent(transform);

            card.transform.localPosition = new Vector3(0, 0.7f, 0);
            card.transform.localRotation = Quaternion.identity;

            if (movement != null)
            {
                movement.enabled = false;

                CardMovement.isDragging = false;
                CardMovement.currentCardInPlay = null;
            }
        }
        else
        {
            Debug.Log("Slot is occupied");
        }
    }

    private void RejectCard(CardMovement movement)
    {
        Debug.Log("Invalid land type or not enough Droplets");

        if (movement != null)
        {
            movement.SendMessage("TransitionToStateZero");
        }
    }

    public void DiscardCard()
    {
        if (cardIsPlaced)
        {
            Debug.Log("discard this card?");
        }
    }

    public bool CanPayCard(CardDisplay cardDisplay)
    {

        CardData cardData = cardDisplay.cardData;

        if (!cardIsPlaced && PlayerData.playerDroplets >= cardData.cost)
        {
            PlayerData.playerDroplets -= cardData.cost;
            Debug.Log("I'm rich wohooo");

            return true;
        }
        else
        {
            return false;
        }
    }
}
