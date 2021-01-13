using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;


public class OpenCV : MonoBehaviour
{

    [DllImport("dlltest")]
    private static extern IntPtr CreateCascade();

    [DllImport("dlltest")]
    private static extern IntPtr GetCamera();
    [DllImport("dlltest")]
    private static extern void CloseCamera(IntPtr camera);
    [DllImport("dlltest")]
    private static extern void CameraUpdate(IntPtr camera, IntPtr pixel, int width, int height, bool mosaic);
    [DllImport("dlltest")]
    private static extern void CameraUpdateOverRide(IntPtr camera, int width, int height, bool mosaic);
    [DllImport("__Internal")]
    private static extern Vector2 Multiply(Vector2 vec, float m);

    private IntPtr _camera;
    private IntPtr _frame;
    private Texture2D _texture;
    private Color32[] _pixels;
    private GCHandle _pHandle;
    private IntPtr _pixels_ptr;


    //test
    private Vector2 vec;
    private bool _mosaic;

    // Start is called before the first frame update
    void Awake()
    {
        _camera = GetCamera();
        _texture = new Texture2D(320, 180, TextureFormat.ARGB32, false);
        _pixels = _texture.GetPixels32();
        _pHandle = GCHandle.Alloc(_pixels, GCHandleType.Pinned);
        _pixels_ptr = _pHandle.AddrOfPinnedObject();
        GetComponent<Renderer>().material.mainTexture = _texture;

        _mosaic = false;
    }

    private void Start()
    {
        vec = new Vector2(10, 5);
    }

    // Update is called once per frame
    void Update()
    {
        vec = Multiply(vec, 5);
        CameraUpdateOverRide(_camera, _texture.width, _texture.height, _mosaic);
        _texture.SetPixels32(_pixels);
        _texture.Apply();
        _mosaic = true;
    }

    void OnApplicationQuit()
    {
        _pHandle.Free();
        CloseCamera(_camera);
    }
}
