using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
//using static UnityEditor.PlayerSettings;

public class Comet : MonoBehaviour, Iinteractable
{
    public Vector3 CurVelocity = Vector3.zero;
    public float acc = 1;


    [SerializeField] private float accMinRange = 5;
    //[SerializeField] private float VelDecRatio = 1.5f;
    [SerializeField] private float RocketRotateSpeed = 5f;
    [SerializeField] private float RotateSpeed = 1f;
    [SerializeField] private float TurnRatio = 1f;
    //������С�õģ�������ײ���Ķ���֪����ǰComet��С
    [SerializeField] private float CometScale = 1f;


    [SerializeField] private List<float> SpeedThreshold = new List<float> {2,4};
    [SerializeField] private GameObject trail_1;
    [SerializeField] private GameObject trail_2;

    private Vector3 moveDir;
    private Vector3 TmpVelocity;
    [SerializeField]private Planet curTarget;
    private Rigidbody rb;
    private float minx,maxx,miny,maxy;
    private bool hasTarget = false;
    private bool isRotating = false;
    private bool isTeleport = false;


    public void Teleport(Vector3 pos, Vector3 velocity,bool isHorizontal)
    {
        Debug.Log("pos:" + pos + ", velocity;" + velocity+", self:"+this);
        transform.position= pos;
        CurVelocity = velocity;
        StartCoroutine(TeleportCoroutine(isHorizontal));
    }

