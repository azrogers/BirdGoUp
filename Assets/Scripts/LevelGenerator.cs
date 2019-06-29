using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
	public GameObject NodePrefab;

	private HashSet<Vector2Int> _grid = new HashSet<Vector2Int>();

	public float WidthMult = 1.0f;
	public float HeightMult = 1.0f;

	private void Generate(Vector2Int startingPoint, int num, bool firstGenerated)
	{
		var lastPoint = startingPoint;
		var nodes = new Vector2Int[num];
		for(var i = 0; i < nodes.Length; i++)
		{
			var possibilities = new List<Vector2Int>()
			{
				new Vector2Int(lastPoint.x - 1, lastPoint.y),
				new Vector2Int(lastPoint.x + 1, lastPoint.y),
				new Vector2Int(lastPoint.x, lastPoint.y + 1),
				new Vector2Int(lastPoint.x + 1, lastPoint.y + 1),
				new Vector2Int(lastPoint.x - 1, lastPoint.y + 1),
			};

			/*if(firstGenerated)
			{
				possibilities = new List<Vector2Int>()
				{
					new Vector2Int(lastPoint.x - 1, lastPoint.y),
					new Vector2Int(lastPoint.x + 1, lastPoint.y),
					new Vector2Int(lastPoint.x, lastPoint.y + 1),
					new Vector2Int(lastPoint.x + 1, lastPoint.y + 1),
					new Vector2Int(lastPoint.x - 1, lastPoint.y + 1),
				};

				firstGenerated = false;
			}
			else
			{
				possibilities = new List<Vector2Int>()
				{
					new Vector2Int(lastPoint.x - 1, lastPoint.y),
					new Vector2Int(lastPoint.x + 1, lastPoint.y),
					new Vector2Int(lastPoint.x, lastPoint.y + 1),
					new Vector2Int(lastPoint.x + 1, lastPoint.y + 1),
					new Vector2Int(lastPoint.x - 1, lastPoint.y + 1),
					new Vector2Int(lastPoint.x + 1, lastPoint.y - 1),
					new Vector2Int(lastPoint.x - 1, lastPoint.y - 1),
					new Vector2Int(lastPoint.x, lastPoint.y - 1)
				};
			}*/

			possibilities = possibilities.OrderBy(p => Random.value).ToList();

			var foundPlace = false;
			foreach(var p in possibilities)
			{
				// there's already something here, don't place another!
				if(_grid.Contains(p))
				{
					continue;
				}

				nodes[i] = p;
				lastPoint = p;
				_grid.Add(p);
				foundPlace = true;
				break;
			}

			if(!foundPlace)
			{
				Debug.LogError("FAILED TO GENERATE POS");
			}
		}

		// create each node
		for(var i = 0; i < nodes.Length; i++)
		{
			var n = nodes[i];
			var nodeObj = Instantiate(NodePrefab, transform);
			nodeObj.transform.localPosition = new Vector3(n.x * WidthMult, n.y * HeightMult);
			nodeObj.name = $"Node {i}";
		}
	}

	private void Awake()
	{
		_grid.Add(Vector2Int.zero);
		Generate(new Vector2Int(0, 0), 20, true);
	}
}