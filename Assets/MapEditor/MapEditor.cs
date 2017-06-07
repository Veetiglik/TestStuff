using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maps;
using System.IO;
using System.Text;

public class MapEditor : MonoBehaviour {


	Map map;
	MapEditorView viewReferenceComponent;

	public int debugX;
	public int debugY;
	public int debugZ;
	public e_TileType debugTileType;
	public bool createTile = false;
	public bool debugTileCDActivate = false;
	public bool debugTileSaveMap = false;

	public string mapsLocation;
	public int mapNum;


	// Use this for initialization
	void Start () 
	{
		viewReferenceComponent = gameObject.GetComponent<MapEditorView> ();

		string mapName = Application.dataPath + mapsLocation + mapNum.ToString () + ".txt";

	    if (System.IO.File.Exists (mapName))
		{
			CreateMap (mapName);
		}
		else
		{
			Debug.Log ("Map not found");
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (debugTileCDActivate)
		{
			DebugTileCreate ();
		}

		if (debugTileSaveMap)
		{
			SaveMap ();
		}
	}

	void SaveMap()
	{
		//clear button toggle
		debugTileSaveMap = false;

		StreamWriter streamW = new StreamWriter (Application.dataPath + mapsLocation + mapNum.ToString () + ".txt");

		//If the map has lots of empty space from deleting edge tiles, only save the worthwhile centre.
		//determine actual map size with tiles in it
		int lowerBoundX = 999;
		int lowerBoundY = 999;
		int lowerBoundZ = 999;
		int upperBoundX = 0;
		int upperBoundY = 0;
		int upperBoundZ = 0;

		//this will be the least efficient version, and I can clean it up if I find it to run slowly. Otherwise it's fine

		for (int i = 0; i < map.xSize; i++)
		{
			for (int j = 0; j < map.ySize; j++)
			{
				for (int k = 0; k < map.zSize; k++)
				{
					if (map.mapArray [i, j, k].contents != e_tileContents.NONEXISTANT)
					{
						if (i < lowerBoundX)
						{
							lowerBoundX = i;
						}
						if (j < lowerBoundY)
						{
							lowerBoundY = j;
						}
						if (k < lowerBoundZ)
						{
							lowerBoundZ = k;
						}

						if (i > upperBoundX)
						{
							upperBoundX = i;
						}
						if (j > upperBoundY)
						{
							upperBoundY = j;
						}
						if (k > upperBoundZ)
						{
							upperBoundZ = k;
						}
					}
				}
			}
		}

		upperBoundX++;
		upperBoundY++;
		upperBoundZ++;

		int mapSizeX = upperBoundX - lowerBoundX;
		int mapSizeY = upperBoundY - lowerBoundY;
		int mapSizeZ = upperBoundZ - lowerBoundZ;
		using (streamW)
		{
			//Write size values first
			streamW.WriteLine (mapSizeX.ToString() + " " + mapSizeY.ToString() + " " + mapSizeZ.ToString() + " ");

			string line = "";

			for (int i = lowerBoundY; i < upperBoundY; i++)
			{
				for (int j = lowerBoundX; j < upperBoundX; j++)
				{
					for (int k = lowerBoundZ; k < upperBoundZ; k++)
					{
						if (map.mapArray [j, i, k] == null)
						{
							line += "x";
						}
						else
						{
							switch (map.mapArray [j, i, k].contents)
							{
								case e_tileContents.NONEXISTANT:
									{
										line += "x";
										break;
									}							
								default:
									{
										line += (int)map.mapArray [j, i, k].type;
										break;
									}
							}
						}
						if (k < upperBoundZ - 1)
						{
							line += " ";
						}
						else
						{
							streamW.WriteLine (line);
							line = "";
						}
					}
				}
				//insert spacer line
				streamW.WriteLine("-");
			}
		}
	}

	void DebugTileCreate()
	{
		//clear button toggle
		debugTileCDActivate = false;

		if (createTile)
		{
			CreateTile (debugX, debugY, debugZ, debugTileType);
		}
		else//destroy tile
		{
			DestroyTile (debugX, debugY, debugZ);
		}
	}

	void CreateMap(string mapName)
	{
		/*This is simply to initialize an array of a given size. 
		tiles will only be created in indices where tiles actually exist
		the rest will be null
		this will require lots of null checking, and I guess really doesn't affect memory usage even since the array is still all there
		a map will have set bound probably for x and y, but the height will be very dynamic
		I guess right now I should work with a set maximum, and revamp as necessary. 
		*/

		int x, y, z;
		string line;
		bool parseSuccess = false;
		StreamReader streamR = new StreamReader (mapName, Encoding.Default);
		using (streamR)
		{
			line = streamR.ReadLine ();
			string[] lineTokenArray = line.Split (' ');
			parseSuccess |= int.TryParse (lineTokenArray [0], out x);
			parseSuccess |= int.TryParse (lineTokenArray [1], out y);
			parseSuccess |= int.TryParse (lineTokenArray [2], out z);
			map = new Map (x, y, z);
			viewReferenceComponent.CreateMap (x, y, z);

			if (parseSuccess)
			{
				for (int i = 0; i < y; i++)
				{
					if (parseSuccess)
					{
						for (int j = 0; j < x; j++)
						{
							line = streamR.ReadLine ();
							lineTokenArray = line.Split (' ');

							if (lineTokenArray.Length == z)
							{
								for (int k = 0; k < z; k++)
								{
									switch (lineTokenArray [k])
									{
										case "x":
											{
												break;
											}							
										default:
											{
												int type;
												int.TryParse (lineTokenArray [k], out type);

												if (!IsTileTypeValid ((e_TileType)type))
												{
													parseSuccess = false;
													Debug.Log ("Invalid tile type found in map data");
													break;
												}
											
												CreateTile (j, i, k, (e_TileType)type);
												break;
											}
									}
								}
							}
							else
							{
								parseSuccess = false;
								Debug.Log ("Incorrect map size for map data");
								break;
							}
						}
					}
					//discard spacer line in file
					line = streamR.ReadLine ();
				}
			}
			else
			{
				Debug.Log ("Could not parse map size");
			}
		}
	}

	void CreateTile(int x, int y, int z, e_TileType type)
	{
		bool leftOfBoundsX = false;
		bool leftOfBoundsY = false;
		bool leftOfBoundsZ = false;

		if (!IsTileLocValid (x, y, z))
		{
			//need to create new array of a larger size and transfer contents over
		
			//determine new bounds
			int newX;
			int newY;
			int newZ;

			//expand map outwards
			if(x + 1 > map.xSize)
			{
				newX = x + 1;
			}
			//expand map to the 'left' by shifting all current tiles to the 'right'
			else if(x < 0)
			{
				newX = map.xSize - x;
				leftOfBoundsX = true;
			}
			//no need to change map size
			else
			{
				newX = map.xSize;
			}
			//expand map outwards
			if(y + 1 > map.ySize)
			{
				newY = y + 1;
			}
			//expand map to the 'left' by shifting all current tiles to the 'right'
			else if(y < 0)
			{
				newY = map.ySize - y;
				leftOfBoundsY = true;
			}
			//no need to change map size
			else
			{
				newY = map.ySize;
			}
			//expand map outwards
			if(z + 1 > map.zSize)
			{
				newZ = z + 1;
			}
			//expand map to the 'left' by shifting all current tiles to the 'right'
			else if(z < 0)
			{
				newZ = map.zSize - z;
				leftOfBoundsZ = true;
			}
			//no need to change map size
			else
			{
				newZ = map.zSize;
			}

			Map newMap = new Map (newX, newY, newZ);

			//if for example I have a new tile at 1, -5, 4
			//all tiles copied over will need to be shifted by 5 in the positive direction in the y axis 
			//so will make adjustments using ternary operator
			//uuuurrrrghrghr I need to perform the same shifting for the tile objects on the render side also.
			//I only need to code it once though!
			//copy old map array over
			for (int i = 0; i < map.xSize; i++)
			{
				for (int j = 0; j < map.ySize; j++)
				{
					for (int k = 0; k < map.zSize; k++)
					{
						//newMap.mapArray [i,j,k] = map.mapArray [i,j,k];
						newMap.mapArray [!leftOfBoundsX ? i:i-x, !leftOfBoundsY ? j:j-y, !leftOfBoundsZ ? k:k-z] = map.mapArray [i,j,k];
					}
				}
			}
			//update view editor components as well
			viewReferenceComponent.UpdateMapArray(map.xSize, map.ySize, map.zSize, newX, newY, newZ, 
				leftOfBoundsX, leftOfBoundsY, leftOfBoundsZ, x, y, z);

			map = newMap;

        }

        //add new tile
        //the ternary operators make sure that if a new tile is added to the left it gets put at the start of the array

        int finalX = !leftOfBoundsX ? x : 0;
        int finalY = !leftOfBoundsY ? y : 0;
        int finalZ = !leftOfBoundsZ ? z : 0;

        map.mapArray [finalX, finalY, finalZ] = new Tile (finalX, finalY, finalZ, type, e_tileContents.EMPTY);
		viewReferenceComponent.RenderTile (map.mapArray [finalX, finalY, finalZ], x, y, z);
	}

	void DestroyTile(int x, int y, int z)
	{
		map.mapArray [x, y, z].contents = e_tileContents.NONEXISTANT;
		viewReferenceComponent.DestroyTile (x, y, z);
	}

	bool IsTileTypeValid(e_TileType type)
	{
		return ((int)type >= (int)e_TileType.FIRSTTILETYPE && (int)type < (int)e_TileType.MAXTILETYPE);
	}

	bool IsTileLocValid(int x, int y, int z)
	{
		return (x >= 0 && x < map.xSize && y >= 0 && y < map.ySize && z >= 0 && z < map.zSize);
	}
}
