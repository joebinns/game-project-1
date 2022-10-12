using System;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Tiles
{
    public abstract class Tile : MonoBehaviour
    {
        public TileSettings TileSettings;

        protected abstract void BeginEffect();
        protected abstract void EndEffect();

        private void Awake()
        {
            if (TileSettings._isIndefinite)
            {
                FindObjectOfType<RoadGenerator>().generationMode = RoadGenerator.GenerationModes.Indefinite;
            }
        }

        private void OnEnable()
        {
            TileSettings.OnBeginEffect += BeginEffect;
            TileSettings.OnBeginEffect += EndEffect;
        }
        
        private void OnDisable()
        {
            TileSettings.OnBeginEffect -= BeginEffect;
            TileSettings.OnBeginEffect -= EndEffect;
        }
    }
}
