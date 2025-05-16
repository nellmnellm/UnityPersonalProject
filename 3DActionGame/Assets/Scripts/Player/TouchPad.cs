using System;
using UnityEngine;

public class TouchPad : MonoBehaviour
{
    private RectTransform _touchPad; // UI 위치

    private int _touchId = -1;       // 영역안에 있는 입력 구분

    private Vector3 _startPos = Vector3.zero;   //입력 시작 좌표

    public float _dragRadius = 60.0f;   // 방향 컨트롤러 움직임 반지름

    public PlayerMovement _player;   // 뱡향키 변경에 따라 캐릭터 움직임

    private bool _buttonPressed = false; //버튼 눌림 여부

    private void Start()
    {
       _touchPad = GetComponent<RectTransform>();   
        _startPos = _touchPad.position; 
    }

    public void ButtonDown()
    {
        _buttonPressed = true;

    }

    public void ButtonUp()
    {
        _buttonPressed = false;
    }

    private void FixedUpdate()
    {
//크로스 플랫폼 
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE 
        HandleTouchInput(Input.mousePosition);
#else
        HandleTouchInput();

#endif
    }
    //메서드 오버로딩. 매개변수 리스트(시그니쳐) 가 다른 경우 
    private void HandleTouchInput()
    {
        Debug.Log("모바일 빌드");
    }

    private void HandleTouchInput(Vector3 mousePosition)
    {
        if (_buttonPressed)
        {
            //입력받은 좌표
            Vector3 diff = mousePosition - _startPos;

            if (diff.sqrMagnitude > _dragRadius * _dragRadius)
            {
                diff.Normalize(); // 방향 벡터 거리 1

                _touchPad.position = _startPos + diff *_dragRadius;
            }
            else
            {
                _touchPad.position = mousePosition;
            }

            Vector3 distance = _touchPad.position - _startPos;
            Vector2 normalDiff = new Vector3(distance.x / _dragRadius, 
                                             distance.y / _dragRadius, 
                                             distance.z / _dragRadius);

            if (_player != null)
            {
                _player.OnStickChanged(normalDiff);
            }
        }
    }
}
