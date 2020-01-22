using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    [SerializeField]
    private float shotcnt;
    private float shottime = 0.7f;

        // Update is called once per frame
        void Update () {
       
 
        mouseAngle();
       
       
    }

    void mouseAngle()
    {

        //自機がマウスに向くためのコード
        var _pos = Camera.main.WorldToScreenPoint(transform.position);
        
        var direction = Input.mousePosition - _pos;
        
        var angle = Utils.GetAngle(Vector3.zero, direction);

        var angles = transform.localEulerAngles;
        angles.z = angle - 90;
        transform.localEulerAngles = angles;






        //ショットのカウントを追加
        shotcnt += Time.deltaTime;

        //もしshottimeより以下なら終わり
        if (shotcnt < shottime) return;

        //もしshottimeより以上なら撃てる準備OK!
        //右クリックを押し続けてるなら弾が表示される！
        if (Input.GetMouseButton(0))
        {
           
            ShootWay(angle, 20, (int)bulletcnt, this.gameObject);

            if(opObj.activeSelf == true)
            {
             ShootWay(angle, 20, ((int)bulletcnt -2), opObj.gameObject);
            }

            if (opObj1.activeSelf == true)
            {
                ShootWay(angle, 20, ((int)bulletcnt -2), opObj1.gameObject);
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



                //ObjectPoolのshotObjPooledObject()を使う時の例
                GameObject obj = ObjectPoolScript.current.shotObjPooledObject();

                if (obj == null) return;

                //1.弾を発生する時に座標、角度を指定する。
                obj.transform.position = Fromobj.transform.position;
                obj.transform.rotation = Fromobj.transform.rotation;
                //弾を表示
                obj.SetActive(true);
                

                Init(angle, obj);

              
            }

        }
        else
        {
            if (cnt <= 2)
            {

                //ObjectPoolのshotObjPooledObject()を使う時の例
                GameObject obj = ObjectPoolScript.current.shotObjPooledObject();

                if (obj == null) return;

                //弾を指定する座標、角度を
                obj.transform.position = Fromobj.transform.position;
                obj.transform.rotation = Fromobj.transform.rotation;
                //弾を表示
                obj.SetActive(true);

                Init(AngleBase, obj);

               
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

}
