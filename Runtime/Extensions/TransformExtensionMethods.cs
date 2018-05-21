using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class TransformExtensionMethods
{

    /// <summary>
    /// Resets local position and rotation to identity (zero)
    /// </summary>
    public static GameObject AddChild(this Transform tx, string name, params System.Type[] types)
    {
        var g = new GameObject(name, types);
        g.transform.parent = tx;
        g.transform.Zero();
        return g;
    }

    /// <summary>
    /// Resets local position and rotation to identity (zero)
    /// </summary>
    public static void Zero(this Transform tx)
    {
        tx.localPosition = Vector3.zero;
        tx.localRotation = Quaternion.identity;
    }

    /// <summary>
    /// Returns the .position attributes from a list of transforms.
    /// </summary>
    /// <param name='transforms'>
    /// Transforms.
    /// </param>
    public static Vector3[] Positions(this IList<Transform> transforms)
    {
        Vector3[] positions = new Vector3[transforms.Count];
        for (var i = 0; i < transforms.Count; i++)
            positions[i] = transforms[i].position;
        return positions;
    }

    /// <summary>
    /// Returns the .rotation attributes from a list of transforms.
    /// </summary>
    /// <param name='transforms'>
    /// Transforms.
    /// </param>
    public static Quaternion[] Rotations(this IList<Transform> transforms)
    {
        Quaternion[] rotations = new Quaternion[transforms.Count];
        for (var i = 0; i < transforms.Count; i++)
            rotations[i] = transforms[i].rotation;
        return rotations;
    }


    /// <summary>
    /// Smoothly move to a position over T seconds using world or local position.
    /// </summary>
    /// <param name="tx">
    /// A <see cref="Transform"/>
    /// </param>
    /// <param name="position">
    /// A <see cref="Vector3"/> The target position.
    /// </param>
    /// <param name="T">
    /// A <see cref="System.Single"/> The time taken to move to the target.
    /// </param>
    /// <param name="space">Space to operate in, world or local.</param>
    public static void MoveTo(this Transform tx, Vector3 position, float T, Space space)
    {
        if (space == Space.World)
            UniExtender.Instance.StartCoroutine(_MoveTo(tx, position, T, null));
        if (space == Space.Self)
            UniExtender.Instance.StartCoroutine(_MoveToLocal(tx, position, T, null));
    }


    /// <summary>
    /// Smoothly move to a position over T seconds.
    /// </summary>
    /// <param name="tx">
    /// A <see cref="Transform"/>
    /// </param>
    /// <param name="position">
    /// A <see cref="Vector3"/> The target position.
    /// </param>
    /// <param name="T">
    /// A <see cref="System.Single"/> The time taken to move to the target.
    /// </param>
    public static void MoveTo(this Transform tx, Vector3 position, float T)
    {
        UniExtender.Instance.StartCoroutine(_MoveTo(tx, position, T, null));
    }

    /// <summary>
    /// Moves to a position of T seconds, then run an action.
    /// </summary>
    /// <param name='tx'>
    /// Tx.
    /// </param>
    /// <param name='position'>
    /// Position.
    /// </param>
    /// <param name='T'>
    /// T.
    /// </param>
    /// <param name='OnFinish'>
    /// On finish.
    /// </param>

    public static void MoveTo(this Transform tx, Vector3 position, float T, System.Action OnFinish)
    {
        UniExtender.Instance.StartCoroutine(_MoveTo(tx, position, T, OnFinish));
    }

    /// <summary>
    /// Moves to a position of T seconds, then run an action.
    /// </summary>
    /// <param name='tx'>
    /// Tx.
    /// </param>
    /// <param name='position'>
    /// Position.
    /// </param>
    /// <param name='T'>
    /// T.
    /// </param>
    /// <param name='OnFinish'>
    /// On finish.
    /// </param>
    /// <param name="space">Space to operate in, world or local.</param>
    public static void MoveTo(this Transform tx, Vector3 position, float T, Space space, System.Action OnFinish)
    {
        if (space == Space.World)
            UniExtender.Instance.StartCoroutine(_MoveTo(tx, position, T, OnFinish));
        if (space == Space.Self)
            UniExtender.Instance.StartCoroutine(_MoveToLocal(tx, position, T, OnFinish));
    }


    /// <summary>
    /// Smoothly rotate to face direction over T seconds.
    /// </summary>
    /// <param name="tx">
    /// A <see cref="Transform"/>
    /// </param>
    /// <param name="direction">
    /// A <see cref="Vector3"/> The target forward direction.
    /// </param>
    /// <param name="T">
    /// A <see cref="System.Single"/> The time taken to rotate to the target direction.
    /// </param>
    public static void RotateTo(this Transform tx, Vector3 direction, float T)
    {
        UniExtender.Instance.StartCoroutine(_RotateTo(tx, direction, T, null));
    }

    /// <summary>
    /// Rotates to a direction over T seconds, then run an action.
    /// </summary>
    /// <param name='tx'>
    /// Tx.
    /// </param>
    /// <param name='direction'>
    /// Direction.
    /// </param>
    /// <param name='T'>
    /// T.
    /// </param>
    /// <param name='OnFinish'>
    /// On finish.
    /// </param>
    public static void RotateTo(this Transform tx, Vector3 direction, float T, System.Action OnFinish)
    {
        UniExtender.Instance.StartCoroutine(_RotateTo(tx, direction, T, OnFinish));
    }



    /// <summary>
    /// Smoothly rotate to face direction over T seconds.
    /// </summary>
    /// <param name="tx">
    /// A <see cref="Transform"/>
    /// </param>
    /// <param name="direction">
    /// A <see cref="Vector3"/> The target forward direction.
    /// </param>
    /// <param name="T">
    /// A <see cref="System.Single"/> The time taken to rotate to the target direction.
    /// </param>
    /// <param name="space">Space to operate in, world or local.</param>
    public static void RotateTo(this Transform tx, Vector3 direction, float T, Space space)
    {
        if (space == Space.World)
            UniExtender.Instance.StartCoroutine(_RotateTo(tx, direction, T, null));
        if (space == Space.Self)
            UniExtender.Instance.StartCoroutine(_RotateToLocal(tx, direction, T, null));
    }

    /// <summary>
    /// Rotates to a direction over T seconds, then run an action.
    /// </summary>
    /// <param name='tx'>
    /// Tx.
    /// </param>
    /// <param name='direction'>
    /// Direction.
    /// </param>
    /// <param name='T'>
    /// T.
    /// </param>
    /// <param name='OnFinish'>
    /// On finish.
    /// </param>
    /// <param name="space">Space to operate in, world or local.</param>
    public static void RotateTo(this Transform tx, Vector3 direction, float T, Space space, System.Action OnFinish)
    {
        if (space == Space.World)
            UniExtender.Instance.StartCoroutine(_RotateTo(tx, direction, T, OnFinish));
        if (space == Space.Self)
            UniExtender.Instance.StartCoroutine(_RotateToLocal(tx, direction, T, OnFinish));
    }


    /// <summary>
    /// Pivot on vertical axis to face a direction in degrees.
    /// </summary>
    /// <param name="tx">
    /// A <see cref="Transform"/>
    /// </param>
    /// <param name="degrees">
    /// A <see cref="System.Single"/> The target degrees of the vertical axis.
    /// </param>
    /// <param name="T"> The time taken to move to the target.
    /// A <see cref="System.Single"/>
    /// </param>
    public static void PivotTo(this Transform tx, float degrees, float T)
    {
        UniExtender.Instance.StartCoroutine(_ChangeRotation(tx, Quaternion.RotateTowards(tx.rotation, Quaternion.LookRotation(tx.right), degrees), T, null));
    }

    /// <summary>
    /// Smoothly rotate to look at a position over T seconds.
    /// </summary>
    /// <param name="tx">
    /// A <see cref="Transform"/>
    /// </param>
    /// <param name="position">
    /// A <see cref="Vector3"/> The target world space position to look at.
    /// </param>
    /// <param name="T">
    /// A <see cref="System.Single"/> The time taken to rotate to look at the target.
    /// </param>
    ///
    public static void LookAt(this Transform tx, Vector3 position, float T)
    {
        UniExtender.Instance.StartCoroutine(_ChangeRotation(tx, Quaternion.LookRotation(position - tx.position), T, null));
    }

    /// <summary>
    /// Smooth rotate to look at a target over T seconds.
    /// </summary>
    /// <param name="tx">
    /// A <see cref="Transform"/>
    /// </param>
    /// <param name="target">
    /// A <see cref="Transform"/> The transform to look at.
    /// </param>
    /// <param name="T">
    /// A <see cref="System.Single"/> The time taken to rotate to look at the target.
    /// </param>
    public static void LookAt(this Transform tx, Transform target, float T)
    {
        UniExtender.Instance.StartCoroutine(_ChangeRotation(tx, Quaternion.LookRotation(target.position - tx.position), T, null));
    }

    /// <summary>
    /// Pivots to a direction over T seconds, then run an action.
    /// </summary>
    public static void PivotTo(this Transform tx, float degrees, float T, System.Action OnFinish)
    {
        UniExtender.Instance.StartCoroutine(_ChangeRotation(tx, Quaternion.RotateTowards(tx.rotation, Quaternion.LookRotation(tx.right), degrees), T, OnFinish));
    }

    /// <summary>
    /// Looks at a direction over T seconds, then run an action.
    /// </summary>
    public static void LookAt(this Transform tx, Vector3 position, float T, System.Action OnFinish)
    {
        UniExtender.Instance.StartCoroutine(_ChangeRotation(tx, Quaternion.LookRotation(position - tx.position), T, OnFinish));
    }

    /// <summary>
    /// Looks at a transform over T seconds, then run an action.
    /// </summary>
    public static void LookAt(this Transform tx, Transform target, float T, System.Action OnFinish)
    {
        UniExtender.Instance.StartCoroutine(_ChangeRotation(tx, Quaternion.LookRotation(target.position - tx.position), T, OnFinish));
    }


    /// <summary>
    /// Returns a screen rect that is centred over this transform.
    /// </summary>
    /// <param name="tx">
    /// A <see cref="Transform"/>
    /// </param>
    /// <param name="width">
    /// A <see cref="System.Single"/> The width of the rectangle.
    /// </param>
    /// <param name="height">
    /// A <see cref="System.Single"/> The height of the rectangle.
    /// </param>
    /// <returns>
    /// A <see cref="Rect"/>
    /// </returns>
    public static Rect ScreenRect(this Transform tx, float width, float height)
    {
        var pos = Camera.main.WorldToScreenPoint(tx.position);
        pos.y = Screen.height - pos.y;
        var r = new Rect(pos.x - (width / 2), pos.y - (height / 2), width, height);
        return r;
    }

    /// <summary>
    /// Returns the closest transform from a list of transforms.
    /// </summary>
    /// <param name="tx">
    /// A <see cref="Transform"/>
    /// </param>
    /// <param name="list">
    /// A <see cref="IList(Transform)"/> The list of transforms to search through.
    /// </param>
    /// <returns>
    /// A <see cref="Transform"/>
    /// </returns>
    public static Transform FindClosest(this Transform tx, IList<Transform> list)
    {
        Transform t = null;
        var closest = -0f;
        for (int j = 0; j < list.Count; j++)
        {
            var i = list[j];
            var m = (i.position - tx.position).sqrMagnitude;
            if (t == null)
            {
                closest = m;
                t = i;
            }
            else
            {
                if (m < closest)
                {
                    t = i;
                    closest = m;
                }
            }
        }
        return t;
    }

    /// <summary>
    /// Returnts the furthest transform from a list of transforms.
    /// </summary>
    /// <param name="tx">
    /// A <see cref="Transform"/>
    /// </param>
    /// <param name="list">
    /// A <see cref="IList(Transform)"/> The list of transforms to search through.
    /// </param>
    /// <returns>
    /// A <see cref="Transform"/>
    /// </returns>
    public static Transform FindFurthest(this Transform tx, IList<Transform> list)
    {
        Transform t = null;
        var furthest = -0f;
        for (int j = 0; j < list.Count; j++)
        {
            var i = list[j];
            var m = (i.position - tx.position).sqrMagnitude;
            if (t == null)
            {
                furthest = m;
                t = i;
            }
            else
            {
                if (m > furthest)
                {
                    t = i;
                    furthest = m;
                }
            }
        }
        return t;
    }

    /// <summary>
    /// Smoothly move along a path over T seconds.
    /// </summary>
    /// <param name="tx">
    /// A <see cref="Transform"/>
    /// </param>
    /// <param name="path">
    /// A <see cref="IList(Transform)"/> The list of transforms which make up the the path.
    /// </param>
    /// <param name="T"> The time taken to move from start to finish.
    /// A <see cref="System.Single"/>
    /// </param>
    public static void MoveAlong(this Transform tx, IList<Vector3> path, float T)
    {
        UniExtender.Instance.StartCoroutine(_MoveAlong(tx, path, T, null));
    }

    /// <summary>
    /// Smoothly move along a path over T seconds, looking at a transform.
    /// </summary>
    /// <param name="tx">
    /// A <see cref="Transform"/>
    /// </param>
    /// <param name="path">
    /// A <see cref="IList(Transform)"/> The list of transforms which make up the path.
    /// </param>
    /// <param name="T">
    /// A <see cref="System.Single"/> The time taken to move from start to finish.
    /// </param>
    /// <param name="lookAt">
    /// A <see cref="Transform"/> The transform to look at while moving.
    /// </param>
    public static void MoveAlong(this Transform tx, IList<Vector3> path, float T, Transform lookAt)
    {
        UniExtender.Instance.StartCoroutine(_MoveAlong(tx, path, T, lookAt, null));
    }

    /// <summary>
    /// Smoothly move along a path over T seconds, optionaly looking forward along the path.
    /// </summary>
    /// <param name="tx">
    /// A <see cref="Transform"/>
    /// </param>
    /// <param name="path">
    /// A <see cref="IList(Transform)"/> The list of transforms which make up the path.
    /// </param>
    /// <param name="T">
    /// A <see cref="System.Single"/> The time taken to move from start to finish.
    /// </param>
    /// <param name="lookAlongPath">
    /// A <see cref="System.Boolean"/> If true, always look in the direction of travel.
    /// </param>
    public static void MoveAlong(this Transform tx, IList<Vector3> path, float T, bool lookAlongPath)
    {
        UniExtender.Instance.StartCoroutine(_MoveAlong(tx, path, T, lookAlongPath, null));
    }

    /// <summary>
    /// Smoothly move along a path over T seconds, then run an action.
    /// </summary>
    public static void MoveAlong(this Transform tx, IList<Vector3> path, float T, System.Action OnFinish)
    {
        UniExtender.Instance.StartCoroutine(_MoveAlong(tx, path, T, OnFinish));
    }

    /// <summary>
    /// Smoothly move along a path over T seconds, then run an action.
    /// </summary>
    public static void MoveAlong(this Transform tx, IList<Vector3> path, float T, Transform lookAt, System.Action OnFinish)
    {
        UniExtender.Instance.StartCoroutine(_MoveAlong(tx, path, T, lookAt, OnFinish));
    }

    /// <summary>
    /// Smoothly move along a path over T seconds, then run an action.
    /// </summary>
    public static void MoveAlong(this Transform tx, IList<Vector3> path, float T, bool lookAlongPath, System.Action OnFinish)
    {
        UniExtender.Instance.StartCoroutine(_MoveAlong(tx, path, T, lookAlongPath, OnFinish));
    }



    /// <summary>
    /// Shake by amount units over T seconds.
    /// </summary>
    /// <param name="tx">
    /// A <see cref="Transform"/>
    /// </param>
    /// <param name="amount">
    /// A <see cref="System.Single"/> Maximum amount of shake, in world space.
    /// </param>
    /// <param name="T"> Shake for this amount of seconds.
    /// A <see cref="System.Single"/>
    /// </param>
    public static void Shake(this Transform tx, float amount, float T)
    {
        var startPosition = tx.position;
        UniExtender.Instance.StartCoroutine(UniExtender.Step(T, delegate (float P)
        {
            tx.position = startPosition + (Random.onUnitSphere * amount * (1 - P));
        }));
    }

    /// <summary>
    /// Shiver an object by amount degrees over T seconds.
    /// </summary>
    /// <param name="tx">
    /// A <see cref="Transform"/>
    /// </param>
    /// <param name="amount">
    /// A <see cref="System.Single"/> Maximum amount of shiver, in degrees.
    /// </param>
    /// <param name="T">
    /// A <see cref="System.Single"/> Shiver for this amount of seconds.
    /// </param>
    public static void Shiver(this Transform tx, float amount, float T)
    {
        var startRotation = tx.eulerAngles;
        UniExtender.Instance.StartCoroutine(UniExtender.Step(T, delegate (float P)
        {
            tx.eulerAngles = startRotation + (Random.onUnitSphere * amount * (1 - P));
        }));
    }

    /// <summary>
    /// Places the transform P (0-1) along path.
    /// </summary>
    /// <param name='tx'>
    /// Tx.
    /// </param>
    /// <param name='path'>
    /// Path.
    /// </param>
    /// <param name='P'>
    /// P.
    /// </param>
    public static void PlaceOnPath(this Transform tx, IList<Transform> path, float P)
    {
        PlaceOnPath(tx, path.Positions(), P);
    }

    /// <summary>
    /// Place this transform P (0-1) along path.
    /// </summary>
    /// <param name="tx">
    /// A <see cref="Transform"/>
    /// </param>
    /// <param name="path">
    /// A <see cref="IList(Vector3)"/> The list of transforms that make up the path.
    /// </param>
    /// <param name="P">
    /// A <see cref="System.Single"/> The percentage value (0-1) along the path.
    /// </param>
    public static void PlaceOnPath(this Transform tx, IList<Vector3> path, float P)
    {
        var points = new Vector3[path.Count + 2];
        for (var i = 0; i < path.Count; i++)
            points[i + 1] = path[i];
        points[0] = path[1];
        points[points.Length - 1] = path[path.Count - 1];
        P = Mathf.Clamp01(P);
        var sections = points.Length - 3;
        var idx = Mathf.Min(Mathf.FloorToInt(P * (float)sections), sections - 1);
        var u = P * (float)sections - (float)idx;
        var a = points[idx];
        var b = points[idx + 1];
        var c = points[idx + 2];
        var d = points[idx + 3];
        tx.position = 0.5f * ((-a + 3f * b - 3f * c + d) * (u * u * u) + (2f * a - 5f * b + 4f * c - d) * (u * u) + (-a + c) * u + 2f * b);
    }

    /// <summary>
    /// Determines whether this instance has line of sight to another transform.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance has line of sight to the specified other; otherwise, <c>false</c>.
    /// </returns>
    /// <param name='tx'>
    /// If set to <c>true</c> tx.
    /// </param>
    /// <param name='other'>
    /// If set to <c>true</c> other.
    /// </param>
    public static bool HasLineOfSightTo(this Transform tx, Transform other)
    {
        var delta = other.position - tx.position;
        RaycastHit hit;
        if (Physics.Raycast(tx.position, delta, out hit))
        {
            if (hit.transform == other)
                return true;
        }
        return false;
    }

    /// <summary>
    /// Distances to point.
    /// </summary>
    /// <returns>
    /// The to.
    /// </returns>
    /// <param name='tx'>
    /// Tx.
    /// </param>
    /// <param name='position'>
    /// Position.
    /// </param>
    public static float DistanceTo(this Transform tx, Vector3 position)
    {
        return (tx.position - position).magnitude;
    }

    /// <summary>
    /// Direction to point.
    /// </summary>
    /// <returns>
    /// The to.
    /// </returns>
    /// <param name='tx'>
    /// Tx.
    /// </param>
    /// <param name='position'>
    /// Position.
    /// </param>
    public static Vector3 DirectionTo(this Transform tx, Vector3 position)
    {
        return (tx.position - position).normalized;
    }

    /// <summary>
    /// Determines whether this instance is near the specified tx position + threshold.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance is near the specified position + threshold; otherwise, <c>false</c>.
    /// </returns>
    /// <param name='tx'>
    /// If set to <c>true</c> tx.
    /// </param>
    /// <param name='position'>
    /// If set to <c>true</c> position.
    /// </param>
    /// <param name='threshold'>
    /// If set to <c>true</c> threshold.
    /// </param>
    public static bool IsNear(this Transform tx, Vector3 position, float threshold)
    {
        return (tx.position - position).sqrMagnitude <= (threshold * threshold);
    }

    /// <summary>
    /// Returns children of the transform.
    /// </summary>
    /// <param name='tx'>
    /// Tx.
    /// </param>
    public static Transform[] Children(this Transform tx)
    {
        var list = new List<Transform>();
        foreach (Transform t in tx)
        {
            list.Add(t);
        }
        return list.ToArray();
    }

    static IEnumerator _MoveTo(Transform tx, Vector3 position, float T, System.Action OnFinish)
    {
        var P = 0f;
        Vector3 start = tx.position;
        while (P <= 1f && tx != null)
        {
            P = P + (Time.deltaTime / T);
            tx.position = Vector3.Lerp(start, position, Mathf.SmoothStep(0, 1, P));
            yield return null;
        }
        if (OnFinish != null)
            OnFinish();
    }

    static IEnumerator _MoveToLocal(Transform tx, Vector3 position, float T, System.Action OnFinish)
    {
        var P = 0f;
        Vector3 start = tx.localPosition;
        while (P <= 1f && tx != null)
        {
            P = P + (Time.deltaTime / T);
            tx.localPosition = Vector3.Lerp(start, position, Mathf.SmoothStep(0, 1, P));
            yield return null;
        }
        if (OnFinish != null)
            OnFinish();
    }

    static IEnumerator _RotateTo(Transform tx, Vector3 forward, float T, System.Action OnFinish)
    {
        var P = 0f;
        var start = tx.rotation;
        var end = Quaternion.LookRotation(forward);
        while (P <= 1f && tx != null)
        {
            P = (P + (Time.deltaTime / T));
            tx.rotation = Quaternion.Slerp(start, end, Mathf.SmoothStep(0, 1, P));
            yield return null;
        }
        if (OnFinish != null)
            OnFinish();
    }

    static IEnumerator _RotateToLocal(Transform tx, Vector3 forward, float T, System.Action OnFinish)
    {
        var P = 0f;
        var start = tx.localRotation;
        var end = Quaternion.LookRotation(forward);
        while (P <= 1f && tx != null)
        {
            P = (P + (Time.deltaTime / T));
            tx.localRotation = Quaternion.Slerp(start, end, Mathf.SmoothStep(0, 1, P));
            yield return null;
        }
        if (OnFinish != null)
            OnFinish();
    }

    static IEnumerator _ChangeRotation(Transform tx, Quaternion rotation, float T, System.Action OnFinish)
    {
        var P = 0f;
        var start = tx.rotation;
        while (P <= 1f && tx != null)
        {
            P = (P + (Time.deltaTime / T));
            tx.rotation = Quaternion.Slerp(start, rotation, Mathf.SmoothStep(0, 1, P));
            yield return null;
        }
        if (OnFinish != null)
            OnFinish();
    }

    static IEnumerator _MoveAlong(Transform tx, IList<Vector3> path, float T, System.Action OnFinish)
    {
        var points = new Vector3[path.Count + 2];
        for (var i = 0; i < path.Count; i++)
            points[i + 1] = path[i];
        points[0] = path[1];
        points[points.Length - 1] = path[path.Count - 1];

        var P = 0f;
        while (P <= 1f && tx != null)
        {
            P = (P + (Time.deltaTime / T));
            var sections = points.Length - 3;
            var i = Mathf.Min(Mathf.FloorToInt(P * (float)sections), sections - 1);
            var u = P * (float)sections - (float)i;
            var a = points[i];
            var b = points[i + 1];
            var c = points[i + 2];
            var d = points[i + 3];
            tx.position = 0.5f * ((-a + 3f * b - 3f * c + d) * (u * u * u) + (2f * a - 5f * b + 4f * c - d) * (u * u) + (-a + c) * u + 2f * b);
            yield return null;
        }
        if (OnFinish != null)
            OnFinish();
    }

    static IEnumerator _MoveAlong(Transform tx, IList<Vector3> path, float T, Transform lookAt, System.Action OnFinish)
    {
        var task = _MoveAlong(tx, path, T, OnFinish);
        while (task.MoveNext())
        {
            tx.LookAt(lookAt);
            yield return null;
        }
        if (OnFinish != null)
            OnFinish();
    }

    static IEnumerator _MoveAlong(Transform tx, IList<Vector3> path, float T, bool lookAlongPath, System.Action OnFinish)
    {
        var task = _MoveAlong(tx, path, T, OnFinish);
        Vector3 position = tx.position;
        while (task.MoveNext())
        {
            if (lookAlongPath)
            {
                var delta = tx.position - position;
                position = tx.position;
                if (delta.sqrMagnitude > 0)
                    tx.rotation = Quaternion.LookRotation(delta);
            }
            yield return null;
        }
        if (OnFinish != null)
            OnFinish();
    }

}

