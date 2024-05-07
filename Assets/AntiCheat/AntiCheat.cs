using UnityEngine;
using System;
using System.Runtime.InteropServices;

public class AntiCheat : MonoBehaviour
{
    // Define the structure for the patch data
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    struct DbgUiRemoteBreakinPatch
    {
        public ushort push_0;
        public byte push;
        public uint CurrentProcessHandle;
        public byte mov_eax;
        public uint TerminateProcess;
        public ushort call_eax;
    }

    // Import necessary WinAPI functions
    [DllImport("kernel32.dll")]
    static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("kernel32.dll")]
    static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

    [DllImport("kernel32.dll")]
    static extern bool VirtualProtect(IntPtr lpAddress, int dwSize, uint flNewProtect, out uint lpflOldProtect);

    // Other constants and variables
    const uint PAGE_READWRITE = 0x04;
    const uint PROCESS_ALL_ACCESS = 0x1F0FFF;

    void Start()
    {
        PatchDbgUiRemoteBreakin();
    }

    void PatchDbgUiRemoteBreakin()
    {
        IntPtr hNtdll = GetModuleHandle("ntdll.dll");
        if (hNtdll == IntPtr.Zero)
            return;

        IntPtr pDbgUiRemoteBreakin = GetProcAddress(hNtdll, "DbgUiRemoteBreakin");
        if (pDbgUiRemoteBreakin == IntPtr.Zero)
            return;

        IntPtr hKernel32 = GetModuleHandle("kernel32.dll");
        if (hKernel32 == IntPtr.Zero)
            return;

        IntPtr pTerminateProcess = GetProcAddress(hKernel32, "TerminateProcess");
        if (pTerminateProcess == IntPtr.Zero)
            return;

        DbgUiRemoteBreakinPatch patch = new DbgUiRemoteBreakinPatch
        {
            push_0 = 0x006A,
            push = 0x68,
            CurrentProcessHandle = 0xFFFFFFFF,
            mov_eax = 0xB8,
            TerminateProcess = (uint)pTerminateProcess.ToInt32(),
            call_eax = 0xD0FF
        };

        int patchSize = Marshal.SizeOf(typeof(DbgUiRemoteBreakinPatch));
        IntPtr ptrDbgUiRemoteBreakin = pDbgUiRemoteBreakin;

        uint oldProtect;
        if (!VirtualProtect(ptrDbgUiRemoteBreakin, patchSize, PAGE_READWRITE, out oldProtect))
            return;

        Marshal.StructureToPtr(patch, ptrDbgUiRemoteBreakin, false);

        VirtualProtect(ptrDbgUiRemoteBreakin, patchSize, oldProtect, out oldProtect);
    }
}
