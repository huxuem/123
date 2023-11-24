using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag_Thing : Dragable
{
    //child�У�2�ǿ��棬0��Ԥ����1��ʵ�塣��ʼ��f,f,t
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
