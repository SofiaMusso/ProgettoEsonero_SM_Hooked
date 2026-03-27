using UnityEngine;
using TMPro;
using UnityEngine.UI;
using CardManager;

public class CardDisplay : MonoBehaviour
{
    public CardData cardData;

    [Header ("Creature Card Data")]
    public Image cardImage;
    //public Image cardSprite;
    public Image[] landTypeImages;
    public Image[] abilty;

    public TMP_Text nameText;
    public TMP_Text costText;
    public TMP_Text healthText;
    public TMP_Text damageText;

    void Start()
    {
        UpdateCreatureCardDisplay();
    }

    void ResetUI()
    {
        healthText.gameObject.SetActive(false);
        damageText.gameObject.SetActive(false);

        foreach (var img in landTypeImages)
            img.gameObject.SetActive(false);

        foreach (var img in abilty)
            img.gameObject.SetActive(false);
    }

    public void UpdateCreatureCardDisplay()
    {
        ResetUI();

        nameText.text = cardData.cardName;
        costText.text = cardData.cost.ToString();
        //cardSprite.sprite = cardData.sprite;

        if (cardData is CreatureCard creature)
        {
        
            healthText.text = creature.health.ToString();
            damageText.text = creature.damage.ToString();

            for (int i = 0; i < landTypeImages.Length; i++)
            {
                if (i < creature.cardType.Count)
                {
                    landTypeImages[i].gameObject.SetActive(true);
                }
                else
                {
                    landTypeImages[i].gameObject.SetActive(false);
                }
            }
        }

        if (cardData is LandCard land)
        {
            healthText.text = land.health.ToString();
            damageText.text = land.damage.ToString();
        }

        if (cardData is TreasureCard treasure)
        {

        }

    }
}
