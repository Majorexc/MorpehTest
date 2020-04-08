using Morpeh;
using Morpeh.Globals;

using TMPro;

using UnityEngine;
using Unity.IL2CPP.CompilerServices;

using UnityEngine.UI;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(ArmorySystem))]
public sealed class ArmorySystem : UpdateSystem {
    [SerializeField] ArmoryConfig _config = default;
    [SerializeField] GameObject _armoryItemPrefab = default;

    Filter _containerFilter;
    
    public override void OnAwake() {
        Debug.Assert(_config != null && _config.Items.Length > 0, $"[{GetType()}] {nameof(ArmoryConfig)} not assigned or empty");

        _containerFilter = World.Filter.With<ArmoryContainerComponent>();

        foreach (var containerEntity in _containerFilter) {
            var parent = containerEntity.GetComponent<ArmoryContainerComponent>().ContainerParent;
            foreach (var item in _config.Items) {
               CreateNewItem(item, parent);
            }
        }
    }

    void CreateNewItem(ArmoryItem item, Transform parent) {
        var itemEntity = World.CreateEntity();
        
        ref var itemComponent = ref itemEntity.AddComponent<ArmoryItemComponent>();
        var armoryGlobal = CreateInstance<GlobalEventObject>();
        itemComponent.Event = armoryGlobal;
        
        var visual = Instantiate(_armoryItemPrefab, parent);
        visual.GetComponentInChildren<TextMeshProUGUI>().text = item.Name;
        visual.GetComponent<Button>().onClick.AddListener(() => armoryGlobal.Publish(item));
    }

    public override void OnUpdate(float deltaTime) {
        // Handle changes
    }
}