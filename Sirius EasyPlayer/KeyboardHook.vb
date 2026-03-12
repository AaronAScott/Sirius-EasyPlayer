Imports System.Runtime.InteropServices
Imports System.Diagnostics

Public Class KeyboardHook
     Private Const WH_KEYBOARD_LL As Integer = 13
     Private Const WM_KEYDOWN As Integer = &H100

     Private Delegate Function LowLevelKeyboardProc(ByVal nCode As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
     Private _proc As LowLevelKeyboardProc = AddressOf HookCallback
     Private _hookID As IntPtr = IntPtr.Zero

     Public Sub Install()
          _hookID = SetHook(_proc)
     End Sub

     Public Sub Uninstall()
          UnhookWindowsHookEx(_hookID)
     End Sub

     Private Function SetHook(ByVal proc As LowLevelKeyboardProc) As IntPtr
          Using curProcess As Process = Process.GetCurrentProcess()
               Using curModule As ProcessModule = curProcess.MainModule
                    Return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0)
               End Using
          End Using
     End Function

     Private Function HookCallback(ByVal nCode As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
          If nCode >= 0 AndAlso wParam.ToInt32() = WM_KEYDOWN Then
               Dim vkCode As Integer = Marshal.ReadInt32(lParam)
               If frmMusicPlayer.IsOpen Then
                    frmMusicPlayer.OnShortcutKeyPressed(CType(vkCode, Keys))
               Else

                    frmMain.OnShortcutKeyPressed(CType(vkCode, Keys))
               End If
          End If
          Return CallNextHookEx(_hookID, nCode, wParam, lParam)
     End Function

     <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
     Private Shared Function SetWindowsHookEx(ByVal idHook As Integer, ByVal lpfn As LowLevelKeyboardProc, ByVal hMod As IntPtr, ByVal dwThreadId As UInteger) As IntPtr
     End Function

     <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
     Private Shared Function UnhookWindowsHookEx(ByVal hhk As IntPtr) As Boolean
     End Function

     <DllImport("user32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
     Private Shared Function CallNextHookEx(ByVal hhk As IntPtr, ByVal nCode As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr
     End Function

     <DllImport("kernel32.dll", CharSet:=CharSet.Auto, SetLastError:=True)>
     Private Shared Function GetModuleHandle(ByVal lpModuleName As String) As IntPtr
     End Function
End Class