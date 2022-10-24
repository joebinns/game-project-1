using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;


public class EnvironmentGenerator : MonoBehaviour
{
    [SerializeField] private Camera mainCam;

    [SerializeField] private Transform firstRoadPos;
    
    [SerializeField] private GameObject[] roadArray;
    [SerializeField] private GameObject[] buildingArray;

    public float roadSpeed = 10f;
    public float buildingSpeed = 7.5f;
    
    private GameObject _lastGeneratedRoad, _lastGeneratedBuilding;
    private Transform _lastGeneratedRoadEndPos, _lastGeneratedBuildingEndPos;

    private List<GameObject> _activeRoadPieces = new List<GameObject>();
    private List<GameObject> _activeBuildingPieces = new List<GameObject>();
    private Queue<GameObject> _objectsToDestroy = new Queue<GameObject>();

    private void Start()
    {
        _lastGeneratedRoadEndPos = firstRoadPos;
        _lastGeneratedBuildingEndPos = firstRoadPos;
        
        //Generate 100 road pieces
        for (int i = 0; i < 100; i++)
        {
            GenerateNewRoad();
        }
        
        //Generate 20 buildings (on each side)
        for (int i = 0; i < 20; i++)
        {
            GenerateNewBuildings();
        }
    }

    private void Update()
    {
        //Move road
        foreach (GameObject roadPiece in _activeRoadPieces)
        {
            roadPiece.transform.position = new Vector3(roadPiece.transform.position.x, roadPiece.transform.position.y,
                roadPiece.transform.position.z + (Time.deltaTime * roadSpeed));

            if (roadPiece.GetComponent<RoadInfo>().endPoint.position.z > mainCam.transform.position.z)
            {
                _objectsToDestroy.Enqueue(roadPiece);
            }
        }
        
        //Move buildings
        foreach (GameObject building in _activeBuildingPieces)
        {
            building.transform.position = new Vector3(building.transform.position.x, building.transform.position.y,
                building.transform.position.z + (Time.deltaTime * buildingSpeed));

            if (building.GetComponent<RoadInfo>().endPoint.position.z > mainCam.transform.position.z)
            {
                _objectsToDestroy.Enqueue(building);
            }
        }
        
        //Destroy leftover objects
        if (_objectsToDestroy.Count > 0)
        {
            GameObject objectToDestroy = _objectsToDestroy.Dequeue();
            if (_activeRoadPieces.Contains(objectToDestroy))
            {
                _activeRoadPieces.Remove(objectToDestroy);

                GenerateNewRoad();
            }

            if (_activeBuildingPieces.Contains(objectToDestroy))
            {
                _activeBuildingPieces.Remove(objectToDestroy);
                
                GenerateNewBuildings();
            }
            
            Destroy(objectToDestroy);
        }
    }

    private void GenerateNewRoad()
    {
        int roadPicker = Random.Range(0, roadArray.Length);
        GameObject roadToGenerate = roadArray[roadPicker];

        _lastGeneratedRoad = Instantiate(roadToGenerate, _lastGeneratedRoadEndPos.position, quaternion.identity);
        _lastGeneratedRoadEndPos = _lastGeneratedRoad.GetComponent<RoadInfo>().endPoint;
            
        _activeRoadPieces.Add(_lastGeneratedRoad);
    }

    private void GenerateNewBuildings()
    {
        int buildingPicker = Random.Range(0, buildingArray.Length);
        GameObject buildingToGenerate = buildingArray[buildingPicker];

        _lastGeneratedBuilding = Instantiate(buildingToGenerate, _lastGeneratedBuildingEndPos.position, quaternion.identity);
        _lastGeneratedBuildingEndPos = _lastGeneratedBuilding.GetComponent<RoadInfo>().endPoint;
            
        _activeBuildingPieces.Add(_lastGeneratedBuilding);
    }
}
