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

    public override void OnHit(Vector3 curVelo, int SpeedLevel, float CometScale, Vector3 normal, out Vector3 SpeedOutput)
    {
        //boss的onhit中没有用SpeedLevel
        AudioManager.instance.HitBossAudio();
        SpeedOutput = Vector3.Reflect(curVelo, normal);

        hitRemain -= 1;

        //受彗星撞击后的移动，需要乘本身的受力比例&&Comet的比例
        rb.velocity = curVelo * Force * CometScale;

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
            Instantiate(GameManager.instance.GetRandomBlock(), transform.position, Quaternion.identity);
        }
        if (hitRemain <= 0)
        {
            for (int i = 0; i < SpawnNum*2; i++)
            {
                Instantiate(GameManager.instance.GetRandomBlock(), transform.position, Quaternion.identity);
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
