using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Planet : MonoBehaviour
{
    public Action<Planet> E_PlanetClick;
    public Action<Planet> E_PlanetRelease;

    public float RangePull = 10;
    public float RangePush = 1;

    public void OnClicked()
    {
        //Debug.Log("Click Planet");
        E_PlanetClick?.Invoke(this);
    }

    public void OnRelease()
    {
        E_PlanetRelease?.Invoke(this);
    }

    //protected virtual void OnTriggerEnter(Collider other)
    //{
    //    //Debug.Log("Hit Block");
    //    if (other.gameObject.CompareTag("Comet"))
    //    {
    //        Debug.Log("Planet rebound");
    //        other.GetComponent<Comet>().Rebound(other.transform.position - transform.position);
    //    }
    //}
}
