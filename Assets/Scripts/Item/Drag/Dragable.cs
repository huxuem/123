using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Dragable : MonoBehaviour
{
    //������Ҫʵ�ֿ�ʼ��ק/������קʱ��Ч��

    public void DragTo(Vector2 position)
    {
        //���������ˮƽλ�ã��ƶ�����λ��
        transform.position = new Vector3(position.x, position.y, transform.position.z);
    }

    public abstract void BeginDrag();

    public abstract void EndDrag();
}
