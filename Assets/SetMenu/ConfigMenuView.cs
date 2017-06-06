/*
Max Wawer
March 30, 2017
ConfigMenuView.cs
This is the view for displaying the button config menu. Requires ConfigMenu.cs for logic. 
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Controls
{
	public class ConfigMenuView : MonoBehaviour {

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		//prefabs
		public GameObject menuBGPrefab;
		public GameObject menuEntryPrefab; 
		public GameObject arrowPrefab;
		public GameObject underlinePrefab;

		GameObject[] menuTextObjectArray;
		GameObject menu;
		//visual elements instantiated as necessary
		GameObject arrow;
		GameObject underline;

		int menuSpacing = 22;
		Vector3 topUnderlinePos;

		public void InitializeMenuView(GenericInput[] inputStorageArray, int menuTextObjectArraySize, int numTextEntries)
		{
			//Generate Menu Screen
			menu = Instantiate(menuBGPrefab);
			arrow = Instantiate(arrowPrefab);
			arrow.transform.SetParent (menu.transform);

			menuTextObjectArray = new GameObject[menuTextObjectArraySize];

			Transform arrowImgTrans = arrow.transform.GetChild(0);
			topUnderlinePos = new Vector3(underlineXPosLeft, arrowImgTrans.position.y - underlineYPosAdjust, arrowImgTrans.position.z);

			int entryPosY = 263;//worry about fixing this later. Should grab the canvas position, have an  incrementing distance (25,50)

			for(int i = 0; i <= numTextEntries; i++)
			{
				GameObject entry = (GameObject)Instantiate(menuEntryPrefab);
				//GetChild grabs canvas, then grab each text object by name
				Transform action = entry.transform.GetChild(0).transform.Find ("Action");
				action.position = new Vector3(action.position.x, entryPosY, action.position.z);
				Transform input = entry.transform.GetChild(0).transform.Find ("Input");
				input.position = new Vector3(input.position.x, entryPosY, input.position.z);
				Transform input2 = entry.transform.GetChild(0).transform.Find ("Input 2");
				input2.position = new Vector3(input2.position.x, entryPosY, input2.position.z);
				entryPosY -= menuSpacing;
				entry.transform.parent = menu.transform;

				if (i < (int)Header.e_ButtonAction.MAXNUMINPUT)
				{
					menuTextObjectArray[i] = entry;

					//set the text from the GenericInput input array
					input.GetComponent<Text> ().text = inputStorageArray [i].nameOfInputAxis;
					input2.GetComponent<Text> ().text = inputStorageArray [i].nameOfInputAxis2;
					action.GetComponent<Text> ().text = inputStorageArray [i].GetString ();
				}
				else//add final extra lines for set default and save/exit
				{
					input.GetComponent<Text> ().text = "";
					input2.GetComponent<Text> ().text = "";

					if (i == (int)Header.e_ButtonAction.MAXNUMINPUT)
					{
						action.GetComponent<Text> ().text = "Set Default";
					}
					else//i == (int)Header.e_ButtonAction.MAXNUMINPUT + 1
					{
						action.GetComponent<Text> ().text = "Save and Exit";
					}
				}
			}
		}

		public void SelectMenuUpView()
		{
			Transform arrowImgTrans = arrow.transform.GetChild(0);
			arrowImgTrans.position = new Vector3(arrowImgTrans.position.x, arrowImgTrans.position.y + menuSpacing, arrowImgTrans.position.z);	
		}

		public void SelectMenuDownView()
		{
			Transform arrowImgTrans = arrow.transform.GetChild(0);
			arrowImgTrans.position = new Vector3(arrowImgTrans.position.x, arrowImgTrans.position.y - menuSpacing, arrowImgTrans.position.z);
		}

		int underlineXPosLeft = -35+408;
		int underlineXPosRight = 110+408;
		int underlineYPosAdjust = 12;

		public void SelectWithCursorAcceptView()
		{
			underline = Instantiate(underlinePrefab);
			underline.transform.SetParent (menu.transform);
			Transform underlineImgTrans = underline.transform.GetChild(0);
			Transform arrowImgTrans = arrow.transform.GetChild(0);
			underlineImgTrans.position = new Vector3(underlineXPosLeft, arrowImgTrans.position.y - underlineYPosAdjust, arrowImgTrans.position.z);
		}
		public void SelectWithUnderlineDeclineView()
		{
			Destroy(underline);
		}
		public void SelectWithUnderlineAcceptView(int columnSelected, int rowSelected)
		{
			//set input key text to ?
			SetEntryText("?", columnSelected, rowSelected);
		}
		public void SelectUnderlineLeftView()
		{
			Transform underlineImgTrans = underline.transform.GetChild(0);
			underlineImgTrans.position = new Vector3(underlineXPosLeft, underlineImgTrans.position.y, underlineImgTrans.position.z);
		}
		public void SelectUnderlineRightView()
		{
			Transform underlineImgTrans = underline.transform.GetChild(0);
			underlineImgTrans.position = new Vector3(underlineXPosRight, underlineImgTrans.position.y, underlineImgTrans.position.z);
		}

		public void ReceiveAnyInputView(bool keyAlreadyInUse, int column, int row, int prevColumn, int prevRow, int key)
		{
			SetEntryText (Header.keyButtonNames [(int)key], prevColumn, prevRow);

			if (keyAlreadyInUse)
			{
				//move underline
				Transform underlineImgTrans = underline.transform.GetChild (0);
				if (column == 0)
				{
					underlineImgTrans.position = new Vector3 (underlineXPosLeft, topUnderlinePos.y - menuSpacing * row, topUnderlinePos.z);
				}
				else
				{
					underlineImgTrans.position = new Vector3 (underlineXPosRight, topUnderlinePos.y - menuSpacing * row, topUnderlinePos.z);
				}

				SetEntryText ("?", column, row);
			}
			else
			{
				Destroy(underline);
			}
		}

		public void ReceiveAnyInputKeyInUseView(int column, int row)
		{
			//move underline
			Transform underlineImgTrans = underline.transform.GetChild (0);
			if (column == 0)
			{
				underlineImgTrans.position = new Vector3 (underlineXPosLeft, topUnderlinePos.y - menuSpacing * row, topUnderlinePos.z);
			}
			else
			{
				underlineImgTrans.position = new Vector3 (underlineXPosRight, topUnderlinePos.y - menuSpacing * row, topUnderlinePos.z);
			}

			SetEntryText ("?", column, row);
		}

		public void ReceiveAnyInputKeyNotInUseView()
		{
			Destroy(underline);
		}

		void SetEntryText(string text, int columnSelected, int rowSelected)
		{
			GameObject entry = menuTextObjectArray [(int)rowSelected];

			Transform slot;
			if (columnSelected == 0) 
			{
				slot = entry.transform.GetChild (0).transform.Find ("Input");
			} 
			else //columnSelected = 1
			{ 
				slot = entry.transform.GetChild (0).transform.Find ("Input 2");
			}

			slot.GetComponent<Text> ().text = text;
		}


		public void SetAllTextEntries(GenericInput[] inputStorageArray, int numInputs)
		{
			for(int i = 0; i < numInputs; i++)
			{
				Transform action = menuTextObjectArray[i].transform.GetChild(0).transform.Find ("Action");
				Transform input = menuTextObjectArray[i].transform.GetChild(0).transform.Find ("Input");
				Transform input2 = menuTextObjectArray[i].transform.GetChild(0).transform.Find ("Input 2");

				//set the text from the GenericInput input array
				input.GetComponent<Text> ().text = inputStorageArray [i].nameOfInputAxis;
				input2.GetComponent<Text> ().text = inputStorageArray [i].nameOfInputAxis2;
			}
		}

	}
}