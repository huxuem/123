using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class Tool_Big : Drag_Tool
{
    private float Ratio = 1.8f;

    public override void EndDrag()
    {
        base.EndDrag();
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if(Physics.Raycast(ray, out hitInfo, Mathf.Infinity, MouseManager.instance.mask))
        {
            Iinteractable target = hitInfo.transform.GetComponent<Iinteractable>();
            Iinteractable target_parent = hitInfo.transform.GetComponentInParent<Iinteractable>();
            if (target != null)
            {
                Debug.Log("Larger!");
                target.OnEnlarge(Ratio);
                Destroy(gameObject);
            }
            else if(target_parent != null)
            {
                Debug.Log("Parent Larger!");
                target_parent.OnEnlarge(Ratio);
                Destroy(gameObject);
            }
            else Debug.Log("Not Target");
        }
    }
}
