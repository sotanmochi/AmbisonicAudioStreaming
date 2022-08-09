// This code is a modified version of the following two scripts.
// https://baba-s.hatenablog.com/entry/2015/02/10/102941
// https://baba-s.hatenablog.com/entry/2018/02/26/085900

using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace AmbisonicAudioStreaming.Editor
{
/// <summary>
/// シンボルを設定するウィンドウを管理するクラス
/// </summary>
public class SymbolWindow : EditorWindow
{
    //===================================================================================================
    // 定数
    //===================================================================================================

    private const string ITEM_NAME      = "Ambisonic Audio Streaming/Symbols";   // コマンド名
    private const string WINDOW_TITLE   = "Symbols";         // ウィンドウのタイトル
    private const string XML_FILENAME   = "Symbols.xml";     // 読み込む .xml のファイルパス
    
    //===================================================================================================
    // クラス
    //===================================================================================================

    /// <summary>
    /// シンボルのデータを管理するクラス
    /// </summary>
    private class SymbolData
    {
        public string   Name        { get; private set; }   // 定義名を返します
        public string   Comment     { get; private set; }   // コメントを返します
        public bool     IsEnable    { get; set;         }   // 有効かどうかを取得または設定します

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SymbolData( XmlNode node )
        {
            Name    = node.Attributes[ "name"    ].Value;
            Comment = node.Attributes[ "comment" ].Value;
        }
    }
    
    //===================================================================================================
    // 変数
    //===================================================================================================

    private static Vector2      mScrollPos;     // スクロール座標
    private static SymbolData[] mSymbolList;    // シンボルのリスト
    
    //===================================================================================================
    // 静的関数
    //===================================================================================================

    /// <summary>
    /// ウィンドウを開きます
    /// </summary>
    [MenuItem( ITEM_NAME )]
    private static void Open()
    {
        var window = GetWindow<SymbolWindow>( true, WINDOW_TITLE );
        window.Init();
    }
    
    //===================================================================================================
    // 関数
    //===================================================================================================

    /// <summary>
    /// 初期化する時に呼び出します
    /// </summary>
    private void Init()
    {
        var path = System.IO.Path.GetDirectoryName(AssetDatabase.GetAssetPath(MonoScript.FromScriptableObject(this)));

        var document = new XmlDocument();
        document.Load( path + "/" + XML_FILENAME );

        var root        = document.GetElementsByTagName( "root" )[ 0 ];
        var symbolList  = new List<XmlNode>();

        foreach ( XmlNode n in root.ChildNodes )
        {
            if ( n.Name == "symbol" )
            {
                symbolList.Add( n );
            }
        }

        mSymbolList = symbolList
            .Select( c => new SymbolData( c ) )
            .ToArray();

        var defineSymbols = PlayerSettings
            .GetScriptingDefineSymbolsForGroup( EditorUserBuildSettings.selectedBuildTargetGroup )
            .Split( ';' );

        foreach ( var n in mSymbolList )
        {
            n.IsEnable = defineSymbols.Any( c => c == n.Name );
        }
    }
    
    /// <summary>
    /// GUI を表示する時に呼び出されます
    /// </summary>
    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        mScrollPos = EditorGUILayout.BeginScrollView( 
            mScrollPos, 
            GUILayout.Height( position.height ) 
        );
        foreach ( var n in mSymbolList )
        {
            EditorGUILayout.BeginHorizontal( GUILayout.ExpandWidth( true ) );
            n.IsEnable = EditorGUILayout.Toggle( n.IsEnable, GUILayout.Width( 16 ) );
            if ( GUILayout.Button( "Copy" ) )
            {
                EditorGUIUtility.systemCopyBuffer = n.Name;
            }
            EditorGUILayout.LabelField( n.Name, GUILayout.ExpandWidth( true ), GUILayout.MinWidth( 0 ) );
            EditorGUILayout.LabelField( n.Comment, GUILayout.ExpandWidth( true ), GUILayout.MinWidth( 0 ) );
            EditorGUILayout.EndHorizontal();
        }
        if ( GUILayout.Button( "Save" ) )
        {
            var defineSymbols = mSymbolList
                .Where( c => c.IsEnable )
                .Select( c => c.Name )
                .ToArray();

            PlayerSettings.SetScriptingDefineSymbolsForGroup(
                EditorUserBuildSettings.selectedBuildTargetGroup, 
                string.Join( ";", defineSymbols )
            );
            Close();
        }
        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();
    }
}
}