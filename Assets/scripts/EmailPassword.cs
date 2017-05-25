﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Firebase;
using Firebase.Database;
using Firebase.Unity.Editor;



public class EmailPassword : MonoBehaviour
{

	private FirebaseAuth auth;
	public InputField UserNameInput, PasswordInput;
	public Button SignupButton, LoginButton;
	public Text ErrorText;

	void Start()
	{
		auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
		print("Entered login.");
		//		UserNameInput.text = "demofirebase@gmail.com";
		//		PasswordInput.text = "abcdefgh";
		print ("entered username input and password input");
		SignupButton.onClick.AddListener(() => Signup(UserNameInput.text, PasswordInput.text));
		LoginButton.onClick.AddListener(() => Login(UserNameInput.text, PasswordInput.text));

		//
		//		SignupButton.onClick.AddListener(() => Signup(UserNameInput.text, PasswordInput.text));
		//		LoginButton.onClick.AddListener(() => Login(UserNameInput.text, PasswordInput.text));
	}

	public void Signup(string email, string password)
	{
		print ("entered signup");
		if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
		{
			print ("bad request signup");
			return;
		}

		auth.CreateUserWithEmailAndPasswordAsync(UserNameInput.text, PasswordInput.text).ContinueWith(task => {
			print("creating user");
			if (task.IsCanceled) {
				Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
				return;
			}
			if (task.IsFaulted) {
				Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
				return;
			}

			// Firebase user has been created.
			Firebase.Auth.FirebaseUser newUser = task.Result;
			Debug.LogFormat("Firebase user created successfully: {0} ({1})",
				newUser.DisplayName, newUser.UserId);
		});
	}

	public void Login(string email, string password)
	{
		print ("login called1");
		auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
			{
				print("in auth");
				if (task.IsCanceled)
				{
					Debug.LogError("SignInWithEmailAndPasswordAsync canceled.");
					return;
				}
				if (task.IsFaulted)
				{
					Debug.LogError("SignInWithEmailAndPasswordAsync error: " + task.Exception);
					if (task.Exception.InnerExceptions.Count > 0)
						UpdateErrorMessage(task.Exception.InnerExceptions[0].Message);
					return;
				}

				FirebaseUser user = task.Result;
				Debug.LogFormat("User signed in successfully: {0} ({1})",
					user.DisplayName, user.UserId);

				PlayerPrefs.SetString("LoginUser", user != null ? user.Email : "Unknown");
				SceneManager.LoadScene("LoginResults");
			});
	}

	private void UpdateErrorMessage(string message)
	{
		ErrorText.text = message;
		Invoke("ClearErrorMessage", 3);
	}








}