using DG.Tweening;
using Players.Physics_Based_Character_Controller;
using UnityEngine;

public class MovementNoise : MonoBehaviour
{
    [SerializeField] private Vector2 _rates = new Vector2(1.5f, 1.5f);
    [SerializeField] private Vector2 _magnitudes = new Vector2(0.15f, 0.15f);
    [SerializeField] private Vector3 _torqueMagnitudes = new Vector3(0.1f, 0.1f, 0.1f);

    private RoadGenerator _roadGenerator;
    private SpeedSelector _speedSelector;
    private PhysicsBasedCharacterController _physicsBasedCharacterController;
    private Vector2 _offsets;
    private float _t;
    private Vector3 _defaultPosition;
    private Rigidbody _rb;

    private void Awake()
    {
        _t = 0f;
        _roadGenerator = FindObjectOfType<RoadGenerator>();
        _speedSelector = FindObjectOfType<SpeedSelector>();
        _physicsBasedCharacterController = GetComponent<PhysicsBasedCharacterController>();
        _rb = GetComponent<Rigidbody>();
        //_offsets = new Vector2(Random.Range(0, Mathf.PI), Random.Range(0, Mathf.PI));
        _offsets = Vector2.zero;
        _defaultPosition = transform.position;
    }

    private void Update()
    {
        _t += Time.deltaTime;

        var magnitudeX = Mathf.Lerp(_magnitudes.x * 0.25f, _magnitudes.x, 1f - (_roadGenerator.roadSpeed / _speedSelector.AbsoluteMaxSpeed));
        var magnitudeY = _magnitudes.y;
        var magnitude = new Vector2(magnitudeX, magnitudeY);

        var rateX = Mathf.Lerp(_rates.x * 0.25f, _rates.x, (_roadGenerator.roadSpeed / _speedSelector.AbsoluteMaxSpeed));
        var rateY = _rates.y;
        var rate = new Vector2(rateX, rateY);
            
        var moveX = magnitude.x * Mathf.Sin((_t + _offsets.x) * rate.x);
        var moveY = magnitude.y * Mathf.Sin((_t + _offsets.y) * rate.y);
        var move = new Vector3(moveX, moveY, 0f);
        
        var playerPosition = new Vector3(_defaultPosition.x, this.GetComponent<Rigidbody>().position.y, _defaultPosition.z);
        var playerPositionWithNoise = new Vector3(playerPosition.x + move.x, playerPosition.y, playerPosition.z + move.y);
        _rb.MovePosition(playerPositionWithNoise);
        
        var torque = -Vector3.Cross(move, _rb.transform.forward);
        torque = new Vector3(torque.x * _torqueMagnitudes.x, torque.y * _torqueMagnitudes.y, torque.y * _torqueMagnitudes.z);
        _rb.AddTorque(torque);
    }
}
