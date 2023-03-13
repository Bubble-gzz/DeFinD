using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils
{
    /*Get the next positive integer starting from index i in string s*/
    static public int GetInt(string s, ref int i)
    {
        int res = 0;
        while (s[i] < '0' || s[i] > '9' && i < s.Length) i++;
        if (i >= s.Length) return 0;
        while (s[i] >= '0' && s[i] <= '9')
        {
            res = res * 10 + (s[i] - '0');
            i++;
            if (i >= s.Length) break;
        }
        return res;
    }
    static public bool GetKeyDown(KeyCode key)
    {
        if (Global.keyEnable == null) KeyInitialize();
        if (Input.GetKeyDown(key) && Global.keyEnable[key]) return true;
        return false;
    }
    static public void KeyInitialize()
    {
        Dictionary<KeyCode, bool> keyEnable = new Dictionary<KeyCode, bool>();
        foreach(KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
            if (!keyEnable.ContainsKey(key)) keyEnable.Add(key, true);
        Global.keyEnable = keyEnable;
    }
}
