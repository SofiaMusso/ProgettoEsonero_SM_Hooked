using System;
using CardManager;
using UnityEngine;

public class AbilitySystem : MonoBehaviour
{

    public static bool CanBeAttacked(CreatureCard target)
    {
        return !target.abilityType.Contains(CreatureCard.AbilityType.Burrower); // Nascosto
    }

    public static void OnAttack(GameObject attackerGO, HabitatSlotManager[] enemySlots, int index)
    {
        var attacker = attackerGO.GetComponent<CardDisplay>().cardData as CreatureCard;

        foreach (var ability in attacker.abilityType)
        {
            switch (ability)
            {
                case CreatureCard.AbilityType.Jump: 
                    //BoardManager.DirectAttack(attacker);
                    return;

                case CreatureCard.AbilityType.DoubleImpact: 
                    //AttackSideSlots(attackerGO, enemySlots, index);
                    return;

                case CreatureCard.AbilityType.Electrified:
                    //DamageAllEnemies(enemySlots, 1);
                    break;
            }
        }
    }

    public static void OnHit(GameObject attackerGO, HabitatSlotManager[] enemySlots, int index)
    {
        var attacker = attackerGO.GetComponent<CardDisplay>().cardData as CreatureCard;

        foreach (var ability in attacker.abilityType)
        {
            switch (ability)
            {
                case CreatureCard.AbilityType.Parasite: 
                   
                    return;

                case CreatureCard.AbilityType.Vengeful:

                    break;
             
            }
        }
    }

    public static void Passive()
    {

    }

    public static void OnDeath()
    {

    }

    /*static void AttackSideSlots(GameObject attacker, HabitatSlotManager[] slots, int index)
    {
        if (index > 0)
            AttackFront(attacker, slots[index - 1]);

        if (index < slots.Length - 1)
            AttackFront(attacker, slots[index + 1]);
    }*/
}
