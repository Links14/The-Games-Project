using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IData
{
    void LoadData(SaveData _data);
    void SaveData(ref SaveData _data);

}
