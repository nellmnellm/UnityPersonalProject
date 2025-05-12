using System;
using UnityEngine;

[Serializable] //클래스나 구조체를 인스펙터에 표시합니다.
public class Data
{
    [Range(1, 5)] public int value;    // 범위를 설정합니다. (슬라이더 형식으로 표현)
    [Multiline(5)] public string s;   // 문자열 작성의 라인 수를 증가시켜줍니다.
    [TextArea(3, 5)] public string s2; //문자열 작성의 최소 라인과 최대 라인을 설정합니다.

    [SerializeField] float f; //필드를 인스펙터에 표시합니다.

    [Tooltip("게임 오브젝트")] public GameObject gameObject; //변수에 마우스를 가져다대면 툴팁이 뜸.
}
public class NotSerial
{
    [Range(1, 5)] public int value;    // 범위를 설정합니다. (슬라이더 형식으로 표현)
    [Multiline(5)] public string s;   // 문자열 작성의 라인 수를 증가시켜줍니다.
    [TextArea(3, 5)] public string s2; //문자열 작성의 최소 라인과 최대 라인을 설정합니다.
    [Multiline(3)][ContextMenuItem("SetStory", "setStroy")] public string s3;
    [SerializeField] float f; //필드를 인스펙터에 표시합니다.
    [Space(10)] public bool ischeck; //적은 숫자만큼 간격을 가진 뒤 필드 표현
    [Tooltip("게임 오브젝트")] public GameObject gameObject; //변수에 마우스를 가져다대면 툴팁이 뜸.
}


//플래그 속성이 들어간 enum은 복수 선택이 가능합니다.
//ex) 카메라의 CullingMask를 보면 Everything, None , 여러개 선택 등을 할 수 있는데
//이 기능이 포함되서 가능하다.
//플래그 설정이 된 값들은 비트 연산을 통해서 칸 이동이 가능합니다.
[Flags]
public enum TYPE
{
    물 = 1,
    풀 = 2,
    전기 = 4,
    화염 = 8,
    고스트 = 16,
    에스퍼 = 32
}

//Add Component 버튼을 이용한 추가 기능 만들어짐
//메뉴의 Component 기능에 추가됩니다.
[AddComponentMenu("Sample/Sample")]
public class Sample : MonoBehaviour
{
    public Data data;
    public NotSerial serial;

    public TYPE type;

    [Multiline(3)][ContextMenuItem("SetStory", "setStory")] public string s3;
    [Space(10)] public bool ischeck; //적은 숫자만큼 간격을 가진 뒤 필드 표현
    public void setStory()
    {
        s3 = "오늘은 날씨가 습하고 덥고 미치겠습니다.";
    }

    [ContextMenu("BoolCheck")]
    public void boolCheck()
    {
        if (ischeck)
        {
            ischeck = false;
        }
        else
        {
            ischeck = true;
        }
    }
}