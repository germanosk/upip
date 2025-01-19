using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(TMP_Text))]
public class UpipTMPText : MonoBehaviour
{
    private const string _inputActionLabel = "<inputaction>";
    private TMP_Text _tmpText;

    [SerializeField][TextArea]
    private string _text;
    
    
    [SerializeField]
    private List<ActionData> _actions = new List<ActionData>();

    private PlayerInput _playerInput;
    
    private void Awake()
    {
        _tmpText = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        _tmpText.text = ReplaceInputActionText();
        //matchedControl.device
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDestroy()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    private void OnDeviceChange(InputDevice inputDevice, InputDeviceChange inputDeviceChange)
    {
        if (inputDeviceChange == InputDeviceChange.UsageChanged)
        {
            
        }
        
        _tmpText.text = ReplaceInputActionText();
        Debug.Log(inputDevice.name + $" [inputDeviceChange]{inputDeviceChange}");
    }

    private string ReplaceInputActionText()
    {

        string textCopy = _text;
        if (!textCopy.Contains(_inputActionLabel) || _actions.Count == 0)
        {
            return textCopy;
        }
        int labelSize = _inputActionLabel.Length;
        int i = 0;
        int actionIndex = 0;
        while ( i <= textCopy.Length - labelSize )
        {
            if (textCopy.Substring(i, labelSize).Equals(_inputActionLabel))
            {
                // Altertnative way 
                // deviceIndex
                //string actionDisplayString = _actions[0].Action.ToInputAction().bindings[deviceIndex].ToString();
                AddActionDisplayString(ref actionIndex, ref i, ref textCopy, labelSize);
            }
            else
            {
                i++;
            }
        }

        return textCopy;
    }

    private void AddActionDisplayString(ref int actionIndex, ref int i, ref string textCopy, int labelSize)
    {
        string actionDisplayString = $"[MISSING ACTION {actionIndex}]";
        if (actionIndex < _actions.Count && _actions[actionIndex].Action != null)
        {
            actionDisplayString = ActionToIconTag(_actions[actionIndex].Action.action.GetBindingDisplayString().Replace("/",""));
        }

        textCopy = textCopy.Remove(i, labelSize);
        textCopy = textCopy.Insert(i,actionDisplayString);
        
        i += actionDisplayString.Length;
        actionIndex++;
    }

    private void FixedUpdate()
    {
        _tmpText.text = ReplaceInputActionText();
        
        // Debug.Log(PlayerInput.all[0].currentControlScheme );
        
    }

    private void OnDrawGizmosSelected()
    {
        GetComponent<TMP_Text>().text = _text;
    }

    private string ActionToIconTag(string actionString)
    {
        Debug.Log(actionString );
        return $"<sprite=\"{PlayerInput.all[0].currentControlScheme.ToLower()}_spritesheet\" name=\"{actionString.ToLower()}\">";
    }
}

[Serializable]
public struct ActionData
{
    [SerializeField]
    public InputActionReference Action;
    
}
