using System.Collections;
using System.Collections.Generic;
using CardManager;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public List<HabitatSlotManager> playerSlots;
    public List<HabitatSlotManager> enemySlots;

    public IEnumerator ResolveCombat()
    {
        for (int i = 0; i < playerSlots.Count; i++)
        {
            var playerSlot = playerSlots[i];
            var enemySlot = enemySlots[i];

            if (playerSlot.HasCreature() && enemySlot.HasCreature())
            {
                yield return ResolveFight(
                    playerSlot.GetCreature(),
                    enemySlot.GetCreature()
                );
            }
            else if (playerSlot.HasCreature())
            {
                yield return DirectAttack(playerSlot.GetCreature());
            }
        }
    }

    IEnumerator ResolveFight(GameObject attacker, GameObject defender)
    {
        var atkDisplay = attacker.GetComponent<CardDisplay>();
        var defDisplay = defender.GetComponent<CardDisplay>();

        var atkData = atkDisplay.cardData as CreatureCard;
        var defData = defDisplay.cardData as CreatureCard;

        int atkDamage = atkDisplay.currentDamage;
        int defDamage = defDisplay.currentDamage;

        int atkHealth = atkDisplay.currentHealth;
        int defHealth = defDisplay.currentHealth;

        // Apply abilities BEFORE damage
        ApplyAbilities(atkData, defData);

        defHealth -= atkDamage;
        atkHealth -= defDamage;

        //Sets current Damage and healt to their new value after the attack and the abilities
        atkDisplay.currentDamage = atkDamage;
        defDisplay.currentDamage = defDamage;

        atkDisplay.currentHealth = atkHealth;
        defDisplay.currentHealth = defHealth;
        //

        Debug.Log(atkData.name + ": " + atkHealth + ", " + atkDamage);
        Debug.Log(defData.name + ": " + defHealth + ", " + defDamage);

        yield return new WaitForSeconds(0.5f);

        if (defHealth <= 0)
        {
            Destroy(defender);

        }

        if (atkHealth <= 0)
        {
            Destroy(attacker);
        }

        UpdateCardUI(attacker);
        UpdateCardUI(defender);
    }

    void ApplyAbilities(CreatureCard attacker, CreatureCard defender)
    {
        Debug.Log("Abilità");
    }

    void UpdateCardUI(GameObject card)
    {
        var display = card.GetComponent<CardDisplay>();
        display.UpdateCreatureCardDisplay();
    }

    IEnumerator DirectAttack(GameObject attacker)
    {
        var data = attacker.GetComponent<CardDisplay>();

        int atkDamage = data.currentDamage;

        PlayerData.playerDmgPoints += atkDamage;
        Debug.Log("Player Damage: " + PlayerData.playerDmgPoints);

        yield return new WaitForSeconds(0.3f);
    }
}
