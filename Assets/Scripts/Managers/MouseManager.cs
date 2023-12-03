using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Events;

public class MouseManager : MonoBehaviour
{
    public static MouseManager instance;
    public LayerMask mask;
    public LayerMask mask_Drag;

    public List<GameObject> PlanetList;

    private Camera camera;
    private RaycastHit hitInfo;
    private Planet curTarget;
    private Dragable curDrag = null;

    private bool IsClicked = false;
    private bool isSpliting = false;



    private void Awake()
    {
        instance = this;

        //��ȡplanet�б�Ϊ�˱�֤��Comet��ȡҪ�����Է�Awake
        GameObject[] objectWithType = GameObject.FindGameObjectsWithTag("Planet");
        //List��[]����תһ�£�����
        PlanetList = new List<GameObject>(objectWithType);
    }

    // Start is called before the first frame update
    void Start()
    {
        //��ȡ���
        camera = Camera.main;


    }

    // Update is called once per frame
    void Update()
    {
        //�������棬��֤releaseҲ���Ե���
        //���˵��/ײ�����ʱ����ѣ����Ȳ�����getbuttondown
        if (Input.GetMouseButton(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            //if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, mask_Drag))
            //{
            //    //�ǿ���ק������
            //    //Debug.Log("Drag Start");
            //    curDrag = hitInfo.collider.gameObject.GetComponentInParent<Dragable>();
            //    curDrag.BeginDrag();
            //}

            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, mask))
            {
                if (hitInfo.collider.gameObject.CompareTag("PlanetClickArea") && !IsClicked)
                {
                    //������
                    IsClicked = true;
                    curTarget = hitInfo.collider.gameObject.GetComponentInParent<Planet>();
                    curTarget.OnClicked();
                }
                else if (hitInfo.collider.gameObject.CompareTag("Comet") && !isSpliting)
                {
                    isSpliting = true;
                    StartCoroutine(CometSplitCoroutine());
                }

            }
            //else Debug.Log("Drag fail");
        }
        else if(Input.GetMouseButtonUp(0) && IsClicked)
        {
            IsClicked = false;
            curTarget.OnRelease();
        }
        //else if(Input.GetMouseButtonUp(0) && curDrag != null)
        //{
        //    //dragֻ����Чһ�Σ���Ч���˾ͱ�ɹ̶�����
        //    curDrag.EndDrag();
        //    curDrag = null;
        //}
    }

    IEnumerator CometSplitCoroutine()
    {
        Comet comet = hitInfo.collider.gameObject.GetComponentInParent<Comet>();
        //Debug.Log("Split");
        comet.SelfSplit();
        yield return new WaitForSeconds(0.5f);
        isSpliting = false;
    }
    //private void FixedUpdate()
    //{
    //    Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    if(curDrag != null)
    //    {
    //        curDrag.DragTo(new Vector2(pos.x, pos.y));
    //    }
    //}

}
