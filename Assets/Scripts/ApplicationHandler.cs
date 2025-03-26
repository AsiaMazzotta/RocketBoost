using UnityEngine;
using UnityEngine.InputSystem;
public class ApplicationHandler : MonoBehaviour
{
    void Update()
    {
       if(Keyboard.current.escapeKey.isPressed)
       {
            Application.Quit();
       }
    }
}
