using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Block
{
    [SerializeField] private float Force;
    private Rigidbody rb;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
    }

    public void OnHit(Vector3 Velocity)
    {
        hitRemain -= 1;
        Debug.Log("Boss get hit,hitRemain:"+hitRemain);

        rb.velocity = Velocity * Force;

        CheckDestory();
        changeMaterial();
    }
}
