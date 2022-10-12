using System.Collections;
using System.Collections.Generic;
using Tiles;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    public enum GenerationModes
    {
        Normal,
        Indefinite
    }
    
    public GenerationModes generationMode;
    
    public float roadSpeed = 2.5f;

    public int tilesUntilEnd = 50;

    public bool shouldGenerate;
    
    public GameObject[] tilesToGenerate;

    [SerializeField] private Camera mainCam;

    [SerializeField] private GameObject startTile, endTile;
    [SerializeField] private GameObject[] indefiniteTiles;

    private GameObject _lastGeneratedTile;

    private List<GameObject> _activeTileList = new List<GameObject>();
    private Queue<GameObject> _destroyQueue = new Queue<GameObject>();

    void Start()
    {
        generationMode = GenerationModes.Normal;
        shouldGenerate = true;
        
        _lastGeneratedTile = Instantiate(startTile, Vector3.zero, Quaternion.identity);
        _activeTileList.Add(_lastGeneratedTile);

        for (int i = 0; i < 9; i++)
        {
            GenerateNextTile();
        }
    }

    public void RemoveActiveTile(GameObject tileToRemove)
    {
        _activeTileList.Remove(tileToRemove);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            generationMode = GenerationModes.Normal;
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            generationMode = GenerationModes.Indefinite;
        }
        
        foreach (GameObject tile in _activeTileList)
        {
            tile.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y,
                tile.transform.position.z + (roadSpeed * Time.deltaTime));

            if (tile.GetComponent<TileInfo>().endPoint.position.z > mainCam.transform.position.z)
            {
                if (tile.GetComponent<Tile>() != null)
                {
                    if(tile.GetComponent<Tile>().TileSettings.isIndefinite == false)
                    {
                        _destroyQueue.Enqueue(tile);
                    }
                }

                else
                {
                    _destroyQueue.Enqueue(tile);
                }
                
            }
        }

        if (_destroyQueue.Count > 0 && shouldGenerate)
        {
            GameObject toDestroy = _destroyQueue.Dequeue();

            _activeTileList.Remove(toDestroy);
            GenerateNextTile();
            Destroy(toDestroy);
            
        }
    }

    public void GenerateNextTile()
    {
        int tilePicker;
        GameObject tileToGenerate;
        
        switch (generationMode)
        {
            case GenerationModes.Normal:

                //Pick a number within the tile-array range
                 tilePicker = Random.Range(0, tilesToGenerate.Length);
                //Set the tile to be generated
                 tileToGenerate = tilesToGenerate[tilePicker];
                //Generate the tile
                _lastGeneratedTile = Instantiate(tileToGenerate, _lastGeneratedTile.GetComponent<TileInfo>().endPoint.position, Quaternion.identity);
                _activeTileList.Add(_lastGeneratedTile);
                
                tilesUntilEnd--;
                
                break;
            
            case GenerationModes.Indefinite:

                //Pick a number within the tile-array range
                tilePicker = Random.Range(0, indefiniteTiles.Length);
                //Set the tile to be generated
                tileToGenerate = indefiniteTiles[tilePicker];
                //Generate the tile
                _lastGeneratedTile = Instantiate(tileToGenerate, _lastGeneratedTile.GetComponent<TileInfo>().endPoint.position, Quaternion.identity);
                _activeTileList.Add(_lastGeneratedTile);
                
                break;
        }
        
        if (tilesUntilEnd == 0)
        {
            GenerateEnd();
        }
    }

    private void GenerateEnd()
    {
        shouldGenerate = false;
        
        _lastGeneratedTile = Instantiate(endTile, _lastGeneratedTile.GetComponent<TileInfo>().endPoint.position, Quaternion.identity);
        _activeTileList.Add(_lastGeneratedTile);
    }
}
