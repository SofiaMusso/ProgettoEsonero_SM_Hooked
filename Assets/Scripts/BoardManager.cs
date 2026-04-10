using System.Collections;
using System.Collections.Generic;
using CardManager;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public List<HabitatSlotManager> playerSlots;
    public List<HabitatSlotManager> enemySlots;

    public AudioSource hitSfx;

    public IEnumerator ResolveCombat(List<HabitatSlotManager> attackerSlots, List<HabitatSlotManager> defenderSlots)
    {
        List<GameObject> attackerCards = new List<GameObject>();

        for (int i = 0; i < attackerSlots.Count; i++)
        {
            GameObject card = attackerSlots[i].GetCreature();

            if (card != null)
            {
                attackerCards.Add(card);
            }
        }

        AbilitySystem.Instance.ApplyPassive(attackerCards);

        for (int i = 0; i < attackerSlots.Count; i++)
        {
            var attackerSlot = attackerSlots[i];
            var defenderSlot = defenderSlots[i];

            GameObject attacker = attackerSlot.GetCreature();

            if (attacker == null)
                continue;

            CardDisplay cardDisplay = attacker.GetComponent<CardDisplay>();

            if (cardDisplay == null)
                continue;

            CreatureCard creature = cardDisplay.cardData as CreatureCard;

            if (creature == null)
                continue;


            if (attackerSlot.HasCreature() && defenderSlot.HasCreature())
            {
                AbilitySystem.Instance.OnAttack(attacker, enemySlots, i);

                if (creature.abilityType.Contains(CreatureCard.AbilityType.DoubleImpact) || creature.abilityType.Contains(CreatureCard.AbilityType.Jump))
                {
                    continue;
                }
                else
                {
                    yield return StartCoroutine(AttackAnimation(attacker, defenderSlot.GetCreature()));
                    yield return ResolveFight(attacker, defenderSlot.GetCreature(), attackerSlot, defenderSlot);
                }
            }
            else if (attackerSlot.HasCreature())
            {
                if (creature.abilityType.Contains(CreatureCard.AbilityType.DoubleImpact))
                {
                    yield return StartCoroutine(AttackAnimation(attacker, defenderSlot.GetCreature()));
                    AbilitySystem.Instance.OnAttack(attacker, enemySlots, i);
                }
                else
                {
                    yield return StartCoroutine(AttackAnimation(attacker, defenderSlot.GetCreature()));
                    yield return DirectAttack(attacker);
                }
            }
        }


    }

    IEnumerator ResolveFight(GameObject attacker, GameObject defender, HabitatSlotManager atkSlot, HabitatSlotManager defSlot)
    {
        var atkDisplay = attacker.GetComponent<CardDisplay>();
        var defDisplay = defender.GetComponent<CardDisplay>();

        var atkData = atkDisplay.cardData as CreatureCard;
        var defData = defDisplay.cardData as CreatureCard;

        int atkDamage = atkDisplay.currentDamage;
        int defDamage = defDisplay.currentDamage;

        int atkHealth = atkDisplay.currentHealth;
        int defHealth = defDisplay.currentHealth;


        if (defData != null && AbilitySystem.Instance.CanBeAttacked(defData))
        {
            defHealth -= atkDamage;
        }

        atkDisplay.currentDamage = atkDamage;
        defDisplay.currentDamage = defDamage;

        atkDisplay.currentHealth = atkHealth;
        defDisplay.currentHealth = defHealth;

        AbilitySystem.Instance.OnHit(attacker, defender);

        Debug.Log(atkData.name + ": " + atkHealth + ", " + atkDamage);
        Debug.Log(defData.name + ": " + defHealth + ", " + defDamage);

        yield return new WaitForSeconds(0.5f);

        if (defHealth <= 0)
        {
            AbilitySystem.Instance.OnDeath(defender, TurnManager.Instance.playerCardIsAttacker);
            Destroy(defender);
            defSlot.creatureCardIsPlaced = false;
        }

        if (atkHealth <= 0)
        {
            AbilitySystem.Instance.OnDeath(attacker, TurnManager.Instance.playerCardIsAttacker);
            Destroy(attacker);
            atkSlot.creatureCardIsPlaced = false;
        }

        UpdateCardUI(attacker);
        UpdateCardUI(defender);
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

        if (TurnManager.Instance.playerCardIsAttacker)
        {
            PlayerData.playerDmgPoints += atkDamage;
            Debug.Log("Player Damage: " + PlayerData.playerDmgPoints);
        }
        else
        {
            PlayerData.enemyDmgPoints += atkDamage;
            Debug.Log("Enemy Damage: " + PlayerData.playerDmgPoints);
        }

            yield return new WaitForSeconds(0.3f);
    }

    IEnumerator AttackAnimation(GameObject attacker, GameObject defender, float duration = 0.2f, float offset = 1f)
    {
        Vector3 originalPos = attacker.transform.position;
        Vector3 targetPos;

        attacker.transform.SetAsLastSibling();

        if (defender == null)
        {
            if (TurnManager.Instance.currentTurn == TurnManager.TurnOwner.Player)
            {
                targetPos = (attacker.transform.position + (Vector3.up * 200f)) - (Vector3.left * offset);
                hitSfx.Play();
            }
            else
            {
                targetPos = (attacker.transform.position + (Vector3.down * 200f)) - (Vector3.left * offset);
                hitSfx.Play();
            }
        }
        else
        {
            targetPos = defender.transform.position + (Vector3.left * offset); // piccola distanza prima dell'impatto
            hitSfx.Play();
        }

        float elapsed = 0f;

        while (elapsed < duration)
        {
            attacker.transform.position = Vector3.Lerp(originalPos, targetPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        attacker.transform.position = targetPos;

        // Qui puoi attivare un effetto visivo
        //SpawnHitEffect(defender.transform.position);

        // Ritorno alla posizione originale
        elapsed = 0f;
        while (elapsed < duration)
        {
            attacker.transform.position = Vector3.Lerp(targetPos, originalPos, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        attacker.transform.position = originalPos;
    }

    /*void SpawnHitEffect(Vector3 position)
    {
        GameObject effect = Instantiate(hitEffectPrefab, position, Quaternion.identity);
        Destroy(effect, 0.5f); // autodistruzione dopo 0.5s
    }*/
}
