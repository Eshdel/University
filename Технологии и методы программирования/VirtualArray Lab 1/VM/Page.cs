using System.Collections;

namespace VM;

public class Page: IComparable<Page>
{
    public int Id { get; private set; } = BASE_ID;

    private int[] _intArray;

    private BitArray _bitMap;

    public long LastTimeCall { get; private set; }

    public PageStatus Status { get; private set; }
    
    public static readonly int SizeInBytes = 512;
    
    public static int Lenght => SizeInBytes * 8 / (sizeof(int) * 8 + sizeof(byte));
    
    public const int BASE_ID = -1;
    private Page(int id,int [] arr, BitArray bitMap)
    {
        Id = id;
        _intArray = arr;
        _bitMap = bitMap;
        Status = PageStatus.NONE;
        UpdatateTimeCall();
    }
    
    private Page(int id)
    {
        Id = id;
        _intArray = new int [Lenght];
        _bitMap = new BitArray(Lenght);
        Status = PageStatus.NONE;
        UpdatateTimeCall();
    }

    private void UpdatateTimeCall()
    {
        LastTimeCall = DateTime.Now.Millisecond;
    }

    public int GetInteger(int index)
    {
        UpdatateTimeCall();
        return _intArray[index];
    }

    public void SetInteger(int index, int value)
    {
        _intArray[index] = value;
        _bitMap[index] = true;
        Status = PageStatus.MODIFY;
        
        UpdatateTimeCall();
    }

    public bool GetBitMapValue(int index)
    {
        if(index >= Page.Lenght)
            throw new ArgumentOutOfRangeException();
        
        UpdatateTimeCall();
        return _bitMap[index];
    }

    public static Page LoadPage(int id,int [] arr, BitArray bitMap)
    {
        return new Page(id, arr, bitMap);
    }
    
    public static Page CreatePage()
    {
        return new Page(BASE_ID);
    }

    public byte[] GetBitMap()
    { 
        return  Converter.toByteArray(_bitMap);
    }
    
    public byte[] GetIntArray()
    { 
        return Converter.toByteArray(_intArray);
    }

    public int CompareTo(Page? other)
    {
        if (other == null) 
            return 1;
        
        return LastTimeCall.CompareTo(other.LastTimeCall);
    }
    
}