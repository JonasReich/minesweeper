using UnityEngine;

/// <summary>
/// A single field of a grid
/// </summary>
public class Tile : MonoBehaviour
{
	public bool isMine, uncovered, flagged;
	public int adjacentMines;

	static bool firstClickMade;


	void Awake()
	{
		isMine = false;
		uncovered = false;
		flagged = false;

		firstClickMade = false;
	}

	void Update()
	{
		Animator animator = GetComponent<Animator>();

		animator.SetBool("isMine", isMine);
		animator.SetBool("uncovered", uncovered);
		animator.SetBool("flagged", flagged);
	}

	void OnMouseOver()
	{
		if (Input.GetMouseButtonDown(0) && !firstClickMade)
		{
			Grid.instance.PlaceMines(this);
			firstClickMade = true;
		}

		if (!uncovered && Input.GetMouseButtonDown(1))
			SetFlag();

		if (!flagged && !uncovered && Input.GetMouseButtonDown(0))
			Uncover();
	}


	public void DetectAdjacentMines()
	{
		adjacentMines = Grid.instance.GetComponent<Grid>().AdjacentMines(this);
		GetComponent<Animator>().SetInteger("adjacentMines", adjacentMines);
	}


	void SetFlag()
	{
		if (!uncovered)
		{
			flagged = !flagged;
		}
	}

	void Uncover()
	{
		if (!flagged)
		{
			uncovered = true;
			Grid.instance.decrementCoveredCount();

			if (isMine)
				Grid.instance.UncoverAll();
			else
				UncoverChain();
		}
	}

	void UncoverChain()
	{
		if (!uncovered)
			Grid.instance.decrementCoveredCount();

		uncovered = true;
		if (adjacentMines == 0)
		{
			Tile[] tileset = Grid.instance.AdjacentTiles(this);
			foreach (Tile t in tileset)
			{
				if (!t.isMine && !t.flagged && !t.uncovered)
					t.GetComponent<Tile>().UncoverChain();
			}
		}
	}
}
