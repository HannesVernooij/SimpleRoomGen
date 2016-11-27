using UnityEngine;

public class Floor
{
    GameObject _object;

    public Floor(Vector2 position, Transform parent)
    {
        _object = GameObject.CreatePrimitive(PrimitiveType.Quad);
        _object.transform.SetParent(parent);
        _object.transform.position = position;
    }
}