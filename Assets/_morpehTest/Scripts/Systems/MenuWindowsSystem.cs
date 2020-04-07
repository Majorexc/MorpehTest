using DG.Tweening;

using Morpeh;

using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(MenuWindowsSystem))]
public sealed class MenuWindowsSystem : UpdateSystem {
    Filter _scrollRectFilter;
    Filter _windowsFilter;
    Filter _buttonFilter;

    public override void OnAwake() {
        _scrollRectFilter = World.Filter.With<MenuScrollComponent>();
        _windowsFilter = World.Filter.With<MenuWindowComponent>();
        _buttonFilter = World.Filter.With<MenuButtonComponent>();
    }

    public override void OnUpdate(float deltaTime) {
        foreach (var entity in _scrollRectFilter) {
            var scrollRectComponent = entity.GetComponent<MenuScrollComponent>();
            if (scrollRectComponent.EndDragGlobal) {
                var rectPos = scrollRectComponent.ScrollRect.horizontalNormalizedPosition;
                var nearestPos = 1f;
                var nearestId = 0;
                foreach (var windowEntity in _windowsFilter) {
                    var window = windowEntity.GetComponent<MenuWindowComponent>();
                    var position = window.Index / ((float)_windowsFilter.Length - 1);
                    if (Mathf.Abs(rectPos - position) <= nearestPos) {
                        nearestId = window.Index;
                        nearestPos = Mathf.Abs(rectPos - position);
                    }
                }

                var pos = nearestId / ((float)_windowsFilter.Length - 1);
                DOTween.Kill(this);
                DOTween.To(() => scrollRectComponent.ScrollRect.horizontalNormalizedPosition,
                    x => scrollRectComponent.ScrollRect.horizontalNormalizedPosition = x,
                    pos, 0.5f).SetId(this);
                SwitchButton(nearestId);
            }
        }

        foreach (var windowEntity in _windowsFilter) {
            ref var window = ref windowEntity.GetComponent<MenuWindowComponent>();
            if (window.OpenEvent) {
                foreach (var rectEntity in _scrollRectFilter) {
                    var scrollRectComponent = rectEntity.GetComponent<MenuScrollComponent>();
                    var pos = window.Index / ((float) _windowsFilter.Length - 1);
                    DOTween.Kill(this);
                    DOTween.To(() => scrollRectComponent.ScrollRect.horizontalNormalizedPosition,
                        x => scrollRectComponent.ScrollRect.horizontalNormalizedPosition = x,
                        pos, 0.5f).SetId(this);
                    SwitchButton(window.Index);
                }
            }
        }
    }
    
    void SwitchButton(int index) {
        foreach (var buttonEntity in _buttonFilter) {
            var button = buttonEntity.GetComponent<MenuButtonComponent>();
            if (button.Index == index) {
                button.Button.interactable = false;
            } else {
                button.Button.interactable = true;
            }
        }
    }
}