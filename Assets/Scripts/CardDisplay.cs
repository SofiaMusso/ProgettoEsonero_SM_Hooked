using UnityEngine;
using TMPro;
using UnityEngine.UI;
using CardManeger;

public class CardDisplay : MonoBehaviour
{
    public CreatureCard cardData;

    [Header ("Card Data")]
    public Image cardImage;
    public Image[] landTypeImages;
    public Image[] abilty;

    public TMP_Text nameText;
    public TMP_Text costText;
    public TMP_Text healthText;
    public TMP_Text damageText;

    void Start()
    {
        UpdateCardDisplay();
    }

    public void UpdateCardDisplay()
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
