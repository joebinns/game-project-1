using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayManager : MonoBehaviour
{
    [SerializeField] private GameObject player1RendererObj, player2RendererObj, player1HelmRenderer, player2HelmRenderer, p1HoverboardObj, p2HoverboardObj;

    private void Awake()
    {
        GameSettings.Instance.p1 = player1RendererObj;
        GameSettings.Instance.p2 = player2RendererObj;

        GameSettings.Instance.p1Helm = player1HelmRenderer;
        GameSettings.Instance.p2Helm = player2HelmRenderer;

        GameSettings.Instance.p1Hoverboard = p1HoverboardObj;
        GameSettings.Instance.p2Hoverboard = p2HoverboardObj;
    }

    private void Start()
    {
        GameSettings.Instance.ApplyPlayerMats();
        GameSettings.Instance.ApplyPlayerThings();
    }
}
