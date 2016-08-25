using UnityEngine;
using UnityEditor;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
/// 
public partial class GoogleDataAssetUtility
{
    [MenuItem("Assets/Create/Google/Groups")]
    public static void CreateGroupsAssetFile()
    {
        Groups asset = CustomAssetUtility.CreateAsset<Groups>();
        asset.SheetName = "Game Config - cardboard-shoot";
        asset.WorksheetName = "Groups";
        EditorUtility.SetDirty(asset);        
    }
    
}