using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;
//using static UnityEditor.PlayerSettings;

public class Comet : MonoBehaviour
{
    public Vector3 CurVelocity = Vector3.zero;
    public float acc = 1;


    [SerializeField]private float accMinRange = 5;
    //[SerializeField] private float VelDecRatio = 1.5f;
    [SerializeField] private float RocketRotateSpeed = 5f;
    [SerializeField] private float RotateSpeed = 1f;
    [SerializeField] private float TurnRatio = 1f;

    [SerializeField] private List<float> SpeedThreshold = new List<float> {2,4};
    [SerializeField] private GameObject trail_1;
    [SerializeField] private GameObject trail_2;

    private Vector3 moveDir;
    private Vector3 TmpVelocity;
    [SerializeField]private Planet curTarget;
    private Rigidbody rb;
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
        if (isHorizontal)
        {
            transform.position = new Vector3(transform.position.x * -1, transform.position.y, 0f);
        }

        else transform.position = new Vector3(transform.position.x, transform.position.y * -1, 0f);

        isTeleport = true;
        yield return new WaitForSeconds(0.4f);
        isTeleport = false;
    }


    // Start is called before the first frame update
    void Start()
    {
        InitPlanetList();

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
                //改成moveDir，近距离加大引力
                CurVelocity += moveDir * acc/100 / Mathf.Min(dir.magnitude, accMinRange);
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


        CheckSpeedThresholdAnim();
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
        //Debug.Log("Enter");
        if (collision.gameObject.CompareTag("Block") || collision.gameObject.CompareTag("Boss"))
        {
            //如果撞方块了
            bool isBoss = false;
            Block block = collision.gameObject.GetComponent<Block>();
            Boss boss = null;
            if (collision.gameObject.CompareTag("Boss"))
            {
                isBoss = true;
                boss = collision.gameObject.GetComponent<Boss>();
            }
            switch (CurSpeedLevel())
            {
                case 2:
                    //当速度第二档时，方块耐久-1，无耐久则打穿，有耐久则反弹（之后要改成减对应耐久的）
                    if (isBoss)
                    {
                        boss.OnHit(CurVelocity);
                        AudioManager.instance.HitBossAudio();
                        CurVelocity = Vector3.Reflect(CurVelocity, collision.GetContact(0).normal);
                    }
                    else
                    {
                        if (block.IsStatic)
                        {
                            DecSpeedLevel();
                            DecSpeedLevel();
                            DecSpeedLevel();
                        }
                        block.OnHit();
                        if (block != null)
                        {
                            block.OnHit();
                        }
                        AudioManager.instance.HitBlockAudio();
                        //if (block.IsStatic)
                        //{
                        //    CurVelocity = Vector3.Reflect(CurVelocity, collision.GetContact(0).normal);
                        //}
                    }

                    //减速
                    DecSpeedLevel();
                    break;


                case 1:
                    //Debug.Log("Collide level 1");
                    //做反射运算，得到反射后的选择向量。OnHit可能要写在后面，要不getcontact已经没了
                    CurVelocity = Vector3.Reflect(CurVelocity, collision.GetContact(0).normal);
                    if (isBoss)
                    {
                        boss.OnHit(CurVelocity);
                        if (boss != null)
                        {
                            AudioManager.instance.HitBossAudio();
                        }
                        else AudioManager.instance.DefeatBossAudio();
                    }
                    else 
                    {
                        block.OnHit();
                        //播放音频
                        AudioManager.instance.HitBlockAudio();
                    } 
                    //speedlevel为1级时不减速，这样就可以保证撞出来了

                    break;

                case 0:
                    //Debug.Log("Collide level 0");

                    //当速度为第0档时，方块耐久不变化，反弹
                    //做反射运算，得到反射后的选择向量
                    CurVelocity = Vector3.Reflect(CurVelocity, collision.GetContact(0).normal);
                    //为0时不触发声音，也不触发减速

                    break;

            }
            //Debug.Log(CurVelocity);
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            //Wall有速度衰减
            //Debug.Log("HitWall");
            CurVelocity = Vector3.Reflect(CurVelocity, collision.GetContact(0).normal)/ 1.5f;

            AudioManager.instance.HitWallAudio();
        }
        else if (collision.gameObject.CompareTag("Acc")){}
        else
        {
            //做反射运算，得到反射后的选择向量
            //Debug.Log("Hit other thing");
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
    private void DecSpeedLevel()
    {
        if (CurSpeedLevel() == 1)
        {
            CurVelocity = CurVelocity.normalized * (CurVelocity.magnitude - 0.5f);
        }
        else if(CurSpeedLevel() == 2)
        {
            CurVelocity = CurVelocity.normalized * (CurVelocity.magnitude - 0.5f);
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

    public void Item_Acc()
    {
        CurVelocity += CurVelocity.normalized * 2;
    }



    #endregion
}
