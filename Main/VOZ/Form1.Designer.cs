namespace VOZ;

partial class Form1
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
        questionTextLabel.Location = new System.Drawing.Point(12, 9);
        questionTextLabel.Name = "questionTextLabel";
        questionTextLabel.Size = new System.Drawing.Size(776, 86);
        questionTextLabel.TabIndex = 0;
        questionTextLabel.Text = "label1";
        //
        // Form1
        //
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        ClientSize = new System.Drawing.Size(800, 450);
        Controls.Add(questionTextLabel);
        Text = "Form1";
        ResumeLayout(false);
    }

    #endregion
}
