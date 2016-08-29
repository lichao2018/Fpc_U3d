using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
	public static GameManager Instance = null;
	int m_score = 0;
	static int m_hiscore = 0;
	public int m_ammo = 30;
	Player m_player;

	public Text txt_health;
	public Text txt_kill;
	public Text txt_historyKill;
	public Text txt_ammos;

	public GameObject inGameMenu;
	public GameObject gameOverMenu;

	// Use this for initialization
	void Start () {
		Instance = this;
		m_player = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player> ();

		txt_health.text = "5";
		txt_ammos.text = "30/30";
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey (KeyCode.Escape)) {
			onPause();
		}
		if (m_player.m_life <= 0) {
			gameOverMenu.SetActive(true);
		}
	}

	public void SetScore(int score){
		m_score += score;
		if (m_score > m_hiscore)
			m_hiscore = m_score;

		txt_kill.text = m_score.ToString ();
		txt_historyKill.text = m_hiscore.ToString ();
	}

	public void SetAmmo(int ammo){
		m_ammo -= ammo;
		if (m_ammo <= 0) {
			m_ammo = 30 - m_ammo;
		}

		txt_ammos.text = m_ammo.ToString () + "/30";
	}

	public void SetLife(int life){
		txt_health.text = life.ToString ();
	}

	void onPause(){
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		Time.timeScale = 0;
		inGameMenu.SetActive (true);
	}

	public void onResume(){
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		Time.timeScale = 1f;
		inGameMenu.SetActive (false);
		gameOverMenu.SetActive(false);
	}

	public void onReStart(){
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		Time.timeScale = 1f;
		inGameMenu.SetActive (false);
		gameOverMenu.SetActive(false);
		Application.LoadLevel (Application.loadedLevelName);
	}

	public void onFinish(){
		Application.Quit ();
	}
}


































