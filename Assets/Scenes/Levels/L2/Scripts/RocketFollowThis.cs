using Cinemachine;
using UnityEngine;

public class RocketFollowThis : MonoBehaviour
{
    private CinemachineVirtualCamera vcam;

    void Start()
    {
        vcam = gameObject.GetComponent<CinemachineVirtualCamera>();
    }
    public void FindAndFollowCapsule(Transform childObj)
    {
        vcam.LookAt = childObj;
        vcam.Follow = childObj;
    }
}
