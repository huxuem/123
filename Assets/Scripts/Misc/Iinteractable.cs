using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Iinteractable
{
    //因为不同物体的处理不一样（星星要扩大它的环绕范围）,所以必须实现OnEnLarge
    public void OnEnlarge(float Ratio);
    public void OnDiminish(float Ratio);
}
