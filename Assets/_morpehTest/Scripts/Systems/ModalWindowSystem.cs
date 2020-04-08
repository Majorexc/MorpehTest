using DG.Tweening;

using Morpeh;
using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(ModalWindowSystem))]
public sealed class ModalWindowSystem : UpdateSystem {
    Filter _armoryItemsFilter;
    Filter _modalWindowFilter;

    const float FADING_DURATION = 0.5f;
    const string ITEM_EQUIPPED_TEXT = "Item equipped!";
    
    public override void OnAwake() {
        _armoryItemsFilter = World.Filter.With<ArmoryItemComponent>();
        _modalWindowFilter = World.Filter.With<ModalWindowComponent>();
        
        HideAll();
    }

    public override void OnUpdate(float deltaTime) {
        foreach (var armoryEntity in _armoryItemsFilter) {
            var item = armoryEntity.GetComponent<ArmoryItemComponent>();
            if (item.Event.IsPublished) {
                var obj = item.Event.BatchedChanges.Peek() as ArmoryItem;
                Show(ITEM_EQUIPPED_TEXT, obj.Name);
            }
        } 
        
        CheckAutoHide();
    }

    void CheckAutoHide() {
        foreach (var windowEntity in _modalWindowFilter) {
            ref var component = ref windowEntity.GetComponent<ModalWindowComponent>();
            if (component.IsShowing) {
                var isReadyToHide = Time.time - component.LastShowTimestamp > component.AutohideDuration;
                if (isReadyToHide)
                    HideComponent(ref component);
            }
        }
    }
    
    void HideComponent(ref ModalWindowComponent component) {
        component.CanvasGroup.DOKill();
        component.CanvasGroup.DOFade(0, FADING_DURATION);
        component.IsShowing = false;
    }
    
    void HideAll() {
        foreach (var windowEntity in _modalWindowFilter) {
            HideComponent(ref windowEntity.GetComponent<ModalWindowComponent>());
        }
    }

    void Show(string title, string description) {
        foreach (var windowEntity in _modalWindowFilter) {
            ref var component = ref windowEntity.GetComponent<ModalWindowComponent>();
            component.CanvasGroup.DOKill(true);
            component.CanvasGroup.DOFade(1, FADING_DURATION);
            component.Description.text = description;
            component.Title.text = title;
            component.LastShowTimestamp = Time.time;
            component.IsShowing = true;
        }
    }
}