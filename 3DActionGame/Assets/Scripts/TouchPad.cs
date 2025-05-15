using System;
using UnityEngine;

public class TouchPad : MonoBehaviour
{
    private RectTransform _touchPad; // UI ��ġ

    private int _touchId = -1;       // �����ȿ� �ִ� �Է� ����

    private Vector3 _startPos = Vector3.zero;   //�Է� ���� ��ǥ

    public float _dragRadius = 60.0f;   // ���� ��Ʈ�ѷ� ������ ������

    public PlayerMovement _player;   // ����Ű ���濡 ���� ĳ���� ������

    private bool _buttonPressed = false; //��ư ���� ����

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
//ũ�ν� �÷��� 
#if UNITY_EDITOR || UNITY_STANDALONE_WIN || UNITY_STANDALONE 
        HandleTouchInput(Input.mousePosition);
#else
        HandleTouchInput();

#endif
    }
    //�޼��� �����ε�. �Ű����� ����Ʈ(�ñ״���) �� �ٸ� ��� 
    private void HandleTouchInput()
    {
        Debug.Log("����� ����");
    }

    private void HandleTouchInput(Vector3 mousePosition)
    {
        if (_buttonPressed)
        {
            //�Է¹��� ��ǥ
            Vector3 diff = mousePosition - _startPos;

            if (diff.sqrMagnitude > _dragRadius * _dragRadius)
            {
                diff.Normalize(); // ���� ���� �Ÿ� 1

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
