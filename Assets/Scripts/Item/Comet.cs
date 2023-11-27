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
    //给变大变小用的，让其他撞击的东西知道当前Comet大小
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

        //分左右，向中心偏移一单位（因为protal需要放在外面。之后protal全部放在边界外移1.5单位的地方）
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

        //用于控制尾迹开关
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
        //获取最大值最小值
        //靠，x一开始是反着摆的
        maxx = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).x;
        minx = Camera.main.ViewportToWorldPoint(new Vector2(1, 0)).x;

        miny = Camera.main.ViewportToWorldPoint(new Vector2(0, 0)).y;
        maxy = Camera.main.ViewportToWorldPoint(new Vector2(0, 1)).y;

        //Debug.Log(minx+" "+maxx+" "+miny+" "+maxy);
    }

    //当被点击时，复制Comet。从mousemgr里调用
    public void SelfSplit()
    {

        //获取Comet物体并实例化
        GameObject CometObj = Resources.Load<GameObject>("Prefabs/Comet");
        Instantiate(CometObj);
        Comet newComet = CometObj.GetComponent<Comet>();

        //设置速度
        newComet.CurVelocity = RotateVector3(CurVelocity, new Vector3(0, 0, 1), 90);

    }

    //用于vector3旋转
    private Vector3 RotateVector3(Vector3 source, Vector3 axis, float angle)
    {
        Quaternion q = Quaternion.AngleAxis(angle, axis);// 旋转系数
        return q * source;// 返回目标点
    }

    // Update is called once per frame
    void Update()
    {
        //当被planet吸引时，改变其方向
        if(hasTarget)
        {
            Vector3 dir = curTarget.transform.position - transform.position;
            //Debug.Log(Vector3.Cross(dir, CurVelocity).z);


            //不确定歪的原因，总之直接在这把歪去掉
            dir = new Vector3(dir.x, dir.y, 0);

            moveDir = dir.normalized;

            //Debug.Log("Here");
            if (dir.magnitude > curTarget.RangePush)
            {
                //改成moveDir，近距离加大引力。要乘上星球自身的引力
                CurVelocity += moveDir * acc / Mathf.Min(dir.magnitude, accMinRange) * curTarget.Force * Time.deltaTime;
                CurVelocity = new Vector3(CurVelocity.x, CurVelocity.y, 0);
                //当方块被击中时，通过检测彗星的速度来判断是否会被击穿/反弹，并直接更改彗星的速度
                transform.Translate(CurVelocity * Time.deltaTime, Space.World);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(CurVelocity), RocketRotateSpeed * Time.deltaTime);
            }
            else
            {
                Vector3 tmpMoveDir = Vector3.zero;
                //在范围之内，变成旋转
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
                    //本来的作用是按入射方向改环绕方向，所以做了*-1
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
                //上一帧在旋转，把tmpvelocity赋给Curvelocity
                CurVelocity = TmpVelocity;
                isRotating = false;
            }
            else
            {
                //当不是在绕轨道旋转时，继续按速度前进
                CurVelocity = new Vector3(CurVelocity.x, CurVelocity.y, 0);
            }
            //当方块被击中时，通过检测彗星的速度来判断是否会被击穿/反弹，并直接更改彗星的速度
            CurVelocity = new Vector3(CurVelocity.x, CurVelocity.y, 0);
            transform.Translate(CurVelocity * Time.deltaTime, Space.World);
            //if(CurVelocity.magnitude > 0) { 
            //    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(CurVelocity), RocketRotateSpeed * Time.deltaTime); 
            //}
        }

        CheckOutRange();
        CheckSpeedThresholdAnim();
    }

    //检查是否飞出边界
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


    //每帧根据速度更改当前的动画
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

    #region 给方块调用

    //public void Rebound()
    //{
    //    //暂时把速度衰减去掉，因为block那边会给速度降一档
    //    Debug.Log("Rebound");
    //    //CurVelocity = (CurVelocity - 2 * Vector3.Dot(CurVelocity, normal) * normal);
    //    CurVelocity = Vector3.Reflect(CurVelocity, collision.contacts[0].normal);
    //}

    private void OnCollisionEnter(Collision collision)
    {
        Hitable block = collision.gameObject.GetComponent<Hitable>();

        //现在只要它是需要和球碰撞的，就需要继承Hitable，在里面写一个对应的碰撞函数
        if (block != null)
        {
            block.OnHit(CurVelocity, CurSpeedLevel(), CometScale, collision.GetContact(0).normal, out CurVelocity);
        }
        else
        {
            //默认为全反弹。一般需要撞到的东西都继承Hitable
            CurVelocity = Vector3.Reflect(CurVelocity, collision.GetContact(0).normal);
        }

}




    //返回当前的速度挡位，0为无法击破，1为击破并反弹，2为击穿
    public int CurSpeedLevel()
    {
        //要求必须要分3个以上档
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

    //碰撞方块后，调到下一档速度的开始
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
        //    //本来速度>第二个门槛，碰撞后就调到第一个门槛
        //    case 2:
        //        Debug.Log("Dec to 1");
        //        CurVelocity = CurVelocity.normalized * (CurVelocity.magnitude - 1);
        //        break;
        //    //本来速度只过了第一个门槛时，碰撞后调到第一个门槛的一半
        //    case 1:
        //        Debug.Log("Dec to 0");
        //        CurVelocity = CurVelocity.normalized * (SpeedThreshold[0]/2);
        //        break;
        //    //剩下的当成是speedlevel为0
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

    #region 道具

    //在变大时，Comet加速度减小，但撞击摧毁方块更轻松
    //撞击摧毁方块的效率是通过CometScale来控制的
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
