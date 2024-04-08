using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TileMaterialManager : Singleton<TileMaterialManager>
{
    [SerializeField]
    List<Material> tileMaterialList;

    public int ColorCount 
    {
        get
        {
            return tileMaterialList.Count;
        }
    }

    public Material GetMaterial(int index)
    {
        return tileMaterialList[index];
    }

}
