using DG.Tweening;

using Morpeh;

using UnityEngine;
using Unity.IL2CPP.CompilerServices;

using UnityEngine.UI;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(MenuWindowsSystem))]
public sealed class MenuWindowsSystem : UpdateSystem {
    Filter _scrollRectFilter;
    Filter _windowsFilter;
    Filter _buttonFilter;

    const float WINDOW_SWITCHING_DURATION = 0.5f;

    public override void OnAwake() {
        _scrollRectFilter = World.Filter.With<MenuScrollComponent>();
        _windowsFilter = World.Filter.With<MenuWindowComponent>();
        _buttonFilter = World.Filter.With<MenuButtonComponent>();
    }

    public override void OnUpdate(float deltaTime) {
       SwitchWindowOnScrollDrag();
       SwitchWindowOnButtonClick();
    }

    void SwitchWindowOnScrollDrag() {
        foreach (var entity in _scrollRectFilter) {
            var scrollRectComponent = entity.GetComponent<MenuScrollComponent>();
            if (!scrollRectComponent.EndDragGlobal) 
                continue;
            
            var startPos = scrollRectComponent.ScrollRect.horizontalNormalizedPosition;
            var nearestPos = 1f;
            var nearestIdx = 0;
            foreach (var windowEntity in _windowsFilter) {
                var window = windowEntity.GetComponent<MenuWindowComponent>();
                var windowPos = window.Index / (float)(_windowsFilter.Length - 1);
                var deltaPosition = Mathf.Abs(startPos - windowPos);
                
                if (deltaPosition < nearestPos) {
                    nearestIdx = window.Index;
                    nearestPos = deltaPosition;
                }
            }

            var targetPos = nearestIdx / (float)(_windowsFilter.Length - 1);
            
            MoveScrollRect(scrollRectComponent.ScrollRect, targetPos);
            SwitchButtonsInteraction(nearestIdx);
        }
    }

    void MoveScrollRect(ScrollRect scrollRect, float horizontalPosition) {
        DOTween.Kill(scrollRect);
        DOTween.To(() => scrollRect.horizontalNormalizedPosition,
            x => scrollRect.horizontalNormalizedPosition = x,
            horizontalPosition, WINDOW_SWITCHING_DURATION).SetId(scrollRect);
    }
    
    void SwitchButtonsInteraction(int indexButtonToDisable) {
        foreach (var buttonEntity in _buttonFilter) {
            ref var button = ref buttonEntity.GetComponent<MenuButtonComponent>();
            button.Button.interactable = button.Index != indexButtonToDisable;
        }
    }

    void SwitchWindowOnButtonClick() {
        foreach (var windowEntity in _windowsFilter) {
            ref var window = ref windowEntity.GetComponent<MenuWindowComponent>();
            if (!window.OpenEvent)
                continue;
            
            foreach (var rectEntity in _scrollRectFilter) {
                var scrollComponent = rectEntity.GetComponent<MenuScrollComponent>();
                var pos = window.Index / ((float) _windowsFilter.Length - 1);
                
                MoveScrollRect(scrollComponent.ScrollRect, pos);
                SwitchButtonsInteraction(window.Index);
            }
        }
    }
}