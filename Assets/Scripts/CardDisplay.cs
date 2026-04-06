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

    public int currentHealth;
    public int currentDamage;

    void Start()
    {   
        if (cardData is CreatureCard creature)
        {
            currentHealth = creature.health;
            currentDamage = creature.damage;

            Debug.Log(cardData.name + ": " + currentHealth + ", " + currentDamage);
        }

        UpdateCreatureCardDisplay();
    }

    void ResetUI()
    {
        //healthText.gameObject.SetActive(false);
        //damageText.gameObject.SetActive(false);

        foreach (var img in landTypeImages)
        {
            img.gameObject.SetActive(false);
        }
        foreach (var img in abilty)
        {
            img.gameObject.SetActive(false);
        }
    }

    public void UpdateCreatureCardDisplay()
    {
        ResetUI();

        nameText.text = cardData.cardName;
        costText.text = cardData.cost.ToString();
        //cardSprite.sprite = cardData.sprite;

        if (cardData is CreatureCard creature)
        {
        
            healthText.text = currentHealth.ToString();
            damageText.text = currentDamage.ToString();

            Debug.Log("Update UI of " + cardData.name + ": " + currentHealth + ", " + currentDamage);

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
