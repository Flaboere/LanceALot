using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

class THInfoWindow : EditorWindow {

    #region Fields

    Vector2 scrollPosition;

    int selectedView = 0;

    int selectedBlackboardGlobalBoard;

    bool isUpdateAutomatically = false;

    bool isBlackboardWriting;

    #endregion

    #region Base

    [MenuItem("Window/THInfoWindow")]
    static void Main() {

        EditorWindow.GetWindow<THInfoWindow>();
    }

    void Update() {

        if(isUpdateAutomatically)
            Repaint();
    }

    void OnGUI() {

        // Buttons that works like toggle hax
        GUILayout.BeginHorizontal();
        isUpdateAutomatically = GUILayout.Toggle(isUpdateAutomatically, "Auto Update", GUI.skin.GetStyle("Button"));
        GUILayout.EndHorizontal();

        GUILayout.Space(5);
        selectedView = GUILayout.SelectionGrid(selectedView, new string[] {"Messages and Subscriptions", "Blackboard", "Dispatcher Info", "TimerDaemon" }, 4);
        GUILayout.Space(10);

        scrollPosition = GUILayout.BeginScrollView(scrollPosition);

        // Draw messages and subscriptions
        if (selectedView == 0) {
         
            DrawMessageAndSubscriptionInfo();
        }

        // Draw blackboard content
        if (selectedView == 1) {

            DrawBlackBoardContents();
        }

        // Draw dispatcher info
        if (selectedView == 2) { 
        }

        // Draw TimerDaemon info
        if (selectedView == 3) {

            DrawTimerDaemonInfo();
        }

        GUILayout.EndScrollView();
    }

    #endregion

    #region GUI

    #region Messages and Subscriptions

    void DrawMessageAndSubscriptionInfo() { 

        MessageDocumentation[] messages = Reflection.GetAttributes<MessageDocumentation>();
        SubscriptionDocumentation[] subscriptions = Reflection.GetAttributes<SubscriptionDocumentation>();

        Array.Sort<MessageDocumentation>(messages, MessageDocumentation.Compare);
        Array.Sort<SubscriptionDocumentation>(subscriptions, SubscriptionDocumentation.Compare);

        // int length = Mathf.Max(messages.Length, subscriptions.Length);

        // Find max width for fields
        List<float> widths = GetMessageAndSubscriptionFieldWidths(messages, subscriptions);

        // Draw messages and subscription fields
        int left = 0;
        int right = 0;

        while (true) {

            MessageDocumentation message = left < messages.Length ? messages[left] : null;
            SubscriptionDocumentation subscription = right < subscriptions.Length ? subscriptions[right] : null;

            int result = 0;

            if (message == null && subscription == null)
                break;

            if (message == null && subscription != null)
                result = 1;
            else if (message != null && subscription == null)
                result = -1;
            else 
                result = string.Compare(message.message, subscription.message);

            switch (result) {
                case 0: { DrawMessageAndSubscriptionField(message, subscription, widths); left++; right++; break; }
                case -1: { DrawMessageAndSubscriptionField(message, null, widths); left++; break; }
                case 1: { DrawMessageAndSubscriptionField(null, subscription, widths); right++; break; }
            }
        }
    }

    void DrawMessageAndSubscriptionField(MessageDocumentation message, SubscriptionDocumentation subscription, List<float> widths) {

        GUILayout.BeginHorizontal();

        if (message == null || subscription == null)
            GUI.color = new Color(1.0f, 0.8f, 0.8f);
        else
            GUI.color = Color.white;

        if (message != null)
            DrawCSVFields(message.ToString(), widths);
        else
            DrawCSVFields(",,,", widths);

        GUILayout.Space(20);

        if (subscription != null)
            DrawCSVFields(subscription.ToString(), widths);            
        else
            DrawCSVFields(",,,", widths);            

        GUILayout.EndHorizontal();
    }

    void DrawCSVFields(string fields, List<float> widths) {

        string[] s = fields.ToString().Split(',');

        for (int i = 0; i < s.Length; i++) {

            //if (s[i] == "") {
            //    GUI.color = GUI.backgroundColor;
            //}

            if (i < 2)
                GUILayout.Box(s[i], GUILayout.Width(widths[i]));
            else
                GUILayout.Box(s[i], GUILayout.Width(widths[i]));
        }
    }

    List<float> GetMessageAndSubscriptionFieldWidths(MessageDocumentation[] messages, SubscriptionDocumentation[] subscriptions) {

        List<float> widths = new List<float>();

        float charSize = 8;

        foreach (MessageDocumentation message in messages) {

            string[] s = message.ToString().Split(',');

            for (int i = 0; i < s.Length; i++) {

                if (widths.Count <= i)
                    widths.Add(30);

                if (widths[i] < (s[i].Length * charSize))
                    widths[i] = (s[i].Length * charSize);

                widths[i] = Mathf.Clamp(widths[i], 30, 200);
            }
        }

        foreach(SubscriptionDocumentation subscription in subscriptions) {

            string[] s = subscription.ToString().Split(',');

            for (int i = 0; i < s.Length; i++) {

                if (widths.Count <= i)
                    widths.Add(30);

                if (widths[i] < (s[i].Length * charSize))
                    widths[i] = (s[i].Length * charSize);

                widths[i] = Mathf.Clamp(widths[i], 30, 200);
            }
        }

        return widths;
    }

