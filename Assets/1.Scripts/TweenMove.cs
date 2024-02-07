using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine.UI;


public class TweenMove : MonoBehaviour
{

    public enum State
    {
        Up_down_move,
        Left_Right_move,
        ToThrow,
        Scale,
        Rot,
        None,
        ReStart_Left_Right_Move,
        Text_onoff
    }
    public State _state;


    [FoldoutGroup("Move")] public float Move_x, Move_y;
    [FoldoutGroup("Move")] public float Move_time;

    [FoldoutGroup("Scale")] public float Add_Sacle = 0.2f;
    [FoldoutGroup("Scale")] public float Scale_time = 0.75f;

    [FoldoutGroup("Rot")] public Vector3 Rot;
    [FoldoutGroup("Rot")] public float Rot_time = 0.3f;

    public DG.Tweening.Ease _ease = Ease.Linear;

    Text _text;

    void Start()
    {
        _text = GetComponent<Text>();

        switch (_state)
        {
            case State.Up_down_move:
                transform.DOMoveY(Move_y, Move_time)
           .SetEase(_ease).SetRelative(true).SetLoops(-1, LoopType.Yoyo);
                break;

            case State.Left_Right_move:
                transform.DOMoveX(Move_x, Move_time)
        .SetEase(_ease).SetRelative(true).SetLoops(-1, LoopType.Yoyo);
                break;

            case State.Scale:
                transform.DOScale(Add_Sacle, Scale_time)
                    .SetEase(_ease).SetRelative(true).SetLoops(-1, LoopType.Yoyo);
                break;

            case State.Rot:

                transform.DORotateQuaternion(Quaternion.Euler(Rot), Rot_time)
                    .SetEase(_ease).SetRelative(true).SetLoops(-1, LoopType.Yoyo);
                break;

            case State.ReStart_Left_Right_Move:
                transform.DOMoveX(Move_x, Move_time)
       .SetEase(_ease).SetRelative(true).SetLoops(-1, LoopType.Restart);

                break;

            case State.Text_onoff:

                _text.DOColor(new Vector4(1f, 1f, 1f, 0f), Move_time)
                     .SetEase(_ease).SetRelative(true).SetLoops(-1, LoopType.Yoyo);

                break;

            default:
                break;
        }


    }

    private void OnTriggerEnter(Collider other)
    {
        if (_state == State.ToThrow)
        {
            if (other.CompareTag("Popcorn"))
            {
                DOTween.Sequence().Append(other.transform.DOMove(transform.GetChild(0).transform.position, Move_time).SetEase(Ease.Linear))
                .OnComplete(() => other.GetComponent<Rigidbody>().velocity = transform.forward * 10f);
            }
        }
    }


}
