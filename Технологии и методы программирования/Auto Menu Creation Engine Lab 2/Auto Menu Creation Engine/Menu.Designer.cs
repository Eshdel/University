namespace Auto_Menu_Creation_Engine;

partial class Menu
{
    /// <summary>
    ///  Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    ///  Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    ///  Required method for Designer support - do not modify
    ///  the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this.components = new System.ComponentModel.Container();
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(1000, 800);
        this.Text = "Auto Menu Creation Engine";
        
        MenuStrip menuStrip = new MenuStrip();

        this.menuStrip = new System.Windows.Forms.MenuStrip();
        this.SuspendLayout();
        // 
        // menuBar
        // 
        this.menuStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
        this.menuStrip.Location = new System.Drawing.Point(0, 0);
        this.menuStrip.Name = "menuBar";
        this.menuStrip.Size = new System.Drawing.Size(800, 24);
        this.menuStrip.TabIndex = 0;
        this.menuStrip.Text = "menuBar";
        // 
        // MenuGenerator
        // 
        this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(800, 450);
        this.Controls.Add(this.menuStrip);
        this.MainMenuStrip = this.menuStrip;
        this.Name = "Auto Menu Creation Engine";
        this.Text = "Auto Menu Creation Engine";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    #endregion
    
    private System.Windows.Forms.MenuStrip menuStrip;
}