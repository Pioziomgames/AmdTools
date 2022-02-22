using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace PiosAmdLibrary
{
    public class Functions
    {
        public static int CalculatePadding(int position)
        {
            int h = position % 16;
            if (h != 0)
            {
                if ((position + h) % 16 == 0)
                    return h;
                else
                {
                    h += (position + h) % 16;
                    if (h > 15)
                        h -= 16;

                    if ((position + h) % 16 != 0)
                        h -= (position + h) % 16;

                    if (h < 0)
                        h += 16;

                    return h;
                }
            }
            return h;
        }
        public static ulong CalculatePadding(ulong position)
        {
            ulong h = position % 16;
            if (h != 0)
            {
                if ((position + h) % 16 == 0)
                    return h;
                else
                {
                    h += (position + h) % 16;
                    if (h > 15)
                        h -= 16;

                    if ((position + h) % 16 != 0)
                        h -= (position + h) % 16;

                    if (h < 0)
                        h += 16;

                    return h;
                }
            }
            return h;
        }
        public static long CalculatePadding(long position)
        {
            long h = position % 16;
            if (h != 0)
            {
                if ((position + h) % 16 == 0)
                    return h;
                else
                {
                    h += (position + h) % 16;
                    if (h > 15)
                        h -= 16;

                    if ((position + h) % 16 != 0)
                        h -= (position + h) % 16;

                    if (h < 0)
                        h += 16;

                    return h;
                }
            }
            return h;
        }
    }
}
