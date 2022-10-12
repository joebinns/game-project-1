using System;
using System.Collections;
using System.Collections.Generic;
using Managers.Audio;
using UI;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Tiles
{
    public class Tile : MonoBehaviour
    {
        public TileSettings TileSettings;

        public static event Action<Tile> OnBeginEffect;
        public static event Action<Tile> OnEndEffect;

        public virtual void BeginEffect()
        {
            // Subscribe inputs to this tile's effect
            OnBeginEffect?.Invoke(this);

            // Play BeginEffectAudio and activate effect canvas
            if (TileSettings.BeginEffectAudio != null)
            {
                AudioManager.PlaySound(TileSettings.BeginEffectAudio);
            }

            var uIHandler = FindObjectOfType<UIHandler>();
            uIHandler.ActivateEffectCanvas();
            uIHandler.SetEffectSprite(TileSettings.BeginEffectSprite);
        }

        public virtual void EndEffect()
        {
            // Subscribe inputs back to player
            OnEndEffect?.Invoke(null);
            
            // Play EndEffectAudio and deactivate effect canvas
            if (TileSettings.EndEffectAudio != null)
            {
                AudioManager.PlaySound(TileSettings.EndEffectAudio);
            }
            
            Invoke("DeactivateCanvas", TileSettings.DeactivateCanvasDelay);

            // Destroy tiles if indefinite
            if (TileSettings.IsIndefinite)
            {
                StartCoroutine(WaitToDestroy());
            }
        }

        private void DeactivateCanvas()
        {
            FindObjectOfType<UIHandler>().DeactivateEffectCanvas();
        }

        IEnumerator WaitToDestroy()
        {
            RoadGenerator roadGen = FindObjectOfType<RoadGenerator>();
            
            yield return new WaitForSeconds(1.5f);
            
            roadGen.RemoveActiveTile(this.gameObject);
            roadGen.generationMode = RoadGenerator.GenerationModes.Normal;
            roadGen.DestroyQueue.Enqueue(this.gameObject);
        }
        
        private void Awake()
        {
            if (TileSettings.IsIndefinite)
            {
                FindObjectOfType<RoadGenerator>().generationMode = RoadGenerator.GenerationModes.Indefinite;
            }
        }

        public virtual void HandleInput(int playerId)
        {
            // Play HandleInputAudio
            if (TileSettings.HandleInputAudio != null)
            {
                AudioManager.PlaySound(TileSettings.HandleInputAudio);
            }
        }
    }
}
