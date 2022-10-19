using Managers.Points;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finishLineTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameSettings.Instance.SetPlayerPoints(PointsManager.Instance.GetPoints(0), PointsManager.Instance.GetPoints(1));

        GameSettings.Instance.SwitchScene(2);
    }
}
