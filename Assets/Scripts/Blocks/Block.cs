using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public bool IsStatic;

    [SerializeField]protected int hitRemain  = 2;
    [SerializeField] List<Material> mat_hit;

    private Renderer renderer;


    protected virtual void Start()
    {
        renderer = gameObject.GetComponent<Renderer>();
        //Debug.Log("Start,:" + hitRemain);
    }


    public virtual void OnHit()
    {
        hitRemain -= 1;


        CheckDestory();

        if(hitRemain > 0)
        {
            changeMaterial();
        }
    }

    protected void changeMaterial()
    {
        //Debug.Log("HitRemain:" + hitRemain+", name:"+transform.gameObject.GetInstanceID());
        renderer.material = mat_hit[hitRemain-1]; 
    }

    protected virtual void CheckDestory()
    {
        //Debug.Log("Destory,id:"+ transform.gameObject.GetInstanceID());
        if (hitRemain <= 0)
        {
            Destroy(gameObject);
        }
    }
}
