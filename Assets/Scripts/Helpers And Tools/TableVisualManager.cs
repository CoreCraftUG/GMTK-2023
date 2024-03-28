using CoreCraft.Core;
using Sirenix.OdinInspector;
using System;
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
    private const string _saveKey = "TableVisualSave"; //DO NOT CHANGE THIS
    private const string _encryptionPassword = "CoreCraftsSuperSavePassword"; //DO NOT CHANGE THIS
    private ES3Settings _settings;

    [FoldoutGroup("Table Visual Save"), SerializeField] private string _saveLocationFolderName;

    [FoldoutGroup("Table Visual Save"), SerializeField, ReadOnly] private TableVisual _tableVisuals;
    public TableVisual TableVisuals { get { return _tableVisuals; } }

    private TableVisual _tempTableVisuals;
    public TableVisual tempTableVisuals { get { return _tempTableVisuals; } }

    public event EventHandler OnTableVisualsUpdated;


    private void Start()
    {
        _savePath = $"{Application.dataPath}/{_saveLocationFolderName}/{_saveKey}";
        Debug.Log(_savePath);
        _settings = new ES3Settings() { encryptionType = ES3.EncryptionType.AES, encryptionPassword = _encryptionPassword, compressionType = ES3.CompressionType.Gzip, bufferSize = 250000 };

        if (ES3.KeyExists(_saveKey, _savePath, _settings))
        {
            _tableVisuals = ES3.Load<TableVisual>(_saveKey, _savePath, _settings);
        }
        else
        {
            _tableVisuals = new TableVisual();
        }

        if (ES3.KeyExists("TableTop", _savePath, _settings))
        {
            _tableVisuals.TableTop = ES3.Load<GameObject>("TableTop", _savePath, _settings);
        }
        else
        {
            _tableVisuals.TableTop = new TableTop().TopObject;
        }

        if (ES3.KeyExists("TableTopMaterial", _savePath, _settings))
        {
            _tableVisuals.TableTop.GetComponent<MeshRenderer>().sharedMaterial = ES3.Load<Material>("TableTopMaterial", _savePath, _settings);
        }

        if (ES3.KeyExists("TableTopShader", _savePath, _settings))
        {
            _tableVisuals.TableTop.GetComponent<MeshRenderer>().sharedMaterial.shader = ES3.Load<Shader>("TableTopShader", _savePath, _settings);
        }

        if (ES3.KeyExists("TableFrame", _savePath, _settings))
        {
            _tableVisuals.TableFrame = ES3.Load<GameObject>("TableFrame", _savePath, _settings);
        }
        else
        {
            _tableVisuals.TableFrame = new TableFrame().FrameObject;
        }

        if (ES3.KeyExists("TableFrameMaterial", _savePath, _settings))
        {
            _tableVisuals.TableFrame.GetComponent<MeshRenderer>().sharedMaterial = ES3.Load<Material>("TableFrameMaterial", _savePath, _settings);
        }

        if (ES3.KeyExists("TableFrameShader", _savePath, _settings))
        {
            _tableVisuals.TableFrame.GetComponent<MeshRenderer>().sharedMaterial.shader = ES3.Load<Shader>("TableFrameShader", _savePath, _settings);
        }

        DontDestroyOnLoad(_tableVisuals.TableTop);
        DontDestroyOnLoad(_tableVisuals.TableFrame);

        //SetTableVisual(_tableVisuals.TableTop == null ? _octagonTableTops[_tableTopIndex].TopObject : _tableVisuals.TableTop, _tableVisuals.TableFrame == null? _octagonTableFrames[_tableFrameIndex].FrameObject : _tableVisuals.TableFrame);

        _tempTableVisuals = _tableVisuals;
    }

    private void SetTableVisual(GameObject tableTop,GameObject tableFrame)
    {
        _tableVisuals.TableTop = tableTop;
        _tableVisuals.TableFrame = tableFrame;

        ES3.Save(_saveKey, _tableVisuals, _savePath, _settings);
    }

    public void SetTableType(int index)
    {
        _tableTypeIndex = index;

        //ES3.Save("TableType", _tableTypeIndex, _savePath, _settings);

        SetTableTopVisual(_tableTopIndex);
        SetTableFrameVisual(_tableFrameIndex);

        //ES3.Save(_saveKey, _tableVisuals, _savePath, _settings);
    }

    public void SetTableTopVisual(int index)
    {
        switch (_tableTypeIndex)
        {
            case 0:
                _tableVisuals.TableTop = _octagonTableTops[index].TopObject;
                break;
            case 1:
                _tableVisuals.TableTop = _circleTableTops[index].TopObject;
                break;
            case 2:
                _tableVisuals.TableTop = _crossTableTops[index].TopObject;
                break;
        }

        ES3.Save("TableTop", _tableVisuals.TableTop, _savePath, _settings);
        ES3.Save("TableTopMaterial", _tableVisuals.TableTop.GetComponent<MeshRenderer>().sharedMaterial, _savePath, _settings);
        ES3.Save("TableTopShader", _tableVisuals.TableTop.GetComponent<MeshRenderer>().sharedMaterial.shader, _savePath, _settings);
        ES3.Save(_saveKey, _tableVisuals, _savePath, _settings);

        OnTableVisualsUpdated?.Invoke(this, EventArgs.Empty);
    }

    public void SetTableFrameVisual(int index)
    {
        switch (_tableTypeIndex)
        {
            case 0:
                _tableVisuals.TableFrame = _octagonTableFrames[index].FrameObject;
                break;
            case 1:
                _tableVisuals.TableFrame = _circleTableFrames[index].FrameObject;
                break;
            case 2:
                _tableVisuals.TableFrame = _crossTableFrames[index].FrameObject;
                break;
        }

        ES3.Save("TableFrame", _tableVisuals.TableFrame, _savePath, _settings);
        ES3.Save("TableFrameMaterial", _tableVisuals.TableFrame.GetComponent<MeshRenderer>().sharedMaterial, _savePath, _settings);
        ES3.Save("TableFrameShader", _tableVisuals.TableFrame.GetComponent<MeshRenderer>().sharedMaterial.shader, _savePath, _settings);
        ES3.Save(_saveKey, _tableVisuals, _savePath, _settings);

        OnTableVisualsUpdated?.Invoke(this, EventArgs.Empty);
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