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
            var uIHandler = FindObjectOfType<UIHandler>();
            uIHandler.DeactivateEffectCanvas();
            FindObjectOfType<UIHandler>().DeactivateEffectCanvas();
            
            // Destroy tiles if indefinite
            if (TileSettings.IsIndefinite)
            {
                StartCoroutine(WaitToDestroy());
            }
        }

        IEnumerator WaitToDestroy()
        {
            yield return new WaitForSeconds(3f);
            
            FindObjectOfType<RoadGenerator>().RemoveActiveTile(this.gameObject);
            Destroy(this.gameObject);
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
