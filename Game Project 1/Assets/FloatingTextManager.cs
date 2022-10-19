using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class FloatingTextManager : MonoBehaviour
{
    public GameObject textPrefab;

    public int poolSize;

    public GameObject canvas;

    static List<FloatingText> textPool = new List<FloatingText>();

    private void Awake()
    {
        for (int i = 0; i < poolSize; i++)
        {
            textPool.Add(Instantiate(textPrefab, canvas.transform).GetComponent<FloatingText>());
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
            SpawnText("test", Vector3.zero, 1, 1, Color.red);
    }

    static FloatingText GetText()
    {
        FloatingText text = textPool[0];

        textPool.Remove(text);
        textPool.Add(text);

        return textPool[0];
    }

    public static void SpawnText(string content, Vector3 position, float speed, float duration, Color color)
    {
        FloatingText text = GetText();

        text.transform.position = position;

        text.Spawned(content, speed, duration, color);

    }

    
}
