using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackPanel : MonoBehaviour
{
    public UIManager uiManager;

    public void OnFinishedBuildingTheRocket()
    {
        uiManager.OnFinishedBuilding();
    }
}
