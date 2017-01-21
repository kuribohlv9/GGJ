using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

    public enum FollowType
    {
        MoveTowards,
        Lerp
    }

    public FollowType Type = FollowType.MoveTowards;
    public PathDefenition path;
    public float speed = 1;
    public float MaxDistanceToGoal = 0.2f;

    private IEnumerator<Transform> _currentPoint;

	// Use this for initialization
	void Start () {
        if (path == null)
            return;

        _currentPoint = path.GetPathEnumerator();
        _currentPoint.MoveNext();

        if (_currentPoint.Current == null)
            return;

        transform.position = _currentPoint.Current.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (_currentPoint == null || _currentPoint.Current == null)
            return;

        if (Type == FollowType.MoveTowards)
            transform.position = Vector3.MoveTowards(transform.position, _currentPoint.Current.position, Time.deltaTime * speed);
        else if (Type == FollowType.Lerp)
            transform.position = Vector3.Lerp(transform.position, _currentPoint.Current.position, Time.deltaTime * speed);

        if (Vector2.Distance(transform.position, _currentPoint.Current.position) < MaxDistanceToGoal)
            _currentPoint.MoveNext();
	}
}
