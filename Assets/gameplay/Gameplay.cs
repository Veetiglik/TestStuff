/*
Max Wawer
March 30, 2017
Gameplay.cs
This module runs the main input acceptance loop for the game. It accepts input from the keyboard or controller, and passes it through to other scripts that require it.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;

namespace Controls
{
    public class Gameplay : MonoBehaviour
    {
		public enum e_GameState
		{
			BUTTONCONFIGMENU,
			BUTTONCONFIGMENUANYINPUT
		};
			
        public Header.Func<int> GetFunc(int button)
        {
            switch ((Header.e_ButtonAction)button)
            {
                case Header.e_ButtonAction.UP: return UpPressed;
                case Header.e_ButtonAction.RIGHT: return RightPressed;
                case Header.e_ButtonAction.DOWN: return DownPressed;
                case Header.e_ButtonAction.LEFT: return LeftPressed;
                case Header.e_ButtonAction.ACCEPT: return AcceptPressed;
                case Header.e_ButtonAction.DECLINE: return DeclinePressed;
                case Header.e_ButtonAction.MENUROTATER: return MenuRotateRPressed;
                case Header.e_ButtonAction.MENUROTATEL: return MenuRotateLPressed;
                case Header.e_ButtonAction.MENU: return MenuPressed;
                default: return Error;
            }
        }

        public void UpPressed(int playerID)
        {
			switch (gameState) 
			{
			case e_GameState.BUTTONCONFIGMENU:
				{
					menuReferenceComponent.MenuInputReceieve (playerID, Header.e_ButtonAction.UP);
					break;
				}
			}
            Debug.Log("Up pressed");
        }
		public void RightPressed(int playerID)
        {
			switch (gameState) 
			{
			case e_GameState.BUTTONCONFIGMENU:
				{
					menuReferenceComponent.MenuInputReceieve (playerID, Header.e_ButtonAction.RIGHT);
					break;
				}
			}
            Debug.Log("Right pressed");
        }
		public void DownPressed(int playerID)
        {
			switch (gameState) 
			{
			case e_GameState.BUTTONCONFIGMENU:
				{
					menuReferenceComponent.MenuInputReceieve (playerID, Header.e_ButtonAction.DOWN);
					break;
				}
			}
            Debug.Log("Down pressed");
        }
		public void LeftPressed(int playerID)
        {
			switch (gameState) 
			{
			case e_GameState.BUTTONCONFIGMENU:
				{
					menuReferenceComponent.MenuInputReceieve (playerID, Header.e_ButtonAction.LEFT);
					break;
				}
			}
            Debug.Log("Left pressed");
        }
		public void AcceptPressed(int playerID)
        {
			switch (gameState) 
			{
			case e_GameState.BUTTONCONFIGMENU:
				{
					menuReferenceComponent.MenuInputReceieve (playerID, Header.e_ButtonAction.ACCEPT);
					break;
				}
			}
            Debug.Log("Accept pressed");
        }
		public void DeclinePressed(int playerID)
        {
			switch (gameState) 
			{
			case e_GameState.BUTTONCONFIGMENU:
				{
					menuReferenceComponent.MenuInputReceieve (playerID, Header.e_ButtonAction.DECLINE);
					break;
				}
			}
            Debug.Log("Decline pressed");
        }
		public void MenuRotateRPressed(int playerID)
        {
            Debug.Log("Menu Rotate R pressed");
			if(System.IO.File.Exists(Application.dataPath + "/config/InputConfig2.txt"))
				Debug.Log("File exists");
        }
		public void MenuRotateLPressed(int playerID)
        {
            Debug.Log("Menu Rotate L pressed");
        }
		public void MenuPressed(int playerID)
        {
            Debug.Log("Menu pressed");
        }
		public void Error(int playerID)
        {
            Debug.Log("Invalid button pressed");
        }

		//temp: will remove once Gameplay is responsible for instantiating menu
		//then it doesn't need to be public anymore
		public GameObject menuReference;
		ConfigMenu menuReferenceComponent;

		GenericInput[] inputStorageArray;

		//Gamestate is the control of the gameplay flow and decides what actions do at different portions of gameplay
		e_GameState gameState;

		//this playerId will be variable once online multiplayer is created
		int pID = 1;

		//inspector variables
		public string[] defaultInputsArray1;
		public string[] defaultInputsArray2;
		public string InputConfigLocation;

		//Gameplay should create the menu object, and upon creation, give it a reference to itself? Or maybe just give it a reference to this one array?
		//for now assign reference to gameplay object so menu can grab the component which allows it to call this
		public GenericInput[] GetInputStorageArray()
		{
			return inputStorageArray;
		}

		public void FillDefaultMenuValues()
		{
			inputStorageArray[(int)Header.e_ButtonAction.UP] = new GenericInput (Header.e_ButtonAction.UP, GetFunc ((int)Header.e_ButtonAction.UP), defaultInputsArray1[0], defaultInputsArray2[0]);
			inputStorageArray[(int)Header.e_ButtonAction.RIGHT] = new GenericInput (Header.e_ButtonAction.RIGHT, GetFunc((int)Header.e_ButtonAction.RIGHT), defaultInputsArray1[1],defaultInputsArray2[1]);
			inputStorageArray[(int)Header.e_ButtonAction.DOWN] = new GenericInput (Header.e_ButtonAction.DOWN, GetFunc((int)Header.e_ButtonAction.DOWN), defaultInputsArray1[2], defaultInputsArray2[2]);
			inputStorageArray[(int)Header.e_ButtonAction.LEFT] = new GenericInput (Header.e_ButtonAction.LEFT, GetFunc((int)Header.e_ButtonAction.LEFT), defaultInputsArray1[3], defaultInputsArray2[3]);
			inputStorageArray[(int)Header.e_ButtonAction.ACCEPT] = new GenericInput (Header.e_ButtonAction.ACCEPT, GetFunc((int)Header.e_ButtonAction.ACCEPT), defaultInputsArray1[4], defaultInputsArray2[4]);
			inputStorageArray[(int)Header.e_ButtonAction.DECLINE] = new GenericInput (Header.e_ButtonAction.DECLINE, GetFunc((int)Header.e_ButtonAction.DECLINE), defaultInputsArray1[5], defaultInputsArray2[5]);
			inputStorageArray[(int)Header.e_ButtonAction.MENUROTATEL] = new GenericInput (Header.e_ButtonAction.MENUROTATEL, GetFunc((int)Header.e_ButtonAction.MENUROTATEL), defaultInputsArray1[6], defaultInputsArray2[6]);
			inputStorageArray[(int)Header.e_ButtonAction.MENUROTATER] = new GenericInput (Header.e_ButtonAction.MENUROTATER, GetFunc((int)Header.e_ButtonAction.MENUROTATER), defaultInputsArray1[7], defaultInputsArray2[7]);
			inputStorageArray[(int)Header.e_ButtonAction.MENU] = new GenericInput (Header.e_ButtonAction.MENU, GetFunc((int)Header.e_ButtonAction.MENU), defaultInputsArray1[8], defaultInputsArray2[8]);
		}

        // Use this for initialization
        void Start()
        {
			inputStorageArray = new GenericInput[(int)Header.e_ButtonAction.MAXNUMINPUT];

			//check if file with config values exist
			if (!System.IO.File.Exists (Application.dataPath + InputConfigLocation)) 
			{
				FillDefaultMenuValues ();
			} 
			else//read values from file
			{
				string line;
				StreamReader streamR = new StreamReader (Application.dataPath + InputConfigLocation, Encoding.Default);
				int inputCounter = 0;
				using (streamR) 
				{

					line = streamR.ReadLine ();
					while (line != null)
					{
						string[] lineTokenArray = line.Split (' ');
						//0 = discard
						//1 = key1
						//2 = key2
						//need to check each entry before filling them in to make sure they're valid
						bool entry1Found = false;
						bool entry2Found = false;
						bool allEntriesValid = false;
						for (int i = 0; i < (int)Controls.e_KeyButtonName.MAXNUMKEYBUTTON; i++)
						{
							if (!entry1Found)
							{
								entry1Found = lineTokenArray [1] == Header.keyButtonNames [i];
							}
							if (!entry2Found)
							{
								entry2Found = lineTokenArray [2] == Header.keyButtonNames [i];
							}

							if (entry1Found && entry2Found)
							{
								allEntriesValid = true;
								break;
							}
						}

						if (allEntriesValid)
						{
							bool inputStringAlreadyUsed = false;
							for (int i = 0; i < inputCounter; i++)
							{
								//check to make sure no repeated strings
								if (inputStorageArray [i].nameOfInputAxis == lineTokenArray [1] || inputStorageArray [i].nameOfInputAxis == lineTokenArray [2] ||
								   inputStorageArray [i].nameOfInputAxis2 == lineTokenArray [1] || inputStorageArray [i].nameOfInputAxis2 == lineTokenArray [2])
								{
									//error in reading, use default values
									Debug.Log ("Unable to read all saved input values");
									FillDefaultMenuValues ();
									inputStringAlreadyUsed = true;
									break;
								}
							}
							if (inputStringAlreadyUsed)
							{
								break;
							}
							//set read values to inputStorageArray
							inputStorageArray [inputCounter] = new GenericInput ((Header.e_ButtonAction)inputCounter, GetFunc (inputCounter), lineTokenArray [1], lineTokenArray [2]);
							inputCounter++;
						}
						else
						{
							//error in reading, use default values
							Debug.Log ("Unable to read all saved input values");
							FillDefaultMenuValues ();
							break;						
						}
						line = streamR.ReadLine ();
					}
				}
				if(inputCounter != (int)Header.e_ButtonAction.MAXNUMINPUT)
				{
					//error in reading, use default values
					Debug.Log ("File has incorrect number of lines");
					FillDefaultMenuValues ();
				}
			}

			//get all reference components
			menuReferenceComponent = menuReference.GetComponent<ConfigMenu> ();

			//temp until an entry screen is made
			gameState = e_GameState.BUTTONCONFIGMENU;

			//set up menu now that inputs are confirmed
			//menuReferenceComponent.InitializeMenu ();
        }

        // Update is called once per frame
        void Update()
        {
			//the only exception to the below general input loop is when configuring a new input in the menu
			if (gameState == e_GameState.BUTTONCONFIGMENUANYINPUT) 
			{				
				for (int i = 0; i < (int)e_KeyButtonName.MAXNUMKEYBUTTON; i++) 
				{
					if(Input.GetButtonDown(Header.keyButtonNames[i]))
					{
						menuReferenceComponent.ReceiveAnyInput ((e_KeyButtonName)i);
					}
				}
				return;
			}
			for (int i = 0; i < (int)Header.e_ButtonAction.MAXNUMINPUT; i++) 
			{
				if (Input.GetButtonDown(inputStorageArray[i].nameOfInputAxis) || Input.GetButtonDown(inputStorageArray[i].nameOfInputAxis2))
				{
					GetFunc (i) (pID);
				}
			}
        }
			
		//This toggles whether the gameplay system will only accept configured buttons (standard), or any button (when configuring)
		public void ToggleButtonConfigState()
		{
			if (gameState == e_GameState.BUTTONCONFIGMENU) 
			{
				gameState = e_GameState.BUTTONCONFIGMENUANYINPUT;
			}
			else
			{
				gameState = e_GameState.BUTTONCONFIGMENU;
			}
		}

		//Updates the input array. To be used when setting button config
		public void SetSingleInputArrayValue(int actionindex, int inputIndex, string inputName)//inputIndex being being which column
		{
			inputStorageArray [actionindex].UpdateInputAxisString (inputIndex, inputName);
		}

		//Note that if the key in use is what already exists for that input (replacing A with A for accept), this will return false
		public bool isKeyInUse(string inputName, ref int rowIndex, ref int columnIndex, int currentRow)
		{
			for (int i = 0; i < (int)Header.e_ButtonAction.MAXNUMINPUT; i++)
			{
				if (i == currentRow)
					continue;

				if(inputName == inputStorageArray[i].nameOfInputAxis)
				{
					rowIndex = i;
					columnIndex = 0;
					return true;
				}
				if (inputName == inputStorageArray [i].nameOfInputAxis2)
				{
					rowIndex = i;
					columnIndex = 1;
					return true;
				}
			}
			//if key not found return false
			return false;
		}
			
		public void SaveAndWriteInputConfig()
		{
			//do I need to check for a file already existing?
			//put together string and just write it all at once
			string output = "";
			for (int i = 0; i < (int)Header.e_ButtonAction.MAXNUMINPUT; i++)
			{
				output += (inputStorageArray [i].GetString () + " ");
				output += (inputStorageArray [i].nameOfInputAxis + " ");
				output += (inputStorageArray [i].nameOfInputAxis2 + '\n');
			}
			System.IO.File.WriteAllText(Application.dataPath + InputConfigLocation, output);
		}
    }
}
