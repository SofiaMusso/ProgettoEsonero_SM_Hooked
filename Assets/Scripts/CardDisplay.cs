using UnityEngine;
using TMPro;
using UnityEngine.UI;
using CardManager;

public class CardDisplay : MonoBehaviour
{
    public CardData cardData;

    [Header ("Creature Card Data")]
    public Image cardImage;
    public Image[] abilty;
    public Image cardSprite;

    public Image abilitySprite;

    public TMP_Text landText;
    public Image landIcon;

    public TMP_Text nameText;
    public TMP_Text costText;
    public TMP_Text healthText;
    public TMP_Text damageText;

    public int currentHealth;
    public int currentDamage;

    private Color[] typeColors =
    {
        new Color(0.42f, 0.8f, 1f), //Ocean
        new Color(1f, 0.96f, 0.61f), //Beach
        new Color(0.47f, 1f, 0.75f), //Reef
        new Color(0.48f, 0.44f, 0.7f), //Abyss
    };

    void Start()
    {   
        if (cardData is CreatureCard creature)
        {
            currentHealth = creature.health;
            currentDamage = creature.damage;

            Debug.Log(cardData.name + ": " + currentHealth + ", " + currentDamage);
        }
        else
        {
            abilitySprite = null;
        }

            UpdateCreatureCardDisplay();
    }


    public void UpdateCreatureCardDisplay()
    {
        nameText.text = cardData.cardName;
        costText.text = cardData.cost.ToString();

        if (cardSprite != null && cardData.sprite != null)
        {
            cardSprite.sprite = cardData.sprite;
        }


        if (cardData is CreatureCard creature)
        {
            if (abilitySprite != null && creature.abilitySprite != null)
            {
                abilitySprite.sprite = creature.abilitySprite;
                abilitySprite.color = typeColors[(int)creature.cardType[0]];
                Debug.Log("Ability set");
            }

            landText.text = creature.land;
            landIcon.color = typeColors[(int)creature.cardType[0]];

            cardImage.color = typeColors[(int)creature.cardType[0]];

            healthText.text = currentHealth.ToString();
            damageText.text = currentDamage.ToString();

            Debug.Log("Update UI of " + cardData.name + ": " + currentHealth + ", " + currentDamage);
        }

        if (cardData is LandCard land)
        {
            cardImage.color = new Color(1f, 1f, 1f);
            healthText.text = land.health.ToString();
            damageText.text = land.damage.ToString();
        }

        if (cardData is TreasureCard treasure)
        {
            cardImage.color = new Color( 1f, 1f, 1f);
        }

    }
}
