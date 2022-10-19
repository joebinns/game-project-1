using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class FloatingTextManager : MonoBehaviour
{
    public GameObject textPrefab;

    public int poolSize;

    public GameObject canvas;

    public List<FloatingText> textPool = new List<FloatingText>();

    public static FloatingTextManager Instance { get; private set; }
    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

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

    FloatingText GetText()
    {

        FloatingText text = textPool[0];

        textPool.Remove(text);
        textPool.Add(text);
      


        return textPool[0];
    }

    public void SpawnText(string content, Vector3 position, float speed, float duration, Color color)
    {
        FloatingText text = GetText();

        text.transform.position = new Vector3(position.x, position.y, position.z);

        text.Spawned(content, speed, duration, color);

    }

    
}
