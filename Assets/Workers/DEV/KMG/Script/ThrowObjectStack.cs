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
        if (objectStack.Count <= 0)
            return null;

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

    // ThrowObject의 Owner가 씬이 이동하면 파괴되므로 게임이 시작할 때마다 재설정 하기 위한 함수
    public void SetOwner(PlayerController owner)
    {
        foreach (ThrowObject tobj in objectStack)
        {
            tobj.Owner = owner;
        }
    }
}
