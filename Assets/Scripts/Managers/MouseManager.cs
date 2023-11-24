using System;
using System.Collections;
using System.Collections.Generic;
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
    private bool IsClicked = false;
    private Planet curTarget;
    private Dragable curDrag = null;



    private void Awake()
    {
        instance = this;

        //获取planet列表，为了保证比Comet获取要后所以放Awake
        GameObject[] objectWithType = GameObject.FindGameObjectsWithTag("Planet");
        //List到[]还得转一下，拉跨
        PlanetList = new List<GameObject>(objectWithType);
    }

    // Start is called before the first frame update
    void Start()
    {
        //获取相机
        camera = Camera.main;


    }

    // Update is called once per frame
    void Update()
    {
        //放在外面，保证release也可以调用
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, mask_Drag))
            {
                //是可拖拽的物体
                //Debug.Log("Drag Start");
                curDrag = hitInfo.collider.gameObject.GetComponentInParent<Dragable>();
                curDrag.BeginDrag();
            }

            else if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, mask))
            {
                if (hitInfo.collider.gameObject.CompareTag("PlanetClickArea") && !IsClicked)
                {
                    //是星球
                    IsClicked = true;
                    curTarget = hitInfo.collider.gameObject.GetComponentInParent<Planet>();
                    curTarget.OnClicked();
                }

            }
            else Debug.Log("Drag fail");
        }
        else if(Input.GetMouseButtonUp(0) && IsClicked)
        {
            IsClicked = false;
            curTarget.OnRelease();
        }
        else if(Input.GetMouseButtonUp(0) && curDrag != null)
        {
            //drag只能生效一次，生效完了就变成固定的了
            curDrag.EndDrag();
            curDrag = null;
        }
    }

    private void FixedUpdate()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(curDrag != null)
        {
            curDrag.DragTo(new Vector2(pos.x, pos.y));
        }
    }

}
