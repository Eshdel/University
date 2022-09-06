using System.Text;
namespace AmceLibrary;

public class MenuCreator
{
    private readonly List<MenuItem> _items;

    public ToolStripMenuItem[] MenuItems => _items.Select(menuItem => (ToolStripMenuItem) menuItem).ToArray();

    public MenuCreator(string path)
    {
        var streamReader = new StreamReader(new FileStream(path, FileMode.Open, FileAccess.Read), Encoding.UTF8);
        
        _items = toListMenuItems(
            streamReader.ReadToEnd()
                .Trim()
                .Split("\n")
                .Select(str => str.Split(new []{' '}, StringSplitOptions.RemoveEmptyEntries))
                .ToArray()
        );
        
        streamReader.Close();
    }

    private List<MenuItem> toListMenuItems(string[][] data)
    {
        var listMenuItems = new List<MenuItem>();
        
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i].Length == 3)
            {
                var index = int.Parse(data[i][0]);
               
                var submenu = data.Skip(i + 1).TakeWhile(strings => int.Parse(strings[0]) > index).ToArray();

                listMenuItems.Add(new MenuItem(data[i][1], int.Parse(data[i][2]), toListMenuItems(submenu)));

                i += submenu.Length;
            }
            
            else
            {
                var text = data[i][3];
                listMenuItems.Add(new MenuItem(data[i][1], int.Parse(data[i][2]),clickEvent:(_,_) => MessageBox.Show(text)));
            }
        }

        return listMenuItems;
    }
}