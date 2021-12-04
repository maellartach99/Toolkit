using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEditor;

public class Tile : MonoBehaviour
{
	public Vector2Int size;

	void OnDrawGizmos()
	{
		if (Application.isEditor)
		{
			if (transform.position.x % size.x != 0 || transform.position.z % size.y != 0)
				DestroyImmediate(gameObject);
			/*foreach (Transform child in transform.parent)
			{
				var d = transform.position - child.position;
				if (child == transform) continue;
				if (Math.Abs(d.x) < size.x && Math.Abs(d.z) < size.y)
				{
					DestroyImmediate(gameObject);
					break;
				}
			}*/
		}
	}
}
