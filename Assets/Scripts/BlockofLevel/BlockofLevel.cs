using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;





[CreateAssetMenu(menuName = "MyScriptableObject/BlockofLevel ")]
public class BlockofLevel : ScriptableObject
{

    public List<ShowContent> blocks;
    [HideInInspector] public List<List<List<int>>> BlockPos = new List<List<List<int>>>();


    [Serializable]
    public class ShowContent
    {
        public List<content> con1;
    };
    [Serializable]
    public class content
    {
        public List<int> con2;
    }

    public void InitBlock()
    {
        //Ҫ�ӵ�ʱ����Ҫ��С���
        //BlockPos.Add(new List<List<int>>
        //    {
        //        new List<int> {-6,6},
        //        new List<int> {6,6}
        //    });
        //BlockPos.Add(new List<List<int>>
        //    {
        //        new List<int> {-6,-6},
        //        new List<int> {6,-6}
        //    });

        //���������Ǹ����췽�����л��������б������
        for(int i = 0; i < blocks.Count; i++)
        {
            // ����һ���µĶ�ά�б����ڴ洢��ǰ ShowContent �е�����
            List<List<int>> blockData = new List<List<int>>();

            for (int j=0;j<2;j++)
            {
                // ����һ���µ�һά�б����ڴ洢��ǰ content �е�����
                List<int> contentData = new List<int>();

                // ����ǰ content �е�������ӵ� contentData ��
                contentData.AddRange(blocks[i].con1[j].con2);

                // �� contentData ��ӵ� blockData ��
                blockData.Add(contentData);
            }
            BlockPos.Add(blockData);
        }
    }
}