    #endregion

    #region Blackboard

    void DrawBlackBoardContents() {

        // Blackboard is empty while game is nor running
        if (!Application.isPlaying)
            return;

        GUILayout.BeginHorizontal();
        GUILayout.Button("Refresh");
        isBlackboardWriting = GUILayout.Toggle(isBlackboardWriting, "Edit Fields", GUI.skin.GetStyle("Button"));
        GUILayout.EndHorizontal();

        // User selected objects
        List<string> selection = new List<string>();
        foreach (GameObject go in Selection.gameObjects)
            selection.Add(go.name);

        // All boards
        List<string> boards = new List<string>();
        boards.AddRange(BlackBoard.GetAllBoardNames());

        // All objects in the scene
        List<string> sceneObjects = new List<string>();
        foreach (GameObject go in FindSceneObjectsOfType(typeof(GameObject)) as GameObject[])
            sceneObjects.Add(go.name);

        // Draw dropdown list of boards that isn't owned by a GameObject
        GUILayout.Label("Global Boards");

        List<string> globalBoards = new List<string>();
        globalBoards.Add("None");
        foreach (string board in boards) { 
            if(!sceneObjects.Contains(board))               
                globalBoards.Add(board);
        }

        selectedBlackboardGlobalBoard = EditorGUILayout.Popup(selectedBlackboardGlobalBoard, globalBoards.ToArray());

        // Draw selected popup item
        if (selectedBlackboardGlobalBoard > 0) {

            DrawBlackboardBoard(globalBoards[selectedBlackboardGlobalBoard]);
        }

        // Draw user selected fields
        GUILayout.Label("Selected Boards");

        if (selection.Count > 0)
            foreach (string board in selection)                
                if(boards.Contains(board))
                    DrawBlackboardBoard(board);
    }

    void DrawBlackboardBoard(string board) {

        GUILayout.TextField(board);

        string[] fields = BlackBoard.GetAllFieldsFromBoard(board);
        Array.Sort(fields);

        foreach (string field in fields) {
            GUILayout.BeginHorizontal();
            GUILayout.Space(50);
            GUILayout.TextField(field, GUILayout.Width(200));

            object value = BlackBoard.Read<object>(board, field);
            string type = value.GetType().ToString();
            
            switch (type) {
                case "System.String": value = EditorGUILayout.TextField((string)value); break;
                case "System.Boolean": value = EditorGUILayout.Toggle((bool)value); break;
                case "System.Int32": value = EditorGUILayout.IntField((int)value); break;
                case "System.Single": value = EditorGUILayout.FloatField((float)value); break;
                case "UnityEngine.Vector2": value = EditorGUILayout.Vector2Field("", (Vector2)value); break;
                case "UnityEngine.Vector3": value = EditorGUILayout.Vector3Field("", (Vector3)value); break;
                case "UnityEngine.Vector4": value = EditorGUILayout.Vector4Field("", (Vector4)value); break;
                case "UnityEngine.Quaternion": {Quaternion q = (Quaternion)value; EditorGUILayout.Vector4Field("", new Vector4(q.x, q.y, q.z, q.w)); break;}
                default: { EditorGUILayout.LabelField("", value.ToString()); break; }
            }

            if (isBlackboardWriting)
                BlackBoard.Write(board, field, value);

            GUILayout.EndHorizontal();
        }

        GUILayout.Space(10);
    }

    #endregion

    #region TimerDaemon

    void DrawTimerDaemonInfo() {

        if (!Application.isPlaying)
            return;

        List<Timer> timers = TimerDaemon._timerDaemon.timers;

        if (timers == null)
            return;

        float time = Time.realtimeSinceStartup + Time.deltaTime;

        foreach (Timer timer in timers) {

            if (timer == null || timer.callback == null)
                continue;

            GUI.color = Color.black;

            if (time >= timer.startTime + timer.delay)
                GUI.contentColor = Color.yellow;            

            GUILayout.BeginHorizontal();

            GUILayout.Label("Callback: " + timer.callback.Target.ToString() + "." + timer.callback.Method.ToString());
            GUILayout.Label("Delay: " + timer.delay);
            GUILayout.Label("Is Repeatable: " + timer.repeatable.ToString());
            GUILayout.Label("Start Time: " + timer.startTime);
            //GUILayout.Label("Autodestruct: " + timer.autodestruct.ToString());

            GUILayout.EndHorizontal();
        }
    }

    #endregion

    #endregion

    #region Utils

    void GetSortedBlackboardDomains() { }

    void GetSortedBlackboardFields(string domain) { }

    #endregion
}