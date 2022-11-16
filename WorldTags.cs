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
    private int HeightMultiplier = 0;

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
            var ChestPos = Networking.LocalPlayer.GetBonePosition(HumanBodyBones.Chest);

            InstantiatedObj.transform.position = new Vector3(HeadPos.x, HeadPos.y, HeadPos.z + (Vector3.Distance(ChestPos, HeadPos) * HeightMultiplier));
        }
    }
}
