using UnityEngine;
using CardManeger;
using System.Collections;
using System.Collections.Generic;

public class HandManager : MonoBehaviour
{
    public DeckManager deckManager;

    public GameObject cardPrefab;
    public Transform handTransform;

    public float fanSpread;
    public float cardSpacing;
    public float verticalSpacing;

    public List<GameObject> cardsInHand = new List<GameObject>();
  
    void Start()
    {

    }

    private void FixedUpdate()
    {
        //UpdateHandVisuals();
    }

    public void AddCardToHand(CreatureCard cardData)
    {
        //Instantiate the cards and adds it in the list
        GameObject newCard = Instantiate(cardPrefab, handTransform.position, Quaternion.identity, handTransform);
        cardsInHand.Add(newCard);

        //set card data
        newCard.GetComponent<CardDisplay>().cardData = cardData;

        UpdateHandVisuals();
    }

    public void UpdateHandVisuals()
    {
        int cardCount = cardsInHand.Count;

        if (cardCount == 1)
        {
            cardsInHand[0].transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
            cardsInHand[0].transform.localPosition = new Vector3(0f, 0f, 0f);
            return;
        }

        for(int i = 0; i < cardCount; i++)
        {
            float rotationAngle = (fanSpread * (i - (cardCount - 1) / 2f));
            cardsInHand[i].transform.localRotation = Quaternion.Euler(0f, 0f, rotationAngle);

            float hOffset = (cardSpacing * (i - (cardCount - 1) / 2f));

            float normalizedPosition = (2f * i / (cardCount - 1) - 1f); //Normalize card position between -1, 1
            float vOffset = verticalSpacing * (1 - normalizedPosition * normalizedPosition);

            //set card position
            cardsInHand[i].transform.localPosition = new Vector3(hOffset, vOffset, 0f);
        }
    }
    void Update()
    {
        
    }
}
