using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Iinteractable
{
    //��Ϊ��ͬ����Ĵ���һ��������Ҫ�������Ļ��Ʒ�Χ��,���Ա���ʵ��OnEnLarge
    public void OnEnlarge(float Ratio);
    public void OnDiminish(float Ratio);
}
