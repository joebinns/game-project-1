using System;
using System.Collections;
using Inputs;
using Managers.Audio;
using Managers.Points;
using Players;
using UI;
using UnityEngine;
using FMOD;
using FMODUnity;

namespace Tiles
{
    public class Tile : MonoBehaviour
    {
        public TileSettings TileSettings;
        [HideInInspector] public bool IsActive;

        public static event Action<Tile> OnBeginEffect;
        public static event Action<Tile> OnEndEffect;
        private FMOD.Studio.EventInstance PlayFailButton2;
        private FMOD.Studio.EventInstance EventSuccess;

        private void Awake()
        {
            PlayFailButton2 = FMODUnity.RuntimeManager.CreateInstance("event:/FailButton2");
            EventSuccess = FMODUnity.RuntimeManager.CreateInstance("event:/EventStart");
        }
        private void Start()
        {
            if (TileSettings.IsIndefinite)
            {
                FindObjectOfType<RoadGenerator>().generationMode = RoadGenerator.GenerationModes.Indefinite;
            }
        }
        

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
                //AudioManager.Instance.PlaySound(TileSettings.BeginEffectAudio);
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
                //AudioManager.Instance.PlaySound(TileSettings.EndEffectAudio);
            }
            
            Invoke("DeactivateCanvas", TileSettings.DeactivateCanvasDelay);

            // Destroy tiles if indefinite
            if (TileSettings.IsIndefinite)
            {
                StartCoroutine(WaitToDestroy());
            }
        }
        
        public virtual void HandleInput(Player player, OneFitsAllInput input)
        {
            // Play HandleInputAudio
            if (TileSettings.HandleInputAudio != null)
            {
                //AudioManager.Instance.PlaySound(TileSettings.HandleInputAudio);
            }
        }

        // This method gets galled from the 'ObstacleSuccessTileTrigger' which is the Gameobject trigger in the scene
        public virtual void EffectSuccess(Player player)
        {
            if (TileSettings.EffectSuccessAudio != null)
            {
                //AudioManager.Instance.PlaySound(TileSettings.EffectSuccessAudio);
                EventSuccess.start();
            }
            PointsManager.GainPoints(player.ID, TileSettings.EffectSuccessPoints, player.transform.position);
        }

        // This method gets galled from the 'ObstacleSuccessTileTrigger' which is the Gameobject trigger in the scene
        public virtual void EffectFail(Player player)
        {
            if (TileSettings.EffectFailAudio != null)
            {
                //AudioManager.Instance.PlaySound(TileSettings.EffectFailAudio);
                PlayFailButton2.start();
            }
            PointsManager.GainPoints(player.ID, TileSettings.EffectFailPoints, player.transform.position);
            player.GetComponent<HitEffects>().Play();

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
    }
}
