using UnityEngine;

[CreateAssetMenu(fileName = "New Treasure Card", menuName = "Treasure Card")]
public class TreasureCard : ScriptableObject
{
    public string cardName;

    public int cost;
    public int health;

}
