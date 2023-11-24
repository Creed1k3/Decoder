using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace утютютютю
{
    public partial class Form1 : Form
    {
        private RSACryptoServiceProvider rsa;
        private RSAParameters privateKey;

        public Form1()
        {
            InitializeComponent();
            rsa = new RSACryptoServiceProvider();
            privateKey = rsa.ExportParameters(true);
            button2.Click += button2_Click;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы хотите сохранить ключ для рассшифровки?", "Подтверждение", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                try
                {
                    string text = textBox1.Text;
                    byte[] data = Encoding.UTF8.GetBytes(text);
                    byte[] encryptedData = rsa.Encrypt(data, false);
                    textBox2.Text = Convert.ToBase64String(encryptedData);

                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        using (XmlWriter writer = XmlWriter.Create(saveFileDialog.FileName))
                        {
                            writer.WriteStartElement("PrivateKey");
                            writer.WriteElementString("Modulus", Convert.ToBase64String(privateKey.Modulus));
                            writer.WriteElementString("Exponent", Convert.ToBase64String(privateKey.Exponent));
                            writer.WriteElementString("D", Convert.ToBase64String(privateKey.D));
                            writer.WriteElementString("P", Convert.ToBase64String(privateKey.P));
                            writer.WriteElementString("Q", Convert.ToBase64String(privateKey.Q));
                            writer.WriteElementString("DP", Convert.ToBase64String(privateKey.DP));
                            writer.WriteElementString("DQ", Convert.ToBase64String(privateKey.DQ));
                            writer.WriteElementString("InverseQ", Convert.ToBase64String(privateKey.InverseQ));
                            writer.WriteEndElement();
                        }

                        MessageBox.Show("Файл успешно сохранен", "Сохранение файла", MessageBoxButtons.OK);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
            else
            {
                try
                {
                    string text = textBox1.Text;
                    byte[] data = Encoding.UTF8.GetBytes(text);
                    byte[] encryptedData = rsa.Encrypt(data, false);
                    textBox2.Text = Convert.ToBase64String(encryptedData);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Вы хотите использовать ключ для расшифровки?", "Подтверждение", MessageBoxButtons.YesNo);

            if (result == DialogResult.Yes)
            {
                try
                {
                    string encryptedText = textBox2.Text;
                    byte[] encryptedData = Convert.FromBase64String(encryptedText);

                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*";
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string privateKeyPath = openFileDialog.FileName;
                        rsa.FromXmlString(File.ReadAllText(privateKeyPath));

                        byte[] decryptedData = rsa.Decrypt(encryptedData, false);
                        textBox3.Text = Encoding.UTF8.GetString(decryptedData);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ключ не подходит");
                }
            }
            else
            {
                try
                {
                    string encryptedText = textBox2.Text;
                    byte[] encryptedData = Convert.FromBase64String(encryptedText);

                    byte[] decryptedData = rsa.Decrypt(encryptedData, false);
                    textBox3.Text = Encoding.UTF8.GetString(decryptedData);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }
    }
}