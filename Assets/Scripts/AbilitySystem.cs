using System;
using System.Linq;
using System.Collections.Generic;
using CardManager;
using UnityEngine;

public class AbilitySystem : MonoBehaviour
{
    public static AbilitySystem Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // elimina duplicati
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // se vuoi mantenerlo tra scene
    }

    public bool CanBeAttacked(CreatureCard target)
    {
        return !target.abilityType.Contains(CreatureCard.AbilityType.Burrower);
    }

    public void OnAttack(GameObject attackerCard, List<HabitatSlotManager> enemySlots, int index)
    {

        CardDisplay display = attackerCard.GetComponent<CardDisplay>();

        CreatureCard attacker = display.cardData as CreatureCard;

        if (attacker == null) return;

        foreach (var ability in attacker.abilityType)
        {
            switch (ability)
            {
                case CreatureCard.AbilityType.Jump:
                    DirectAttack(attackerCard);
                    return; 

                case CreatureCard.AbilityType.DoubleImpact:
                    AttackSideSlots(attackerCard, enemySlots, index);
                    return;

                case CreatureCard.AbilityType.Electrified:
                    DamageAllEnemies(enemySlots, 1);
                    break;

                /*case CreatureCard.AbilityType.Move:
                    ShuffleEnemy(enemySlots, index);
                    break;*/
            }
        }
    }

    public void OnHit(GameObject attackerCard, GameObject defenderCard)
    {
        CardDisplay atkDisplay = attackerCard.GetComponent<CardDisplay>();
        CreatureCard attacker = atkDisplay.cardData as CreatureCard;

        if (attacker == null) return;

        foreach (var ability in attacker.abilityType)
        {
            switch (ability)
            {
                case CreatureCard.AbilityType.Parasite:
                    HealFromDamage(atkDisplay);
                    break;
            }
        }

        var defDisplay = defenderCard.GetComponent<CardDisplay>();
        var defender = defDisplay.cardData as CreatureCard;

        if (defender == null) return;

        foreach (var ability in defender.abilityType)
        {
            switch (ability)
            {
                case CreatureCard.AbilityType.Aggressive:
                    defDisplay.currentDamage += 1;
                    break;
            }
        }
    }

    public void OnDeath(GameObject dyingCard, bool isPlayerCard)
    {
        CardDisplay display = dyingCard.GetComponent<CardDisplay>();
        CreatureCard creature = display.cardData as CreatureCard;

        if (creature == null) return;

        foreach (var ability in creature.abilityType)
        {
            switch (ability)
            {
                case CreatureCard.AbilityType.Vengeful:
                    ApplyVengeful(display.currentDamage, isPlayerCard);
                    break;

                case CreatureCard.AbilityType.Immortal:
                    HandleImmortal(dyingCard, display);
                    break;
            }
        }
    }

    public void ApplyPassive(List<GameObject> allCreatures)
    {
        ApplySchool(allCreatures);
    }

    private void DirectAttack(GameObject attackerCard)
    {
        var display = attackerCard.GetComponent<CardDisplay>();
        int damage = display.currentDamage;

        PlayerData.playerDmgPoints += damage;
        Debug.Log("Direct attack: " + damage);
    }

    private void AttackSideSlots(GameObject attackerGO, List<HabitatSlotManager> slots, int index)
    {
        if (index > 0)
            AttackSingle(attackerGO, slots[index - 1]);

        if (index < slots.Count - 1)
            AttackSingle(attackerGO, slots[index + 1]);
    }

    private void AttackSingle(GameObject attackerGO, HabitatSlotManager slot)
    {
        if (!slot.HasCreature()) 
        { 
            return; 
        }

        var defender = slot.GetCreature();

        var atk = attackerGO.GetComponent<CardDisplay>();
        var def = defender.GetComponent<CardDisplay>();

        def.currentHealth -= atk.currentDamage;
        atk.currentHealth -= def.currentDamage;
    }

    private void DamageAllEnemies(List<HabitatSlotManager> slots, int dmg)
    {
        foreach (var slot in slots)
        {
            if (slot.HasCreature())
            {
                var display = slot.GetCreature().GetComponent<CardDisplay>();
                display.currentHealth -= dmg;
            }
        }
    }

    /*static void ShuffleEnemy(List<HabitatSlotManager> slots, int index)
    {
        int newIndex = UnityEngine.Random.Range(0, slots.Count);

        if (slots[index].HasCreature() || slots[newIndex].HasCreature())
        {
            var temp = slots[index].GetCreature();

            slots[index].cardsOnBoard[0] = slots[newIndex].GetCreature();
            slots[newIndex].cardsOnBoard[0] = temp;
        }
    }*/

    private void HealFromDamage(CardDisplay display)
    {
        int maxHealth = ((CreatureCard)display.cardData).health;

        display.currentHealth += display.currentDamage;
        display.currentHealth = Mathf.Min(display.currentHealth, maxHealth);
    }

    private void ApplyVengeful(int damage, bool isPlayerCard)
    {
        if (isPlayerCard)
        {
            PlayerData.enemyDmgPoints += damage;
        }
        else
        {
            PlayerData.playerDmgPoints += damage;
        }

        Debug.Log("Vengeful damage: " + damage);
    }

    private void HandleImmortal(GameObject card, CardDisplay display)
    {
        int refund = Mathf.Max(1, display.cardData.cost);
        PlayerData.playerDroplets += refund;

        Debug.Log("Immortal triggered");
    }

    private void ApplySchool(List<GameObject> allCreatures)
    {
        foreach (var c in allCreatures)
        {
            if (c == null) continue;

            var display = c.GetComponent<CardDisplay>();
            if (display == null) continue;

            var data = display.cardData as CreatureCard;

            if (data != null)
            {
                display.currentDamage = data.damage;
                display.currentHealth = Mathf.Min(display.currentHealth, data.health);
            }
        }

        int count = allCreatures.Count(c => //Controlla quali carte in tutte le carte sono Carte Creature e tra queste quali hanno l'abilità School
        {
            var data = c.GetComponent<CardDisplay>().cardData as CreatureCard;
            return data != null && data.abilityType.Contains(CreatureCard.AbilityType.School);
        });

        foreach (var c in allCreatures)
        {
            CardDisplay display = c.GetComponent<CardDisplay>();
            CreatureCard data = display.cardData as CreatureCard;

            if (data == null) continue;

            if (data != null && data.abilityType.Contains(CreatureCard.AbilityType.School))
            {
                display.currentDamage += count;
                display.currentHealth += count;
            }
        }
    }
}

