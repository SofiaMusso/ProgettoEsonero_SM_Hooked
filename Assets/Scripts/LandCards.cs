using System.Collections.Generic;
using UnityEngine;

namespace CardManager
{

    [CreateAssetMenu(fileName = "New Land Card", menuName = "Land Card")]
    public class LandCard : ScriptableObject
    {
        public string cardName;

        public int cost;
        public int damage;
        public int health;

        public List<LandCardAbility> lCardAbility;
        public enum LandCardAbility
        {
            miao,
        }
    }
}
