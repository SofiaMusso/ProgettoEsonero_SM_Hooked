using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CardManager
{
    [CreateAssetMenu(fileName = "New Creature Card", menuName = "Creature Card")]

    public class CreatureCard : CardData
    {
        public List<CardType> cardType;

        public int damage;
        public int health;

        public List<AbilityType> abilityType;
        public enum CardType
        {
            Ocean,

            Beach,

            CoralReef,

            Abyss

        }

        public enum AbilityType
        {
            Burrower,

            DoubleImpact,

            Parasite,

            Immortal,

            School,

            Aggressive,

            Jump,

            Electrified,

            Move,

            Vengeful,

            NoAbility
        }
    }

}

