using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

public class WorldTags : UdonSharpBehaviour
{
    [SerializeField]
    private GameObject dynamicPrefabToApply;

    [SerializeField]
    private string[] displayNamesToApplyTo;
	
    [SerializeField]
    private float HeightMultiplier = 0.9f;

    private GameObject InstantiatedObj;

    void Start()
    {
        foreach(var adminPlayers in displayNamesToApplyTo)
        {
            if (Networking.LocalPlayer.displayName.ToLower() == adminPlayers.ToLower())
            {
                InstantiatedObj = VRCInstantiate(dynamicPrefabToApply);
            }
        }
    }

    void Update()
    {
        if (InstantiatedObj != null)
        {
            var HeadPos = Networking.LocalPlayer.GetBonePosition(HumanBodyBones.Head);
            var FootPos = Networking.LocalPlayer.GetBonePosition(HumanBodyBones.Hips);

            InstantiatedObj.transform.position = new Vector3(HeadPos.x, HeadPos.y, HeadPos.z + (Vector3.Distance(FootPos, HeadPos) * HeightMultiplier));
        }
    }
}
