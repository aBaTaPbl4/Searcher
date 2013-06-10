using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace Searcher.Implementation
{
    public class GlassHelper
    {
        [DllImport("DwmApi.dll")]
        private static extern int DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS pMarInset);

        [DllImport("dwmapi.dll")]
        private static extern int DwmIsCompositionEnabled(ref int en);


        public static bool ExtendGlass(Window win, int left, int right, int top, int bottom)
        {
            try
            {
                int ret = 0;
                DwmIsCompositionEnabled(ref ret);

                if (Environment.OSVersion.Version.Major > 5 && ret > 0)
                {
                    IntPtr hwnd = new WindowInteropHelper(win).Handle;
                    if (hwnd != IntPtr.Zero)
                    {
                        // Set the background to transparent from both the WPF and Win32 perspectives
                        win.Background = Brushes.Transparent;
                        HwndSource.FromHwnd(hwnd).CompositionTarget.BackgroundColor = Colors.Transparent;

                        // Adjust the margins to take the system DPI into account.
                        var margins = new MARGINS(left, right, top, bottom);

                        // Extend the glass frame.
                        if (DwmExtendFrameIntoClientArea(hwnd, ref margins) < 0)
                        {
                            win.Background = SystemColors.WindowBrush;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
                else
                {
                    win.Background = Brushes.White;
                }
                return false;
            }
            catch (DllNotFoundException)
            {
                return false;
            }
        }

        #region Nested type: MARGINS

        [StructLayout(LayoutKind.Sequential)]
        public struct MARGINS
        {
            public MARGINS(int left, int right, int top, int bottom)
            {
                cxLeftWidth = left;
                cxRightWidth = right;
                cyTopHeight = top;
                cyBottomHeight = bottom;
            }

            public int cxLeftWidth;
            public int cxRightWidth;
            public int cyTopHeight;
            public int cyBottomHeight;
        }

        #endregion
    }
}