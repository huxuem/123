using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hitable : MonoBehaviour
{
    public bool IsStatic;

    [SerializeField] protected int hitRemain;
    public abstract void OnHit(Vector3 curSpeed, int SpeedLevel, Vector3 normal, out Vector3 SpeedOutput);
}
