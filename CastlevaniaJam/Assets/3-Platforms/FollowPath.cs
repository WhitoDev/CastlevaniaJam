using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowPath : MonoBehaviour {

	public enum FollowType
	{
		MoveTowards,
		Lerp,
	}

	public FollowType Type = FollowType.MoveTowards;
	public ShowPath Path;
	public float Speed = 1;
	public float MaxDistanceToGoal = .1f;

    Vector2 previousPoint;
    Vector2 currentPoint;

	private IEnumerator<Transform> _currentPoint;

	public void Start()
	{
		if (Path == null) {
			Debug.LogError("Path cannot be null", gameObject);
				return;
				}
		_currentPoint = Path.GetPathsEnumerator ();
		_currentPoint.MoveNext ();

		if (_currentPoint.Current == null)
						return;

		transform.position = _currentPoint.Current.position;

	}

	public void Update()
	{
		if (_currentPoint == null || _currentPoint.Current == null)
			return;

        if (Type == FollowType.MoveTowards)
        {
            previousPoint = transform.position;
            transform.position = Vector3.MoveTowards(transform.position, _currentPoint.Current.position, Time.deltaTime * Speed);
            currentPoint = transform.position;
        }

        if (Type == FollowType.Lerp)
        {
            previousPoint = transform.position;
            transform.position = Vector3.Lerp(transform.position, _currentPoint.Current.position, Time.deltaTime * Speed);
            currentPoint = transform.position;
        }

		var distanceSquared = (transform.position - _currentPoint.Current.position).sqrMagnitude;
		if (distanceSquared < MaxDistanceToGoal * MaxDistanceToGoal)
						_currentPoint.MoveNext ();
	}

}