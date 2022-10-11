using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Tiles
{
    public abstract class TileSettings : ScriptableObject
    {
        [Tooltip("A short descriptor for the behaviour of this tile.")]
        [SerializeField] protected string _description;
        [SerializeField] protected List<AudioClip> _audioClips;
        [SerializeField] protected Sprite _sprite;

        public static event Action<TileSettings> OnTileChanged;

        public virtual void BeginEffect()
        {
            // Subscribe inputs to this tile's effect
            OnTileChanged?.Invoke(this);
        }

        public virtual void EndEffect()
        {
            // Unsubscribe inputs to this tile
            OnTileChanged?.Invoke(null);
        }

        public abstract void HandleInput(int playerId);
    }
}
