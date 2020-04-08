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
    Filter _armoryItemsFilter;
    
    public override void OnAwake() {
        Debug.Assert(_config != null && _config.Items.Length > 0, $"[{GetType()}] {nameof(ArmoryConfig)} not assigned or empty");

        _containerFilter = World.Filter.With<ArmoryContainerComponent>();
        _armoryItemsFilter = World.Filter.With<ArmoryItemComponent>();


        foreach (var containerEntity in _containerFilter) {
            var parent = containerEntity.GetComponent<ArmoryContainerComponent>().ContainerParent;
            foreach (var item in _config.Items) {
                var visual = Instantiate(_armoryItemPrefab, parent);
                var itemEntity = World.CreateEntity();
                ref var itemComponent = ref itemEntity.AddComponent<ArmoryItemComponent>();
                var armoryGlobal = CreateInstance<GlobalEventObject>();
                // armoryGlobal.Value = Instantiate(item);
                itemComponent.Event = armoryGlobal;
                visual.GetComponentInChildren<TextMeshProUGUI>().text = item.Name;
                visual.GetComponent<Button>().onClick.AddListener(()=>armoryGlobal.Publish(item));
            }
        }
    }

    public override void OnUpdate(float deltaTime) {
        foreach (var armoryEntity in _armoryItemsFilter) {
            var item = armoryEntity.GetComponent<ArmoryItemComponent>();
            if (item.Event.IsPublished) {
                Debug.Log(item.Event.BatchedChanges.Peek());
            }
        }   
    }
}