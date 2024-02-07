using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.Purchasing;

public class ColorChanger : MonoBehaviour
{
    public Material[] _ChangeMat;


    [SerializeField] float Floating_Current_time = 0f;
    [SerializeField] float Floating_Max_Time = 0.3f;
    [SerializeField] GameManager _gamemanager;

    public int TypeNum = 1;

    private void OnEnable()
    {
        StartCoroutine(Cor_Timer());
        if (_gamemanager == null)
            _gamemanager = GameManager.instance;
    }

    IEnumerator Cor_Timer()
    {
        WaitForSeconds _time = new WaitForSeconds(Time.deltaTime);
        while (true)
        {
            yield return _time;

            Floating_Current_time += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Popcorn"))
        {
            if (other.GetComponent<Popcorn>().PopcornState == Popcorn.State.Type0)
            {
                other.GetComponent<MeshRenderer>().material = _ChangeMat[Random.Range(0, _ChangeMat.Length)];
                _gamemanager.Add_Order_count(GameManager.Order.ColorChange);

                other.GetComponent<Popcorn>().PopcornState = (Popcorn.State)TypeNum;
            }

            Popcorn _corn = other.GetComponent<Popcorn>();
            //_gamemanager.ManagerAddMoney();
            _gamemanager.ManagerAddMoney((int)_corn.PopcornState);

            if (_gamemanager.Floating_Waiting_Pool.childCount <= 0)
            {
                _gamemanager.Add_Floating_Pool(_gamemanager.Add_Pool_Size);
            }
            if (Floating_Current_time >= Floating_Max_Time)
            {
                Floating_Current_time = 0;

                Transform _floating = _gamemanager.Floating_Waiting_Pool.GetChild(0);
                _floating.SetParent(_gamemanager.Floating_Using_Pool);
                _floating.transform.position = new Vector3(other.transform.position.x, other.transform.position.y, -6f) + Vector3.up * 1f;
                _floating.localScale = Vector3.one * 0.01f;
                _floating.gameObject.SetActive(true);

                //_floating.GetChild(1).GetComponent<Text>().text = "$" + GameManager.ToCurrencyString(_gamemanager.temp_money);

                switch ((int)_corn.PopcornState)
                {
                    case 0:
                    case 1:
                    case 2:
                        _floating.GetChild(1).GetComponent<Text>().text = "$" + GameManager.ToCurrencyString(_gamemanager.temp_money);
                        break;

                    case 3:
                        _floating.GetChild(1).GetComponent<Text>().text = "$" + GameManager.ToCurrencyString(_gamemanager.temp_money * 3d);
                        break;

                }


                DOTween.Sequence().Append(_floating.transform.DOMove(_floating.transform.position + Vector3.up * 2f, 0.75f))

                    .OnComplete(() =>
                    {
                        _floating.SetParent(_gamemanager.Floating_Waiting_Pool);
                        _floating.gameObject.SetActive(false);
                    });

            }

        }
    }
}
