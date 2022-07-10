using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.InteropServices;
using _4thWall.DEVMODEInterface;
using System.Diagnostics;
using Microsoft.Win32.SafeHandles;

namespace _4thWall {

    public enum Orientations { DEGREES_CW_0, DEGREES_CW_270, DEGREES_CW_180, DEGREES_CW_90 }

    internal static class NativeMethods {
        [DllImport("user32.dll")]
        internal static extern DISP_CHANGE ChangeDisplaySettingsEx(
            string lpszDeviceName, ref DEVMODE lpDevMode, IntPtr hwnd,
            DisplaySettingsFlags dwflags, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern bool EnumDisplayDevices(
            string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice,
            uint dwFlags);

        [DllImport("user32.dll", CharSet = CharSet.Ansi)]
        internal static extern int EnumDisplaySettings(
            string lpszDeviceName, int iModeNum, ref DEVMODE lpDevMode);

        internal const int DMDO_DEFAULT = 0;
        internal const int DMDO_90 = 1;
        internal const int DMDO_180 = 2;
        internal const int DMDO_270 = 3;

        public const int ENUM_CURRENT_SETTINGS = -1;

        [DllImport("user32.dll")]
        internal static extern IntPtr GetActiveWindow();
        [DllImport("user32.dll")]
        internal static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        internal static extern bool SetWindowText(IntPtr Handle, string Name);

        [DllImport("user32.dll")]
        internal static extern IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool ExitWindowsEx(ExitWindows uFlags, ShutdownReason dwReason);

        internal const short SWP_NOMOVE = 0x2;
        internal const short SWP_NOSIZE = 1;
        internal const short SWP_NOZORDER = 0x4;
        internal const short SWP_SHOWWINDOW = 0x0040;

        [DllImport("user32.dll")]
        internal static extern IntPtr SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, int wFlags);

        internal const short MONITOR_DEFAULTTOPRIMARY = 0x0000001;
        internal const short MONITOR_DEFAULTTONEAREST = 0x0000002;

        [DllImport("user32.dll")]
        public static extern bool GetMonitorInfo(IntPtr hMonitor, NativeMonitorInfo lpmi);
    }

    public static class _4thWall {

        /*public static bool RotateWindow(uint DisplayNumber, Orientations Orientation) {
            if (DisplayNumber == 0)
                throw new ArgumentOutOfRangeException("DisplayNumber", DisplayNumber, "First display is 1.");

            bool result = false;
            DISPLAY_DEVICE d = new DISPLAY_DEVICE();
            DEVMODE dm = new DEVMODE();
            d.cb = Marshal.SizeOf(d);

            if (!NativeMethods.EnumDisplayDevices(null, DisplayNumber - 1, ref d, 0))
                throw new ArgumentOutOfRangeException("DisplayNumber", DisplayNumber, "Number is greater than connected displays.");

            if (0 != NativeMethods.EnumDisplaySettings(d.DeviceName, NativeMethods.ENUM_CURRENT_SETTINGS, ref dm)) {
                if ((dm.dmDisplayOrientation + (int)Orientation) % 2 == 1) {// Need to swap height and width?
                    int temp = dm.dmPelsHeight;
                    dm.dmPelsHeight = dm.dmPelsWidth;
                    dm.dmPelsWidth = temp;
                }

                switch (Orientation) {
                    case Orientations.DEGREES_CW_90:
                        dm.dmDisplayOrientation = NativeMethods.DMDO_270;
                        break;
                    case Orientations.DEGREES_CW_180:
                        dm.dmDisplayOrientation = NativeMethods.DMDO_180;
                        break;
                    case Orientations.DEGREES_CW_270:
                        dm.dmDisplayOrientation = NativeMethods.DMDO_90;
                        break;
                    case Orientations.DEGREES_CW_0:
                        dm.dmDisplayOrientation = NativeMethods.DMDO_DEFAULT;
                        break;
                    default:
                        break;
                }

                DISP_CHANGE ret = NativeMethods.ChangeDisplaySettingsEx(
                    d.DeviceName, ref dm, IntPtr.Zero,
                    DisplaySettingsFlags.CDS_UPDATEREGISTRY, IntPtr.Zero);

                result = ret == 0;
            }

            return result;
        }*/

