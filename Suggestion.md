# Bugs
There's a bug where ShowHint will not work after a failed swap. Because the hint cache was cleared after the swap.

# Suggestion
- Hardcoding file paths (Constants.cs) is not scalable. The files might be moved and the paths will be invalidated (without us knowing until the game crashes.) Use prefabs instead.
- There's a lot of circular references. This is considered a bad pattern. Consider using top-down relations only and event bubbling instead.
- UI's callbacks should be connected by the Editor instead of by code. It greatly reduces circular references.
- Duplicate prefabs and enums. All itemNormal00x prefabs are the same. Only differ in sprite's texture. Should just use one prefab and change the texture instead.
- Using Resources.Load too much. It loads the resources at runtime everytime it's called and leads to high disk access. We should use prefabs instead.