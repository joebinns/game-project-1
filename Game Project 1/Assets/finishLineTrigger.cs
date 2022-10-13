using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finishLineTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameSettings.Instance.SwitchScene(2);
    }
}
