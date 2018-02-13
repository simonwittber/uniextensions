using UnityEngine;
using System.Collections;


public static class RigidbodyExtensionMethods
{
    /// <summary>
    /// LookAt target by applying torque forces.
    /// </summary>
    /// <param name='rigidbody'>
    /// Rigidbody.
    /// </param>
    /// <param name='target'>
    /// Target.
    /// </param>
    /// <param name='force'>
    /// Force.
    /// </param>
    public static void LookAt(this Rigidbody rigidbody, Vector3 target, float force)
    {
        var transform = rigidbody.transform;
        var delta = target - transform.position;
        var m = Vector3.Angle(transform.forward, delta);
        var cross = Vector3.Cross(transform.forward, delta);
        rigidbody.AddTorque(cross * m * force);
    }


    /// <summary>
    /// Stabilizes the pitch and roll of a rigid body.
    /// </summary>
    /// <param name='rigidbody'>
    /// Rigidbody.
    /// </param>
    /// <param name='stability'>
    /// Stability.
    /// </param>
    /// <param name='speed'>
    /// Speed.
    /// </param>
    public static void StabilizePitchAndRoll(this Rigidbody rigidbody, float stability, float speed)
    {
        var up = Quaternion.AngleAxis(
        rigidbody.angularVelocity.magnitude * Mathf.Rad2Deg * stability / speed,
        rigidbody.angularVelocity
        ) * rigidbody.transform.up;

        var torque = Vector3.Cross(up, Vector3.up);
        rigidbody.AddTorque(torque * speed);
    }


    /// <summary>
    /// Stabilizes the roll of a rigidbody.
    /// </summary>
    /// <param name='rigidbody'>
    /// Rigidbody.
    /// </param>
    /// <param name='stability'>
    /// Stability.
    /// </param>
    /// <param name='speed'>
    /// Speed.
    /// </param>
    public static void StabilizeRoll(this Rigidbody rigidbody, float stability, float speed)
    {
        var transform = rigidbody.transform;
        var up = Quaternion.AngleAxis(
        rigidbody.angularVelocity.magnitude * Mathf.Rad2Deg * stability / speed,
        rigidbody.angularVelocity
        ) * transform.up;
        var torque = Vector3.Cross(up, Vector3.up);
        torque = Vector3.Project(torque, transform.forward);
        rigidbody.AddTorque(torque * speed);
    }
}
