namespace AmceLibrary;

public class MenuItem
{
    public string Name {get; private  set;}
    
    public List<MenuItem> MenuItems {get; private set;}
    
    public Status Status { get; private  set;}

    private EventHandler _clickEvent;
    
    public static explicit operator ToolStripMenuItem(MenuItem mt) => mt.toToolStripMenuItem();

    public MenuItem(string name, int status, List<MenuItem>? menuItems = null, EventHandler clickEvent = null)
    {
        Name = name;
        Status = (Status)status;
        MenuItems = menuItems ?? new List<MenuItem>();
        _clickEvent = clickEvent;
    }
    
    private ToolStripMenuItem toToolStripMenuItem()
    {
        var item = new ToolStripMenuItem(Name);

        if (_clickEvent != null)
        {
            item.Click += _clickEvent;
        }
        
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
