using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option : MonoBehaviour {

    GameObject player;

    GameObject optionobj;

    PlayerScript playerscript;

    Vector3 pos;


    // Use this for initialization
    void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerscript = player.GetComponent<PlayerScript>();

        if(this.gameObject.tag == "Option/op1")
        {
            optionobj = GameObject.FindGameObjectWithTag("Option/op").gameObject;
        }

    }
	
	// Update is called once per frame
	void Update () {
        Optionmove();

      

    }

    void Optionmove()
    {
        if (this.gameObject.tag == "Option/op")
        {
            if (playerscript.movebool == true)
            {
                pos = this.transform.position;

                pos.x += (player.transform.position.x - transform.position.x) * Time.deltaTime * (playerscript.speed + 2f);

                pos.y += (player.transform.position.y - transform.position.y) * Time.deltaTime * (playerscript.speed + 2f);

                this.transform.position = pos;
                transform.rotation = player.transform.rotation;
            }
            else
            {
                transform.rotation = player.transform.rotation;
            }

        }

        if (this.gameObject.tag == "Option/op1")
        {
            if (playerscript.movebool == true)
            {
                pos = this.transform.position;

                pos.x += (optionobj.transform.position.x - transform.position.x) * Time.deltaTime * (playerscript.speed + 2f);

                pos.y += (optionobj.transform.position.y - transform.position.y) * Time.deltaTime * (playerscript.speed + 2f);

                this.transform.position = pos;
                transform.rotation = player.transform.rotation;
            }
            else
            {
                transform.rotation = player.transform.rotation;
            }

        }




    }

    private void ShootWay(
     float AngleBase, float AngleRange, int cnt)
    {
        if (1 < cnt)
        {
            for (int i = 0; i < cnt; ++i)
            {
                var angle = AngleBase +
                     AngleRange * ((float)i / (cnt - 1) - 0.5f);

                GameObject obj = ObjectPoolScript.current.shotObjPooledObject();

                if (obj == null) return;
                obj.transform.position = transform.position;
                obj.transform.rotation = transform.rotation;
                obj.SetActive(true);

                Init(angle, obj);
                playerscript.shotbool = false;
                Debug.Log("確認1");
            }

        }
        else if (cnt == 1)
        {
            GameObject obj = ObjectPoolScript.current.shotObjPooledObject();

            if (obj == null) return;
            obj.transform.position = transform.position;
            obj.transform.rotation = transform.rotation;
            obj.SetActive(true);

            Init(AngleBase, obj);
            playerscript.shotbool = false;
            Debug.Log("確認");
        }

    }

    void Init(float angle, GameObject obj)
    {
        //弾が進行方向を向くようにする
        var angles = obj.transform.localEulerAngles;
        angles.z = angle - 90;
        obj.transform.localEulerAngles = angles;

    }

   

}
