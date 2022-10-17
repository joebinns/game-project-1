using System;
using System.Collections.Generic;
using Players.Physics_Based_Character_Controller;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Tiles
{
    [CreateAssetMenu(menuName = "Tile Settings")]
    public class TileSettings : ScriptableObject
    {
        [Tooltip("A short descriptor for the behaviour of this tile.")]
        [SerializeField] protected string _description;

        [Header("Options")]
        public PhysicsBasedCharacterController.MovementOptions MovementOption =
            PhysicsBasedCharacterController.MovementOptions.Default;
        public bool IsIndefinite;

        [Header("Begin Effect")]
        public AudioClip BeginEffectAudio;
        public string BeginEffectText;
        public EffectAnimation EffectAnimation = EffectAnimation.None;
        
        [Header("Handle Input")]
        public AudioClip HandleInputAudio;
        
        [Header("Effect Success")]
        public AudioClip EffectSuccessAudio;
        public int EffectSuccessPoints;
        
        [Header("Effect Fail")]
        public AudioClip EffectFailAudio;
        public int EffectFailPoints;
        
        [Header("End Effect")]
        public AudioClip EndEffectAudio;
        public float DeactivateCanvasDelay = 0f;

        [Header("Advanced")]
        public List<AudioClip> MiscellaneousAudioClips;
        public List<Sprite> MiscellaneousSprites;
        public Color EffectColor = Color.white;
    }
}
