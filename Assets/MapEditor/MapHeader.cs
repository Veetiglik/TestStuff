using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Maps
{
	public enum e_TileType
	{
		FIRSTTILETYPE = 0,
		GROUND = FIRSTTILETYPE,
		WATER,
		GRASS,
		MAXTILETYPE
	}

	public enum e_tileContents
	{
		EMPTY,
		ALLY,
		ENEMY,
		NEUTRAL,//unsure if necessary
		OBSTACLE,
		NONEXISTANT
	}

	public class Tile
	{
		public int xLoc;
		public int yLoc;
		public int zLoc;//essentially height. May rename later for clarity

		public e_TileType type;
		public e_tileContents contents;

		public Tile()
		{
			contents = e_tileContents.NONEXISTANT;
		}

		public Tile(int x, int y, int z, e_TileType typeIn, e_tileContents contentsIn)
		{
			xLoc = x;
			yLoc = y;
			zLoc = z;
			type = typeIn;
			contents = contentsIn;
		}
	}

	public class Map
	{
		public Tile[,,] mapArray;
		public int xSize;
		public int ySize;
		public int zSize;

		public Map(int x, int y, int z)
		{
			xSize = x;
			ySize = y;
			zSize = z;
			mapArray = new Tile[x,y,z];

			for (int i = 0; i < xSize; i++)
			{
				for (int j = 0; j < ySize; j++)
				{
					for (int k = 0; k < zSize; k++)
					{
						mapArray [i, j, k] = new Tile ();
					}
				}
			}
		}
	}

	public class MapSize
	{
		public int xSize;
		public int ySize;
		public int zSize;

		public MapSize(int x, int y, int z)
		{
			xSize = x;
			ySize = y;
			zSize = z;
		}

		public void mapSizeEdit(int x, int y, int z)
		{
			xSize = x;
			ySize = y;
			zSize = z;
		}
	}
}
