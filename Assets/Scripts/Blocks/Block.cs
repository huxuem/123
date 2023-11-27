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
        //���������/����֮����ٶȣ�����SpeedOutput����
        switch (SpeedLevel)
        {
            case 2:
                //���ٶȵڶ���ʱ�������;�-1�����;���򴩣����;��򷴵���֮��Ҫ�ĳɼ���Ӧ�;õģ�

                if (IsStatic)
                {
                    DecSpeedLevel(SpeedLevel, curVelo, CometScale, 3);
                }
                //�ٶȵȼ�Ϊ2ʱ��ײ���ķ���ֱ�ӱ��ݻ�
                hitRemain = 0;
                //������Ƶ
                AudioManager.instance.HitBlockAudio();

                //����
                SpeedOutput = DecSpeedLevel(SpeedLevel, curVelo, CometScale, 1);
                break;


            case 1:
                //Debug.Log("Collide level 1");
                //���������㣬�õ�������ѡ��������OnHit����Ҫд�ں��棬Ҫ��getcontact�Ѿ�û��
                SpeedOutput = Vector3.Reflect(curVelo, normal);
                hitRemain -= 1;
                //������Ƶ
                AudioManager.instance.HitBlockAudio();
                //speedlevelΪ1��ʱ�����٣������Ϳ��Ա�֤ײ������

                break;

            case 0:

                //���ٶ�Ϊ��0��ʱ�������;ò��仯������
                //���������㣬�õ�������ѡ������
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

    #region ����
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
