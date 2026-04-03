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
        var atkData = attacker.GetComponent<CardDisplay>().cardData as CreatureCard;
        var defData = defender.GetComponent<CardDisplay>().cardData as CreatureCard;

        int atkDamage = atkData.damage;
        int defDamage = defData.damage;

        // Apply abilities BEFORE damage
        ApplyAbilities(atkData, defData);

        defData.health -= atkDamage;
        atkData.health -= defDamage;

        UpdateCardUI(attacker);
        UpdateCardUI(defender);

        yield return new WaitForSeconds(0.5f);

        if (defData.health <= 0)
            Destroy(defender);

        if (atkData.health <= 0)
            Destroy(attacker);
    }

    void ApplyAbilities(CreatureCard attacker, CreatureCard defender)
    {
        foreach (var ability in attacker.abilityType)
        {
            switch (ability)
            {
                case CreatureCard.AbilityType.DoubleImpact:
                    attacker.damage *= 2;
                    break;

                case CreatureCard.AbilityType.Aggressive:
                    defender.health -= 1;
                    break;

                case CreatureCard.AbilityType.Vengeful:
                    attacker.damage += 2;
                    break;
            }
        }
    }

    void UpdateCardUI(GameObject card)
    {
        var display = card.GetComponent<CardDisplay>();
        display.UpdateCreatureCardDisplay();
    }

    IEnumerator DirectAttack(GameObject attacker)
    {
        var data = attacker.GetComponent<CardDisplay>().cardData as CreatureCard;

        PlayerData.playerDmgPoints += data.damage;

        yield return new WaitForSeconds(0.3f);
    }
}
