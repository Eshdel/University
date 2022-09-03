using System.Collections;
using System.Text;
using VM;

namespace Virtual_Array_UTests;

public class Tests
{
    [SetUp]
    public void Setup() {}

    [Test]
    public void GetSetValueInVirualArray()
    {
        VirtualArray vm = new VirtualArray(400);
        
        int index = 0;
        int value = 25;
        vm.Set(index,value);
        Assert.AreEqual(vm.Get(index),value);
    }
    
    [Test]
    public void GetSetValuesInVirualArray()
    {
        int size = 1000;
        
        int[] valuesArray = new int[size];
        VirtualArray vm = new VirtualArray(size);
        
        for (int i = 0; i < size; i++)
        {
            int index = new Random().Next(0, size);
            
            valuesArray[index] = new Random().Next(int.MinValue, int.MaxValue);
            vm.Set(index,valuesArray[index]);
        }
        
        for (int i = 0; i < size; i++)
        {
            Assert.AreEqual(valuesArray[i],vm.Get(i));
        }
        
        for (int i = 0; i < size; i++)
        {
            Assert.AreEqual(vm.Get(i) != 0,vm.GetBitMapValue(i));
        }
    }

    [Test]
    public void CreateFile()
    {
        VirtualArray vm = new VirtualArray(1);
        Assert.AreEqual(File.Exists(vm.Path),true);
    }
    
    [Test]
    public void ByteConvert()
    {
        var t = new BitArray(11);
        
        t[0] = true;
        t[1] = true;
        t[2] = true;
        t[3] = true;
        t[4] = false;
        t[5] = false;
        t[6] = true;
        t[7] = true;
        t[9] = false;
        t[8] = false;
        t[10] = true;
        
        var t1 = Converter.toByteArray(t);
        var t2 = Converter.toBitArray(t1);
    }
}