    IEnumerator TeleportCoroutine(bool isHorizontal)
    {
        curTarget = null;
        //InitPlanetList();

        //�����ң�������ƫ��һ��λ����Ϊprotal��Ҫ�������档֮��protalȫ�����ڱ߽�����1.5��λ�ĵط���
        if (isHorizontal)
        {
            if(transform.position.x < 0)
            {
                transform.position = new Vector3(transform.position.x * -1 -1, transform.position.y, 0f);
            }
            else transform.position = new Vector3(transform.position.x * -1 +1, transform.position.y, 0f);
        }

        else
        {
            if (transform.position.y < 0)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y * -1 -1, 0f);
            }
            else transform.position = new Vector3(transform.position.x, transform.position.y * -1 +1, 0f);
        }

        //���ڿ���β������
        isTeleport = true;
        yield return new WaitForSeconds(0.4f);
        isTeleport = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        InitPlanetList();
        InitBoarder();
        rb = GetComponent<Rigidbody>();
    }

    private void InitPlanetList()
    {
       // Debug.Log("MouseManager:"+MouseManager.instance);
        foreach (GameObject planet in MouseManager.instance.PlanetList)
        {
            //Debug.Log("Planet:"+planet);
            planet.GetComponent<Planet>().E_PlanetClick += PullToPlanet;
            planet.GetComponent<Planet>().E_PlanetRelease += RealseFromPlanet;
        }
    }
    private void InitBoarder()
    {
        //��ȡ���ֵ��Сֵ
        //����xһ��ʼ�Ƿ��Űڵ�
        maxx = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).x;
        minx = Camera.main.ViewportToWorldPoint(new Vector2(1, 0)).x;

        miny = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).y;
        maxy = Camera.main.ViewportToWorldPoint(new Vector2(0, 1)).y;

        //Debug.Log(minx+" "+maxx+" "+miny+" "+maxy);
    }

    //�������ʱ������Comet����mousemgr�����
    public void SelfSplit()
    {

        //��ȡComet���岢ʵ����
        GameObject CometObj = Resources.Load<GameObject>("Prefabs/Comet");
        Instantiate(CometObj);
        Comet newComet = CometObj.GetComponent<Comet>();

        //�����ٶ�
        newComet.CurVelocity = RotateVector3(CurVelocity, new Vector3(0, 0, 1), 90);

    }

    //����vector3��ת
    private Vector3 RotateVector3(Vector3 source, Vector3 axis, float angle)
    {
        Quaternion q = Quaternion.AngleAxis(angle, axis);// ��תϵ��
        return q * source;// ����Ŀ���
    }

    // Update is called once per frame
    void Update()
    {
        //����planet����ʱ���ı��䷽��
        if(hasTarget)
        {
            Vector3 dir = curTarget.transform.position - transform.position;
            //Debug.Log(Vector3.Cross(dir, CurVelocity).z);


            //��ȷ�����ԭ����ֱ֮���������ȥ��
            dir = new Vector3(dir.x, dir.y, 0);

            moveDir = dir.normalized;

            //Debug.Log("Here");
            if (dir.magnitude > curTarget.RangePush)
            {
                //�ĳ�moveDir��������Ӵ�������Ҫ�����������������
                CurVelocity += moveDir * acc / Mathf.Min(dir.magnitude, accMinRange) * curTarget.Force * Time.deltaTime;
                CurVelocity = new Vector3(CurVelocity.x, CurVelocity.y, 0);
                //�����鱻����ʱ��ͨ��������ǵ��ٶ����ж��Ƿ�ᱻ����/��������ֱ�Ӹ������ǵ��ٶ�
                transform.Translate(CurVelocity * Time.deltaTime, Space.World);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(CurVelocity), RocketRotateSpeed * Time.deltaTime);
            }
            else
            {
                Vector3 tmpMoveDir = Vector3.zero;
                //�ڷ�Χ֮�ڣ������ת
                if(isRotating == false)
                {
                    TmpVelocity = CurVelocity;
                    tmpMoveDir = moveDir;
                    isRotating = true;
                }
                rb.velocity = Vector3.zero;

                //Debug.Log(Vector3.Cross(moveDir, CurVelocity) +" "+CurVelocity);


                if (Vector3.Cross(tmpMoveDir, TmpVelocity).z > 0)
                {
                    //�����������ǰ����䷽��Ļ��Ʒ�����������*-1
                    transform.RotateAround(curTarget.transform.position, new Vector3(0, 0, 1), TmpVelocity.magnitude * Time.deltaTime * RotateSpeed / (curTarget.transform.position-transform.position).magnitude * -1);
                }
                else
                {
                    transform.RotateAround(curTarget.transform.position, new Vector3(0, 0, 1), TmpVelocity.magnitude * Time.deltaTime * RotateSpeed / (curTarget.transform.position - transform.position).magnitude);
                }

                TmpVelocity = CurVelocity.magnitude * Vector3.Cross(dir, new Vector3(0, 0, 1)).normalized;
                //transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.Cross( dir, new Vector3(0, 0, 1))), RocketRotateSpeed * Time.deltaTime);

            }
        }
        else
        {
            if (isRotating)
            {
                //��һ֡����ת����tmpvelocity����Curvelocity
                CurVelocity = TmpVelocity;
                isRotating = false;
            }
            else
            {
                //���������ƹ����תʱ���������ٶ�ǰ��
                CurVelocity = new Vector3(CurVelocity.x, CurVelocity.y, 0);
            }
            //�����鱻����ʱ��ͨ��������ǵ��ٶ����ж��Ƿ�ᱻ����/��������ֱ�Ӹ������ǵ��ٶ�
            CurVelocity = new Vector3(CurVelocity.x, CurVelocity.y, 0);
            transform.Translate(CurVelocity * Time.deltaTime, Space.World);
            //if(CurVelocity.magnitude > 0) { 
            //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(CurVelocity), RocketRotateSpeed * Time.deltaTime); 
            //}
        }

        CheckOutRange();
        CheckSpeedThresholdAnim();
    }

    //����Ƿ�ɳ��߽�
    private void CheckOutRange()
    {

        if(transform.position.x < minx-1)
        {
            transform.position = new Vector3 (minx+1, transform.position.y, 0);
        }
        else if(transform.position.x > maxx+1)
        {
            transform.position = new Vector3(maxx-1, transform.position.y, 0);
        }
        else if(transform.position.y < miny-1)
        {
            transform.position = new Vector3(transform.position.x, miny + 1, 0);
        }
        else if (transform.position.y > maxy+1)
        {
            transform.position = new Vector3(transform.position.x, maxy-1, 0);
        }
    }


    //ÿ֡�����ٶȸ��ĵ�ǰ�Ķ���
    private void CheckSpeedThresholdAnim()
    {
        switch (CurSpeedLevel())
        {
            case 2:
                SetTrail(true, true);
                break;
            case 1: 
                SetTrail(true, false);
                break;
            case 0:
                SetTrail(false, false);
                break;
            default:
                Debug.Log("TrailAnim Error");
                break;

        }
    }

    private void SetTrail(bool isAct_1, bool isAct_2)
    {
        if (!isTeleport)
        {
            trail_1.SetActive(isAct_1);
            trail_2.SetActive(isAct_2);
        }
        else
        {
            trail_1.SetActive(false);
            trail_2.SetActive(false);
        }
    }

    #region ���������

    //public void Rebound()
    //{
    //    //��ʱ���ٶ�˥��ȥ������Ϊblock�Ǳ߻���ٶȽ�һ��
    //    Debug.Log("Rebound");
    //    //CurVelocity = (CurVelocity - 2 * Vector3.Dot(CurVelocity, normal) * normal);
    //    CurVelocity = Vector3.Reflect(CurVelocity, collision.contacts[0].normal);
    //}

    private void OnCollisionEnter(Collision collision)
    {
        Hitable block = collision.gameObject.GetComponent<Hitable>();

        //����ֻҪ������Ҫ������ײ�ģ�����Ҫ�̳�Hitable��������дһ����Ӧ����ײ����
        if (block != null)
        {
            block.OnHit(CurVelocity, CurSpeedLevel(), CometScale, collision.GetContact(0).normal, out CurVelocity);
        }
        else
        {
            //Ĭ��Ϊȫ������һ����Ҫײ���Ķ������̳�Hitable
            CurVelocity = Vector3.Reflect(CurVelocity, collision.GetContact(0).normal);
        }

}




    //���ص�ǰ���ٶȵ�λ��0Ϊ�޷����ƣ�1Ϊ���Ʋ�������2Ϊ����
    public int CurSpeedLevel()
    {
        //Ҫ�����Ҫ��3�����ϵ�
        if(CurVelocity.magnitude < SpeedThreshold[0])
        {
            return 0;
        }
        else if(CurVelocity.magnitude < SpeedThreshold[1])
        {
            return 1;
        }
        else if (CurVelocity.magnitude >= SpeedThreshold[1])
        {
            return 2;
        }
        else
        {
            Debug.Log("SpeedLevel Error");
            return 0;
        }
    }

    //��ײ����󣬵�����һ���ٶȵĿ�ʼ
    private void DecSpeedLevel(int level)
    {
        if (CurSpeedLevel() == 1)
        {
            CurVelocity = CurVelocity.normalized * (CurVelocity.magnitude - 0.5f * level);
        }
        else if(CurSpeedLevel() == 2)
        {
            CurVelocity = CurVelocity.normalized * (CurVelocity.magnitude - 0.5f * level);
        }
        //switch(CurSpeedLevel())
        //{
        //    //�����ٶ�>�ڶ����ż�����ײ��͵�����һ���ż�
        //    case 2:
        //        Debug.Log("Dec to 1");
        //        CurVelocity = CurVelocity.normalized * (CurVelocity.magnitude - 1);
        //        break;
        //    //�����ٶ�ֻ���˵�һ���ż�ʱ����ײ�������һ���ż���һ��
        //    case 1:
        //        Debug.Log("Dec to 0");
        //        CurVelocity = CurVelocity.normalized * (SpeedThreshold[0]/2);
        //        break;
        //    //ʣ�µĵ�����speedlevelΪ0
        //    default: break;
        //}
    }


    #endregion


    private void PullToPlanet(Planet planet)
    {
        if (IsPlanetInRange(planet))
        {
            curTarget = planet;
            hasTarget = true;
        }
    }
    private void RealseFromPlanet(Planet planet)
    {
        if(IsPlanetInRange(planet)) hasTarget = false;
    }

    private bool IsPlanetInRange(Planet planet)
    {
        return (planet.transform.position - transform.position).magnitude < planet.RangePull;
    }

    #region ����

    //�ڱ��ʱ��Comet���ٶȼ�С����ײ���ݻٷ��������
    //ײ���ݻٷ����Ч����ͨ��CometScale�����Ƶ�
    public void OnEnlarge(float Ratio)
    {
        transform.localScale = transform.localScale * Ratio;
        acc /= Ratio;
        CometScale *= Ratio;
    }

    public void OnDiminish(float Ratio)
    {
        transform.localScale = transform.localScale / Ratio;
        acc *= Ratio;
        //CometScale /= Ratio;
    }



    #endregion
}
