using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	public Grid gridPrefab;

	bool gameRunning;
	int rows, columns, mineCount;
	Grid gridInstance;


	void Start()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);

		gameRunning = false;
	}

	void OnGUI()
	{
		if (!gameRunning)
		{
			if (GUI.Button(new Rect(5, 5, 100, 20), "Beginner"))
				StartBeginner();
			if (GUI.Button(new Rect(5, 30, 100, 20), "Advanced"))
				StartAdvanced();
			if (GUI.Button(new Rect(5, 55, 100, 20), "Professional"))
				StartProfessional();
		}
		else if (GUI.Button(new Rect(5, 5, 100, 20), "Restart"))
			RestartGame();
	}


	void StartBeginner()
	{
		rows = columns = 8;
		mineCount = 10;
		StartGame();
	}

	void StartAdvanced()
	{
		rows = columns = 16;
		mineCount = 40;
		StartGame();
	}

	void StartProfessional()
	{
		rows = 20;
		columns = 24;
		mineCount = 99;
		StartGame();
	}

	void StartGame()
	{
		gameRunning = true;
		gridInstance = (Grid)Instantiate(gridPrefab, new Vector3(0f, 0f), new Quaternion());
		gridInstance.rowCount = rows;
		gridInstance.columnCount = columns;
		gridInstance.mineCount = mineCount;
	}

	void RestartGame()
	{
		SceneManager.LoadScene(0);
	}
}
