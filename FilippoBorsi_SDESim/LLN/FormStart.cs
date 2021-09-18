using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LLN
{
    public partial class FormStart : Form
    {
        public FormStart()
        {
            InitializeComponent();
        }

        private void buttonGlivenkoCantelli_Click(object sender, EventArgs e)
        {
            FormGlivenkCantelli gc = new FormGlivenkCantelli();
            gc.Show();
       
        }

        private void buttonLNN_Click(object sender, EventArgs e)
        {
            FormLLN lln = new FormLLN();
            lln.Show();
        }

        private void buttonRandomWalk_Click(object sender, EventArgs e)
        {
            FormRandomWalk rw = new FormRandomWalk();
            rw.Show();
        }

        private void buttonNewRandPR_Click(object sender, EventArgs e)
        {
            NewRandomProcess nrp = new NewRandomProcess();
            nrp.Show();
        }

        private void buttonBrownian_Click(object sender, EventArgs e)
        {
            FormBrownian fbr = new FormBrownian();
            fbr.Show();
        }

        private void buttonGBM_Click(object sender, EventArgs e)
        {
            FormGeometricBrownian fgbm = new FormGeometricBrownian();
            
            fgbm.Show();
        }

        private void buttonVasicek_Click(object sender, EventArgs e)
        {
            FormVasicek vas = new FormVasicek();
            vas.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormCEV cev = new FormCEV();
            cev.Show();
        }
    }
}
