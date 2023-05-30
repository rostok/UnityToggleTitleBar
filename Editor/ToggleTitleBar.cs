using UnityToolbarExtender;

using UnityEngine;
using UnityEditor;

using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

[InitializeOnLoad]
public static class ToggleTitleBar
{
    [DllImport("user32.dll")] static extern IntPtr GetActiveWindow();
    [DllImport("user32.dll")] static extern IntPtr SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int Y, int cx, int cy, int wFlags);
    [DllImport("user32.dll")] static extern int GetWindowLong(IntPtr hWnd, int nIndex);
    [DllImport("user32.dll")] static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
    [DllImport("user32.dll")] static extern IntPtr GetMenu(IntPtr hWnd);
    [DllImport("user32.dll")] static extern IntPtr SetMenu(IntPtr hWnd, IntPtr hMenu);
    [DllImport("user32.dll")] private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);
    [DllImport("user32.dll")] private static extern IntPtr GetForegroundWindow();
    [DllImport("user32.dll")] private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    private const int GWL_STYLE = -16;
    private const int WS_CAPTION = 0x00C00000;
    private const int WS_SYSMENU = 0x00080000;
    private const int SWP_NOMOVE = 0x0002;
    private const int SWP_NOSIZE = 0x0001;
    private const int SWP_FRAMECHANGED = 0x0020;
    private const int SWP_SHOWWINDOW = 0x0040;
    private const int SW_MAXIMIZE = 3;

    private static readonly IntPtr HWND_TOP = IntPtr.Zero;

    private static bool hidden = false;
    private static IntPtr hwnd = IntPtr.Zero;
    private static IntPtr hmenu = IntPtr.Zero;

    private static void SetHandles() {
        IntPtr foregroundHwnd = GetForegroundWindow();
        int processId;
        GetWindowThreadProcessId(foregroundHwnd, out processId);
        Process currentProcess = Process.GetProcessById(processId);
        hwnd = currentProcess.MainWindowHandle;
        // var hwnd = GetActiveWindow();

        var menu = GetMenu(hwnd);
        if (menu != IntPtr.Zero) hmenu = menu;
    }

    private static void Show() {
        SetHandles();
        if (hwnd == IntPtr.Zero) return;
        SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) | WS_CAPTION);
        SetWindowPos(hwnd, HWND_TOP, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_FRAMECHANGED);
        Menu.SetChecked("Title Bar/Toggle", false);
        if (hmenu!=IntPtr.Zero) SetMenu(hwnd,hmenu);
        hidden = false;
    }

    private static void Hide() {
        SetHandles();
        if (hwnd == IntPtr.Zero) return;
        SetWindowLong(hwnd, GWL_STYLE, GetWindowLong(hwnd, GWL_STYLE) & ~(WS_CAPTION));
        SetWindowPos(hwnd, HWND_TOP, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE | SWP_FRAMECHANGED ); 
        ShowWindow(hwnd, SW_MAXIMIZE ); // also maximize
        Menu.SetChecked("Title Bar/Toggle", true);
        SetMenu(hwnd,IntPtr.Zero);
        hidden = true;
    }

    [MenuItem("Title Bar/Toggle _F11")]
    public static void Toggle() {
        if (hidden) {
            Show();
            EditorApplication.update -= OnUpdate;
        } else {
            Hide();
            EditorApplication.update += OnUpdate;
        }
    }

    private static void OnUpdate()
    {
        if (hidden && GetMenu(hwnd)!=IntPtr.Zero) Hide();
    }
}


// https://github.com/marijnz/unity-toolbar-extender
[InitializeOnLoad]
public class ToggleTitleBarButton
{
    static ToggleTitleBarButton()
    {
        ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);
    }

    static void OnToolbarGUI()
    {
        GUILayout.FlexibleSpace();
        if(GUILayout.Button(new GUIContent("Titlebar toggle", "Toggle Unity's Titlebar and main menu"), EditorStyles.toolbarButton)) ToggleTitleBar.Toggle();
    }
}