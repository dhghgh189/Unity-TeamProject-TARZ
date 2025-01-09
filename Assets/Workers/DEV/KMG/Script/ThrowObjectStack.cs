using System.Collections.Generic;
using UnityEngine;

public class ThrowObjectStack : MonoBehaviour
{
    private Stack<ThrowObject> objectStack = new();
    public int Count => objectStack.Count;
    public Transform stackTransform;
    public void Push(ThrowObject tobj)
    {
        objectStack.Push(tobj);
    }

    public ThrowObject Pop()
    {
        ThrowObject throwObject = objectStack.Pop();
        throwObject.transform.parent = Camera.main.transform;
        return throwObject;
    }

    public void Clear()
    {
        foreach (Transform item in stackTransform)
        {
            Debug.Log(item);
            Destroy(item.gameObject);
        }
        objectStack.Clear();
    }
}
