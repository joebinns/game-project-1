using System;
using System.Collections;
using Managers.Audio;
using UI;
using UnityEngine;

namespace Tiles
{
    public class Tile : MonoBehaviour
    {
        public TileSettings TileSettings;
        public bool IsActive;

        public static event Action<Tile> OnBeginEffect;
        public static event Action<Tile> OnEndEffect;

        public virtual void BeginEffect()
        {
            IsActive = true;
            
            // Subscribe inputs to this tile's effect
            OnBeginEffect?.Invoke(this);

            // Switch movement options
            FindObjectOfType<PlayerManager>().SwitchAllMovementOptions(TileSettings.MovementOption);

            // Play BeginEffectAudio
            if (TileSettings.BeginEffectAudio != null)
            {
                AudioManager.PlaySound(TileSettings.BeginEffectAudio);
            }

            // Activate effect canvas
            var uIHandler = FindObjectOfType<UIHandler>();
            uIHandler.ActivateEffectCanvas();
            uIHandler.SetEffectText(TileSettings.BeginEffectText);
            uIHandler.SetEffectAnimation(TileSettings.EffectAnimation);
        }

        public virtual void EndEffect()
        {
            IsActive = false;
            
            // Subscribe inputs back to player
            OnEndEffect?.Invoke(null);

            // Reset movement options
            FindObjectOfType<PlayerManager>().ResetAllMovementOptions();
            
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
            
            yield return new WaitForSeconds(0.5f);
            
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
