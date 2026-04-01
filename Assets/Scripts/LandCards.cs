using System.Collections.Generic;
using UnityEngine;

namespace CardManager
{

    [CreateAssetMenu(fileName = "New Land Card", menuName = "Land Card")]
    public class LandCard : CardData
    {
        public int damage;
        public int health;

        public List<LandCardAbility> lCardAbility;
        public enum LandCardAbility
        {
            LightHouse,

            FishingBoat,

            Reef,

            Submarine,

            UnderwaterVulcano,

            Shipwreck,

            SubmergedTreasure,

            Atlantis,

            School,

            Iceberg,

        }
    }
}
