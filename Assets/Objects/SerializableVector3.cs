using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializableVector3
{
    float x, y, z;

    public SerializableVector3(Vector3 _vector)
    {
        this.x = _vector.x;
        this.y = _vector.y;
        this.z = _vector.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }

    public Vector2 ToVector2()
    {
        return new Vector2(x, y);
    }
}
