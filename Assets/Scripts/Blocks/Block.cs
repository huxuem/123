using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Hitable, Iinteractable
{
    [SerializeField] List<Material> mat_hit;

    private Renderer renderer;


    protected virtual void Start()
    {
        renderer = gameObject.GetComponent<Renderer>();
        //Debug.Log("Start,:" + hitRemain);
    }


    public override void OnHit(Vector3 curVelo, int SpeedLevel, float CometScale, Vector3 normal, out Vector3 SpeedOutput)
    {
        //计算出减速/反射之后的速度，并用SpeedOutput返回
        switch (SpeedLevel)
        {
            case 2:
                //当速度第二档时，方块耐久-1，无耐久则打穿，有耐久则反弹（之后要改成减对应耐久的）

                if (IsStatic)
                {
                    DecSpeedLevel(SpeedLevel, curVelo, CometScale, 3);
                }
                //速度等级为2时，撞到的方块直接被摧毁
                hitRemain = 0;
                //播放音频
                AudioManager.instance.HitBlockAudio();

                //减速
                SpeedOutput = DecSpeedLevel(SpeedLevel, curVelo, CometScale, 1);
                break;


            case 1:
                //Debug.Log("Collide level 1");
                //做反射运算，得到反射后的选择向量。OnHit可能要写在后面，要不getcontact已经没了
                SpeedOutput = Vector3.Reflect(curVelo, normal);
                hitRemain -= 1;
                //播放音频
                AudioManager.instance.HitBlockAudio();
                //speedlevel为1级时不减速，这样就可以保证撞出来了

                break;

            case 0:

                //当速度为第0档时，方块耐久不变化，反弹
                //做反射运算，得到反射后的选择向量
                SpeedOutput = Vector3.Reflect(curVelo, normal);

                break;
            default:
                SpeedOutput = curVelo;
                break;

        }


        CheckDestory();

        if(hitRemain > 0)
        {
            changeMaterial();
        }
    }

    #region 道具
    //public void OnEnlarge(float Ratio)
    //{
    //    transform.localScale = transform.localScale * Ratio;
    //}
    //public void OnDiminish(float Ratio)
    //{
    //    transform.localScale = transform.localScale / Ratio;
    //}

    #endregion

    protected void changeMaterial()
    {
        //Debug.Log("HitRemain:" + hitRemain+", name:"+transform.gameObject.GetInstanceID());
        renderer.material = mat_hit[hitRemain%2]; 
    }

    protected virtual void CheckDestory()
    {
        //Debug.Log("Destory,id:"+ transform.gameObject.GetInstanceID());
        if (hitRemain <= 0)
        {
            Destroy(gameObject);
        }
    }

    private Vector3 DecSpeedLevel(int SpeedLevel, Vector3 CurVelocity, float CometScale, int level)
    {
        if (SpeedLevel == 1)
        {
            return CurVelocity.normalized * (CurVelocity.magnitude - 0.5f * level / CometScale);
        }
        else if (SpeedLevel == 2)
        {
            return CurVelocity.normalized * (CurVelocity.magnitude - 0.5f * level / CometScale);
        }
        else return CurVelocity;
    }
}
