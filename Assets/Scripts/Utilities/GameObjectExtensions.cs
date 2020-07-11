using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public static class GameObjectExtensions
{
    public static GameObject FindChildObject(this GameObject parent, string childName)
    {
        return parent.GetComponentsInChildren<Transform>()
            .Select(x => x.gameObject)
            .Where(x => x.name.Equals(childName))
            .FirstOrDefault();
    }

    public static IEnumerable<GameObject> FindChildObjects(this GameObject parent, string childName)
    {
        return parent.GetComponentsInChildren<Transform>()
            .Select(x => x.gameObject)
            .Where(x => x.name.Equals(childName));
    }
}
