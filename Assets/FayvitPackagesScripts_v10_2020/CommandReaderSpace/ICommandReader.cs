using UnityEngine;
using System.Collections;

namespace FayvitCommandReader_10_2020
{
    public interface ICommandReader
    {
        int IndexOfControl { get; }
        Controlador ControlId { get; }
        bool VerifyThisControlUse();
        bool SubmitButtonDown();
        bool GetButton(int numButton);
        bool GetButtonDown(int numButton);
        bool GetButtonUp(int numButton);
        bool GetButton(string nameButton);
        bool GetButtonDown(string nameButton);
        bool GetButtonUp(string nameButton);
        float GetAxis(string esseGatilho);
        int GetIntTriggerDown(string esseGatilho);
        Vector3 DirectionalVector();
        
    }
}