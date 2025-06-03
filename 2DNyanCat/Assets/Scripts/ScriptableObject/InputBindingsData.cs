using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "InputBindingsData", menuName = "Settings/Input Bindings")]
public class InputBindingsData : ScriptableObject
{
    public InputActionReference attack1;
    public InputActionReference attack2;
    public InputActionReference move;
    public InputActionReference ground;
    public InputActionReference jump;
    public InputActionReference save;
    public InputActionReference save1;
}