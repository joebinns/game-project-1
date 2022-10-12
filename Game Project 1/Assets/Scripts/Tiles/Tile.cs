using System;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Tiles
{
    public abstract class Tile : MonoBehaviour
    {
        public TileSettings TileSettings;

        public static event Action<Tile> OnBeginEffect;
        public static event Action<Tile> OnEndEffect;

        public virtual void BeginEffect()
        {
            // Subscribe inputs to this tile's effect
            OnBeginEffect?.Invoke(this);
        }

        public virtual void EndEffect()
        {
            // Subscribe inputs to this tile's effect
            OnEndEffect?.Invoke(null);
        }

        private void Awake()
        {
            if (TileSettings.isIndefinite)
            {
                FindObjectOfType<RoadGenerator>().generationMode = RoadGenerator.GenerationModes.Indefinite;
            }
        }

        public abstract void HandleInput(int playerId);
    }
}
