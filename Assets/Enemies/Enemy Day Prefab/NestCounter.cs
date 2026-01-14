using System.Collections.Generic;

public static class NestCounter
{
    private static HashSet<EnemyNest> activeNests = new HashSet<EnemyNest>();

    public static int ActiveNestCount => activeNests.Count;

    public static void RegisterNest(EnemyNest nest)
    {
        activeNests.Add(nest);
    }

    public static void UnregisterNest(EnemyNest nest)
    {
        activeNests.Remove(nest);
    }

    public static void Clear()
    {
        activeNests.Clear();
    }
}
