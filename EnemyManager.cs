using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

	MainController maincontroller;

	[SerializeField]
	private float _intarvalFrom;
	
	public float _intervalTo;

	[SerializeField]
	private float _intarvalFrom1;

	public float _intervalTo1;

	// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
	// Level design は ScriptableObject を使ったほうが良い
	// 理由：
	// ・整理しやすい
	// ・Asset化出来るので入れ替えたりなど自由度が高い
	// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ichi

	//出現パターン
	[SerializeField]
	private float mainspawncnt;
	[SerializeField]
	private float spawnpatterncnt;
	[SerializeField]
	private float spawnpatterntime = 20f;
	[SerializeField]
	private int spawnpatternstatus;

	private int spawnintmin;
	private int spawnintmax;



	[SerializeField]
	private float _Spawnrtime;

	[SerializeField]
	private float _Spawnrtime1;

	[SerializeField]
	private float _Spawncnt;

	[SerializeField]
	private float _Spawncnt1;

	[SerializeField]
	private int _RandomSpawmt;

	[SerializeField]
	private int _RandomSpawmt1;

	[SerializeField]
	private float _timer;

	[SerializeField]
	private float _timer1;

	[SerializeField]
	private int _Midcnt;

	[SerializeField]
	public float EnemyHpRank;

	int midspawn;

	public int _movetype1;

	AudioSource Audiowarning01;

	void Awake()
	{
	// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
	// Findは重いのでどうしても書くなら full pathで書くこと
	// 理由：
	// ・むしろFindしなければいけない状況が良くない。整理出来てないなど。
	// ・Test codeや prottype に使うのはありだが、可能なら製品としてReleaseするものには入れたくない
	// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ichi
		
		maincontroller = GameObject.Find("GameController").GetComponent<MainController>();
		Audiowarning01 = GameObject.Find("Audio").transform.Find("warning01").GetComponent<AudioSource>();
		_intervalTo = 3;
		_intervalTo1 = 7;
		EnemyHpRank = 0;
	}

	// Update is called once per frame
	void Update() {

	// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
	// Updateのような毎Frame動作する場所でのflag分岐は可能な限り減らす（無くす）
	// 理由：
	// ・無駄
	// 　1個ならいいなら2個ならいけるのか？可能なら無くしたい
	// 　delete を使うなど、状態遷移がしっかり出来ていれば必要はないはず
	// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ichi

		//MainController 「Redy&GameOver」で動作停止
		if (maincontroller.MainControllerbool == false) return;

		mainspawncnt += Time.deltaTime;

		_Spawncnt += Time.deltaTime;
		_Spawncnt1 += Time.deltaTime;

		_timer += Time.deltaTime;
		_timer1 += Time.deltaTime;

		spawnpatterncnt += Time.deltaTime;

	// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
	// Timerは一本化できるのでは？
	// 理由：
	// ・コードの可読性がさがり、バグを生みやすい。また変更もしづらい。
	//　全ての変数にTime.deltaTimeを足し込んでいるが、Time.deltaTimeを足し込む変数は1本に出来る（気がする）
	//　出すタイミングが違うだけで、TimerをTriggerにするという動作は同じなので、もっとSimpleにかけるはず。
	//　
	// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ichi

		var t = _Spawncnt / _Spawnrtime;
		var t1 = _Spawncnt1 / _Spawnrtime1;
		var interval = Mathf.Lerp(_intarvalFrom, _intervalTo, t);
		var interval1 = Mathf.Lerp(_intarvalFrom1, _intervalTo1,t1);


		if(spawnpatterncnt >= spawnpatterntime)
		{
			if (mainspawncnt <= 50f)
			{
				spawnpatternstatus = Random.Range(0, 4);
			}
			else
			{
				spawnpatternstatus = Random.Range(4, 6);
			}

			Spawnpattern(spawnpatternstatus);
			Debug.Log(spawnpatternstatus);
			spawnpatterncnt = 0;
		}


		if (_timer > interval)
		{
			_RandomSpawmt = Random.Range(spawnintmin, spawnintmax);

			ZakoType1Spawn(_RandomSpawmt);


			_Midcnt++;
			MidSpawm(_Midcnt);

			if (EnemyHpRank <= 3f)
			{
				EnemyHpRank += 0.005f;
			}

			_timer = 0;
		}

		if (_timer1 > interval1)
		{
			_RandomSpawmt1 = Random.Range(spawnintmin, spawnintmax);

			for (int i = 0; i < 5; ++i)
			{
				ZakoType2_3Spawn(_RandomSpawmt1);
			}

			for(int i = 0; i < EnemyHpRank; i++)
			{
				ZakoType3_Spawn(_RandomSpawmt1);
			}

			if (EnemyHpRank <= 3f)
			{
				EnemyHpRank += 0.001f;
			}

			_timer1 = 0;
		}



	}

	// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
	// Tableにしたほうが良い
	// 理由：
	// ・修正しづらい。Logicが無い。
	//　Logicが無いなら純粋なDataにしてしまったほうが良い。
	// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ichi


	void Spawnpattern(int pattern)
	{
		switch (pattern)
		{
			case 0:
				spawnintmin = 0;
				spawnintmax = 1;
				break;

			case 1:
				spawnintmin = 1;
				spawnintmax = 2;
				break;

			case 2:
				spawnintmin = 2;
				spawnintmax = 3;
				break;

			case 3:
				spawnintmin = 3;
				spawnintmax = 4;
				break;

			case 4:
				spawnintmin = 0;
				spawnintmax = 2;
				break;

			case 5:
				spawnintmin = 2;
				spawnintmax = 4;
				break;

		}
		Debug.Log(pattern);
	}


	// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
	// Class化したほうが良い
	// 理由：
	// ・やっていることがほぼ同じなので。
	//　　こちらもDataなのかLogicなのかぱっとみで判断しづらい。
	// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ichi

	void ZakoType1Spawn(int cnt)
	{
		switch (cnt)
		{
			case 0:
				_movetype1 = 0;
				for (int i = 0; i < 3; ++i)
				{
					GameObject obj = ObjectPoolScript.current.enemyObjPooledObject();

					if (obj == null) return;
					obj.transform.position = new Vector3(Random.Range(-13f,13f),10.8f,0);
					obj.transform.rotation = transform.rotation;
					obj.SetActive(true);
				}
					break;

			case 1:
				_movetype1 = 1;
				for (int i = 0; i < 3; ++i)
				{
					GameObject obj1 = ObjectPoolScript.current.enemyObjPooledObject();

					if (obj1 == null) return;
					obj1.transform.position = new Vector3(13f, Random.Range(-10.8f,10.8f), 0);
					obj1.transform.rotation = transform.rotation;
					obj1.SetActive(true);
				}
					break;

			case 2:
				_movetype1 = 2;
				for (int i = 0; i < 3; ++i)
				{
					GameObject obj2 = ObjectPoolScript.current.enemyObjPooledObject();

					if (obj2 == null) return;
					obj2.transform.position = new Vector3(Random.Range(-13f, 13f), -10.8f, 0);
					obj2.transform.rotation = transform.rotation;
					obj2.SetActive(true);
				}
					break;

			case 3:
				_movetype1 = 3;
				for (int i = 0; i < 3; ++i)
				{
					GameObject obj3 = ObjectPoolScript.current.enemyObjPooledObject();

					if (obj3 == null) return;
					obj3.transform.position = new Vector3(-13f, Random.Range(-10.8f, 10.8f), 0);
					obj3.transform.rotation = transform.rotation;
					obj3.SetActive(true);
				}
					break;


		}

	}

	// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
	// Class化したほうが良い
	// 理由：
	// ・↑と同様
	// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ichi

	void ZakoType2_3Spawn(int cnt)
	{

		switch (cnt)
		{
			case 0:
				GameObject obj = ObjectPoolScript.current.enemy1ObjPooledObject();

				if (obj == null) return;
				obj.transform.position = new Vector3(Random.Range(-13f, 13f), 10.8f, 0);
				obj.transform.rotation = transform.rotation;
				obj.SetActive(true);
				break;

			case 1:
				GameObject obj1 = ObjectPoolScript.current.enemy1ObjPooledObject();

				if (obj1 == null) return;
				obj1.transform.position = new Vector3(13f, Random.Range(-10.8f, 10.8f), 0);
				obj1.transform.rotation = transform.rotation;
				obj1.SetActive(true);
				break;

			case 2:
				GameObject obj2 = ObjectPoolScript.current.enemy1ObjPooledObject();

				if (obj2 == null) return;
				obj2.transform.position = new Vector3(Random.Range(-13f, 13f), -10.8f, 0);
				obj2.transform.rotation = transform.rotation;
				obj2.SetActive(true);
				break;

			case 3:
				GameObject obj3 = ObjectPoolScript.current.enemy1ObjPooledObject();

				if (obj3 == null) return;
				obj3.transform.position = new Vector3(-13f, Random.Range(-10.8f, 10.8f), 0);
				obj3.transform.rotation = transform.rotation;
				obj3.SetActive(true);
				break;

		}

	}

	// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
	// Class化したほうが良い
	// 理由：
	// ・↑と同様
	// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ichi

	void ZakoType3_Spawn(int cnt)
	{

		switch (cnt)
		{

			case 0:
				GameObject obj4 = ObjectPoolScript.current.enemy2ObjPooledObject();

				if (obj4 == null) return;
				obj4.transform.position = new Vector3(Random.Range(-13f, 13f), 10.8f, 0);
				obj4.transform.rotation = transform.rotation;
				obj4.SetActive(true);
				break;

			case 1:
				GameObject obj5 = ObjectPoolScript.current.enemy2ObjPooledObject();

				if (obj5 == null) return;
				obj5.transform.position = new Vector3(13f, Random.Range(-10.8f, 10.8f), 0);
				obj5.transform.rotation = transform.rotation;
				obj5.SetActive(true);
				break;

			case 2:
				GameObject obj6 = ObjectPoolScript.current.enemy2ObjPooledObject();

				if (obj6 == null) return;
				obj6.transform.position = new Vector3(Random.Range(-13f, 13f), -10.8f, 0);
				obj6.transform.rotation = transform.rotation;
				obj6.SetActive(true);
				break;

			case 3:
				GameObject obj7 = ObjectPoolScript.current.enemy2ObjPooledObject();

				if (obj7 == null) return;
				obj7.transform.position = new Vector3(-13f, Random.Range(-10.8f, 10.8f), 0);
				obj7.transform.rotation = transform.rotation;
				obj7.SetActive(true);
				break;

		}

	}


	// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
	// Class化したほうが良い
	// 理由：
	// ・↑と同様
	// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ichi

	void MidSpawm(int cnt)
	{
		if (cnt < 20) return;

		midspawn = Random.Range(0, 4);
		_intervalTo -= 1f;
		_intervalTo1 -= 1f;

		switch (midspawn)
		{
			case 0:
				GameObject obj = ObjectPoolScript.current.enemy3ObjPooledObject();

				if (obj == null) return;
				obj.transform.position = new Vector3(Random.Range(-13f, 13f), 10.8f, 0);
				obj.transform.rotation = transform.rotation;
				obj.SetActive(true);

				
				break;

			case 1:
				GameObject obj1 = ObjectPoolScript.current.enemy3ObjPooledObject();

				if (obj1 == null) return;
				obj1.transform.position = new Vector3(13f, Random.Range(-10.8f, 10.8f), 0);
				obj1.transform.rotation = transform.rotation;
				obj1.SetActive(true);


				break;

			case 2:
				GameObject obj2 = ObjectPoolScript.current.enemy3ObjPooledObject();

				if (obj2 == null) return;
				obj2.transform.position = new Vector3(Random.Range(-13f, 13f), -10.8f, 0);
				obj2.transform.rotation = transform.rotation;
				obj2.SetActive(true);


				break;

			case 3:
				GameObject obj3 = ObjectPoolScript.current.enemy3ObjPooledObject();

				if (obj3 == null) return;
				obj3.transform.position = new Vector3(-13f, Random.Range(-10.8f, 10.8f), 0);
				obj3.transform.rotation = transform.rotation;
				obj3.SetActive(true);


				break;


		}

		Audiowarning01.Play();
		_Midcnt = 0;
	}

	 

}
