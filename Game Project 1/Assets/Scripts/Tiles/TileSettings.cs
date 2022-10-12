using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tiles
{
    [CreateAssetMenu(menuName = "Tile Settings")]
    public class TileSettings : ScriptableObject
    {
        [Tooltip("A short descriptor for the behaviour of this tile.")]
        [SerializeField] protected string _description;

        [Header("Basic")]
        public AudioClip BeginEffectAudio;
        public AudioClip EndEffectAudio;
        public AudioClip HandleInputAudio;
        public Sprite BeginEffectSprite;
        public bool IsIndefinite;
        
        [Header("Advanced")]
        public List<AudioClip> MiscellaneousAudioClips;
        public List<Sprite> MiscellaneousSprites;
    }
}
