using System.Collections;

namespace VM;

public class Converter
{
    public static byte [] toByteArray(BitArray bits)
    {
        int numBytes = bits.Count / 8;
        if (bits.Count % 8 != 0) numBytes++;
            
        byte[] bytes = new byte[numBytes];
        int byteIndex = 0, bitIndex = 0;

        for (int i = 0; i < bits.Count; i++)
        {
            if (bits[i])
                bytes[byteIndex] |= (byte)(1 << (7 - bitIndex));

            bitIndex++;
            
            if (bitIndex == 8)
            {
                bitIndex = 0;
                byteIndex++;
            }
        }
        
        return bytes;
    }
    
    public static byte [] toByteArray(int [] arr)
    {
        int index = 0;
        
        byte[] result = new byte[arr.Length * sizeof(int)];
        
        for (int i = 0; i < arr.Length; i++)
        {
            byte[] bytes = BitConverter.GetBytes(arr[i]);
            
            for (int j = 0; j < bytes.Length; j++)
            {
                result[index] = bytes[j];
                index++;
            }
        }
        
        return result;
    }

    public static BitArray toBitArray(byte[] arr)
    {
        BitArray bitArray = new BitArray(arr);
        BitArray _bitArray = new BitArray(bitArray.Count);
        
        for (int i = 0; i < bitArray.Count; i += 8)
        {
            int firstPos;
            int lastPos = 7;
            
            for (firstPos = 0; lastPos - firstPos > 0 ; firstPos++)
            {
                _bitArray[firstPos + i] = bitArray[lastPos + i];
                _bitArray[lastPos + i] = bitArray[firstPos + i];
                
                lastPos--;
            }   
        }

        return _bitArray;
    }
}
