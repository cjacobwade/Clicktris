using UnityEngine;
using System.Collections;

public class RigidbodyDebugger : WadeBehaviour 
{
#pragma warning disable 414
    [SerializeField]
    Vector3 _velocity = Vector3.zero;

    Vector3 _prevVelocity = Vector3.zero;

    [SerializeField]
    Vector3 _velocityDelta = Vector3.zero;

    [SerializeField]
    float _magnitude = 0f;

    [SerializeField]
    float _magnitudeDelta = 0f;

    void FixedUpdate()
    {
        _prevVelocity = _velocity;
        _velocity = rigidbody.velocity;

        _velocityDelta = _velocity - _prevVelocity;

        _magnitude = _velocity.magnitude;
        _magnitudeDelta = _velocityDelta.magnitude;
    }
#pragma warning restore 414
}
