using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace GraphQlClient.Editor
{
    public class SearchOptionWindow : ScriptableObject, ISearchWindowProvider
    {
        private Texture2D icon;
        private string[] _options;
        private string _title;
        private Action<int> _callBack;

        public static void Show(Vector2 mousePosition, string[] options, string title, Action<int> callBack)
        {
            var screenPoint = GUIUtility.GUIToScreenPoint(mousePosition);
            var searchWindowProvider = CreateInstance<SearchOptionWindow>();
            searchWindowProvider.Init(options, title, callBack);
            var wdContext = new SearchWindowContext(screenPoint, 240, 320);
            SearchWindow.Open(wdContext, searchWindowProvider);
        }

        public void Init(string[] options, string title, Action<int> callBack)
        {
            _options = options;
            _title = title;
            _callBack = callBack;
            icon = new Texture2D(1, 1);
            icon.SetPixel(0, 0, Color.clear);
            icon.Apply();
        }
        
        
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            var tree = new List<SearchTreeEntry>
            {
                new SearchTreeGroupEntry(new GUIContent(_title))
            };
            for (int i = 0; i < _options.Length; i++)
            {
                var option = _options[i];

                tree.Add(new SearchTreeEntry(new GUIContent(option, icon)) { level = 1, userData = i});
            }

            return tree;
        }

        public bool OnSelectEntry(SearchTreeEntry searchTreeEntry, SearchWindowContext context)
        {
            _callBack?.Invoke((int)searchTreeEntry.userData);
            return true;
        }
    }
}