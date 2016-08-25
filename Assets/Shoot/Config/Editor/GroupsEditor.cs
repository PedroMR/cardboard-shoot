using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Text;

using GDataDB;
using GDataDB.Linq;

using UnityQuickSheet;

///
/// !!! Machine generated code !!!
///
[CustomEditor(typeof(Groups))]
public class GroupsEditor : BaseGoogleEditor<Groups>
{	    
    public override bool Load()
    {        
        Groups targetData = target as Groups;
        
        var client = new DatabaseClient("", "");
        string error = string.Empty;
        var db = client.GetDatabase(targetData.SheetName, ref error);	
        var table = db.GetTable<GroupsData>(targetData.WorksheetName) ?? db.CreateTable<GroupsData>(targetData.WorksheetName);
        
        List<GroupsData> myDataList = new List<GroupsData>();
        
        var all = table.FindAll();
        foreach(var elem in all)
        {
            GroupsData data = new GroupsData();
            
            data = Cloner.DeepCopy<GroupsData>(elem.Element);
            myDataList.Add(data);
        }
                
        targetData.dataArray = myDataList.ToArray();
        
        EditorUtility.SetDirty(targetData);
        AssetDatabase.SaveAssets();
        
        return true;
    }
}
