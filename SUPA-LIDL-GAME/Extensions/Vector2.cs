using Godot;
using System;

public static class Vector2Extension
{
    public static Vector2 FromPolar(float radius, float angle)
    {
        return new Vector2(radius * Mathf.Cos(angle),
                radius * Mathf.Sin(angle));
    }
}

