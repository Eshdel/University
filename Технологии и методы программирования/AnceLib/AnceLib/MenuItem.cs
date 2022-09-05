namespace Auto_Menu_Creation_Engine;
using System.Windows;
public class MenuItem
{
    public string Name {get; private  set;}
    
    public List<MenuItem> MenuItems {get; private set;}
    
    public Status Status { get; private  set;}
    
    public static explicit operator ToolStripMenuItem(MenuItem mt) => mt.toMenuItem();

    public MenuItem(string name, int status, List<MenuItem>? menuItems = null)
    {
        Name = name;
        Status = (Status)status;
        MenuItems = menuItems ?? new List<MenuItem>();
    }
    
    private ToolStripMenuItem toMenuItem()
    {
        var item = new ToolStripMenuItem(Name);

        switch (Status)
        {
            case Status.ENABLE:
                item.DropDownItems.AddRange(MenuItems.Select(menuItem => (ToolStripMenuItem) menuItem).ToArray());
                break;
            
            case Status.DISABLE:
                item.Enabled = false;
                break;
            
            case Status.INVISIABLE:
                item.Visible = false;
                break;
        }

        return item;
    }
}
