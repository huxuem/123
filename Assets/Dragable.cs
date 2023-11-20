using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragable : MonoBehaviour
{
    public void DragTo(Vector2 position)
    {
        //���������ˮƽλ�ã��ƶ�����λ��
        transform.position = new Vector3(position.x,position.y,transform.position.z);
    }


    //child�У�2�ǿ��棬0��Ԥ����1��ʵ�塣��ʼ��f,f,t

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
