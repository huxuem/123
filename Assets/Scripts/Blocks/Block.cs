using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField]protected int hitRemain  = 2;
    [SerializeField] List<Material> mat_hit;

    private Renderer renderer;


    protected virtual void Start()
    {
        renderer = gameObject.GetComponent<Renderer>();
    }


    public virtual void OnHit()
    {
        hitRemain -= 1;


        CheckDestory();
        changeMaterial();
    }

    protected void changeMaterial()
    {
        renderer.material = mat_hit[hitRemain-1]; 
    }

    protected void CheckDestory()
    {
        Debug.Log("Destory");
        if (hitRemain <= 0)
        {
            Destroy(gameObject);
        }
    }
}
