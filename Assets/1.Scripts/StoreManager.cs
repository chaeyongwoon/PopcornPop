using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


//using System;

public class StoreManager : MonoBehaviour
{
    //public GameObject[] Corn_Obj;
    public int Max_Corn = 400;
    public int[] Corn_Count;
    //public List<GameObject>[] Corn_List;

    //public Transform[] Spawn_Pos;
    //public Transform[] Waiting_Pool, Using_Pool;

    //public float Spawn_Power = 1000f;
    public bool[] Corn_bool;
    public float _reduceTime = 0.05f;

    ////////// UI
    public RawImage SignBoard_RawImg;
    public Texture[] Raw_Textures;
    public GameObject Money_img;

    public Text Mission_Text;

    public int Mission_Num;
    public int Mission_Max_Count;
    public int[] Mission_Max_List = { 50, 100, 200 };
    public int Mission_Current_Count;


    // ////////////////////////////////////
    public GameObject Staff;
    public Transform[] Customers;
    public Queue<Transform> Queue_Customers;
    public Transform[] WaitPos;

    public GameObject[] Buzzer_Group;
    public Transform[] Buzzers;

    public double Mission_Money;

    public bool isPress = false;
    public enum State
    {
        Greeting,
        Mission,
        Clear,
        Wait

    }
    public State Mission_State;

    GameManager _gamemamanger;

    public Animator _storeTextAni;

    //     ///////////////////////////////////////////

    public Transform[] Corn_Box;
    public float Min_Height = -6f;
    public float Max_Height = 0f;


    // /////////////////////////////////////////////////
    private void Awake()
    {

        Corn_Count = new int[3];
        //Corn_List = new List<GameObject>[3];

        //Waiting_Pool = new Transform[3];
        //Using_Pool = new Transform[3];
        Corn_bool = new bool[3];

        Queue_Customers = new Queue<Transform>();
        for (int i = 0; i < 6; i++)
        {
            Queue_Customers.Enqueue(Customers[i]);
        }

        //for (int i = 0; i < 3; i++)
        //{

        //    Waiting_Pool[i] = new GameObject().transform;
        //    Using_Pool[i] = new GameObject().transform;

        //    Waiting_Pool[i].name = $"{i}.Waiting_Pool";
        //    Using_Pool[i].name = $"{i}.Using_Pool";

        //    Waiting_Pool[i].SetParent(transform);
        //    Using_Pool[i].SetParent(transform);

        //    AddPool(i);
        //}
        Move_Customer_Pos();
        Mission_Update();

        if (_gamemamanger == null) _gamemamanger = GameManager.instance;
        StartCoroutine(Cor_Update());

        InitPool();
    }

