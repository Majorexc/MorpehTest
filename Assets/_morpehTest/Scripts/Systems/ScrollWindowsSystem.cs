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

    public override void OnAwake() {
        _scrollRectFilter = World.Filter.With<MainScrollComponent>();
        _windowsFilter = World.Filter.With<WindowComponent>();
    }

    public override void OnUpdate(float deltaTime) {
        
        // if (_vector2Global) {
            // Debug.Log(_vector2Global.BatchedChanges.Peek());
        // }
        foreach (var entity in _scrollRectFilter) {
            ref var scrollRectComponent = ref entity.GetComponent<MainScrollComponent>();
            if (scrollRectComponent.EndDragGlobal) {
                var rectPos = scrollRectComponent.ScrollRect.horizontalNormalizedPosition;
                var nearestPos = 1f;
                var nearestId = 0f;
                for (int i = 0; i < _windowsFilter.Length; i++) {
                    var position = i / ((float)_windowsFilter.Length - 1);
                    if (Mathf.Abs(rectPos - position) <= nearestPos) {
                        nearestId = i;
                        nearestPos = Mathf.Abs(rectPos - position);
                    }
                }

                var pos = nearestId / ((float)_windowsFilter.Length - 1);
                scrollRectComponent.ScrollRect.horizontalNormalizedPosition = pos;
                // var orderedWindows = _windowsFilter.OrderBy((e)=>e.GetComponent<WindowComponent>().canvasGroup.tr)
                // var windows = _windowsFilter.Length;
                // var positionOnScroll = scrollRectComponent.ScrollRect.sc
                // scrollRectComponent.ScrollRect.
            }
            
            // scrollRectComponent.ScrollRect.
        }
        
    }
}