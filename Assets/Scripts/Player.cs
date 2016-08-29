using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public Transform m_transform;
	CharacterController m_ch;
	float m_moveSpeed = 3.0f;
	float m_gravity = 2.0f;
	public int m_life = 5;

	Transform m_camTransform;
	Vector3 m_camRot;
	float m_camHeight = 1.4f;

	Transform m_muzzlepoint;
	public LayerMask m_layer;
	public Transform m_fx;
	public AudioClip m_audio;
	float m_shootTimer = 0;
	float m_setAmmoTimer = 0;

	// Use this for initialization
	void Start () {
		m_transform = this.transform;
		m_ch = this.GetComponent<CharacterController> ();

		m_camTransform = Camera.main.transform;

		Vector3 pos = m_transform.position;
		pos.y += m_camHeight;
		m_camTransform.position = pos;
		m_camTransform.rotation = m_transform.rotation;
		m_camRot = m_camTransform.eulerAngles;

		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		m_muzzlepoint = m_camTransform.FindChild ("M16/weapon/muzzlepoint").transform;
	}
	
	// Update is called once per frame
	void Update () {
	    if (m_life <= 0)
			return;
		if(Time.timeScale != 0)
		    Control ();

		m_shootTimer -= Time.deltaTime;
		m_setAmmoTimer -= Time.deltaTime;
		if (Input.GetMouseButton (0) && m_shootTimer <= 0 && m_setAmmoTimer <= 0) {
			m_shootTimer = 0.1f;
			if(GameManager.Instance.m_ammo <= 1)
			    m_setAmmoTimer = 1.0f;
			this.GetComponent<AudioSource>().PlayOneShot(m_audio);
			GameManager.Instance.SetAmmo(1);
			RaycastHit info;
			bool hit = Physics.Raycast(m_muzzlepoint.position, m_camTransform.TransformDirection(Vector3.forward), out info, 100, m_layer);
			if(hit){
				if(info.transform.tag.CompareTo("enemy")==0){
					Enemy enemy = info.transform.GetComponent<Enemy>();
					enemy.OnDamage(1);
				}
				Instantiate(m_fx, info.point, info.transform.rotation);
			}
		}
	}

	void Control(){
		float rh = Input.GetAxis ("Mouse X");
		float rv = Input.GetAxis ("Mouse Y");

		m_camRot.x -= rv;
		m_camRot.y += rh;
		m_camTransform.eulerAngles = m_camRot;

		Vector3 camrot = m_camTransform.eulerAngles;
		camrot.x = 0; camrot.z = 0;
		m_transform.eulerAngles = camrot;

		float xm = 0, ym = 0, zm = 0;
		ym -= m_gravity * Time.deltaTime;

		if (Input.GetKey (KeyCode.W)) {
			zm += m_moveSpeed *Time.deltaTime;
		}else if(Input.GetKey(KeyCode.S)){
			zm -= m_moveSpeed * Time.deltaTime;
		}
		if(Input.GetKey(KeyCode.A)){
			xm -= m_moveSpeed * Time.deltaTime;
		}else if(Input.GetKey(KeyCode.D)){
			xm += m_moveSpeed * Time.deltaTime;
		}

		m_ch.Move(m_transform.TransformDirection(new Vector3(xm, ym, zm)));

		Vector3 pos = m_transform.position;
		pos.y += m_camHeight;
		m_camTransform.position = pos;
	}

	void OnGrawGizmos(){
		Gizmos.DrawIcon (this.transform.position, "Gizmos/Spawn.tif");
	}

	public void OnDamage(int damage){
		m_life -= damage;
		GameManager.Instance.SetLife (m_life);
		if (m_life <= 0) {
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
		}
	}
}





























