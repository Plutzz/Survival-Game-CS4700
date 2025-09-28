using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField] private CinemachineVirtualCamera playerFollowCam;

    public void SetPlayerFollow(Transform player)
    {
        playerFollowCam.Follow = player;
    }
}
