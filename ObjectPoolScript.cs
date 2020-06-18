using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolScript : MonoBehaviour {
	public static ObjectPoolScript current;

	//-------------------------------------------------------------------------------
	//	  生成させる数
	//-------------------------------------------------------------------------------	

	[System.Serializable]
	public class Amount
	{
		public int shotamount;

	}

	//-------------------------------------------------------------------------------
	//	  生成させる親オブジェクト
	//-------------------------------------------------------------------------------

	[SerializeField]
	public GameObject shotOyaObj;


	//-------------------------------------------------------------------------------
	//	  生成させる子オブジェクト
	//-------------------------------------------------------------------------------
	[System.Serializable]
	public class Object
	{
		public GameObject shotObj;
	   
	}

	//-------------------------------------------------------------------------------
	//	 生成させるオブジェクトのリスト
	//-------------------------------------------------------------------------------
	[System.Serializable]
	public class List
	{
		public List<GameObject> listshotobj;
	}

	//-------------------------------------------------------------------------------

	//Amount-------------------------------------------------------------------------
	[SerializeField]
	Amount amount;

	//Obj----------------------------------------------------------------------------
	[SerializeField]
	Object Obj;

	//List---------------------------------------------------------------------------
	[SerializeField]
	List list;


	void Awake()
	{
		current = this;
		Instantiate(shotOyaObj);

	}
	
	// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- 
	// Find使わない
	// 理由：
	// ・生成時にAddするのはなぜ駄目なのか？
	// ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ---- ichi
	
	// Use this for initialization
	void Start () {
  　　　//生成した親を取得(タグ)
		shotOyaObj = GameObject.FindGameObjectWithTag("OYA/shot");

		list.listshotobj = new List<GameObject>();

		for (int i = 0; i < amount.shotamount; i++)
		{
			GameObject obj = (GameObject)Instantiate(Obj.shotObj);

			//生成した子は親にセット！
			obj.transform.SetParent(shotOyaObj.transform, true);

			obj.SetActive(false);
			//リストに追加
			list.listshotobj.Add(obj);
		}

	 
	}
	
	public GameObject shotObjPooledObject()
	{
		for (int i = 0; i < list.listshotobj.Count; i++)
		{
			//n番目に非表示されてるオブジェクトを呼び出す
			if(!list.listshotobj[i].activeInHierarchy)
			{
				return list.listshotobj[i];
			}

		}
		return null;
	}

   
}
