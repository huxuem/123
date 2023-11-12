using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MouseManager : MonoBehaviour
{
    public static MouseManager instance;
    public LayerMask mask;

    public List<GameObject> PlanetList;

    private Camera camera;
    private RaycastHit hitInfo;
    private bool IsClicked = false;



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
            if (Physics.Raycast(ray, out hitInfo,Mathf.Infinity, mask))
            {
                if (hitInfo.collider.gameObject.CompareTag("PlanetClickArea") && !IsClicked)
                {
                    //还是需要用bool管理单次触发的东西
                    IsClicked = true;
                    hitInfo.collider.gameObject.GetComponentInParent<Planet>().OnClicked();
                }

                // 在这里处理点击事件，根据需要执行操作
            }
        }
        else if(Input.GetMouseButtonUp(0) && IsClicked)
        {
            IsClicked = false;
            hitInfo.collider.gameObject.GetComponentInParent<Planet>().OnRelease();
        }
    }
}
