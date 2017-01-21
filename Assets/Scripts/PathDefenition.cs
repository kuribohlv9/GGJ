using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathDefenition : MonoBehaviour {

    public Transform[] points;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public IEnumerator<Transform> GetPathEnumerator()
    {
        if (points == null || points.Length < 1)
            yield break;

        int direction = 1;
        int index = 0;
        while(true)
        {
            if (points[index] == null)
                break;

            yield return points[index];

            index += direction;
        }
    }

    public void OnDrawGizmos()
    {
        if (points == null || points.Length < 2)
            return;

        for(int i = 1; i < points.Length; i++)
        {
            Gizmos.DrawLine(points[i - 1].position, points[i].position);
        }
    }
}
