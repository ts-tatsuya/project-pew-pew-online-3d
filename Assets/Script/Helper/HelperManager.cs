using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class HelperManager
{

    public static Dictionary<int, bool> RoomPlayerReadyCast(object objectToCast)
    {
        Dictionary<int, bool> result;
        string jsonString = objectToCast.ToString();
        Debug.Log(jsonString);
        result = JsonUtility.FromJson<Dictionary<int, bool>>(jsonString);
        return result;
    }

}
