using System;
using System.Drawing;
using System.Windows.Forms;

namespace DidoonReborn
{
    partial class Form1
    {

        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.controlButton = new System.Windows.Forms.Button();
            this.statusLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // controlButton
            // 
            this.controlButton.Location = new System.Drawing.Point(12, 62);
            this.controlButton.Name = "controlButton";
            this.controlButton.Size = new System.Drawing.Size(131, 58);
            this.controlButton.TabIndex = 0;
            this.controlButton.Text = "Start";
            this.controlButton.UseVisualStyleBackColor = true;
            this.controlButton.Click += new System.EventHandler(this.controlButton_Click);
            // 
            // statusLabel
            // 
            this.statusLabel.AccessibleName = "";
            this.statusLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.statusLabel.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.statusLabel.Location = new System.Drawing.Point(12, 9);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(131, 35);
            this.statusLabel.TabIndex = 2;
            this.statusLabel.Text = "label1";
            this.statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(162, 134);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.controlButton);
            this.Name = "Form1";
            this.Text = "DidoonBorn - beta";
            this.ResumeLayout(false);

        }

        #endregion

        private Button controlButton;
        private Label statusLabel;
    }
}

