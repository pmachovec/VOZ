namespace VOZ;

partial class MainWindow
{
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    private System.Windows.Forms.Label questionTextLabel;

    /// <summary>
    /// Clean up any resources being used.
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
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        questionTextLabel = new System.Windows.Forms.Label();
        SuspendLayout();
        //
        // questionTextLabel
        //
        questionTextLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
        questionTextLabel.Location = new System.Drawing.Point(12, 9);
        questionTextLabel.Name = "questionTextLabel";
        questionTextLabel.Size = new System.Drawing.Size(776, 25);
        questionTextLabel.TabIndex = 0;
        questionTextLabel.Text = "Question text";
        //
        // MainWindow
        //
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(800, 450);
        Controls.Add(questionTextLabel);
        Text = "VOZ";
        ResumeLayout(false);
    }

    #endregion
}
