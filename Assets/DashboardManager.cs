﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;


public class DashboardManager : MonoBehaviour {
	public Text GameHistoryText;

	// Use this for initialization
	void Start () {

		FirebaseApp.DefaultInstance.SetEditorDatabaseUrl ("https://strokesurvivors-605a1.firebaseio.com/");
		DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;

		GameHistoryText.text = "      Date\t\t\t\t\t\t\t\t\tReps\n";

		FirebaseDatabase.DefaultInstance
			.GetReference("games")
			.GetValueAsync().ContinueWith(task => {
				if (task.IsFaulted) {
					// Handle the error...
				}
				else if (task.IsCompleted) {
					DataSnapshot snapshot = task.Result;
					// Do something with snapshot...
					print(snapshot.Children);
					print(snapshot.Child("-KkgCwU06xh0wsMT7q0Q").GetRawJsonValue());
					foreach(DataSnapshot item in snapshot.Children)
					{
						// do something with entry.Value or entry.Key
						print(item.Child("date").Value);
						string date = item.Child("date").Value.ToString();
						string repCount = item.Child("repCount").Value.ToString();

						GameHistoryText.text += date + "\t\t\t" + repCount + "\n";

					}
				}
			});
	}
	
	// Update is called once per frame
	void Update () {



	}
}