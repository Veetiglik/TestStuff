/*
Max Wawer
March 30, 2017
InputManager.cs
Contains important class GenericInput, which is necessary for storing and handling input<->action relationship
*/

using UnityEngine;

namespace Controls
{ 
	//if you update this, also update keyButtonNames in Header.cs
	public enum e_KeyButtonName
    {
		Up,
		Down,
		Right,
		Left,
        Q,
        W,
        E,
        R,
        T,
        Y,
        U,
        I,
        O,
        P,
        A,
        S,
        D,
        F,
        G,
        H,
        J,
        K,
        L,
        Z,
        X,
        C,
        V,
        B,
        N,
        M,
        JS0,
        JS1,
        JS2,
        JS3,
        JS4,
		JS5,
        JS6,
        JS7,
        JS8,
        JS9,
        MAXNUMKEYBUTTON,
        UNKNOWN = MAXNUMKEYBUTTON
    };

    public class GenericInput
    {
        //the name game action such as "accept"
        Header.e_ButtonAction action;
        //the function to call when the tied button or key is pressed 
        Header.Func<int> actionPressed;
        //the name of the action
        public string nameOfInputAxis;
		public string nameOfInputAxis2;

		public GenericInput(Header.e_ButtonAction actionSet, Header.Func<int> ActionPressedSet, string nameOfInputAxisSet, string nameOfInputAxisSet2)
		{
			action = actionSet;
			actionPressed = ActionPressedSet;
			nameOfInputAxis = nameOfInputAxisSet;
			nameOfInputAxis2 = nameOfInputAxisSet2;
		}

		public void Set(Header.e_ButtonAction actionSet, Header.Func<int> ActionPressedSet, string nameOfInputAxisSet, string nameOfInputAxisSet2)
        {
            action = actionSet;
            actionPressed = ActionPressedSet;
			nameOfInputAxis = nameOfInputAxisSet;
			nameOfInputAxis2 = nameOfInputAxisSet2;
        }

		public void UpdateInputAxisString(int index, string str)
		{
			switch(index)
			{
				case 0:
					nameOfInputAxis = str;
					break;
				case 1:
					nameOfInputAxis2 = str;
					break;
			}
		}

        public string GetString()
        {
            switch (action)
            {
                case Header.e_ButtonAction.UP: return "Up";
                case Header.e_ButtonAction.RIGHT: return "Right";
                case Header.e_ButtonAction.DOWN: return "Down";
                case Header.e_ButtonAction.LEFT: return "Left";
                case Header.e_ButtonAction.ACCEPT: return "Accept";
                case Header.e_ButtonAction.DECLINE: return "Decline";
                case Header.e_ButtonAction.MENUROTATER: return "Rotate/Menu Right";
                case Header.e_ButtonAction.MENUROTATEL: return "Rotate/Menu Left";
                case Header.e_ButtonAction.MENU: return "Menu";
                default: return "ERROR";
            }
        }
    };
}
