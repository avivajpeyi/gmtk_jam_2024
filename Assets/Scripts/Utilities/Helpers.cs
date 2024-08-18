using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Object = UnityEngine.Object;
using UnityEditor;

/// <summary>
/// A static class for general helpful methods
/// </summary>
public static class Helpers
{
    private static Camera _camera;

    public static Camera Camera
    {
        get
        {
            if (_camera==null) _camera = Camera.main;
            return _camera;
        }
    }


    private static readonly Dictionary<float, WaitForSeconds> WaitDictonary =
        new Dictionary<float, WaitForSeconds>();

    public static WaitForSeconds GetWait(float time)
    {
        if (WaitDictonary.TryGetValue(time, out var wait))
            return wait;

        WaitDictonary.Add(time, new WaitForSeconds(time));
        return WaitDictonary[time];
    }


    public static void DestroyChildren(this Transform t)
    {
        foreach (Transform child in t) Object.Destroy(child.gameObject);
    }

    public static bool AllTransformsExist(List<Transform> transforms)
    {
        foreach (Transform t in transforms)
        {
            if (t == null)
                return false;
        }

        return true;
    }

    public static string RepeatStr(this string input, int count)
    {
        if (string.IsNullOrEmpty(input) || count <= 0)
            return "";

        var builder = new StringBuilder(input.Length * count);

        for(var i = 0; i < count; i++) builder.Append(input);

        return builder.ToString();
    }


    public static List<T> RepeatedDefaultInstance<T>(int count)
    {
        List<T> ret = new List<T>(count);
        for (var i = 0; i < count; i++)
        {
            ret.Add((T)Activator.CreateInstance(typeof(T)));
        }
        return ret;
    }





}
