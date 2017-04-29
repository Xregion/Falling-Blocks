using UnityEngine;
using System.Collections;

public class Grid : MonoBehaviour {

	public Vector2 gridWorldSize;
	public float nodeRadius;

	Node[,] grid;
	float nodeDiameter;
	int gridSizeX, gridSizeY;

	void Start () {
		nodeDiameter = nodeRadius * 2;
		gridSizeX = Mathf.RoundToInt (gridWorldSize.x / nodeDiameter);
		gridSizeY = Mathf.RoundToInt (gridWorldSize.y / nodeDiameter);
		CreateGrid ();
	}

	void CreateGrid () {
		grid = new Node[gridSizeX, gridSizeY];
		Vector3 worldBottomLeft = transform.position - (Vector3.right * (gridWorldSize.x / 2)) - (Vector3.forward * (gridWorldSize.y / 2));

		for (int x = 0; x < gridSizeX; x++) {
			for (int y = 0; y < gridSizeX; y++) {
				Vector3 worldPoint = worldBottomLeft + (Vector3.right * (x * (nodeDiameter + nodeRadius))) + (Vector3.forward * (y * (nodeDiameter + nodeRadius)));
				grid [x, y] = new Node (worldPoint);
			}
		}

	}

	void OnDrawGizmos () {
		Gizmos.DrawWireCube (transform.position, new Vector3 (gridWorldSize.x, 1, gridWorldSize.y));

		if (grid != null) {
			foreach (Node n in grid) {
				Gizmos.color = Color.red;
				Gizmos.DrawCube (n.worldPosition, Vector3.one * (nodeDiameter - 0.1f));
			}
		}
	}

}
