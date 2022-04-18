using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace KG2
{
    class BitmapWrapper
    {
        Bitmap _instance;
        bool _locked = false;

        IntPtr _ptr;
        int _length;
        byte[] _byteData;
        BitmapData _bmData;

        public BitmapWrapper(Bitmap bm)
        {
            _instance = bm;
        }

        public void Lock()
        {
            if (_locked) return;

            _locked = true;
            Rectangle rect = new Rectangle(0, 0, _instance.Width, _instance.Height);

            _bmData = _instance.LockBits(rect, ImageLockMode.ReadWrite, _instance.PixelFormat);


            _ptr = _bmData.Scan0;
            _length = Math.Abs(_bmData.Stride) * _instance.Height;
            _byteData = new byte[_length];

            Marshal.Copy(_ptr, _byteData, 0, _length);
        }

        public void Unlock()
        {
            if (!_locked) return;

            _locked = false;
            Marshal.Copy(_byteData, 0, _ptr, _length);
            _instance.UnlockBits(_bmData);
        }

        public bool IsLocked => _locked;

        public int Length => _length;
        public int Height => _bmData.Height;
        public int Width => _bmData.Width;

        public Color this[int x, int y]
        {
            get
            {
                if (!_locked) throw new Exception("Ради ускорения работы программы чтение и запись в незакрытый битмап запрещены.");

                int i = y * Math.Abs(_bmData.Stride) + x * 4;
                return Color.FromArgb(_byteData[i + 3], _byteData[i + 2], _byteData[i + 1], _byteData[i]);
            }
            
            set
            {
                if (!_locked) throw new Exception("Ради ускорения работы программы чтение и запись в незакрытый битмап запрещены.");

                int i = y * Math.Abs(_bmData.Stride) + x * 4;

                _byteData[i] = value.B;
                _byteData[i + 1] = value.G;
                _byteData[i + 2] = value.R;
                _byteData[i + 3] = value.A;
            }
        }

        public Color this[int i]
        {
            get
            {
                if (!_locked) throw new Exception("Ради ускорения работы программы чтение и запись в незакрытый битмап запрещены.");

                i *= 4;
                return Color.FromArgb(_byteData[i + 3], _byteData[i + 2], _byteData[i + 1], _byteData[i]);
            }

            set
            {
                if (!_locked) throw new Exception("Ради ускорения работы программы чтение и запись в незакрытый битмап запрещены.");

                i *= 4;

                _byteData[i] = value.B;
                _byteData[i + 1] = value.G;
                _byteData[i + 2] = value.R;
                _byteData[i + 3] = value.A;
            }
        }

        public static implicit operator Bitmap(BitmapWrapper @this)
        {
            if (@this._locked) throw new Exception("Нельзя использовать закрытый битмап.");
            return @this._instance;
        }
    }
}
