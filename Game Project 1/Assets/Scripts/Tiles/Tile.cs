using System;
using System.Collections;
using System.Collections.Generic;
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
            // Subscribe inputs back to player
            OnEndEffect?.Invoke(null);
            
            //Destroy tiles if indefinite
            if (TileSettings.isIndefinite)
            {
                StartCoroutine(WaitToDestroy());
            }
        }

        IEnumerator WaitToDestroy()
        {
            RoadGenerator roadGen = FindObjectOfType<RoadGenerator>();
            
            yield return new WaitForSeconds(1.5f);
            
            roadGen.RemoveActiveTile(this.gameObject);
            roadGen.generationMode = RoadGenerator.GenerationModes.Normal;
            roadGen.GenerateNextTile();
            Destroy(this.gameObject);
            
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
