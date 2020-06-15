using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemyscript : MonoBehaviour {

    EnemyManager Enemycontroller;
    ScoreText_Animation scorescript;
    MainController maincontroller;
    PlayerScript playerscript;

    GameObject player;
   

    private float _Hitpoint;
    private float _HitMax;
    private float rankspeed;
    private float shotcnt;
    private float shottime = 4f;
    private float movespeed;
    private float midshot;
    private float Combonew;

    float scoreRank;

    bool movexbool = false;

    bool moveybool = false;

    bool midshotbool = false;

    int move;

    float mutekicnt = 0;

    bool rensabool = false;

    AudioSource AudioEnemyDeath;

    // Use this for initialization
    void OnEnable()
    {
        Enemycontroller = GameObject.Find("GameController").GetComponent<EnemyManager>();
        maincontroller = GameObject.Find("GameController").GetComponent<MainController>();
        scorescript = GameObject.Find("Canvas").transform.Find("ScoreText").GetComponent<ScoreText_Animation>();
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
        playerscript = player.GetComponent<PlayerScript>();
        AudioEnemyDeath = GameObject.Find("Audio").transform.Find("Enemydeath").GetComponent<AudioSource>();

        rensabool = false;
        mutekicnt = 0;
        _HitMax = 1f + Mathf.Floor(Enemycontroller.EnemyHpRank);

        if (Mathf.Floor(Enemycontroller.EnemyHpRank) <= 1)
        {
            rankspeed = 1f;
        }
        else
        {
            rankspeed = Mathf.Floor(Enemycontroller.EnemyHpRank);
        }
        

        if (this.gameObject.tag == "Enemy/Mid")
        {
            _Hitpoint = _HitMax + 10f;
        }
        else
        {
            _Hitpoint = _HitMax;
        }

        shotcnt = 0;

        move = Enemycontroller._movetype1;

        if (this.gameObject.tag == "Enemy/Type2")
        {
            movespeed = 1f;

        }

        if (this.gameObject.tag == "Enemy/Mid")
        {
            movespeed = 0.5f;
        }
    }
	
	// Update is called once per frame
	void Update () {
        if (maincontroller.MainControllerbool == false)
        {
            Effect_active();
            this.gameObject.SetActive(false);
        }

        if (maincontroller.Bombool == true)
        {
            AddScore();
            Effect_active();
            this.gameObject.SetActive(false);
        }

        mutekicnt -= Time.deltaTime;
        if(mutekicnt >= 0)
        {
            this.gameObject.GetComponent<BoxCollider2D>().enabled = false;
        }
        else
        {

            this.gameObject.GetComponent<BoxCollider2D>().enabled = true;
        }

        Move();

        if (this.gameObject.tag == "Enemy/Type3")
            bulletshot(Enemycontroller.EnemyHpRank);

        if(this.gameObject.tag == "Enemy/Mid")
        {
            if (midshotbool == true)
            {
                midshot = 0;
            }
            else
            {
                midshot = 4;
            }

            bulletshot(midshot);
        }

    }

    void bulletshot(float cnt)
    {
       

        var _pos = this.transform.position;
       var playerx = player.transform.position.x - _pos.x;
       var playery = player.transform.position.y - _pos.y;

       


        if (playerx <= 5f && -5f <= playerx && playery <= 5f && -5f <= playery)
        {

            shotcnt += Time.deltaTime;

            if (shotcnt < (shottime - Mathf.Floor(Enemycontroller.EnemyHpRank))) return;

            var direction = player.transform.position - this.transform.position;
            var angle = Utils.GetAngle(Vector3.zero, direction);

            if (this.gameObject.tag == "Enemy/Type3")
            {
                if (cnt >= 3f)
                {
                    //3way
                    ShootWay(angle, 40, 3, this.gameObject);
                    shotcnt = 0;
                }
                else
                {
                    //自機狙い
                    ShootWay(angle, 40, 1, this.gameObject);
                    shotcnt = 0;
                }
            }

            if(this.gameObject.tag == "Enemy/Mid")
            {
                if (cnt >= 3f)
                {
                    //4way
                    ShootWay(angle, 40, 5, this.gameObject);
                    midshotbool = true;
                    shotcnt = 0;
                }
                else
                {
                    //5way
                    ShootWay(angle, 40, 4, this.gameObject);
                    midshotbool = false;
                    shotcnt = 0;
                }


            }


        }

    }

    private void ShootWay(
     float AngleBase, float AngleRange, int cnt, GameObject Fromobj)
    {
        if (1 < cnt)
        {
            for (int i = 0; i < cnt; ++i)
            {
                var angle = AngleBase +
                     AngleRange * ((float)i / (cnt - 1) - 0.5f);

                GameObject obj = ObjectPoolScript.current.shotObjPooledObject1();

                if (obj == null) return;
                obj.transform.position = Fromobj.transform.position;
                obj.transform.rotation = Fromobj.transform.rotation;
                obj.SetActive(true);

                Init(angle, obj);

          
            }

        }
        else if (cnt <= 1)
        {
            GameObject obj = ObjectPoolScript.current.shotObjPooledObject1();

            if (obj == null) return;
            obj.transform.position = Fromobj.transform.position;
            obj.transform.rotation = Fromobj.transform.rotation;
            obj.SetActive(true);

            Init(AngleBase, obj);

         
        }

    }

    void Init(float angle,GameObject obj)
    {
        var angles = obj.transform.localEulerAngles;
        angles.z = angle - 90;
        obj.transform.localEulerAngles = angles;
    }


    void Move()
    {
        float Randommove = Random.Range(0.3f, 1.5f);

        if(this.gameObject.tag == "Enemy/Type1")
        {
           
            switch (move)
            {
                case 0:
                    Vector3 _AwakePosy = this.transform.position;
                    _AwakePosy.y -= Randommove * rankspeed * Time.deltaTime;
                    this.transform.position = _AwakePosy;
                    break;

                case 1:
                    Vector3 _AwakePosx = this.transform.position;
                    _AwakePosx.x -= Randommove * rankspeed * Time.deltaTime;
                    this.transform.position = _AwakePosx;
                    break;

                case 2:
                    Vector3 _AwakePosy1 = this.transform.position;
                    _AwakePosy1.y += Randommove * rankspeed * Time.deltaTime;
                    this.transform.position = _AwakePosy1;
                    break;

                case 3:
                    Vector3 _AwakePosx1 = this.transform.position;
                    _AwakePosx1.x += Randommove * rankspeed * Time.deltaTime;
                    this.transform.position = _AwakePosx1;
                    break;
            }

            if(this.transform.position.x <= -16f || this.transform.position.x >= 16f ||
               this.transform.position.y <= -12f || this.transform.position.y >= 12f
                ) { this.gameObject.SetActive(false); }


        }

        if(this.gameObject.tag == "Enemy/Type2" || this.gameObject.tag == "Enemy/Mid")
        {
            

            Vector3 genten = new Vector3(0, 0, 0);

            var direction = genten  - this.transform.position;
            var angle = Utils.GetAngle(Vector3.zero, direction);

            Init(angle, this.gameObject);

            transform.Translate(0, Time.deltaTime * movespeed, 0);
          
        }

        if(this.gameObject.tag == "Enemy/Type3")
        {
            var direction = player.transform.position - this.transform.position;
            var angle = Utils.GetAngle(Vector3.zero, direction);

            Init(angle, this.gameObject);

            transform.Translate(0,Time.deltaTime* 1.5f *rankspeed , 0);

            if (this.transform.position.x <= -16f || this.transform.position.x >= 16f ||
               this.transform.position.y <= -12f || this.transform.position.y >= 12f
                ) { this.gameObject.SetActive(false); }
        }


    }

    void OnTriggerEnter2D(Collider2D obj)
    {
        if (obj.gameObject.tag == "Shot/Player")
        {
            _Hitpoint -= 1f;
            if (_Hitpoint <= 0)
            {
             
                ItemDrop();
                Effect_active();
                Conboscript.current.Combo_add();
                AddScore();
                AudioEnemyDeath.Play();
                this.gameObject.SetActive(false);
            }
            else
            {
                mutekicnt = 0.5f;
            }
        }

        if(obj.gameObject.tag == "Effect/bakuha")
        {
            if (rensabool == false)
            {
                _Hitpoint -= 1f;
                if (_Hitpoint <= 0)
                {
                    ItemDrop();
                    Effect_active();
                    Conboscript.current.Combo_add();
                    AddScore();
                    AudioEnemyDeath.Play();
                    this.gameObject.SetActive(false);
                }
                rensabool = true;
            }
        }
    }


    void ItemDrop()
    {
        if(this.gameObject.tag == "Enemy/Mid")
        {
            GameObject obj = ObjectPoolScript.current.Item1ObjPooledObject1();

            if (obj == null) return;
          
            obj.transform.position = this.transform.position;
            obj.SetActive(true);

            GameObject obj1 = ObjectPoolScript.current.Item2ObjPooledObject2();

            if (obj1 == null) return;
           
            obj1.transform.position = this.transform.position;           
            obj1.SetActive(true);


        }
        else
        {

            if (playerscript.bulletcnt <= 5)
            {
                for (int i = 0; i < Random.Range((int)0, (int)6); ++i)
                {

                  GameObject obj = ObjectPoolScript.current.PowerUpObjPooledObject();
                        
                    if (obj == null) return;

                    obj.transform.position = this.transform.position;
                    obj.SetActive(true);
                }

            }
            else
            {
                for (int i = 0; i < Random.Range((int)0, (int)6); ++i)
                {

                    GameObject obj = ObjectPoolScript.current.PowerUp_blueObjPooledObject();

                    if (obj == null) return;

                    obj.transform.position = this.transform.position;
                    obj.SetActive(true);
                }
            }



        }

    }

    void Effect_active()
    {
        if (this.gameObject.tag == "Enemy/Mid")
        {
            for (int i = 0; i < 3; i++)
            {
                GameObject obj = ObjectPoolScript.current.EffectObjPooledObject();

                if (obj == null) return;
                obj.transform.position = new Vector3(this.gameObject.transform.position.x + Random.Range(-1f, 1f),
                                                     this.gameObject.transform.position.y + Random.Range(-1f, 1f), 0f);
                obj.transform.rotation = this.transform.rotation;
                obj.SetActive(true);
            }

        }
        else
        {
            GameObject obj = ObjectPoolScript.current.EffectObjPooledObject();

            if (obj == null) return;
            obj.transform.position = new Vector3(this.gameObject.transform.position.x + Random.Range(-1f, 1f),
                                                 this.gameObject.transform.position.y + Random.Range(-1f, 1f), 0f);
            obj.transform.rotation = this.transform.rotation;
            obj.SetActive(true);
        }

    }


    void AddScore()
    {
        //ランク倍率
        if(Enemycontroller.EnemyHpRank <= 1)
        {
            scoreRank = 1f;
        }
        else
        {
            scoreRank = Mathf.Floor(Enemycontroller.EnemyHpRank);
        }

        if(Conboscript.current.Combo <= 29)
        {
            Combonew = Conboscript.current.Combo;

        }
        else
        {
            Combonew = 30;

        }

        //素点　×　コンボ数 ×　ランク　= スコア
        if (this.gameObject.tag == "Enemy/Type1")
        {
            float scoretype1 = 10f * Combonew * scoreRank;
            scorescript.AddScore(scoretype1);
        }

        if (this.gameObject.tag == "Enemy/Type2" || this.gameObject.tag == "Enemy/Type3")
        {
           
            float scoretype2 = 50f * Combonew * scoreRank;
            scorescript.AddScore(scoretype2);
        }

        if (this.gameObject.tag == "Enemy/Mid")
        {
           
            float scoretype2 = 500f * Combonew * scoreRank;
            scorescript.AddScore(scoretype2);

        }
    }


}