    IEnumerator Cor_Update()
    {
        WaitForSeconds _time = new WaitForSeconds(_reduceTime);
        while (true)
        {
            for (int i = 0; i < 3; i++)
            {
                if (Corn_bool[i] == true)
                {
                    RemoveCorn(i);

                }

            }



            yield return _time;
        }
    }


    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.F))
        {
            Move_Customer_Pos();
        }

        if (isPress)
        {
            GameManager.instance.Vibe(0);
            //_gamemamanger.Sound(3,1);
        }

        //for (int i = 0; i < 3; i++)
        //{
        //    if (Corn_bool[i] == true)
        //    {
        //        RemoveCorn(i);
        //        if (i == Mission_Num)
        //        {
        //            Mission_Current_Count--;
        //            if (Mission_Current_Count <= 0)
        //            {
        //                Mission_Current_Count = 0;
        //                Mission_Clear();
        //            }
        //        }
        //    }

        //}
    }

    //public void AddPool(int _num, int _count = 100)
    //{
    //    for (int i = 0; i < _count; i++)
    //    {
    //        int _num2 = GameManager.instance.Current_Stage_Level != 1 ? _num : _num + 3;

    //        Transform _corn = Instantiate(Corn_Obj[_num2]).transform;
    //        //Transform _corn = Instantiate(Corn_Obj[_num]).transform;
    //        _corn.SetParent(Waiting_Pool[_num]);
    //        _corn.transform.localPosition = Vector3.zero;
    //        _corn.gameObject.SetActive(false);
    //    }
    //}


    public void AddCount(int _num = 0)
    {

        //Corn_Count[_num] = _num == 0 ? Corn_Count[_num] + 1 : Corn_Count[_num] + 2;
        if (_num == 3) _num = 0;

        if (Corn_Count[_num] < Max_Corn)
        {
            //SpawnCorn(_num);
            Corn_Count[_num]++;
            if (_num != 0)
            {
                //SpawnCorn(_num);
                Corn_Count[_num]++;
            }

            Corn_Box[_num].DOLocalMoveY(-6f + (float)Corn_Count[_num] / (float)Max_Corn * Mathf.Abs(Max_Height - Min_Height), 0.02f)
                .SetEase(Ease.Linear);
            //= new Vector3(0f, -6f + (float)Corn_Count[_num] / (float)Max_Corn * Mathf.Abs(Max_Height - Min_Height), 0f);

        }

    }


    //public void SpawnCorn(int _num)
    //{
    //    if (Waiting_Pool[_num].childCount <= 0)
    //    {
    //        AddPool(_num);
    //    }
    //    Transform _corn = Waiting_Pool[_num].GetChild(0);
    //    _corn.SetParent(Using_Pool[_num]);
    //    _corn.transform.position = Spawn_Pos[_num].position;
    //    _corn.rotation = Quaternion.Euler(new Vector3(Random.Range(0f, 360f), Random.Range(0f, 360f), Random.Range(0f, 360f)));
    //    _corn.GetComponent<Rigidbody>().AddForce(_corn.forward * Spawn_Power);
    //    _corn.gameObject.SetActive(true);

    //    DOTween.Sequence(_corn.localScale = Vector3.zero)
    //        .Append(_corn.DOScale(0.4f, 0.3f));

    //}


    public void RemoveCorn(int _num)
    {
        if (Mission_State != State.Clear)
        {

            if (Corn_Count[_num] > 0)
            {
                Corn_Count[_num]--;
                if (Mission_Num == _num)
                {
                    Mission_Current_Count--;
                    Corn_Box[_num].DOLocalMoveY(-6f + (float)Corn_Count[_num] / (float)Max_Corn * Mathf.Abs(Max_Height - Min_Height), 0.02f)
                .SetEase(Ease.Linear);
                    //     Corn_Box[_num].localPosition
                    //= new Vector3(0f, -6f + (float)Corn_Count[_num] / (float)Max_Corn * Mathf.Abs(Max_Height - Min_Height), 0f);
                    Mission_Text.text = $"X{Mission_Current_Count}";
                    if (Mission_Current_Count <= 0)
                    {
                        Mission_Current_Count = 0;
                        Mission_Text.text = $"X{Mission_Current_Count}";
                        Mission_Clear();
                    }
                }


            }



            //if (Using_Pool[_num].childCount > 0)
            //{
            //    Transform _corn = Using_Pool[_num].GetChild(0);
            //    _corn.gameObject.SetActive(false);
            //    _corn.SetParent(Waiting_Pool[_num]);

            //    DOTween.Sequence(_corn.localScale = Vector3.one * 0.4f)
            //    .Append(_corn.DOScale(0f, 0.3f));

            //    Corn_Count[_num]--;
            //    if (Mission_Num == _num)
            //    {
            //        Mission_Current_Count--;
            //        Mission_Text.text = $"X{Mission_Current_Count}";
            //        if (Mission_Current_Count <= 0)
            //        {
            //            Mission_Current_Count = 0;
            //            Mission_Text.text = $"X{Mission_Current_Count}";
            //            Mission_Clear();
            //        }
            //    }
            //}
        }
    }

    public void PointerDown(int _num)
    {
        if (Corn_bool[_num] == false)
            Buzzers[_num /*+ GameManager.instance.Current_Stage_Level * 3*/].GetChild(0).transform.localPosition = new Vector3(0f, -0.13f, 0f);

        Corn_bool[_num] = true;

        isPress = true;
    }

    public void PointerUp(int _num)
    {
        if (Corn_bool[_num] == true)
            Buzzers[_num /*+ GameManager.instance.Current_Stage_Level * 3*/].GetChild(0).transform.localPosition = Vector3.zero;
        Corn_bool[_num] = false;

        isPress = false;
    }


    public void InitPool()
    {


        for (int i = 0; i < 3; i++)
        {
            //int _count = Using_Pool[i].childCount;
            //for (int j = 0; j < _count; j++)
            //{
            //    Destroy(Using_Pool[i].GetChild(j).gameObject);

            //}

            //int _count2 = Waiting_Pool[i].childCount;
            //for (int j = 0; j < _count2; j++)
            //{
            //    Destroy(Waiting_Pool[i].GetChild(j).gameObject);
            //}

            Corn_Count[i] = 0;
            Corn_Box[i].DOLocalMoveY(-6f + (float)Corn_Count[i] / (float)Max_Corn * Mathf.Abs(Max_Height - Min_Height), 0.02f)
                .SetEase(Ease.Linear);
            //Corn_Box[i].localPosition
            //   = new Vector3(0f, -6f + (float)Corn_Count[i] / (float)Max_Corn * Mathf.Abs(Max_Height - Min_Height), 0f);
        }

        if (GameManager.instance.Current_Stage_Level != 1)
        {
            Buzzer_Group[0].SetActive(true);
            Buzzer_Group[1].SetActive(false);
        }
        else
        {
            Buzzer_Group[0].SetActive(false);
            Buzzer_Group[1].SetActive(true);
        }

        Mission_Update();

    }


    public void Mission_Update()
    {
        //Debug.Log("mission update");
        _storeTextAni.SetBool("isStop", false);
        Mission_State = State.Mission;
        SignBoard_RawImg.gameObject.SetActive(true);
        Money_img.SetActive(false);

        Staff.GetComponent<Animator>().SetBool("Greeting", true);
        Mission_Num = Random.Range(0, 4);


        if (Mission_Num == 3) Mission_Num = 0;

        Mission_Max_Count = Mission_Max_List[Random.Range(0, 3)];
        Mission_Current_Count = Mission_Max_Count;

        SignBoard_RawImg.texture = Raw_Textures[Mission_Num];

        Mission_Text.text = $"X{Mission_Current_Count}";

        for (int i = 0; i < 6; i++)
        {
            DOTween.Kill(Buzzers[i]);
            Buzzers[i].transform.localScale = Vector3.one;
        }

        Buzzers[Mission_Num /*+ GameManager.instance.Current_Stage_Level * 3*/]
            .DOScale(Vector3.one * 0.2f, 0.2f).SetEase(Ease.Linear).SetRelative(true).SetLoops(-1, LoopType.Yoyo);




    }

    public void Mission_Clear()
    {
        Mission_State = State.Clear;

        for (int i = 0; i < 3; i++)
        {
            Corn_bool[i] = false;
            isPress = false;
        }
        for (int i = 0; i < 6; i++)
        {
            DOTween.Kill(Buzzers[i]);
            Buzzers[i].transform.localScale = Vector3.one;
            Buzzers[i].GetChild(0).transform.localPosition = Vector3.zero;
        }


        StartCoroutine(Cor_Mission_Clear());

        IEnumerator Cor_Mission_Clear()
        {
            yield return new WaitForSeconds(0.5f);
            Mission_Text.text = $"Clear!";
            yield return new WaitForSeconds(1f);
            SignBoard_RawImg.gameObject.SetActive(false);
            Money_img.SetActive(true);
            Mission_Money = GameManager.instance.temp_money * Mission_Max_Count * 10;
            Mission_Text.text = string.Format("+" + GameManager.ToCurrencyString(Mission_Money, 3));
            _storeTextAni.SetBool("isStop", true);
            _gamemamanger.Money += Mission_Money;
            _gamemamanger.Sound(2, 1);
            yield return new WaitForSeconds(1f);
            Move_Customer_Pos();
            yield return new WaitForSeconds(1f);
            Mission_Update();
        }
    }


    public void Move_Customer_Pos()
    {
        //Debug.Log("move");
        Staff.GetComponent<Animator>().SetBool("Greeting", false);
        Queue_Customers.Enqueue(Queue_Customers.Dequeue());

        for (int i = 0; i < 6; i++)
        {
            Transform _customer = Queue_Customers.Peek();
            _customer.transform.GetChild(0).GetComponent<Animator>().SetBool("Wave", false);
            switch (i)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                    _customer.transform.GetChild(0).GetComponent<Animator>().SetBool("Walking", true);

                    break;

                case 5:
                    _customer.transform.GetChild(0).GetComponent<Animator>().SetBool("Walking_Box", true);
                    break;


            }

            _customer.DORotate(WaitPos[i].rotation.eulerAngles, 0.2f);
            _customer.DOMove(WaitPos[i].position, 1f)
                .OnComplete(() =>
                {
                    _customer.transform.GetChild(0).GetComponent<Animator>().SetBool("Walking", false);
                    _customer.transform.GetChild(0).GetComponent<Animator>().SetBool("Walking_Box", false);
                    //_customer.transform.GetChild(0).GetComponent<Animator>().SetBool("Wave", true);
                    StartCoroutine(Cor_Wave());
                });
            Queue_Customers.Enqueue(Queue_Customers.Dequeue());


            IEnumerator Cor_Wave()
            {
                float _time = Random.Range(0.0f, 1.0f);
                //Debug.Log(_time);
                yield return new WaitForSeconds(_time);
                _customer.transform.GetChild(0).GetComponent<Animator>().SetBool("Wave", true);
            }
        }

    }



}
