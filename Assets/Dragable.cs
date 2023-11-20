using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragable : MonoBehaviour
{
    public void DragTo(Vector2 position)
    {
        //给出物体的水平位置，移动到该位置
        transform.position = new Vector3(position.x,position.y,transform.position.z);
    }


    //child中，2是卡面，0是预览，1是实体。初始是f,f,t

    public void BeginDrag()
    {
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void EndDrag()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);    
    }
}
