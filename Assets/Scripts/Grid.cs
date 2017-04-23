using UnityEngine;
using System.Collections;
using System;

/// <summary>
/// A grid of Tiles
/// </summary>
public class Grid : MonoBehaviour
{
	public static Grid instance;

	public Tile prefab;
	public int rowCount;
	public int columnCount;
	public int mineCount;

	Tile[,] tileGrid;
	int coveredCount;


	void Start()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);

		CreateTiles();

		coveredCount = rowCount * columnCount;

		if (rowCount > columnCount)
			Camera.main.orthographicSize = (rowCount + 2) / (2 * Camera.main.aspect);
		else
			Camera.main.orthographicSize = (columnCount + 2) / 2;
	}

	void Update()
	{
		if (mineCount == coveredCount)
		{
			UncoverAll();
			foreach (Tile t in tileGrid)
				t.GetComponent<Animator>().SetBool("victory", true);
		}
	}


	//SETUP BOARD

	private void CreateTiles()
	{
		tileGrid = new Tile[rowCount, columnCount];

		int tilesCreated = 0;

		for (int rows = 0; rows < rowCount; rows++)
		{
			for (int columns = 0; columns < columnCount; columns++)
			{
				tileGrid[rows, columns] = (Tile)Instantiate(prefab, new Vector3(0.5f + rows - rowCount / 2, 0.5f + columns - columnCount / 2), transform.rotation);
				tilesCreated++;
			}
		}
	}

	public void PlaceMines(Tile firstTile)
	{
		for (int mines = 0; mines < mineCount; mines++)
		{
			int tileX = 0;
			int tileY = 0;
			do
			{
				tileX = (int)UnityEngine.Random.Range(0f, rowCount - 1);
				tileY = (int)UnityEngine.Random.Range(0f, columnCount - 1);
			} while (tileGrid[tileX, tileY] == firstTile);

			tileGrid[tileX, tileY].isMine = true;
		}

		for (int rows = 0; rows < rowCount; rows++)
			for (int columns = 0; columns < columnCount; columns++)
				tileGrid[rows, columns].DetectAdjacentMines();
	}



	//GRID MANIPULATION

	public int[] getGridPosition(Tile originTile)
	{
		for (int rows = 0; rows < rowCount; rows++)
		{
			for (int columns = 0; columns < columnCount; columns++)
			{
				if (originTile == tileGrid[rows, columns])
				{
					return new int[] { rows, columns };
				}
			}
		}
		return new int[] { 0, 0 };
	}

	public int AdjacentMines(Tile originTile)
	{
		int mineCount = 0;

		Tile[] tileset = AdjacentTiles(originTile);

		foreach (Tile t in tileset)
		{
			if (t.isMine)
				mineCount++;
		}

		return mineCount;
	}

	public Tile[] AdjacentTiles(Tile originTile)
	{
		Tile[] tileset = new Tile[9];

		int posX = getGridPosition(originTile)[0];
		int posY = getGridPosition(originTile)[1];

		int tileCount = 0;

		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 3; j++)
			{
				int pX = posX + i - 1;
				int pY = posY + j - 1;

				pX = Math.Min(Math.Max(pX, 0), rowCount - 1);
				pY = Math.Min(Math.Max(pY, 0), columnCount - 1);

				tileset[tileCount] = tileGrid[pX, pY];

				tileCount++;
			}
		}

		return cleanArray(tileset);
	}

	private Tile[] cleanArray(Tile[] inputArray)
	{
		ArrayList outList = new ArrayList();

		for (int i = 0; i < inputArray.Length; i++)
		{
			if (!outList.Contains(inputArray[i]))
			{
				outList.Add(inputArray[i]);
			}
		}

		Tile[] outArray = new Tile[outList.Count];

		for (int i = 0; i < outList.Count; i++)
		{
			outArray[i] = (Tile)outList[i];
		}
		return outArray;
	}



	//GAME STATE CHANGES

	public void decrementCoveredCount()
	{
		coveredCount--;
	}

	public void UncoverAll()
	{
		foreach (Tile t in tileGrid)
		{
			t.uncovered = true;
		}
	}
}


