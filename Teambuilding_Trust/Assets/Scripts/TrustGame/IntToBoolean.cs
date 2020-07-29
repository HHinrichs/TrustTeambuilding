﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IntToBoolean
{
    public static int SetBitTo1(this int value, int position)
    {
        // Set a bit at position to 1.
        return value |= (1 << position);
    }

    public static int SetBitTo0(this int value, int position)
    {
        // Set a bit at position to 0.
        return value & ~(1 << position);
    }

    public static bool IsBitSetTo1(this int value, int position)
    {
        // Return whether bit at position is set to 1.
        return (value & (1 << position)) != 0;
    }

    public static bool IsBitSetTo0(this int value, int position)
    {
        // If not 1, bit is 0.
        return !IsBitSetTo1(value, position);
    }

    public static string GetIntBinaryString(int n)
    {
        char[] b = new char[32];
        int pos = 31;
        int i = 0;

        while (i < 32)
        {
            if ((n & (1 << i)) != 0)
                b[pos] = '1';
            else
                b[pos] = '0';
            pos--;
            i++;
        }
        return new string(b);
    }
}
