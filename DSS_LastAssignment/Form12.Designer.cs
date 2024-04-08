// Namespace declaration
namespace DSS_LastAssignment
{
    // Partial class declaration for Form1
    partial class Form1
    {
        // Components for managing resources
        private System.ComponentModel.IContainer components = null;

        // Dispose method to release managed resources
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        // Method for initializing components
        private void InitializeComponent()
        {
            // Controls declaration
            this.button_Browse = new System.Windows.Forms.Button();
            this.button_Discover = new System.Windows.Forms.Button();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.result = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();

            // Form setup
            this.SuspendLayout();
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(683, 752);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "ML Classifier";
            this.Load += new System.EventHandler(this.Form1_Load);

            // Button_Browse setup
            this.button_Browse.Location = new System.Drawing.Point(234, 12);
            this.button_Browse.Margin = new System.Windows.Forms.Padding(4);
            this.button_Browse.Name = "button_Browse";
            this.button_Browse.Size = new System.Drawing.Size(160, 25);
            this.button_Browse.TabIndex = 0;
            this.button_Browse.Text = "Choose Dataset";
            this.button_Browse.UseVisualStyleBackColor = true;
            this.button_Browse.Click += new System.EventHandler(this.button1_Click);

            // Button_Discover setup
            this.button_Discover.Location = new System.Drawing.Point(234, 23);
            this.button_Discover.Margin = new System.Windows.Forms.Padding(4);
            this.button_Discover.Name = "button_Discover";
            this.button_Discover.Size = new System.Drawing.Size(160, 28);
            this.button_Discover.TabIndex = 1;
            this.button_Discover.Text = "Predict";
            this.button_Discover.UseVisualStyleBackColor = true;
            this.button_Discover.Click += new System.EventHandler(this.button_Discover_Click);

            // TextBox2 setup
            this.textBox2.Location = new System.Drawing.Point(58, 42);
            this.textBox2.Margin = new System.Windows.Forms.Padding(4);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(517, 22);
            this.textBox2.TabIndex = 3;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);

            // Result button setup
            this.result.Location = new System.Drawing.Point(234, 13);
            this.result.Margin = new System.Windows.Forms.Padding(4);
            this.result.Name = "result";
            this.result.Size = new System.Drawing.Size(160, 25);
            this.result.TabIndex = 13;
            this.result.Text = "Compare";
            this.result.UseVisualStyleBackColor = true;
            this.result.Click += new System.EventHandler(this.result_Click);

            // GroupBox1 setup
            this.groupBox1.Controls.Add(this.button_Browse);
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Location = new System.Drawing.Point(16, 15);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(643, 81);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select the dataset";

            // GroupBox2 setup
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.result);
            this.groupBox2.Location = new System.Drawing.Point(16, 103);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(643, 100);
            this.groupBox2.TabIndex = 15;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Observe the best algorithm";

            // Label1 setup
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 16);
            this.label1.TabIndex = 16;

          
            // GroupBox3 setup
            this.groupBox3.Controls.Add(this.dataGridView1);
            this.groupBox3.Location = new System.Drawing.Point(16, 210);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(643, 390);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Fill the new instance fields ";

            // DataGridView1 setup
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(8, 25);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.Size = new System.Drawing.Size(627, 357);
            this.dataGridView1.TabIndex = 0;

            // GroupBox4 setup
            this.groupBox4.Controls.Add(this.label2);
            this.groupBox4.Controls.Add(this.button_Discover);
            this.groupBox4.Location = new System.Drawing.Point(16, 608);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox4.Size = new System.Drawing.Size(643, 115);
            this.groupBox4.TabIndex = 17;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Prediction";

            // Label2 setup
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(292, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 16);
            this.label2.TabIndex = 16;
            this.label2.Text = "result";

            

            // Form1 closing brackets
            this.ResumeLayout(false);
        }

        // Controls declaration
        private System.Windows.Forms.Button button_Browse;
        private System.Windows.Forms.Button button_Discover;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button result;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}
