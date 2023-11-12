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

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo,Mathf.Infinity, mask))
            {
                if (hitInfo.collider.gameObject.CompareTag("PlanetClickArea") && !IsClicked)
                {
                    //������Ҫ��bool�����δ����Ķ���
                    IsClicked = true;
                    hitInfo.collider.gameObject.GetComponentInParent<Planet>().OnClicked();
                }

                // �����ﴦ�����¼���������Ҫִ�в���
            }
        }
        else if(Input.GetMouseButtonUp(0) && IsClicked)
        {
            IsClicked = false;
            hitInfo.collider.gameObject.GetComponentInParent<Planet>().OnRelease();
        }
    }
}
