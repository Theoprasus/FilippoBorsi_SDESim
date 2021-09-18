namespace LLN
{
    partial class FormStart
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.buttonGlivenkoCantelli = new System.Windows.Forms.Button();
            this.buttonLNN = new System.Windows.Forms.Button();
            this.buttonRandomWalk = new System.Windows.Forms.Button();
            this.buttonNewRandPR = new System.Windows.Forms.Button();
            this.buttonBrownian = new System.Windows.Forms.Button();
            this.buttonGBM = new System.Windows.Forms.Button();
            this.buttonVasicek = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonGlivenkoCantelli
            // 
            this.buttonGlivenkoCantelli.Location = new System.Drawing.Point(401, 217);
            this.buttonGlivenkoCantelli.Name = "buttonGlivenkoCantelli";
            this.buttonGlivenkoCantelli.Size = new System.Drawing.Size(271, 58);
            this.buttonGlivenkoCantelli.TabIndex = 0;
            this.buttonGlivenkoCantelli.Text = "Glivenko-Cantelli";
            this.buttonGlivenkoCantelli.UseVisualStyleBackColor = true;
            this.buttonGlivenkoCantelli.Click += new System.EventHandler(this.buttonGlivenkoCantelli_Click);
            // 
            // buttonLNN
            // 
            this.buttonLNN.Location = new System.Drawing.Point(398, 24);
            this.buttonLNN.Name = "buttonLNN";
            this.buttonLNN.Size = new System.Drawing.Size(268, 58);
            this.buttonLNN.TabIndex = 1;
            this.buttonLNN.Text = "LLN";
            this.buttonLNN.UseVisualStyleBackColor = true;
            this.buttonLNN.Click += new System.EventHandler(this.buttonLNN_Click);
            // 
            // buttonRandomWalk
            // 
            this.buttonRandomWalk.Location = new System.Drawing.Point(47, 24);
            this.buttonRandomWalk.Name = "buttonRandomWalk";
            this.buttonRandomWalk.Size = new System.Drawing.Size(271, 58);
            this.buttonRandomWalk.TabIndex = 2;
            this.buttonRandomWalk.Text = "random walk";
            this.buttonRandomWalk.UseVisualStyleBackColor = true;
            this.buttonRandomWalk.Click += new System.EventHandler(this.buttonRandomWalk_Click);
            // 
            // buttonNewRandPR
            // 
            this.buttonNewRandPR.Location = new System.Drawing.Point(398, 121);
            this.buttonNewRandPR.Name = "buttonNewRandPR";
            this.buttonNewRandPR.Size = new System.Drawing.Size(271, 58);
            this.buttonNewRandPR.TabIndex = 3;
            this.buttonNewRandPR.Text = "Poisson jumps";
            this.buttonNewRandPR.UseVisualStyleBackColor = true;
            this.buttonNewRandPR.Click += new System.EventHandler(this.buttonNewRandPR_Click);
            // 
            // buttonBrownian
            // 
            this.buttonBrownian.Location = new System.Drawing.Point(49, 121);
            this.buttonBrownian.Name = "buttonBrownian";
            this.buttonBrownian.Size = new System.Drawing.Size(272, 58);
            this.buttonBrownian.TabIndex = 4;
            this.buttonBrownian.Text = "Brownian";
            this.buttonBrownian.UseVisualStyleBackColor = true;
            this.buttonBrownian.Click += new System.EventHandler(this.buttonBrownian_Click);
            // 
            // buttonGBM
            // 
            this.buttonGBM.Location = new System.Drawing.Point(49, 217);
            this.buttonGBM.Name = "buttonGBM";
            this.buttonGBM.Size = new System.Drawing.Size(270, 58);
            this.buttonGBM.TabIndex = 5;
            this.buttonGBM.Text = "Geometric brownian";
            this.buttonGBM.UseVisualStyleBackColor = true;
            this.buttonGBM.Click += new System.EventHandler(this.buttonGBM_Click);
            // 
            // buttonVasicek
            // 
            this.buttonVasicek.Location = new System.Drawing.Point(52, 313);
            this.buttonVasicek.Name = "buttonVasicek";
            this.buttonVasicek.Size = new System.Drawing.Size(267, 50);
            this.buttonVasicek.TabIndex = 6;
            this.buttonVasicek.Text = "Vasicek";
            this.buttonVasicek.UseVisualStyleBackColor = true;
            this.buttonVasicek.Click += new System.EventHandler(this.buttonVasicek_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(398, 309);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(271, 58);
            this.button1.TabIndex = 7;
            this.button1.Text = "Cir ";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormStart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.buttonVasicek);
            this.Controls.Add(this.buttonGBM);
            this.Controls.Add(this.buttonBrownian);
            this.Controls.Add(this.buttonNewRandPR);
            this.Controls.Add(this.buttonRandomWalk);
            this.Controls.Add(this.buttonLNN);
            this.Controls.Add(this.buttonGlivenkoCantelli);
            this.Name = "FormStart";
            this.Text = "FormStart";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonGlivenkoCantelli;
        private System.Windows.Forms.Button buttonLNN;
        private System.Windows.Forms.Button buttonRandomWalk;
        private System.Windows.Forms.Button buttonNewRandPR;
        private System.Windows.Forms.Button buttonBrownian;
        private System.Windows.Forms.Button buttonGBM;
        private System.Windows.Forms.Button buttonVasicek;
        private System.Windows.Forms.Button button1;
    }
}