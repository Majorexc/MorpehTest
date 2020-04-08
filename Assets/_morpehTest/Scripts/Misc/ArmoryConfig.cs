using UnityEngine;

[CreateAssetMenu(order = 0, fileName = "New armory config", menuName = "MorpehTest/New Armory config")]
public class ArmoryConfig : ScriptableObject {
    public ArmoryItem[] Items => _items;
    
    [SerializeField] ArmoryItem[] _items = default;
}