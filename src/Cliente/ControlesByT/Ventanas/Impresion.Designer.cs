﻿using Trascend.Bolet.ControlesByT;

namespace Trascend.Bolet.ControlesByT.Ventanas
{
    partial class Impresion
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Impresion));
            this._btnCerrar = new System.Windows.Forms.Button();
            this._imprimir = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this._folio = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // _btnCerrar
            // 
            this._btnCerrar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._btnCerrar.BackColor = System.Drawing.Color.White;
            this._btnCerrar.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this._btnCerrar.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this._btnCerrar.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(128)))), ((int)(((byte)(255)))));
            this._btnCerrar.Location = new System.Drawing.Point(897, 427);
            this._btnCerrar.Name = "_btnCerrar";
            this._btnCerrar.Size = new System.Drawing.Size(75, 23);
            this._btnCerrar.TabIndex = 1;
            this._btnCerrar.Text = "Cerrar";
            this._btnCerrar.UseVisualStyleBackColor = false;
            this._btnCerrar.Click += new System.EventHandler(this._btnCerrar_Click);
            // 
            // _imprimir
            // 
            this._imprimir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._imprimir.Location = new System.Drawing.Point(816, 427);
            this._imprimir.Name = "_imprimir";
            this._imprimir.Size = new System.Drawing.Size(75, 23);
            this._imprimir.TabIndex = 2;
            this._imprimir.Text = "Imprimir";
            this._imprimir.UseVisualStyleBackColor = true;
            this._imprimir.Click += new System.EventHandler(this._imprimir_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.Location = new System.Drawing.Point(12, 427);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "Borrador";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // _folio
            // 
            this._folio.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this._folio.Font = new System.Drawing.Font("Courier New", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._folio.Location = new System.Drawing.Point(12, 12);
            this._folio.Multiline = true;
            this._folio.Name = "_folio";
            this._folio.Size = new System.Drawing.Size(960, 409);
            this._folio.TabIndex = 4;
            // 
            // Impresion
            // 
            this.AcceptButton = this._btnCerrar;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(984, 462);
            this.Controls.Add(this._folio);
            this.Controls.Add(this.button2);
            this.Controls.Add(this._imprimir);
            this.Controls.Add(this._btnCerrar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(1000, 900);
            this.MinimumSize = new System.Drawing.Size(900, 500);
            this.Name = "Impresion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _btnCerrar;
        private System.Windows.Forms.Button _imprimir;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox _folio;

    }
}