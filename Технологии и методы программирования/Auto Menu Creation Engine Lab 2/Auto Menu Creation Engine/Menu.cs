using System.Text;
using AmceLibrary;

namespace Auto_Menu_Creation_Engine;

public partial class Menu : Form
{
    public Menu()
    {
        InitializeComponent();
        CreatePresetFile();
        CreateMenu();
    }

    public static void CreatePresetFile()
    {
        if (!File.Exists("menuPreset.amce"))
        {
            var presetData = new string [] {"0 Разное 0 Others",
                "0 Сотрудники 0 Stuff",
                "0 Приказы 0 Orders",
                "0 Документы 0 Docs",
                "0 Справочники 0",
                "1 Отделы 0 Departs",
                "1 Города 0 Towns",
                "1 Должности 0 Posts",
                "0 Окно 0 Window",
                "0 Справка 0 Yelp"
                };
            
            var stream = new StreamWriter(new FileStream("menuPreset.amce", FileMode.Create, FileAccess.Write), Encoding.UTF8);
            
            foreach (var str in presetData)
                stream.WriteLine(str);

            stream.Close();
        } 
    }

    public void CreateMenu()
    {
        OpenFileDialog  openFileDialog = new OpenFileDialog();
        openFileDialog.Filter = "amce files (*.amce)|*.amce|All files (*.*)|*.*";
        openFileDialog.RestoreDirectory = true;
        
        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            var menuCreator = new MenuCreator(openFileDialog.FileName);
            menuStrip.Items.AddRange(menuCreator.MenuItems);
        }
    }
}