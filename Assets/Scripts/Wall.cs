using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private float SpeedDecRatio = 0;
    
    public float GetDecRatio() { return SpeedDecRatio + 1; }
    
}
