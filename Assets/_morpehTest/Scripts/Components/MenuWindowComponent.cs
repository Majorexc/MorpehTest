using Morpeh;
using Morpeh.Globals;

using UnityEngine;
using Unity.IL2CPP.CompilerServices;

[Il2CppSetOption(Option.NullChecks, false)]
[Il2CppSetOption(Option.ArrayBoundsChecks, false)]
[Il2CppSetOption(Option.DivideByZeroChecks, false)]
[System.Serializable]
public struct MenuWindowComponent : IComponent {
    public GlobalEvent OpenEvent;
    public int Index;
}