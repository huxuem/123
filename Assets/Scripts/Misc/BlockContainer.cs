using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BlockContainer : MonoBehaviour
{
    public int NumInLine;
    [SerializeField] private List<Transform> BlockList;

    private void Start()
    {
        //将子节点加入
        for(int i = 0; i < transform.childCount; i++)
        {
            BlockList.Add(transform.GetChild(i));
        }

        if (NumInLine != 0)
        {
            for(int i = 0;i < BlockList.Count / NumInLine; i++)
            {
                //Debug.Log(BlockList.Count / NumInLine);
                for(int j = 0;j < NumInLine;j++)
                {
                    BlockList[i * NumInLine + j].position = new Vector3(
                        (j*0.5f)-0.5f*NumInLine/2, i * 0.5f, 0);
                    //Debug.Log("BlockList:" + (i * NumInLine + j) + " " + BlockList[i * NumInLine + j].position);
                }
            }
            for(int i = 0; i<= BlockList.Count % NumInLine-1; i++)
            {
                BlockList[(int)(BlockList.Count / NumInLine)*NumInLine  + i].position = new Vector3(
                        (i * 0.5f) - 0.5f * NumInLine / 2, (BlockList.Count / NumInLine) * 0.5f, 0);
            }
        }

    }


}
