using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelGenerator : Singleton<LevelGenerator>
{
	private static int _seed = -1;
	
	public GameObject NodePrefab;
	public GameObject ObstaclePrefab;

	private HashSet<Vector2Int> _grid = new HashSet<Vector2Int>();

	public float WidthMult = 1.0f;
	public float HeightMult = 1.0f;
	public int NumPerGenerate = 50;

	private List<Node> _createdNodes = new List<Node>();
	private int _nextNodeIndex = 0;
	private int _nextGenPoint = 0;
	private Dictionary<Vector2Int, int> _posIndexes = new Dictionary<Vector2Int, int>();

	[NonSerialized] public int HighestNodeReached = 0;

	public Vector2 GetPosFor(Vector2Int a) => a * new Vector2(WidthMult, HeightMult);

	public bool PosFilled(Vector2Int a) => _grid.Contains(a);

	private void Generate(Vector2Int startingPoint, int num, bool firstGenerated)
	{
		var startIndex = _nextNodeIndex;
		var lastPoint = startingPoint;
		var nodes = new Vector2Int[num];
		var rand = new System.Random(_seed);
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

			possibilities = possibilities.OrderBy(p => rand.Next()).ToList();

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
				_posIndexes[p] = startIndex + i;
				foundPlace = true;
				break;
			}

			if(!foundPlace)
			{
				Debug.LogError("FAILED TO GENERATE POS");
			}
		}

		// fill in the position of the last node of the previously generated nodes
		if(_createdNodes.Any())
		{
			var lastNode = _createdNodes.Last();
			lastNode.NextNode = nodes[0];
			lastNode.UpdateVisual();
		}

		var lastNodePos = _createdNodes.Any() ? _createdNodes.Last().GridPosition : Vector2Int.zero;
		for(var i = 0; i < nodes.Length - 1; i++)
		{
			var node = nodes[i];
			var nextNode = nodes[i + 1];
			if(i > 0)
			{
				lastNodePos = nodes[i - 1];
			}
			// if this node has any neighbors that aren't its next position,
			// maybe put an obstacle between them
			var neighbors = new[]
			{
				new Vector2Int(node.x - 1, node.y),
				new Vector2Int(node.x + 1, node.y),
				new Vector2Int(node.x, node.y - 1),
				new Vector2Int(node.x, node.y + 1)
			};

			foreach(var n in neighbors.Where(n => _grid.Contains(n) && n != nextNode && n != lastNodePos))
			{
				if(!(rand.NextDouble() > 0.5f))
				{
					continue;
				}

				var indexFrom = Mathf.Min(_posIndexes[node], _posIndexes[n]);
				var obst = Instantiate(ObstaclePrefab, transform);
				// position half way between
				obst.transform.localPosition = Vector2.Lerp(GetPosFor(node), GetPosFor(n), 0.5f);
				var oc = obst.GetComponent<Obstacle>();
				oc.PosFrom = node;
				oc.PosBlockingTo = n;
				oc.IndexFrom = Mathf.Min(_posIndexes[node], _posIndexes[n]);
			}
		}

		// create each node
		for(var i = 0; i < nodes.Length; i++)
		{
			var n = nodes[i];
			var nodeObj = Instantiate(NodePrefab, transform);
			nodeObj.transform.localPosition = new Vector3(n.x * WidthMult, n.y * HeightMult);

			var nc = nodeObj.GetComponent<Node>();
			nc.GridPosition = n;
			if(i != nodes.Length - 1)
			{
				nc.NextNode = nodes[i + 1];
				nc.UpdateVisual();
			}

			nc.NodeIndex = startIndex + i;
			nodeObj.name = $"Node {nc.NodeIndex}";

			_createdNodes.Add(nc);
		}

		// generate more nodes after the player reaches the node index 50% of the way through the generated nodes
		_nextGenPoint = startIndex + Mathf.FloorToInt((_nextNodeIndex - startIndex) * 0.5f);
		_nextNodeIndex = startIndex + nodes.Length;
	}

	private void Awake()
	{
		if(_seed == -1)
		{
			_seed = new System.Random().Next();
		}
		
		_grid.Add(Vector2Int.zero);
		_posIndexes[Vector2Int.zero] = 0;
		_nextNodeIndex = 1;
		Generate(new Vector2Int(0, 0), NumPerGenerate, true);
	}

	private void Update()
	{
		if(HighestNodeReached > _nextGenPoint)
		{
			Generate(_createdNodes.Last().GridPosition, NumPerGenerate, false);
		}

		// clear out old nodes
		var newNodes = new List<Node>();
		foreach(var t in _createdNodes)
		{
			// remove if we've reached a later node and this is off screen
			if(t && t.NodeIndex < HighestNodeReached && !t.Renderer.isVisible)
			{
				Destroy(t.gameObject);
			}
			else if(t)
			{
				newNodes.Add(t);
			}
		}

		_createdNodes = newNodes;
	}
}