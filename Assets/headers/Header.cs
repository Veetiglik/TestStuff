using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Controls
{
    public class Header
    {
        public delegate void Func<T>(int playerID);

        public enum e_ButtonAction
        {
            UP,
            RIGHT,
            DOWN,
            LEFT,
            ACCEPT,
            DECLINE,
            MENUROTATER,
            MENUROTATEL,
            MENU,
            MAXNUMINPUT
        };

		//if you update this, also update e_KeyButtonName in InputManager.cs
		public static string[] keyButtonNames =
		{
			"Up",
			"Down",
			"Right",
			"Left",
			"Q",
			"W",
			"E",
			"R",
			"T",
			"Y",
			"U",
			"I",
			"O",
			"P",
			"A",
			"S",
			"D",
			"F",
			"G",
			"H",
			"J",
			"K",
			"L",
			"Z",
			"X",
			"C",
			"V",
			"B",
			"N",
			"M",
			"JS0",
			"JS1",
			"JS2",
			"JS3",
			"JS4",
			"JS5",
            "JS6",
            "JS7",
            "JS8",
			"JS9"
		};
    }
}
