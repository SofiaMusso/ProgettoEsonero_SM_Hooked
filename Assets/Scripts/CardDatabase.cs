using System.Collections.Generic;
using UnityEngine;

namespace CardManager
{
    [CreateAssetMenu(menuName = "Card Database")]
    public class CardDatabase: ScriptableObject
    {
        public List<CardData> allCards;
    }

}
