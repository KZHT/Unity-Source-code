using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

	[SerializeField]
	GameObject opObj =null;
	[SerializeField]
	GameObject opObj1 = null;

	[SerializeField]
	GameObject coreobj;

	GameObject DamegeEffectObj;
	ParticleSystem DamegeEffect;
	ParticleSystem BomEffect;

	CoreScript corelife;
	lifeUI lifeui;

	EnemyManager enemycontroller;
	MainController maincontroller;
	ScoreText_Animation scorescript;

	private int moveint = 0;
	public float speed = 6f;

	public int life = 3;
	private float mutekicnt = 0;

	[SerializeField]
	private float shotcnt;
	private float shottime = 0.7f;

	private float particlecnt;
	private float particletime;
	private bool particlelock;

	private int testcnt = 1;


	public int extendint = 0;
	public bool movebool =false ;
	public bool shotbool = false;

	private bool Audiolock = false;
	private bool Audiolock1 = false;
	private bool Audiolock2 = false;

	float colortime = 0f;

	Vector3 pos;

	int itemint;
	
	[SerializeField]
	public float bulletcnt = 2.0f;

	[SerializeField]
	private float specialcnt = 0.0f;

	[SerializeField]
	public float specialpower = 0.0f;

   public bool specialbool = false;


	public float bomcnt = 0.0f;

	AudioSource AudioPlayerShot;
	AudioSource Audiopower_up;
	AudioSource AudioCorekaifuku;
	AudioSource AudioOptionSpawn;
	AudioSource AudioDamage;

	SpriteRenderer playerrender;

	// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
	// Find使いすぎ
	// 理由：
	// ・略
	// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ichi

	void Awake()
	{
		maincontroller = GameObject.Find("GameController").GetComponent<MainController>();
		enemycontroller = GameObject.FindGameObjectWithTag("GameManeger").GetComponent<EnemyManager>();
		corelife = GameObject.FindGameObjectWithTag("Core").GetComponent<CoreScript>();
		lifeui = GameObject.Find("Canvas").transform.Find("PlayerHealth").GetComponent<lifeUI>();
		scorescript = GameObject.Find("Canvas").transform.Find("ScoreText").GetComponent<ScoreText_Animation>();
		DamegeEffectObj = GameObject.FindGameObjectWithTag("Effect/Player").gameObject;
		DamegeEffect = DamegeEffectObj.GetComponent<ParticleSystem>();
		BomEffect = GameObject.FindGameObjectWithTag("Effect/Bom").GetComponent<ParticleSystem>();

		playerrender = GetComponent<SpriteRenderer>();

		AudioPlayerShot = GameObject.Find("Audio").transform.Find("Playershot").GetComponent<AudioSource>();
		Audiopower_up = GameObject.Find("Audio").transform.Find("power_up").GetComponent<AudioSource>();
		AudioCorekaifuku = GameObject.Find("Audio").transform.Find("Corekaifuku").GetComponent<AudioSource>();
		AudioOptionSpawn = GameObject.Find("Audio").transform.Find("OptionSpawn").GetComponent<AudioSource>();
		AudioDamage = GameObject.Find("Audio").transform.Find("damage").GetComponent<AudioSource>();
	}


		// Update is called once per frame
		void Update () {
		//MainController 「Redy&GameOver」で動作停止
		if (maincontroller.MainControllerbool == false) return;
		move();
		mouseAngle();
		Bom();
		mutekicnt -= Time.deltaTime;
		if(mutekicnt <= 0f)
		{
			this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
			colortime = 0;
			playerrender.color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 1f);
		}
		else
		{
			this.gameObject.GetComponent<BoxCollider2D>().enabled = false;

			colortime = Mathf.Abs(Mathf.Sin((Time.time) * 3f));
			playerrender.color = new Color(255f / 255f,colortime,colortime,1f);
		}

		if(specialbool == true)
		{
			if(specialpower <= 0f)
			{
				specialbool = false;
			}
			else
			{
				specialpower -= 0.25f * Time.deltaTime;
			}

		}

		if(specialbool == true)
		{
			specialcnt = 3f;
		}
		else
		{
			specialcnt = 0f;
		}


		Damege_Effect();
	}

	void Bom()
	{
		if (Input.GetKeyDown(KeyCode.E))
		{
			if (bomcnt >= 5)
			{
				BomEffect.Play();
				maincontroller.BOMmode();
				bomcnt = 0f;
			}
		}

	}
	void mouseAngle()
	{
		var _pos = Camera.main.WorldToScreenPoint(transform.position);

		var direction = Input.mousePosition - _pos;

		var angle = Utils.GetAngle(Vector3.zero, direction);

		var angles = transform.localEulerAngles;
		angles.z = angle - 90;
		transform.localEulerAngles = angles;

		shotcnt += Time.deltaTime;

		if(specialbool == true)
		{
			shottime = 0.4f;
		}
		else
		{
			shottime = 0.7f;
		}

		var playershot = bulletcnt + specialcnt;

		if (shotcnt < shottime) return;

		if (Input.GetMouseButton(0))
		{
		   
			ShootWay(angle, 20, (int)playershot, this.gameObject);

			if(opObj.activeSelf == true)
			{
			 ShootWay(angle, 20, ((int)playershot - 2), opObj.gameObject);
			}

			if (opObj1.activeSelf == true)
			{
				ShootWay(angle, 20, ((int)playershot - 2), opObj1.gameObject);
			}
			AudioPlayerShot.Play();
			shotcnt = 0;
		}
	}



	private void ShootWay(
	  float AngleBase, float AngleRange,int cnt,GameObject Fromobj)
	{
		if (3 <= cnt)
		{
			for (int i = 0; i < cnt; ++i)
			{
				var angle = AngleBase +
					 AngleRange * ((float)i / (cnt - 1) - 0.5f);

				GameObject obj = ObjectPoolScript.current.shotObjPooledObject();

				if (obj == null) return;
				obj.transform.position = Fromobj.transform.position;
				obj.transform.rotation = Fromobj.transform.rotation;
				obj.SetActive(true);

				Init(angle, obj);

			   // Debug.Log("確認1");
			}

		}
		else
		{
			if (cnt <= 2)
			{
				GameObject obj = ObjectPoolScript.current.shotObjPooledObject();

				if (obj == null) return;
				obj.transform.position = Fromobj.transform.position;
				obj.transform.rotation = Fromobj.transform.rotation;
				obj.SetActive(true);

				Init(AngleBase, obj);

				//Debug.Log("確認");
			}
		}
		
	}

	void Init(float angle,GameObject obj)
	{ 
		//弾が進行方向を向くようにする
		var angles = obj.transform.localEulerAngles;
		angles.z = angle - 90;
		obj.transform.localEulerAngles = angles;

	}


	private void move()
	{
	   

		if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A)
	&& !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.S))
		{
			moveint = 0;
			movebool = false;
		}
		else
		{
			movebool = true;
		}

		//右上移動
		if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
		{
			if ((this.transform.position.x < 13f && this.transform.position.y < 9.8f))
			{
				moveint = 5;
			}
			else
			{

				if (this.transform.position.x < 13f)
				{	//右移動
					moveint = 2;
				}

				if (this.transform.position.y < 9.8f)
				{	//上移動
					moveint = 1;
				}

				if (this.transform.position.x > 13f && this.transform.position.y > 9.8f)
				{	//移動なし
					moveint = 0;
				}
			}

		}

		//右下移動
		if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.S))
		{
			if (this.transform.position.x < 13f && this.transform.position.y > -9.8f)
			{
				moveint = 6;
			}
			else
			{

				if (this.transform.position.x < 13f)
				{	//右移動
					moveint = 2;
				}

				if (this.transform.position.y > -9.8f)
				{  //下移動
					moveint = 3;
				}

				if (this.transform.position.x > 13f && this.transform.position.y < -9.8f)
				{	//移動なし
					moveint = 0;
				}

			}
		}

		//左下移動
		if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.S))
		{
			if (this.transform.position.x > -13f && this.transform.position.y > -9.8f)
			{
				moveint = 7;
			}
			else
			{

				if (this.transform.position.x > -13f)
				{	//左移動
					moveint = 4;
				}

				if (this.transform.position.y > -9.8f)
				{	//下移動
					moveint = 3;
				}

				if (this.transform.position.x < -13f && this.transform.position.y < -9.8f)
				{	//移動なし
					moveint = 0;
				}
			}
		}

		//左上移動
		if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.W))
		{
			if (this.transform.position.x > -13f && this.transform.position.y < 9.8f)
			{
				moveint = 8;
			}
			else
			{

				if (this.transform.position.x > -13f)
				{	//左移動
					moveint = 4;
				}

				if (this.transform.position.y < 9.8f)
				{	//上移動
					moveint = 1;
				}

				if (this.transform.position.x < -13f && this.transform.position.y > 9.8f)
				{	//移動なし
					moveint = 0;
				}
			}
		}

		//上だけ移動
		if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
		{
			
			if (this.transform.position.y < 9.8f)
				moveint = 1;
			else
				moveint = 0;

		}

		//右だけ移動
		if (Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
		{
			if (this.transform.position.x < 13f)
				moveint = 2;
			else
				moveint = 0;
		}

		//下だけ移動
		if (Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
		{
			if (this.transform.position.y > -9.8f)
				moveint = 3;
			else
				moveint = 0;
		}

		//左だけ移動
		if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
		{
			if (this.transform.position.x > -13f)
				moveint = 4;
			else
				moveint = 0;
		}

		switch (moveint)
		{


			//上移動
			case 1:
				pos = this.transform.position;
				pos += new Vector3(0, speed * Time.deltaTime, 0f);
				this.transform.position = pos;
				break;

			//右移動
			case 2:
				pos = this.transform.position;
				pos += new Vector3(speed * Time.deltaTime, 0, 0f);
				this.transform.position = pos;
				break;

			//下移動
			case 3:  
				pos = this.transform.position;
				pos += new Vector3(0, -(speed * Time.deltaTime), 0f);
				this.transform.position = pos;
				break;

			//左移動
			case 4: 		  
				pos = this.transform.position;
				pos += new Vector3(-(speed * Time.deltaTime), 0, 0f);
				this.transform.position = pos;
				break;

			//右上移動
			case 5:
				pos = this.transform.position;
				pos += new Vector3(speed * Time.deltaTime, speed * Time.deltaTime, 0f);
				this.transform.position = pos;
				break;

			//右下移動
			case 6: 		   
				pos = this.transform.position;
				pos += new Vector3(speed * Time.deltaTime, -speed * Time.deltaTime, 0f);
				this.transform.position = pos;
				break;

			//左下移動
			case 7:
				pos = this.transform.position;
				pos += new Vector3(-speed * Time.deltaTime, -speed * Time.deltaTime, 0f);
				this.transform.position = pos;
				break;


			//左上移動
			case 8: 		 
				pos = this.transform.position;
				pos += new Vector3(-speed * Time.deltaTime, speed * Time.deltaTime, 0f);
				this.transform.position = pos;
				break;
		}
	}

	void ItemSpawn()
	{
		switch(itemint)
		{
			case 0:
				opObj.SetActive(true);
				opObj.transform.position = transform.position;
				opObj.transform.rotation = transform.rotation;
				itemint++;
				break;

			case 1:
				opObj1.SetActive(true);
				opObj1.transform.position = transform.position;
				opObj1.transform.rotation = transform.rotation;
				itemint++;
				break;


			default:
				scorescript.AddScore(5000f);
				break;
		}

	}

	void Option_activefalse()
	{
		switch (itemint)
		{
			case 0:
				break;

			case 1:
				opObj.SetActive(false);
				itemint = 0;
				break;

			default:
				opObj1.SetActive(false);
				itemint = 1;
				break;

		}
	}

	void Power_catch()
	{
		switch ((int)bulletcnt)
		{
			case 0:
				break;

			case 1:
				break;

			case 2:
				break;

			case 3:
				if (Audiolock == false)
				{
					Audiopower_up.Play();
					PowerUPtext_active();
					Audiolock = true;
				}
				break;

			case 4:
				if (Audiolock1 == false)
				{
					Audiopower_up.Play();
					PowerUPtext_active();
					Audiolock1 = true;
				}
				break;

			case 5:
				if (Audiolock2 == false)
				{
					Audiopower_up.Play();
					PowerUPtext_active();
					Audiolock2 = true;
				}
				break;

			default:
				//スコア加算
				scorescript.AddScore(100f);
				break;
			

		}
	}

  public void extend()
	{
		if (extendint <= 99) return;
		if (life <= 1)
		{
			life += 1;
			AudioCorekaifuku.Play();
		}
		extendint = 0;
	}


		void life_Update()
	{
		corelife.Corelife -= 10f;

		if(corelife.Corelife <= 0)
		{
			//gameover
			maincontroller.Playerdeathbool = true;
			this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -10f);
			Effect_active();
			AudioDamage.Play();
			for (int i = 0; i < 2; i++)
			{
				Option_activefalse();
			}
		}
		else
		{
			mutekicnt = 5.0f;
			particlecnt = 0.5f;
			enemycontroller._intervalTo += 1f;
			enemycontroller._intervalTo1 += 1f;
			AudioDamage.Play();
			Option_activefalse();
			Damage_active();
		}
	}

	void Damege_Effect()
	{
		particlecnt -= Time.deltaTime;

		if(particlecnt >= particletime)
		{
			if(particlelock == false)
			{
				DamegeEffectObj.transform.position = this.transform.position;
				DamegeEffect.Play();

				particlelock = true;
			}

		}
		else
		{
			DamegeEffect.Stop();
			particlelock = false;
		}

	}

   void Effect_active()
	{
		GameObject obj = ObjectPoolScript.current.EffectObjPooledObject();

		if (obj == null) return;
		obj.transform.position = new Vector3(this.gameObject.transform.position.x,
											 this.gameObject.transform.position.y, 0f);
		obj.transform.rotation = this.transform.rotation;
		obj.SetActive(true);
	}

	void Damage_active()
	{
		GameObject obj = ObjectPoolScript.current.DamageObjPooledObject2();

		if (obj == null) return;
		obj.transform.position = new Vector3(this.gameObject.transform.position.x,
											 this.gameObject.transform.position.y, 0f);

		obj.SetActive(true);
	}

	void PowerUPtext_active()
	{
		GameObject Powertext = GameObject.Find("Canvas").transform.Find("PowerUpText").gameObject;
		Powertext.SetActive(true);
	}


	//void OnTriggerEnter2D(Collider2D collider)
	void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.gameObject.tag == "Item/Option")
		{
			ItemSpawn();
			AudioOptionSpawn.Play();
			collider.gameObject.SetActive(false);
		}

		if (collider.gameObject.tag == "Item/shotpower")
		{
			if (bulletcnt <= 5)
			{
				bulletcnt += 0.05f;
				Power_catch();
			}

			if (bomcnt <= 5f)
			{
				bomcnt += 0.05f;
			}
			collider.gameObject.SetActive(false);
		}

		if(collider.gameObject.tag == "Item/shotpower_blue")
		{

			specialpower += 0.1f;
			if(specialpower >= 3f)
			{
				specialbool = true;
			}

			collider.gameObject.SetActive(false);
		}

		if (collider.gameObject.tag == "Item/Corelifeup")
		{
			corelife.HealthCore();
			Debug.Log("ライフ回復");
			AudioCorekaifuku.Play();
			collider.gameObject.SetActive(false);
		}
		//enemycontroller.EnemyHpRank -= 1f;

		if (collider.gameObject.tag == "Enemy/Type1" ||
		   collider.gameObject.tag == "Enemy/Type2" ||
		   collider.gameObject.tag == "Enemy/Type3")
		{
			life_Update();
		}


		if (collider.gameObject.tag == "Enemy/Mid")
		{
			life_Update();
		}

		if (collider.gameObject.tag == "Shot/enemy")
		{
			life_Update();
			collider.gameObject.SetActive(false);
		}

	}

}
