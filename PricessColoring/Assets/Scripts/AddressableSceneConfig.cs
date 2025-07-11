using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AddressableSceneConfig", menuName = "ScriptableObjects/Config/AddressableSceneConfig", order = 1)]
public class AddressableSceneConfig : ScriptableObject
{
    public List<string> onDemandScenes;
    public List<string> installTimeScenes;
    public List<string> fastFollowScenes;
}