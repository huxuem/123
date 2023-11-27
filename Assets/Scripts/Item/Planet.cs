using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Planet : MonoBehaviour, Iinteractable
{
    public Action<Planet> E_PlanetClick;
    public Action<Planet> E_PlanetRelease;

    public float RangePull = 10;
    public float RangePush = 1;

    public float Force = 1;

    public void OnClicked()
    {
        //Debug.Log("Click Planet");
        E_PlanetClick?.Invoke(this);
    }

    public void OnRelease()
    {
        E_PlanetRelease?.Invoke(this);
    }

    public void OnEnlarge(float Ratio)
    {
        transform.localScale = transform.localScale * Ratio;
        RangePush *= Ratio;
        RangePull *= Ratio;
        Force *= Ratio;
    }

    public void OnDiminish(float Ratio)
    {
        transform.localScale = transform.localScale / Ratio;
        RangePush /= Ratio;
        RangePull /= Ratio;
        //�Ͳ�����Сʱ�����������ˡ���ô˵��Ҳ��������һ������
        //Force /= Ratio;
    }
}
