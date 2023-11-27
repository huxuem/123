using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag_Tool : Dragable
{
    SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public override void BeginDrag()
    {
        Time.timeScale = 0.1f;
        sprite.color = new Color(1f, 1f, 1f, 0.5f);
    }

    public override void EndDrag()
    {
        Time.timeScale = 1f;
        sprite.color = Color.white;
        
    }
}
