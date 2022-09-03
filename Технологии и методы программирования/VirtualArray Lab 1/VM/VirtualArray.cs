using System.Collections;

namespace VM;

public class VirtualArray
{
    public long Size { get; private set; }

    public string Path { get; private set; }

    private int AmountPagesInArray => (int)Math.Ceiling(Math.Log(Size));

    private int AmountPagesInFile => (int)Math.Ceiling((double)Size / Page.Lenght);

    private int GetIndexEnlementInPage(long index) => (int)index % Page.Lenght;

    private int GetIdPage(long index) => (int)index / Page.Lenght;

    private int GetIndexPage(int indexPage) => indexPage * Page.SizeInBytes;

    private BinaryReader _reader;

    private BinaryWriter _writer;

    private Page[] _pages;

    private readonly string _filename = "VirtualArray" + DateTime.Now.Millisecond;

    private const string Filetype = ".VA";

    public VirtualArray(long size, string path = "NOT_SET")
    {
        if (path == "NOT_SET")
            path = _filename + Filetype;

        if (size < 1)
            throw new ArgumentOutOfRangeException();

        Size = size;
        Path = path;
        _pages = new Page[AmountPagesInArray];

        InitFile();
    }

    public int Get(long index)
    {
        return _pages[GetIndexPageInArray(index)].GetInteger(GetIndexEnlementInPage(index));
    }
    
    public bool GetBitMapValue(int index)
    {
        return _pages[GetIndexPageInArray(index)].GetBitMapValue(GetIndexEnlementInPage(index));
    }

    public void Set(long index, int value)
    {
        _pages[GetIndexPageInArray(index)].SetInteger(GetIndexEnlementInPage(index), value);
    }

    private int GetIndexPageInArray(long index)
    {
        Page page = null;
        int indexPage = 0;

        for (int i = 0; i < _pages.Length; i++)
        {
            if (_pages[i] == null)
                break;

            if (_pages[i].Id == GetIdPage(index))
                return i;
        }

        if (page == null)
        {
            page = LoadPage(GetIndexPage(GetIdPage(index)), GetIdPage(index));
            indexPage = Array.IndexOf(_pages, page);
        }

        return indexPage;
    }

    private void SavePage(Page page)
    {
        _writer = new BinaryWriter(File.Open(Path, FileMode.Open));

        if (page.Id == Page.BASE_ID)
        {
            _writer.Seek(0, SeekOrigin.End);
            _writer.Write(page.GetBitMap());
            _writer.Write(page.GetIntArray());
        }

        else
        {
            _writer.Seek(GetIndexPage(page.Id), SeekOrigin.Begin);
            _writer.Write(page.GetBitMap());
            _writer.Write(page.GetIntArray());
        }

        _writer.Close();
    }

    private Page LoadPage(int indexPageInFile,int idPage)
    {
        if (indexPageInFile < 0)
            throw new ArgumentOutOfRangeException();
        
        _reader = new BinaryReader(File.Open(Path, FileMode.Open));
        
        _reader.ReadBytes(indexPageInFile);
        
        BitArray bitMap = Converter.toBitArray(_reader.ReadBytes(Page.SizeInBytes - Page.Lenght * sizeof(int)));
        int[] intArray = new int[Page.Lenght];

        for (int i = 0; i < Page.Lenght; i++)
            intArray[i] = _reader.ReadInt32();
        
        _reader.Close();
        
        Page loadedPage = Page.LoadPage(idPage, intArray, bitMap);
        
        var freeIndex = GetFreeIndexInPageArray();
        
        if(freeIndex == -1)
            SwapPageInArray(loadedPage);
        else
            AddPageInArray(freeIndex,loadedPage);
        
        return loadedPage;
    }

    private void SwapPageInArray(Page page)
    {
        Page? latestPage = _pages.Min();
        
        int index = 0;

        if (latestPage == null)
        {
            _pages[index] = page;
            return;
        }

        for (int i = 0; i < _pages.Length; i++)
        {
            if (_pages[i] == latestPage)
                index = i;
        }

        if (latestPage.Status == PageStatus.MODIFY)
            SavePage(latestPage);
        
        _pages[index] = page;
    }

    private void AddPageInArray(int indexPageInArray, Page page)
    {
        _pages[indexPageInArray] = page;
    }

    private int GetFreeIndexInPageArray()
    {
        int index = 0;
        
        foreach (var page in _pages)
        {
            if (page == null)
                return index;
            
            index++;
        }
    
        return -1;
    }
 
    private void InitFile()                                                                                                                                                    
    {
        _writer = new BinaryWriter(File.Create(Path));
        _writer.Close();
        
        for (int i = 0; i < AmountPagesInFile; i++)
            SavePage(Page.CreatePage());
    }
}