using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using CardManager;

public class HabitatSlotManager : MonoBehaviour, IDropHandler
{
    [Header("Slot Settings")]
    public CreatureCard.CardType slotType;
    public PlayerSlot playerSlot;

    public bool creatureCardIsPlaced;
    public bool landCardIsPlaced;

    private GameObject creatureCard;
    private GameObject landCard;

    public List<GameObject> cardsOnBoard = new List<GameObject>();
    public HandManager handManager;
    public DiscardDeckManager discardDeckManager;

    public AudioSource placeDownCardSfx;
    public AudioSource discardCardSfx;

    public enum PlayerSlot
    {
        player,

        enemy,
    }
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
            Debug.Log("Invalid land type");
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
            Debug.Log("Not enough Droplets");
            return false;
        }
    }


    public void OnDrop(PointerEventData eventData)
    {
        if(TurnManager.Instance.currentTurn != TurnManager.TurnOwner.Player)
{
            return;
        }
        GameObject droppedObject = eventData.pointerDrag;

        if (droppedObject == null) return;

        CardDisplay cardDisplay = droppedObject.GetComponent<CardDisplay>();
        CardMovement cardMovement = droppedObject.GetComponent<CardMovement>();

        if (cardDisplay == null) return;

        if (CanPlaceOnHabitat(cardDisplay) && CanPayCard(cardDisplay) && playerSlot == PlayerSlot.player)
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
        CardDisplay cardDisplay = card.GetComponent<CardDisplay>();

        if (cardDisplay.cardData is CreatureCard && creatureCardIsPlaced)
        {
            Debug.Log("Creature slot already occupied");
            RejectCard(movement);
            return;
        }

        if (cardDisplay.cardData is LandCard && landCardIsPlaced)
        {
            Debug.Log("Land slot already occupied");
            RejectCard(movement);
            return;
        }

        handManager.cardsInHand.Remove(card);
        cardsOnBoard.Add(card);

        card.transform.SetParent(transform);

        placeDownCardSfx.Play();

        if (cardDisplay.cardData is LandCard)
        {
            landCardIsPlaced = true;
            landCard = card;
            card.transform.localPosition = new Vector3(0, -1.2f, 0);
        }
        else if (cardDisplay.cardData is CreatureCard)
        {
            creatureCardIsPlaced = true;
            creatureCard = card;
            card.transform.localPosition = new Vector3(0, 1f, 0);
        }
        else if (cardDisplay.cardData is TreasureCard)
        {
            DiscardSingleCard(card);
            return;
        }

        card.transform.localRotation = Quaternion.identity;

        if (movement != null)
        {
            movement.enabled = false;
            CardMovement.isDragging = false;
            CardMovement.currentCardInPlay = null;
        }
    }

    public void PlaceCardAI(GameObject card)
    {
        CardDisplay cardDisplay = card.GetComponent<CardDisplay>();

        if (cardDisplay.cardData is CreatureCard && !HasCreature())
        {
            card.transform.SetParent(transform);
            card.transform.localPosition = new Vector3(0, 1f, 0);
            card.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);
            creatureCard = card;
        }
    }

    private void RejectCard(CardMovement movement)
    {
        if (movement != null)
        {
            movement.SendMessage("TransitionToStateZero");
        }
    }

    public void DiscardCreature()
    {
        if (creatureCard != null)
        {
            CardDisplay display = creatureCard.GetComponent<CardDisplay>();
            CreatureCard cardData = display.cardData as CreatureCard;

            if (cardData.abilityType.Contains(CreatureCard.AbilityType.Immortal))
            {
                // non disturgge la carta
                PlayerData.playerDroplets += cardData.cost;
                return;
            }
            else
            {
                DiscardSingleCard(creatureCard);
                creatureCard = null;
                creatureCardIsPlaced = false;
            }
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
        discardCardSfx.Play();
        discardDeckManager.AddToDiscard(card);

        Debug.Log(PlayerData.playerDroplets);
    }

    public bool HasCreature()
    {
        return creatureCard != null;
    }

    public GameObject GetCreature()
    {
        return creatureCard;
    }

}
