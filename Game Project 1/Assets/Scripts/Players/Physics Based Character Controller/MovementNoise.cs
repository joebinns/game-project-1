using DG.Tweening;
using Players.Physics_Based_Character_Controller;
using UnityEngine;

public class MovementNoise : MonoBehaviour
{
    [SerializeField] private Vector2 _rates = new Vector2(1.5f, 1.5f);
    [SerializeField] private Vector2 _magnitudes = new Vector2(0.15f, 0.15f);
    
    private PhysicsBasedCharacterController _physicsBasedCharacterController;
    private Vector2 _offsets;
    private float _t;

    private void Awake()
    {
        _t = 0f;
        _physicsBasedCharacterController = GetComponent<PhysicsBasedCharacterController>();
        _offsets = new Vector2(Random.Range(0, Mathf.PI), Random.Range(0, Mathf.PI));
    }

    private void Update()
    {
        _t += Time.deltaTime;
        // Lerp rate from 0 to _rate as _t goes from 0 to _offset (To try to mitigate the player oscillating away from the desired centre)
        var magnitudeX = Mathf.Lerp(0f, _magnitudes.x, _t / _offsets.x);
        var magnitudeY = Mathf.Lerp(0f, _magnitudes.y, _t / _offsets.y);
        var magnitude = new Vector2(magnitudeX, magnitudeY);
        var moveX = magnitude.x * Mathf.Sin((_t + _offsets.x) * _rates.x);
        var moveY = magnitude.y * Mathf.Sin((_t + _offsets.y) * _rates.y);
        var move = new Vector2(moveX, moveY);
        //this.GetComponent<Rigidbody>().MovePosition(new Vector3(move.x, 0, move.y));
        _physicsBasedCharacterController.Move(move); // TODO: Due to the oscillation drifting away from the desired centre, try just moving the player tranform position directly
    }
}
