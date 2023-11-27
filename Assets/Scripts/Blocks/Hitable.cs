using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hitable : MonoBehaviour, Iinteractable
{
    public bool IsStatic;

    [SerializeField] protected int hitRemain;
    public abstract void OnHit(Vector3 curSpeed, int SpeedLevel, float CometScale, Vector3 normal, out Vector3 SpeedOutput);

    public void OnEnlarge(float Ratio)
    {
        transform.localScale = transform.localScale * Ratio;
    }
    public void OnDiminish(float Ratio)
    {
        transform.localScale = transform.localScale / Ratio;
    }
}
