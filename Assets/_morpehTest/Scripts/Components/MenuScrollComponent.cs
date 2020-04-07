using Morpeh;
using Morpeh.Globals;

using UnityEngine;
using Unity.IL2CPP.CompilerServices;

using UnityEngine.UI;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[System.Serializable]
public struct MenuScrollComponent : IComponent {
    public ScrollRect ScrollRect;
    public GlobalEvent EndDragGlobal;
}