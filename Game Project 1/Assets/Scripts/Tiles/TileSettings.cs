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
        public List<AudioClip> audioClips;
        public Sprite sprite;
        public bool isIndefinite;
    }
}
