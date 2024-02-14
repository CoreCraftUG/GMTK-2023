using CoreCraft.Core;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableVisualManager : Singleton<TableVisualManager>
{
    [SerializeField, FoldoutGroup("Table Tops")] private TableTop[] _circleTableTops;
    [SerializeField, FoldoutGroup("Table Tops")] private TableTop[] _octagonTableTops;
    [SerializeField, FoldoutGroup("Table Tops")] private TableTop[] _crossTableTops;

    [SerializeField, FoldoutGroup("Table Frames")] private TableFrame[] _circleTableFrames;
    [SerializeField, FoldoutGroup("Table Frames")] private TableFrame[] _octagonTableFrames;
    [SerializeField, FoldoutGroup("Table Frames")] private TableFrame[] _crossTableFrames;

    private int _tableTopIndex;
    private int _tableFrameIndex;
    private int _tableTypeIndex;

    private string _savePath;
    private string _saveKey = "TableVisualSave"; //DO NOT CHANGE THIS
    private string _encryptionPassword = "CoreCraftsSuperSavePassword"; //DO NOT CHANGE THIS
    private ES3Settings _settings;

    [FoldoutGroup("Table Visual Save"), SerializeField] private string _saveLocationFolderName;

    [FoldoutGroup("Table Visual Save"), SerializeField, ReadOnly] private TableVisual _tableVisuals;
    public TableVisual TableVisuals { get { return _tableVisuals; } }

    private TableVisual _tempTableVisuals;
    public TableVisual tempTableVisuals { get { return _tempTableVisuals; } }


    private void Start()
    {
        _savePath = $"{Application.dataPath}/{_saveLocationFolderName}/{_saveKey}";
        _settings = new ES3Settings() { encryptionType = ES3.EncryptionType.AES, encryptionPassword = _encryptionPassword, compressionType = ES3.CompressionType.Gzip, bufferSize = 250000 };

        if(ES3.KeyExists(_saveKey, _savePath, _settings))
            _tableVisuals = ES3.Load<TableVisual>(_saveKey, _savePath, _settings);
        else
            _tableVisuals = new TableVisual();

        SetTableVisual(_tableVisuals.TableTop == null ? _octagonTableTops[_tableTopIndex].TopObject : _tableVisuals.TableTop, _tableVisuals.TableFrame == null? _octagonTableFrames[_tableFrameIndex].FrameObject : _tableVisuals.TableFrame);

        _tempTableVisuals = _tableVisuals;
    }

    private void SetTableVisual(GameObject tableTop,GameObject tableFrame)
    {
        _tableVisuals.TableTop = tableTop;
        _tableVisuals.TableFrame = tableFrame;

        ES3.Save(_saveKey, _tableVisuals, _savePath, _settings);
    }

    public int NextTableTop()
    {
        return _tableTopIndex = _tableTopIndex >= 2 ? 0 : _tableTopIndex++;
    }

    public int PreviousTableTop()
    {
        return _tableTopIndex = _tableTopIndex <= 0 ? 2 : _tableTopIndex--;
    }

    public int NextTableFrame()
    {
        return _tableFrameIndex = _tableFrameIndex >= 2 ? 0 : _tableFrameIndex++;
    }

    public int PreviousTableFrame()
    {
        return _tableFrameIndex = _tableFrameIndex <= 0 ? 2 : _tableFrameIndex--;
    }

    public int NextTableType()
    {
        return _tableTypeIndex = _tableTypeIndex >= 2 ? 0 : _tableTypeIndex++;
    }

    public int PreviousTableType()
    {
        return _tableTypeIndex = _tableTypeIndex <= 0 ? 2 : _tableTypeIndex--;
    }

    public TableTop GetTableTop(int tableTypeIndex, int tableTopIndex)
    {
        switch (tableTypeIndex)
        {
            case 0:
                return _octagonTableTops[tableTopIndex];
            case 1:
                return _circleTableTops[tableTopIndex];
            case 2:
                return _crossTableTops[tableTopIndex];
            default:
                throw new Exception($"Table Type Index {tableTypeIndex} does not exist!");
        }
    }

    public TableFrame GetTableFrame(int tableTypeIndex, int tableFrameIndex)
    {
        switch (tableTypeIndex)
        {
            case 0:
                return _octagonTableFrames[tableFrameIndex];
            case 1:
                return _circleTableFrames[tableFrameIndex];
            case 2:
                return _crossTableFrames[tableFrameIndex];
            default:
                throw new Exception($"Table Type Index {tableTypeIndex} does not exist!");
        }
    }

    public void ApplyTable()
    {
        TableTop tempTop = new TableTop();
        TableFrame tempFrame = new TableFrame();
        switch(_tableTypeIndex)
        {
            case 0:
                tempTop = _octagonTableTops[_tableTopIndex];
                tempFrame = _octagonTableFrames[_tableFrameIndex];
                break;
            case 1:
                tempTop = _circleTableTops[_tableTopIndex];
                tempFrame = _circleTableFrames[_tableFrameIndex];
                break;
            case 2:
                tempTop = _crossTableTops[_tableTopIndex];
                tempFrame = _crossTableFrames[_tableFrameIndex];
                break;
        }

        if (tempFrame.Unlocked && tempTop.Unlocked)
            SetTableVisual(tempTop.TopObject, tempFrame.FrameObject);
    }
}

[Serializable]
public struct TableVisual
{
    [SerializeField] public GameObject TableFrame;
    [SerializeField] public GameObject TableTop;
}

[Serializable]
public struct TableFrame
{
    [SerializeField] public GameObject FrameObject;
    [SerializeField] public bool Unlocked;
}

[Serializable]
public struct TableTop
{
    [SerializeField] public GameObject TopObject;
    [SerializeField] public bool Unlocked;
}