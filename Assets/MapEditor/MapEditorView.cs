using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maps;

public class MapEditorView : MonoBehaviour 
{

	//prefabs 
	public GameObject tilePrefab;
	GameObject mapContainer;
	GameObject[,,] mapTileArray;

	//inspector variables
	public Color[] tileColorsArray;

	MapSize mapSize;

	// Use this for initialization
	void Start () 
	{
		mapContainer = new GameObject ();
		mapContainer.name = "Map";
	}

	// Update is called once per frame
	void Update () 
	{

	}

	public void CreateMap(int x, int y, int z)
	{
		mapSize = new MapSize (x, y, z);
		mapTileArray = new GameObject[x,y,z];

		GameObject row;
		GameObject col;

		for (int i = 0; i < x; i++)
		{
			row = new GameObject ();
			row.name = "row" + i.ToString ();
			row.transform.parent = mapContainer.transform;

			for (int j = 0; j < y; j++)
			{
				col = new GameObject ();
				col.name = "col" + j.ToString ();
				col.transform.parent = row.transform;
			}
		}

	}

	public void RenderMap(Tile[,,] map)
	{
		//for()
	}

	public void RenderTile(Tile tile)
	{
		if (mapTileArray [tile.xLoc, tile.yLoc, tile.zLoc] != null)
		{
			GameObject.Destroy (mapTileArray [tile.xLoc, tile.yLoc, tile.zLoc]);
		}
			
		GameObject tileObj = Instantiate (tilePrefab);
		tileObj.GetComponent<MeshRenderer>().material.color = tileColorsArray [(int)tile.type];
		tileObj.name = tile.zLoc.ToString ();
		tileObj.transform.parent = mapContainer.transform.GetChild (tile.xLoc).transform.GetChild(tile.yLoc);

		//now actually have to set location
		tileObj.transform.position = new Vector3((float)tile.xLoc, (float)tile.yLoc*.25f, (float)tile.zLoc);

		mapTileArray [tile.xLoc, tile.yLoc, tile.zLoc] = tileObj;
	}

	public void DestroyTile(int x, int y, int z)
	{
		if (x >= 0 && x < mapSize.xSize && y >= 0 && y < mapSize.ySize && z >= 0 && z < mapSize.zSize)
		{
			GameObject.Destroy (mapTileArray [x, y, z]);
		}
		else
		{
			Debug.Log("Tile destruction values outside of bounds");
		}
	}

	//updates the tile object container when a new tile is added that's outside the current bounds
	public void UpdateMapArray(int oldBoundX, int oldBoundY, int oldBoundZ, int newBoundX, int newBoundY, int newBoundZ,
		bool leftOfBoundsX, bool leftOfBoundsY, bool leftOfBoundsZ, int x, int y, int z)
	{
		//create new container array
		GameObject[,,] newMapTileArray = new GameObject[newBoundX, newBoundY, newBoundZ];

		//copy old tile array over
		for (int i = 0; i < oldBoundX; i++)
		{
			for (int j = 0; j < oldBoundY; j++)
			{
				for (int k = 0; k < oldBoundZ; k++)
				{
					newMapTileArray [!leftOfBoundsX ? i:i-x, !leftOfBoundsY ? j:j-y, !leftOfBoundsZ ? k:k-z] = newMapTileArray [i,j,k];
				}
			}
		}
		mapTileArray = newMapTileArray;

		//the above correctly maps the old array of tile GameObjects to the new array, but the actual storage in tiered GameObjects on the 
		//map object still needs updating eg. row 1 > col 2 > 3

		GameObject row;
		GameObject col;

		if (!leftOfBoundsX && !leftOfBoundsY && !leftOfBoundsZ)
		{
			//in a new loop, add the gameobject containers for the new tiles
			for (int i = oldBoundX; i < newBoundX; i++)
			{
				row = new GameObject ();
				row.name = "row" + i.ToString ();
				row.transform.parent = mapContainer.transform;

				for (int j = 0; j < oldBoundY; j++)
				{
					col = new GameObject ();
					col.name = "col" + j.ToString ();
					col.transform.parent = row.transform;
				}
			}

			for (int i = 0; i < newBoundX; i++)
			{
				for (int j = oldBoundY; j < newBoundY; j++)
				{
					col = new GameObject ();
					col.name = "col" + j.ToString ();
					col.transform.parent = mapContainer.transform.GetChild (i);
				}
			}
		}
		else
		{
			//will probably need to shift this around because all of the above stuff is still necessary. Just doing this separate to 
			//make it simpler to think about

			if (leftOfBoundsX)
			{
			}

			if (leftOfBoundsY)
			{
			}

			if (leftOfBoundsZ)
			{
				//loop through every row,col and shift every z tile up that many numbers

				//this updates the name at the z level 
				for (int i = 0; i < newBoundX; i++)
				{
					for (int j = 0; j < newBoundY; j++)
					{
						for (int k = 0; k < newBoundZ; k++)
						{
							mapTileArray [i, j, k].name = k.ToString ();
						}
					}
				}
			}
		}
		//z tile will be added as normal in tile render

		mapSize.mapSizeEdit(newBoundX, newBoundY, newBoundZ);
	}
}