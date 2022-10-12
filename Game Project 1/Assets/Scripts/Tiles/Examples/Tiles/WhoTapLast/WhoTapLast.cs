using System;
using System.Collections;
using System.Collections.Generic;
using Managers.Audio;
using Managers.Points;
using Tiles;
using UnityEngine;

public class WhoTapLast : Tile
{
    private float t = 3f;
    
    private float _player0Time, _player1Time;

    private void Update()
    {
        if (t <= 0)
        {
            EndEffect();
        }

        Debug.Log(t);
    }

    public override void BeginEffect()
    {
        // Redirect inputs to this tile
        base.BeginEffect();

        AudioManager.PlaySound(TileSettings.audioClips[0]); // Play Demo Sound
        // UI Manager change sprite to _sprite

        Debug.Log("Who Tap Last Tile in effect");
            
        StartCoroutine(Cooldown());
    }
        
    public override void EndEffect()
    {
        if (_player0Time < _player1Time)
        {
            PointsManager.GainPoints(0, 100);
            Debug.Log("Player 1 got 100 Points");
        }

        else
        {
            PointsManager.GainPoints(1, 100);
            Debug.Log("Player 2 got 100 Points");
        }
        
        AudioManager.PlaySound(TileSettings.audioClips[0]); // Play Demo Sound
        // UI Manager change sprite to null
            
        RoadGenerator roadGen = FindObjectOfType<RoadGenerator>();

        Debug.Log("Who Tap Last Tile no longer in effect");
        
        // Redirect inputs back to player
        //ALWAYS CALL THIS AT THE END OF EndEffect() !!!!!!!
        base.EndEffect();
    }

    private IEnumerator Cooldown()
    {
        while (t >= 0)
        {
            t -= Time.deltaTime;
            //Update UI     
            yield return null;
        }

        yield return null;
    }
        
    public override void HandleInput(int playerId)
    {
        AudioManager.PlaySound(TileSettings.audioClips[1]); // Play Demo Sound 2

        if (t > 0)
        {
            switch (playerId)
            {
                case 0:
                    _player0Time = t;
                    break;
                
                case 1:
                    _player1Time = t;
                    break;
            }
        }
    }
    
}
