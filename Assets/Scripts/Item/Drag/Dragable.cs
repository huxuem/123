using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Dragable : MonoBehaviour
{
    //子类需要实现开始拖拽/结束拖拽时的效果

    public void DragTo(Vector2 position)
    {
        //给出物体的水平位置，移动到该位置
        transform.position = new Vector3(position.x, position.y, transform.position.z);
    }

    public abstract void BeginDrag();

    public abstract void EndDrag();
}
