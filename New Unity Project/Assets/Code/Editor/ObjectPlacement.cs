using UnityEditor;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;

public class ObjectPlacement : EditorWindow
{    
	private float angle = 90f;
	static bool shortcutToggle = false;
		
	[MenuItem("Tools/Object Placement")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ObjectPlacement));
		SceneView.onSceneGUIDelegate += view =>
		{
			var e = Event.current;
			if (shortcutToggle && e != null && e.keyCode != KeyCode.None)
			{
				ObjectPlacement objectPlacement = ScriptableObject.CreateInstance<ObjectPlacement>();
				if(e.keyCode == KeyCode.Keypad0)
				{
					objectPlacement.RoundAngle ();
				}
				else if(e.keyCode == KeyCode.Keypad1)
				{
					objectPlacement.SetSnap(0.125f);
				}
				else if(e.keyCode == KeyCode.Keypad2)
				{
					objectPlacement.SetSnap(0.25f);
				}
				else if(e.keyCode == KeyCode.Keypad3)
				{
					objectPlacement.SetSnap(0.5f);
				}
				else if(e.keyCode == KeyCode.Keypad4)
				{
					objectPlacement.SetSnap(1.0f);
				}
			}
		};
    }

	private static Vector3 MoveSnap
	{
		get
		{
			return new Vector3(	
				EditorPrefs.GetFloat("MoveSnapX", 1f), 
				EditorPrefs.GetFloat("MoveSnapY", 1f),
				EditorPrefs.GetFloat("MoveSnapZ", 1f));

		}
		set
		{
			EditorPrefs.SetFloat("MoveSnapX", value.x);
			EditorPrefs.SetFloat("MoveSnapY", value.y);
			EditorPrefs.SetFloat("MoveSnapZ", value.z);
		}
	}

	private static Type _snapType;
	private const BindingFlags BINDING = BindingFlags.Public | BindingFlags.Static;

	private static void InitializeValues() 
	{ 
		_snapType = AppDomain.CurrentDomain.GetAssemblies() 
			.SelectMany(assembly => assembly.GetTypes()) 
			.FirstOrDefault(t => t.FullName == "UnityEditor.SnapSettings"); 
	}
    
    void OnGUI()
    {
		if(GUILayout.Button("Snap 0.125 meters (numpad 1)"))
		{
			SetSnap(0.125f);	
		}
		if(GUILayout.Button("Snap 0.25 meters (numpad 2)"))
		{
			SetSnap(0.25f);	
		}
		if(GUILayout.Button("Snap 0.5 meters (numpad 3)"))
		{
			SetSnap(0.5f);	
		}
		if(GUILayout.Button("Snap 1.0 meters (numpad 4)"))
		{
			SetSnap(1.0f);	
		}
		if(GUILayout.Button("Round Angles (numpad 0)"))
		{
			RoundAngle ();	
		}
		
		angle = EditorGUILayout.FloatField(angle);
		if(GUILayout.Button("Turn X"))
		{
			Turn("x");	
		}
		if(GUILayout.Button("Turn Reverse X"))
		{
			Turn("-x");	
		}
		if(GUILayout.Button("Turn Y"))
		{
			Turn("y");	
		}
		if(GUILayout.Button("Turn Reverse Y"))
		{
			Turn ("-y");
		}
		if(GUILayout.Button("Turn Z"))
		{
			Turn("z");	
		}
		if(GUILayout.Button("Turn Reverse Z"))
		{
			Turn("-z");	
		}
		
		if(GUILayout.Button("Reset Transform"))
		{
			ResetTransform();	
		}

		if (GUI.changed) 
		{ 
			updateSnapSettings(); 
		} 

		shortcutToggle = GUILayout.Toggle (shortcutToggle, "Activate Shortcut");
    }



	private void SetSnap(float value)
	{
		MoveSnap = new Vector3 (value, value, value);
		updateSnapSettings ();
		SnapToGrid ();
		Debug.Log ("Snapped " + value + " !");
	}

	void updateSnapSettings()
	{
		if (_snapType == null) 
		{
			InitializeValues ();
		}
		var move = _snapType.GetProperty("move", BINDING); 
		move.SetValue(_snapType, MoveSnap, null);
	}

	private void SnapToGrid()
	{
		foreach (Transform t in Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable)) 
		{
			t.position = new Vector3 (
				Mathf.Round(t.position.x / EditorPrefs.GetFloat("MoveSnapX")) * EditorPrefs.GetFloat("MoveSnapX"),
				Mathf.Round(t.position.y / EditorPrefs.GetFloat("MoveSnapY")) * EditorPrefs.GetFloat("MoveSnapY"),
				Mathf.Round(t.position.z / EditorPrefs.GetFloat("MoveSnapZ")) * EditorPrefs.GetFloat("MoveSnapZ")
			);
		}
	}

	private void RoundAngle()
	{
		foreach (Transform t in Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable)) 
		{
			Vector3 rot = t.rotation.eulerAngles;
			rot.x = Mathf.Round (rot.x);
			rot.y = Mathf.Round (rot.y);
			rot.z = Mathf.Round (rot.z);

			t.rotation = Quaternion.Euler (rot);
		}
		Debug.Log ("Round Angle!");
	}
	
	private void Turn(string axis)
	{
		if(axis == "x")
		{
			foreach (Transform t in Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable)) 
			{
				t.Rotate(new Vector3(angle, 0, 0));
			}
		}
		if(axis == "-x")
		{
			foreach (Transform t in Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable)) 
			{
				t.Rotate(new Vector3(-angle, 0, 0));
			}
		}
		if(axis == "y")
		{
			foreach (Transform t in Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable)) 
			{
				t.Rotate(new Vector3(0, angle, 0));
			}
		}
		if(axis == "-y")
		{
			foreach (Transform t in Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable)) 
			{
				t.Rotate(new Vector3(0, -angle, 0));
			}
		}
		if(axis == "z")
		{
			foreach (Transform t in Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable)) 
			{
				t.Rotate(new Vector3(0, 0, angle));
			}
		}
		if(axis == "-z")
		{
			foreach (Transform t in Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable)) 
			{
				t.Rotate(new Vector3(0, 0, -angle));
			}
		}
	}
	
	private void ResetTransform()
	{
		foreach (Transform t in Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable)) 
		{
			t.rotation = Quaternion.Euler(Vector3.zero);
			t.localScale = new Vector3(1f,1f,1f);
			if(t.parent)
			{
				t.localPosition = Vector3.zero;
				t.localRotation = Quaternion.identity;
			}
		}	
	}
}
