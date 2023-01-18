using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/ToolData"), System.Serializable]
public class ToolData : ScriptableObject
{
    public string toolName;
    public Sprite toolProfile;
}
