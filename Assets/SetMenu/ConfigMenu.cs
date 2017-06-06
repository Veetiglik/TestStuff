/*
Max Wawer
March 30, 2017
ConfigMenu.cs
This is the model for controlling the button config menu. Requires ConfigMenuView.cs to display. 
This module can redefine the list of inputs used by Gameplay.cs
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Controls
{
    public class ConfigMenu : MonoBehaviour
    {
		//enums
		public enum e_ConfigMenuOption
		{
			MINOPTION = 0,
			UP = MINOPTION,
			RIGHT,
			DOWN,
			LEFT,
			ACCEPT,
			DECLINE,
			MENUROTATER,
			MENUROTATEL,
			MENU,
			SETDEFAULT,
			SAVEANDEXIT,
			MAXOPTION = SAVEANDEXIT
		};

		public enum e_MenuInputState
		{
			SELECTINGWITHCURSOR,
			SELECTINGWITHUNDERLINE,
			SELECTINGNEWINPUT
		};

		public class ConfigMenuState
		{
			public e_ConfigMenuOption rowSelected; 
			public int columnSelected;
			public e_MenuInputState menuState;

			public ConfigMenuState()
			{
				//initial state
				rowSelected = e_ConfigMenuOption.MINOPTION;
				columnSelected = 0;//left
				menuState = e_MenuInputState.SELECTINGWITHCURSOR;
			}
		}

		//references
		public GameObject gameplayReference;
		Gameplay gameplayReferenceComponent;
		ConfigMenuView viewReferenceComponent;

		bool menuIsInit = false;
		ConfigMenuState menuStateObject;
		GenericInput[] inputStorageArray;

		public bool MenuIsInit()
		{
			return menuIsInit;
		}

		//right now just this function, but later it'll be a scene switch
        public void InitializeMenu()
        {
			//get all necessary object components
			gameplayReferenceComponent = gameplayReference.GetComponent<Gameplay> ();
			inputStorageArray = gameplayReferenceComponent.GetInputStorageArray(); 
			viewReferenceComponent = gameObject.GetComponent<ConfigMenuView> ();

			menuStateObject = new ConfigMenuState ();//e_ConfigMenuOption.MINOPTION & e_MenuInputState.SELECTINGWITHCURSOR;

			//setup view
			viewReferenceComponent.InitializeMenuView (inputStorageArray, (int)Header.e_ButtonAction.MAXNUMINPUT, (int)e_ConfigMenuOption.MAXOPTION);

			menuIsInit = true;
        }
			
		public void MenuInputReceieve(int playerId, Header.e_ButtonAction action)
		{
			switch(menuStateObject.menuState)
			{
			case e_MenuInputState.SELECTINGWITHCURSOR:
				{
					//different switch statement based on current menu state (selecting up and down versus now choosing an input)
					switch (action) {
					case Header.e_ButtonAction.UP:
						SelectMenuUp ();
						break;
					case Header.e_ButtonAction.RIGHT:
						return;
					case Header.e_ButtonAction.DOWN:
						SelectMenuDown ();
						break;
					case Header.e_ButtonAction.LEFT:
						return;
					case Header.e_ButtonAction.ACCEPT:
						SelectWithCursorAccept ();
						return;
					case Header.e_ButtonAction.DECLINE:
						return;
					default:
						return;
					}
					break;
				}
			case e_MenuInputState.SELECTINGWITHUNDERLINE:
				{
					//different switch statement based on current menu state (selecting up and down versus now choosing an input)
					switch (action) {
					case Header.e_ButtonAction.UP:
						break;
					case Header.e_ButtonAction.RIGHT:
						SelectUnderlineRight ();
						return;
					case Header.e_ButtonAction.DOWN:
						break;
					case Header.e_ButtonAction.LEFT:
						SelectUnderlineLeft ();
						return;
					case Header.e_ButtonAction.ACCEPT:
						SelectWithUnderlineAccept ();
						return;
					case Header.e_ButtonAction.DECLINE:
						SelectWithUnderlineDecline ();
						return;
					default:
						return;
					}
					break;
				}
			case e_MenuInputState.SELECTINGNEWINPUT:
				//TODO:
				break;
			}
		}

		public void SelectMenuUp()
		{
			if (menuStateObject.rowSelected == e_ConfigMenuOption.MINOPTION)
				return;

			menuStateObject.rowSelected--;
				
			viewReferenceComponent.SelectMenuUpView();
		}
		public void SelectMenuDown()
		{
			if (menuStateObject.rowSelected == e_ConfigMenuOption.MAXOPTION)
				return;

			menuStateObject.rowSelected++;

			viewReferenceComponent.SelectMenuDownView();
		}
			
		public void SelectWithCursorAccept()
		{
			switch(menuStateObject.rowSelected)
			{
				case e_ConfigMenuOption.SETDEFAULT: 
				{
					//this will set the values to programmed default values which cannot be changed by the user
					gameplayReferenceComponent.FillDefaultMenuValues();
					viewReferenceComponent.SetAllTextEntries (inputStorageArray, (int)Header.e_ButtonAction.MAXNUMINPUT);
					break;
				}
				case e_ConfigMenuOption.SAVEANDEXIT:
				{
					//this will write the values to the input config file
					gameplayReferenceComponent.SaveAndWriteInputConfig();
					//Later this will exit to the menu from which you can access button config
				    break;
				}
				default:
				{
					menuStateObject.columnSelected = 0;

					rowArrowHasSelected = menuStateObject.rowSelected;

					viewReferenceComponent.SelectWithCursorAcceptView ();

					menuStateObject.menuState = e_MenuInputState.SELECTINGWITHUNDERLINE;
					break;
				}
			}
		}
		public void SelectWithUnderlineDecline()
		{
			viewReferenceComponent.SelectWithUnderlineDeclineView ();
			menuStateObject.menuState = e_MenuInputState.SELECTINGWITHCURSOR;
		}
		public void SelectWithUnderlineAccept()
		{
			viewReferenceComponent.SelectWithUnderlineAcceptView(menuStateObject.columnSelected, (int)menuStateObject.rowSelected);

			//this allows the gameplay module to accept any keystrokes, not just the configured ones.
			gameplayReferenceComponent.ToggleButtonConfigState ();

			menuStateObject.menuState = e_MenuInputState.SELECTINGNEWINPUT;
		}
		public void SelectUnderlineLeft()
		{
			viewReferenceComponent.SelectUnderlineLeftView ();

			menuStateObject.columnSelected = 0;
		}
		public void SelectUnderlineRight()
		{
			viewReferenceComponent.SelectUnderlineRightView ();

			menuStateObject.columnSelected = 1;
		}

		//This value is saved when jumping is required due to configing an already in use key
		e_ConfigMenuOption rowArrowHasSelected;

		//called by Gameplay when a key/button is pressed to be used as a new input
		public void ReceiveAnyInput(e_KeyButtonName key)
		{
			//need to make sure that currently selected key is not already in use.
			//If it is, set that key to ? and move the underline there to force user to update it
			//Maybe this isn't the best system since it jumps people around, but oh well. Gotta do something when you can't use mouse
			//maybe this could use mouse later but :)
			int rowIndexOfKeyInUse = 0;
			int columnIndexOfKeyInUse = 0;
			bool keyAlreadyInUse = gameplayReferenceComponent.isKeyInUse(Header.keyButtonNames [(int)key], ref rowIndexOfKeyInUse, ref columnIndexOfKeyInUse, (int)menuStateObject.rowSelected);

			//using row selected and column selected, overwrite the text with the new text and update the inputStorageArray
			gameplayReferenceComponent.SetSingleInputArrayValue ((int)menuStateObject.rowSelected, menuStateObject.columnSelected, Header.keyButtonNames [(int)key]);

			//this view call has to be done here to keep the correct values for the rows and columns without adding in extra state holders
			viewReferenceComponent.ReceiveAnyInputView (keyAlreadyInUse, columnIndexOfKeyInUse, rowIndexOfKeyInUse, menuStateObject.columnSelected, (int)menuStateObject.rowSelected, (int)key);

			if (keyAlreadyInUse)
			{
				menuStateObject.columnSelected = columnIndexOfKeyInUse;
				menuStateObject.rowSelected = (e_ConfigMenuOption)rowIndexOfKeyInUse;
			}
			else
			{
				menuStateObject.rowSelected = rowArrowHasSelected;

				gameplayReferenceComponent.ToggleButtonConfigState ();

				//set menu state back to initial selection
				menuStateObject.menuState = e_MenuInputState.SELECTINGWITHCURSOR;
			}
		}
    }
}