        public static bool RotateWindow(Orientations Orientation) {
            bool result = false;
            DISPLAY_DEVICE d = new DISPLAY_DEVICE();
            DEVMODE dm = new DEVMODE();
            d.cb = Marshal.SizeOf(d);

            if (0 != NativeMethods.EnumDisplaySettings(d.DeviceName, NativeMethods.ENUM_CURRENT_SETTINGS, ref dm)) {
                if ((dm.dmDisplayOrientation + (int)Orientation) % 2 == 1) {// Need to swap height and width?
                    int temp = dm.dmPelsHeight;
                    dm.dmPelsHeight = dm.dmPelsWidth;
                    dm.dmPelsWidth = temp;
                }

                switch (Orientation) {
                    case Orientations.DEGREES_CW_90:
                        dm.dmDisplayOrientation = NativeMethods.DMDO_270;
                        break;
                    case Orientations.DEGREES_CW_180:
                        dm.dmDisplayOrientation = NativeMethods.DMDO_180;
                        break;
                    case Orientations.DEGREES_CW_270:
                        dm.dmDisplayOrientation = NativeMethods.DMDO_90;
                        break;
                    case Orientations.DEGREES_CW_0:
                        dm.dmDisplayOrientation = NativeMethods.DMDO_DEFAULT;
                        break;
                    default:
                        break;
                }

                DISP_CHANGE ret = NativeMethods.ChangeDisplaySettingsEx(
                    d.DeviceName, ref dm, IntPtr.Zero,
                    DisplaySettingsFlags.CDS_UPDATEREGISTRY, IntPtr.Zero);

                result = ret == 0;
            }

            return result;
        }

        public static void MoveWindow(int x, int y) {
            NativeMethods.SetWindowPos(NativeMethods.GetActiveWindow(), 0, x, y, 1024, 768, NativeMethods.SWP_NOZORDER | NativeMethods.SWP_SHOWWINDOW | 0x2000);
        }
        public static void MoveWindow_Random() {
            NativeMonitorInfo Bounds = new NativeMonitorInfo();
            NativeMethods.GetMonitorInfo(NativeMethods.MonitorFromWindow(NativeMethods.GetActiveWindow(), 0), Bounds);
            MoveWindow(UnityEngine.Random.Range(0, Bounds.Monitor.Right - Bounds.Monitor.Left - 1024), UnityEngine.Random.Range(0, Bounds.Monitor.Bottom - Bounds.Monitor.Top - 768));
        }

        public static void MoveWindow_Random(int DistanceFromCenter) {
            NativeMonitorInfo Bounds = new NativeMonitorInfo();
            NativeMethods.GetMonitorInfo(NativeMethods.MonitorFromWindow(NativeMethods.GetActiveWindow(), 0), Bounds);
            MoveWindow(UnityEngine.Random.Range(0, (Bounds.Monitor.Right - Bounds.Monitor.Left - 1024 / 2 + DistanceFromCenter / 2)),
                UnityEngine.Random.Range(0, (Bounds.Monitor.Bottom - Bounds.Monitor.Top - 768 / 2 + DistanceFromCenter / 2)));
        }

        public static void ChangeName(string newName) {
            NativeMethods.SetWindowText(NativeMethods.GetActiveWindow(), newName);
        }

