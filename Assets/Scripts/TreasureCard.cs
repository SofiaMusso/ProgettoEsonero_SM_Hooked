using System.Collections.Generic;
using UnityEngine;

namespace CardManager
{
    [CreateAssetMenu(fileName = "New Treasure Card", menuName = "Treasure Card")]
    public class TreasureCard : ScriptableObject
    {
        public string cardName;

        public List<TreasureCardAbility> tCardAbility;

        public enum TreasureCardAbility
        {
            miao,
        }
    }

}
