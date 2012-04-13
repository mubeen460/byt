﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Trascend.Bolet.ControlesByT.Ventanas
{
    public partial class EtiquetaMarca : Form
    {

        public EtiquetaMarca(string urlImagen, string nombreMarca)
        {
            //urlImagen = "C:\\nike.jpg";
            InitializeComponent();
            this.Text += nombreMarca;
            MostrarImagen(urlImagen);
        }
        public void MostrarImagen(string urlImagen)
        {
            try
            {
                this._foto.Size = new System.Drawing.Size(268, 213);
                this._foto.SizeMode = PictureBoxSizeMode.CenterImage;
                this._foto.BorderStyle = BorderStyle.Fixed3D;

                this._foto.ImageLocation = urlImagen;
            }
            catch (IOException ex)
            {
                this.Close();
            }
        }



        private void _btnAceptar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
