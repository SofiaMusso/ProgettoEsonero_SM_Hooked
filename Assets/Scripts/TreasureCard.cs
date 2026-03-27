using System.Collections.Generic;
using UnityEngine;

namespace CardManager
{
    [CreateAssetMenu(fileName = "New Treasure Card", menuName = "Treasure Card")]
    public class TreasureCard : CardData
    {

        public List<TreasureCardAbility> tCardAbility;

        public enum TreasureCardAbility
        {
            miao,
        }
    }

}
