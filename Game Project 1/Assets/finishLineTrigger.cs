using Managers.Points;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finishLineTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameSettings.Instance.SetPlayerPoints(PointsManager.GetPoints(0), PointsManager.GetPoints(1));

        GameSettings.Instance.SwitchScene(2);
    }
}
