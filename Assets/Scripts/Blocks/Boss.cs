using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Block
{
    [SerializeField] private float Force;
    [SerializeField] private Block OnHitBlock;
    [SerializeField] private int SpawnNum = 4;
    [SerializeField] private bool isEnd = false;
    private Rigidbody rb;

    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
    }

    public override void OnHit(Vector3 curVelo, int SpeedLevel, Vector3 normal, out Vector3 SpeedOutput)
    {
        //boss的onhit中没有用SpeedLevel
        AudioManager.instance.HitBossAudio();
        SpeedOutput = Vector3.Reflect(curVelo, normal);

        hitRemain -= 1;

        rb.velocity = curVelo * Force;

        CheckDestory();
        if (hitRemain > 0)
        {
            changeMaterial();
        }
    }

    protected override void CheckDestory()
    {
        for (int i = 0; i < SpawnNum; i++)
        {
            Instantiate(OnHitBlock, transform.position, Quaternion.identity);
        }
        if (hitRemain <= 0)
        {
            for (int i = 0; i < SpawnNum*2; i++)
            {
                Instantiate(OnHitBlock, transform.position, Quaternion.identity);
            }
            GameManager.instance.CheckTeleportAppear();
            if (isEnd)
            {
                GameManager.instance.SetEndText(true);
            }
            Destroy(gameObject);
        }
    }
}
