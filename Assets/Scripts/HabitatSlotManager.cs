using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CardManager;

public class HabitatSlotManager : MonoBehaviour, IDropHandler
{
    [Header("Slot Settings")]
    public CreatureCard.CardType slotType;

    private bool creatureCardIsPlaced;
    private bool landCardIsPlaced;

    private GameObject creatureCard;
    private GameObject landCard;

    public List<GameObject> cardsOnBoard = new List<GameObject>();
    public HandManager handManager;
    public DiscardDeckManager discardDeckManager;

    void Awake()
    {
        if (handManager == null)
        {
            handManager = FindAnyObjectByType<HandManager>();
        }
        if (discardDeckManager == null)
        {
            discardDeckManager = FindAnyObjectByType<DiscardDeckManager>();
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

    public bool CanPayCard(CardDisplay cardDisplay)
    {

        CardData cardData = cardDisplay.cardData;

        if (!creatureCardIsPlaced && PlayerData.playerDroplets >= cardData.cost)
        {
            PlayerData.playerDroplets -= cardData.cost;
            Debug.Log(PlayerData.playerDroplets);

            return true;
        }
        if (!landCardIsPlaced && PlayerData.playerDroplets >= cardData.cost)
        {
            PlayerData.playerDroplets -= cardData.cost;
            Debug.Log(PlayerData.playerDroplets);

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
        if (!creatureCardIsPlaced || !landCardIsPlaced)
        {
            handManager.cardsInHand.Remove(card);
            cardsOnBoard.Add(card);

            card.transform.SetParent(transform);

            CardDisplay cardDisplay = card.GetComponent<CardDisplay>();

            if (cardDisplay.cardData is LandCard)
            {
                landCardIsPlaced = true;
                landCard = card;
                card.transform.localPosition = new Vector3(0, -1.2f, 0);
                card.transform.localRotation = Quaternion.identity;
            }
            else if(cardDisplay.cardData is CreatureCard)
            {
                creatureCardIsPlaced = true;
                creatureCard = card;
                card.transform.localPosition = new Vector3(0, 1f, 0);
                card.transform.localRotation = Quaternion.identity;
            }
            else if (cardDisplay.cardData is TreasureCard)
            {
                DiscardSingleCard(card);
            }

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

    public void DiscardCreature()
    {
        if (creatureCard != null)
        {
            DiscardSingleCard(creatureCard);
            creatureCard = null;
            creatureCardIsPlaced = false;
        }
    }

    public void DiscardLand()
    {
        if (landCard != null)
        {
            DiscardSingleCard(landCard);
            landCard = null;
            landCardIsPlaced = false;
        }
    }
    private void DiscardSingleCard(GameObject card)
    {
        CardDisplay cardDisplay = card.GetComponent<CardDisplay>();
        CardData cardData = cardDisplay.cardData;

        if (cardData.cost == 0 || cardData.cost == 1)
        {
            PlayerData.playerDroplets += 1;
        }
        else
        {
            PlayerData.playerDroplets += cardData.cost / 2;
        }

        cardsOnBoard.Remove(card);
        discardDeckManager.AddToDiscard(card);
    }

}