        public static void CRASH(bool Restart = true, int DelayInSeconds = 0) {
            NativeMethods.ExitWindowsEx(ExitWindows.Reboot, ShutdownReason.MajorOther);
        }
    }
    //See: https://stackoverflow.com/questions/39288135/rotating-the-display-programmatically
    namespace DEVMODEInterface {
        // See: https://msdn.microsoft.com/en-us/library/windows/desktop/dd183565(v=vs.85).aspx
        [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Ansi)]
        internal struct DEVMODE {
            public const int CCHDEVICENAME = 32;
            public const int CCHFORMNAME = 32;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHDEVICENAME)]
            [FieldOffset(0)]
            public string dmDeviceName;
            [FieldOffset(32)]
            public Int16 dmSpecVersion;
            [FieldOffset(34)]
            public Int16 dmDriverVersion;
            [FieldOffset(36)]
            public Int16 dmSize;
            [FieldOffset(38)]
            public Int16 dmDriverExtra;
            [FieldOffset(40)]
            public DM dmFields;

            [FieldOffset(44)]
            Int16 dmOrientation;
            [FieldOffset(46)]
            Int16 dmPaperSize;
            [FieldOffset(48)]
            Int16 dmPaperLength;
            [FieldOffset(50)]
            Int16 dmPaperWidth;
            [FieldOffset(52)]
            Int16 dmScale;
            [FieldOffset(54)]
            Int16 dmCopies;
            [FieldOffset(56)]
            Int16 dmDefaultSource;
            [FieldOffset(58)]
            Int16 dmPrintQuality;

            [FieldOffset(44)]
            public POINTL dmPosition;
            [FieldOffset(52)]
            public Int32 dmDisplayOrientation;
            [FieldOffset(56)]
            public Int32 dmDisplayFixedOutput;

            [FieldOffset(60)]
            public short dmColor;
            [FieldOffset(62)]
            public short dmDuplex;
            [FieldOffset(64)]
            public short dmYResolution;
            [FieldOffset(66)]
            public short dmTTOption;
            [FieldOffset(68)]
            public short dmCollate;
            [FieldOffset(72)]
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = CCHFORMNAME)]
            public string dmFormName;
            [FieldOffset(102)]
            public Int16 dmLogPixels;
            [FieldOffset(104)]
            public Int32 dmBitsPerPel;
            [FieldOffset(108)]
            public Int32 dmPelsWidth;
            [FieldOffset(112)]
            public Int32 dmPelsHeight;
            [FieldOffset(116)]
            public Int32 dmDisplayFlags;
            [FieldOffset(116)]
            public Int32 dmNup;
            [FieldOffset(120)]
            public Int32 dmDisplayFrequency;
        }

        // See: https://msdn.microsoft.com/en-us/library/windows/desktop/dd183569(v=vs.85).aspx
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        internal struct DISPLAY_DEVICE {
            [MarshalAs(UnmanagedType.U4)]
            public int cb;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceString;
            [MarshalAs(UnmanagedType.U4)]
            public DisplayDeviceStateFlags StateFlags;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceKey;
        }

        // See: https://msdn.microsoft.com/de-de/library/windows/desktop/dd162807(v=vs.85).aspx
        [StructLayout(LayoutKind.Sequential)]
        internal struct POINTL {
            long x;
            long y;
        }

        internal enum DISP_CHANGE : int {
            Successful = 0,
            Restart = 1,
            Failed = -1,
            BadMode = -2,
            NotUpdated = -3,
            BadFlags = -4,
            BadParam = -5,
            BadDualView = -6
        }

        // http://www.pinvoke.net/default.aspx/Enums/DisplayDeviceStateFlags.html
        [Flags()]
        internal enum DisplayDeviceStateFlags : int {
            /// <summary>The device is part of the desktop.</summary>
            AttachedToDesktop = 0x1,
            MultiDriver = 0x2,
            /// <summary>The device is part of the desktop.</summary>
            PrimaryDevice = 0x4,
            /// <summary>Represents a pseudo device used to mirror application drawing for remoting or other purposes.</summary>
            MirroringDriver = 0x8,
            /// <summary>The device is VGA compatible.</summary>
            VGACompatible = 0x10,
            /// <summary>The device is removable; it cannot be the primary display.</summary>
            Removable = 0x20,
            /// <summary>The device has more display modes than its output devices support.</summary>
            ModesPruned = 0x8000000,
            Remote = 0x4000000,
            Disconnect = 0x2000000
        }

        // http://www.pinvoke.net/default.aspx/user32/ChangeDisplaySettingsFlags.html
        [Flags()]
        internal enum DisplaySettingsFlags : int {
            CDS_NONE = 0,
            CDS_UPDATEREGISTRY = 0x00000001,
            CDS_TEST = 0x00000002,
            CDS_FULLSCREEN = 0x00000004,
            CDS_GLOBAL = 0x00000008,
            CDS_SET_PRIMARY = 0x00000010,
            CDS_VIDEOPARAMETERS = 0x00000020,
            CDS_ENABLE_UNSAFE_MODES = 0x00000100,
            CDS_DISABLE_UNSAFE_MODES = 0x00000200,
            CDS_RESET = 0x40000000,
            CDS_RESET_EX = 0x20000000,
            CDS_NORESET = 0x10000000
        }

        [Flags()]
        internal enum DM : int {
            Orientation = 0x00000001,
            PaperSize = 0x00000002,
            PaperLength = 0x00000004,
            PaperWidth = 0x00000008,
            Scale = 0x00000010,
            Position = 0x00000020,
            NUP = 0x00000040,
            DisplayOrientation = 0x00000080,
            Copies = 0x00000100,
            DefaultSource = 0x00000200,
            PrintQuality = 0x00000400,
            Color = 0x00000800,
            Duplex = 0x00001000,
            YResolution = 0x00002000,
            TTOption = 0x00004000,
            Collate = 0x00008000,
            FormName = 0x00010000,
            LogPixels = 0x00020000,
            BitsPerPixel = 0x00040000,
            PelsWidth = 0x00080000,
            PelsHeight = 0x00100000,
            DisplayFlags = 0x00200000,
            DisplayFrequency = 0x00400000,
            ICMMethod = 0x00800000,
            ICMIntent = 0x01000000,
            MediaType = 0x02000000,
            DitherType = 0x04000000,
            PanningWidth = 0x08000000,
            PanningHeight = 0x10000000,
            DisplayFixedOutput = 0x20000000
        }

        [Flags]
        internal enum ExitWindows : uint {
            LogOff = 0x00,
            ShutDown = 0x01,
            Reboot = 0x02,
            PowerOff = 0x08,
            RestartApps = 0x40,
            Force = 0x04,
            ForceIfHung = 0x10,
        }

        [Flags]
        internal enum ShutdownReason : uint {
            MajorApplication = 0x00040000,
            MajorHardware = 0x00010000,
            MajorLegacyApi = 0x00070000,
            MajorOperatingSystem = 0x00020000,
            MajorOther = 0x00000000,
            MajorPower = 0x00060000,
            MajorSoftware = 0x00030000,
            MajorSystem = 0x00050000,
            MinorBlueScreen = 0x0000000F,
            MinorCordUnplugged = 0x0000000b,
            MinorDisk = 0x00000007,
            MinorEnvironment = 0x0000000c,
            MinorHardwareDriver = 0x0000000d,
            MinorHotfix = 0x00000011,
            MinorHung = 0x00000005,
            MinorInstallation = 0x00000002,
            MinorMaintenance = 0x00000001,
            MinorMMC = 0x00000019,
            MinorNetworkConnectivity = 0x00000014,
            MinorNetworkCard = 0x00000009,
            MinorOther = 0x00000000,
            MinorOtherDriver = 0x0000000e,
            MinorPowerSupply = 0x0000000a,
            MinorProcessor = 0x00000008,
            MinorReconfig = 0x00000004,
            MinorSecurity = 0x00000013,
            MinorSecurityFix = 0x00000012,
            MinorSecurityFixUninstall = 0x00000018,
            MinorServicePack = 0x00000010,
            MinorServicePackUninstall = 0x00000016,
            MinorTermSrv = 0x00000020,
            MinorUnstable = 0x00000006,
            MinorUpgrade = 0x00000003,
            MinorWMI = 0x00000015,
            FlagUserDefined = 0x40000000,
            FlagPlanned = 0x80000000
        }

        [Serializable, StructLayout(LayoutKind.Sequential)]
        internal struct NativeRectangle {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;

            public NativeRectangle(int left, int top, int right, int bottom) {
                Left = left;
                Top = top;
                Right = right;
                Bottom = bottom;
            }
        }


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        internal sealed class NativeMonitorInfo {
            public int Size = Marshal.SizeOf(typeof(NativeMonitorInfo));
            public NativeRectangle Monitor;
            public NativeRectangle Work;
            public int Flags;
        }

    }
}
