using UnityEngine;

[CreateAssetMenu(order = 0, fileName = "New armory item", menuName = "MorpehTest/New Armory")]
public class ArmoryItem : ScriptableObject {
    public string Name => _name;
    
    [SerializeField] string _name = "No name";
}
