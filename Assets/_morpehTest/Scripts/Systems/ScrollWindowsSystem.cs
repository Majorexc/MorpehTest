using System.Linq;

using Morpeh;
using Morpeh.UI.Components;

using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[CreateAssetMenu(menuName = "ECS/Systems/" + nameof(ScrollWindowsSystem))]
public sealed class ScrollWindowsSystem : UpdateSystem {
    Filter _scrollRectFilter;
    Filter _windowsFilter;
    Filter _buttonFilter;

    public override void OnAwake() {
        _scrollRectFilter = World.Filter.With<MainScrollComponent>();
        _windowsFilter = World.Filter.With<MainWindowComponent>();
        _buttonFilter = World.Filter.With<MainButtonComponent>();
    }

    public override void OnUpdate(float deltaTime) {
        foreach (var entity in _scrollRectFilter) {
            ref var scrollRectComponent = ref entity.GetComponent<MainScrollComponent>();
            if (scrollRectComponent.EndDragGlobal) {
                var rectPos = scrollRectComponent.ScrollRect.horizontalNormalizedPosition;
                var nearestPos = 1f;
                var nearestId = 0;
                foreach (var windowEntity in _windowsFilter) {
                    var window = windowEntity.GetComponent<MainWindowComponent>();
                    var position = window.Index / ((float)_windowsFilter.Length - 1);
                    if (Mathf.Abs(rectPos - position) <= nearestPos) {
                        nearestId = window.Index;
                        nearestPos = Mathf.Abs(rectPos - position);
                    }
                }

                var pos = nearestId / ((float)_windowsFilter.Length - 1);
                scrollRectComponent.ScrollRect.horizontalNormalizedPosition = pos;
                SwitchButton(nearestId);
            }
        }

        foreach (var windowEntity in _windowsFilter) {
            ref var window = ref windowEntity.GetComponent<MainWindowComponent>();
            if (window.OpenEvent) {
                foreach (var rectEntity in _scrollRectFilter) {
                    ref var scrollRectComponent = ref rectEntity.GetComponent<MainScrollComponent>();
                    var pos = window.Index / ((float) _windowsFilter.Length - 1);
                    scrollRectComponent.ScrollRect.horizontalNormalizedPosition = pos;
                    SwitchButton(window.Index);
                }
            }
        }
    }

    void SwitchButton(int index) {
        foreach (var buttonEntity in _buttonFilter) {
            var button = buttonEntity.GetComponent<MainButtonComponent>();
            if (button.Index == index) {
                button.Button.interactable = false;
            } else {
                button.Button.interactable = true;
            }
        }
    }
}