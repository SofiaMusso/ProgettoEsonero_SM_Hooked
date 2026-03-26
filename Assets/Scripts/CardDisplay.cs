using UnityEngine;
using TMPro;
using UnityEngine.UI;
using CardManager;

public class CardDisplay : MonoBehaviour
{
    public CreatureCard cardData;

    [Header ("Creature Card Data")]
    public Image cardImage;
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

    public void UpdateCreatureCardDisplay()
    {
        //Updates string data
        nameText.text = cardData.cardName;
        costText.text = cardData.cost.ToString();
        healthText.text = cardData.health.ToString();
        damageText.text = cardData.damage.ToString();

        //Updates Images data
        for (int i = 0; i < landTypeImages.Length; i++)
        {
            if (i < cardData.cardType.Count)
            {
                landTypeImages[i].gameObject.SetActive(true);
            }
            else
            {
                landTypeImages[i].gameObject.SetActive(false);
            }
        }

    }
}
