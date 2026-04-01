using System.Collections.Generic;
using System.Linq;
using CardManager;
using UnityEngine;


public class DiscardDeckManager : MonoBehaviour
{
    public static List<GameObject> discardedCards = new List<GameObject>();

    public void AddToDiscard(GameObject card)
    {
        discardedCards.Add(card);

        // Hide the card from view
        CanvasGroup cg = card.GetComponent<CanvasGroup>();
        if (cg != null)
        {
            cg.alpha = 0;
            cg.interactable = false;
            cg.blocksRaycasts = false;
        }

        // Optional: move to hidden container
        card.transform.SetParent(null);
        card.transform.position = new Vector3(1000f, 1000f, 0f);
    }
}


