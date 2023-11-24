using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag_Thing : Dragable
{
    //child中，2是卡面，0是预览，1是实体。初始是f,f,t
    public override void BeginDrag()
    {
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public override void EndDrag()
    {
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
    }
}
